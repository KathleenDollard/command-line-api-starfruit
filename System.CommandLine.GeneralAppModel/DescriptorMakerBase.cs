using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.Parsing;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{

    /// <summary>
    /// Base class for AppModels.
    /// <br/>
    /// AppModels support translating any artifacts to a common format designed for System.CommandLine
    /// and then taking this format to actual System.CommandLine code - well or something else.
    /// </summary>
    /// <remarks>
    /// Classes that derive from AppModelBase provide the way a particular source is read to create
    /// Descriptors. Descriptors are later used to create System.CommandLine code or other output. 
    /// And example of a derived class the ReflectionAppModel and we plan one for Roslyn to evaluate source code.
    /// <br/>
    /// AppModelBase provides the order of operation of how descriptors are created, such as being depth first.
    /// <br/>
    /// AppModels support Strategies. Strategies each describe a set of rules. A rule might be that items 
    /// ending with "Arg" are arguments. There are rules for a number of AppModel decision points.
    /// <br/>
    /// AppModels describe how to read the source, Strategies describe how to interpret that. 
    /// </remarks>
    public abstract class DescriptorMakerBase
    {
        protected DescriptorMakerBase(Strategy strategy, DescriptorMakerSpecificSourceBase tools, object dataSource)
        {
            DescriptorMakerSpecificSourceBase.SetTools(tools);
            Strategy = strategy;
            DataSource = dataSource;
            ReplaceAbstractRules(strategy, tools);
        }


        protected Strategy Strategy { get; }
        protected object DataSource { get; }
        private IEnumerable<IDescriptionSource> descriptionSources = new List<IDescriptionSource>();

        private (IEnumerable<Candidate> optionItems, IEnumerable<Candidate> subCommandItems, IEnumerable<Candidate> argumentItems)
             ClassifyChildren(IEnumerable<Candidate> candidates, ISymbolDescriptor commandDescriptor)
        {
            var optionItems = new List<Candidate>();
            var subCommandItems = new List<Candidate>();
            var argumentItems = new List<Candidate>();

            // TODO: Provide way to customize this order since the first match wins
            var symbolSelectionOrder = new SymbolType[] { SymbolType.Argument, SymbolType.Command, SymbolType.Option };
            foreach (var symbolType in symbolSelectionOrder)
            {
                switch (symbolType)
                {
                    case SymbolType.Option:
                        optionItems.AddRange(Strategy.SelectSymbolRules
                                        .GetItems(SymbolType.Option, commandDescriptor, candidates));
                        candidates = Remove(candidates, optionItems);
                        break;
                    case SymbolType.Command:
                        subCommandItems.AddRange(Strategy.SelectSymbolRules.GetItems(SymbolType.Command, commandDescriptor, candidates));
                        candidates = Remove(candidates, subCommandItems);
                        break;
                    case SymbolType.Argument:
                        argumentItems.AddRange(Strategy.SelectSymbolRules.GetItems(SymbolType.Argument, commandDescriptor, candidates));
                        candidates = Remove(candidates, argumentItems);
                        break;
                }

                // I had trouble with Except matching on the Candidate and didn't find the problem, so, this works
            }
            return (optionItems, subCommandItems, argumentItems);

            static IEnumerable<Candidate> Remove(IEnumerable<Candidate> candidates, IEnumerable<Candidate> assigned)
            {
                var list = new List<Candidate>();
                foreach (var candidate in candidates)
                {
                    if (assigned.Any(c => c.Item == candidate.Item))
                    { continue; }
                    list.Add(candidate);
                }
                return list;
            }
        }

        protected CommandDescriptor CommandFrom(ISymbolDescriptor parentSymbolDescriptor)
        {
            var ruleSet = Strategy.DescriptorContextRules;
            var candidate = DescriptorMakerSpecificSourceBase.Tools.CreateCandidate(DataSource);
            var sources = ruleSet.DescriptionSourceRules.GetAllValues<Type>(SymbolDescriptor.Empty, candidate, parentSymbolDescriptor);
            descriptionSources = sources
                                    .Select(x => Activator.CreateInstance(x))
                                    .OfType<IDescriptionSource>()
                                    .ToList();
            return GetCommand(candidate, parentSymbolDescriptor);
        }

        private CommandDescriptor GetCommand(Candidate candidate, ISymbolDescriptor parentSymbolDescriptor)
        {
            var descriptor = new CommandDescriptor(parentSymbolDescriptor, candidate.Item);
            var ruleSet = Strategy.CommandRules;
            FillSymbol(descriptor, ruleSet, candidate, parentSymbolDescriptor);
            descriptor.TreatUnmatchedTokensAsErrors = ruleSet.TreatUnmatchedTokensAsErrorsRules.GetFirstOrDefaultValue<bool>(descriptor, candidate, parentSymbolDescriptor);
            SetInvokeMethodIfNeeded(ruleSet, descriptor, candidate, parentSymbolDescriptor);

            var candidates = DescriptorMakerSpecificSourceBase.Tools.GetChildCandidates(Strategy, descriptor);
            candidates = candidates.Where(c => !InNamesToIgnore(Strategy, candidate));
            var (optionItems, subCommandItems, argumentItems) = ClassifyChildren(candidates, descriptor);

            descriptor.Arguments.AddRange(argumentItems.Select(i => GetArgument(i, descriptor)));
            descriptor.Options.AddRange(optionItems.Select(i => GetOption(i, descriptor)));
            descriptor.SubCommands.AddRange(subCommandItems.Select(i => GetCommand(i, descriptor)));
            return descriptor;

            static bool InNamesToIgnore(Strategy strategy, Candidate candidate)
            {
                var identity = candidate.Traits.OfType<IdentityWrapper<string>>().FirstOrDefault();
                return identity is null
                            ? false
                            : strategy.GetCandidateRules.NamesToIgnore.Contains(identity.Value);
            }
        }

        private ArgumentDescriptor GetArgument(Candidate candidate, ISymbolDescriptor parentSymbolDescriptor)
        {
            var argumentType = DescriptorMakerSpecificSourceBase.Tools.GetArgTypeInfo(candidate) ?? throw new InvalidOperationException("Type must be supplied for argument");
            var descriptor = new ArgumentDescriptor(argumentType, parentSymbolDescriptor, candidate.Item);
            var ruleSet = Strategy.ArgumentRules;
            FillSymbol(descriptor, ruleSet, candidate, parentSymbolDescriptor);
            SetArityIfNeeded(ruleSet, descriptor, candidate, parentSymbolDescriptor);
            SetDefaultIfNeeded(ruleSet, descriptor, candidate, parentSymbolDescriptor);
            var allowedValues = ruleSet.AllowedValuesRules.GetAllValues<object[]>(descriptor, candidate, parentSymbolDescriptor)
                                    .SelectMany(x => x);
            descriptor.AllowedValues.AddRange(allowedValues);
            descriptor.Required = ruleSet.RequiredRules.GetFirstOrDefaultValue<bool>(descriptor, candidate, parentSymbolDescriptor);
            return descriptor;
        }

        private void SetInvokeMethodIfNeeded(RuleSetCommand ruleSet, CommandDescriptor descriptor, Candidate candidate, ISymbolDescriptor parentSymbolDescriptor)
        {
            var (success, invokeMethodInfo) = ruleSet.InvokeMethodRules.GetOptionalValue<InvokeMethodInfo>(descriptor, candidate, parentSymbolDescriptor);
            if (success)
            {
                if (invokeMethodInfo.ChildCandidates.Any())  // if not treating parameters as candidates, this will be empty
                {
                    var (optionCandidates, commandCandidates, argumentCandidates) = ClassifyChildren(invokeMethodInfo.ChildCandidates, parentSymbolDescriptor);
                    descriptor.Arguments.AddRange(argumentCandidates.Select(i => GetArgument(i, descriptor)));
                    descriptor.Options.AddRange(optionCandidates.Select(i => GetOption(i, descriptor)));
                }

                descriptor.InvokeMethod = invokeMethodInfo;
            }
        }

        private void SetDefaultIfNeeded(RuleSetArgument ruleSet, ArgumentDescriptor descriptor, Candidate candidate, ISymbolDescriptor parentSymbolDescriptor)
        {
            var (success, value) = ruleSet.DefaultValueRules.GetOptionalValue<object>(descriptor, candidate, parentSymbolDescriptor);
            if (success)
            {
                descriptor.DefaultValue = new DefaultValueDescriptor(value);
            }
        }

        private void SetArityIfNeeded(RuleSetArgument ruleSet, ArgumentDescriptor descriptor, Candidate candidate, ISymbolDescriptor parentSymbolDescriptor)
        {
            var data = ruleSet.ArityRules.GetFirstOrDefaultValue<Dictionary<string, object>>(descriptor, candidate, parentSymbolDescriptor);
            if (data is null || !data.Any())
            {
                return;
            }
            var arity = new ArityDescriptor();
            if (data.TryGetValue(ArityDescriptor.MinimumCountName, out var objMinCount))
            {
                arity.MinimumCount = Conversions.To<int>(objMinCount);
            }
            if (data.TryGetValue(ArityDescriptor.MaximumCountName, out var objMaxCount))
            {
                arity.MaximumCount = Conversions.To<int>(objMaxCount);
            }
            descriptor.Arity = arity;
        }

        private OptionDescriptor GetOption(Candidate candidate, ISymbolDescriptor parentSymbolDescriptor)
        {
            var descriptor = new OptionDescriptor(parentSymbolDescriptor, candidate.Item);
            descriptor.Arguments.Add(GetArgument(candidate, descriptor));
            var ruleSet = Strategy.OptionRules;
            FillSymbol(descriptor, ruleSet, candidate, parentSymbolDescriptor);
            //descriptor.Arity = ruleSet.NameRules.GetFirstOrDefaultValue<string>(descriptor, candidate, parentSymbolDescriptor);
            //descriptor.DefaultValue = ruleSet.NameRules.GetFirstOrDefaultValue<string>(descriptor, candidate, parentSymbolDescriptor);
            descriptor.Required = ruleSet.RequiredRules.GetFirstOrDefaultValue<bool>(descriptor, candidate, parentSymbolDescriptor);
            //descriptor.ArgumentType = ruleSet.NameRules.GetFirstOrDefaultValue<string>(descriptor, candidate, parentSymbolDescriptor);
            return descriptor;
        }

        private void FillSymbol(ISymbolDescriptor symbolDescriptor, RuleSetSymbol ruleSet, Candidate candidate, ISymbolDescriptor parentSymbolDescriptor)
        {
            if (!(symbolDescriptor is SymbolDescriptor descriptor))
            {
                return;
            }
            // Posix rules are hard coded here, because there has been discussion about having System.CommandLine handle prefixes
            var name = ruleSet.NameRules.GetFirstOrDefaultValue<string>(descriptor, candidate, parentSymbolDescriptor) ?? string.Empty;
            var aliases = ruleSet.AliasRules.GetAllValues<string[]>(descriptor, candidate, parentSymbolDescriptor)
                                     .SelectMany(x => x);
            aliases = aliases.SelectMany(x => x.Split(','));
            aliases = descriptor is OptionDescriptor
                        ? aliases.Select(x => x.StartsWith("-") ? x : "-" + x)
                        : aliases;
            descriptor.Name = name;
            descriptor.Aliases = aliases.ToList();
            descriptor.Description = GetDescription(descriptor, ruleSet.DescriptionRules, candidate, parentSymbolDescriptor);
            descriptor.IsHidden = ruleSet.IsHiddenRules.GetFirstOrDefaultValue<bool>(descriptor, candidate, parentSymbolDescriptor);
        }

        private string GetDescription(SymbolDescriptor descriptor, RuleGroup<IRuleGetValue<string>> rules, Candidate candidate, ISymbolDescriptor parentSymbolDescriptor)
        {
            var description = rules.GetFirstOrDefaultValue<string>(descriptor, candidate, parentSymbolDescriptor);
            if (!string.IsNullOrWhiteSpace(description) && !(description is null))
            {
                return description;
            }
            var route = GetRoute(descriptor, parentSymbolDescriptor);
            var source = descriptionSources
                    .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.GetDescription(route)));
            return source is null
                    ? string.Empty
                    : source.GetDescription(route);
        }

        private string GetRoute(SymbolDescriptor descriptor, ISymbolDescriptor parentSymbolDescriptor)
        {
            var parent = GetParentRoute(parentSymbolDescriptor);
            parent = parent.EndsWith("+")
                        ? parent.Substring(0,parent.Length-1)
                        : parent;
            // I don't like this approach and think it suggests we are applying -- too early
            parent = parent.EndsWith("--")
                         ? parent.Substring(2)
                         : parent;

            var plus = string.IsNullOrEmpty(parent) ? string.Empty : "+";
            return $"{parent}{plus}{descriptor.Name}";
        }

        private string GetParentRoute(ISymbolDescriptor parentSymbolDescriptor)
        {
            if (parentSymbolDescriptor is SymbolDescriptor parentDescriptor)
            {
                var parent = GetParentRoute(parentDescriptor.ParentSymbolDescriptorBase);
                var plus = string.IsNullOrEmpty(parent) ? string.Empty : "+";
                return $"{parent}{plus}{parentDescriptor.Name}";
            }
            return string.Empty;
        }

        private void ReplaceAbstractRules(Strategy strategy, DescriptorMakerSpecificSourceBase tools)
        {
            strategy.GetCandidateRules.ReplaceAbstractRules(tools);
            strategy.SelectSymbolRules.ReplaceAbstractRules(tools);
            strategy.CommandRules.ReplaceAbstractRules(tools);
            strategy.ArgumentRules.ReplaceAbstractRules(tools);
            strategy.OptionRules.ReplaceAbstractRules(tools);
        }
    }
}
