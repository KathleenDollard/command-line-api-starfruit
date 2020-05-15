using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
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
        protected DescriptorMakerBase(Strategy strategy, object dataSource, object parentDataSource = null)
        {
            Strategy = strategy;
            DataSource = dataSource;
            ParentDataSource = parentDataSource;
        }

        protected abstract Candidate GetCandidate(object item);
        protected abstract Type GetArgumentType(Candidate candidate);

        protected Strategy Strategy { get; }
        protected object DataSource { get; }
        protected object ParentDataSource { get; }

        protected  IEnumerable<Candidate> GetChildCandidates(SymbolDescriptorBase commandDescriptor)
        {
            return Strategy.GetCandidateRules.GetCandidates(commandDescriptor);
        }

        private (IEnumerable<Candidate> optionItems, IEnumerable<Candidate> subCommandItems, IEnumerable<Candidate> argumentItems)
             ClassifyChildren(SymbolDescriptorBase commandDescriptor)
        {
            IEnumerable<Candidate> optionItems = null;
            IEnumerable<Candidate> subCommandItems = null;
            IEnumerable<Candidate> argumentItems = null;

            var candidates = GetChildCandidates(commandDescriptor );
            // TODO: Provide way to customize this order since the first match wins
            var symbolSelectionOrder = new SymbolType[] { SymbolType.Argument, SymbolType.Command, SymbolType.Option };
            foreach (var symbolType in symbolSelectionOrder)
            {
                switch (symbolType)
                {
                    case SymbolType.Option:
                        optionItems = Strategy.SelectSymbolRules.GetItems(SymbolType.Option, commandDescriptor, candidates);
                        candidates = Remove(candidates, optionItems);
                        break;
                    case SymbolType.Command:
                        subCommandItems = Strategy.SelectSymbolRules.GetItems(SymbolType.Command, commandDescriptor, candidates);
                        candidates = Remove(candidates, subCommandItems);
                        break;
                    case SymbolType.Argument:
                        argumentItems = Strategy.SelectSymbolRules.GetItems(SymbolType.Argument, commandDescriptor, candidates);
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

        protected CommandDescriptor CommandFrom(SymbolDescriptorBase parentSymbolDescriptor)
        {
            var candidate = GetCandidate(DataSource);
            var commandDescriptor = GetCommand(candidate, parentSymbolDescriptor);
            var (optionItems, subCommandItems, argumentItems) = ClassifyChildren(commandDescriptor);

            commandDescriptor.Arguments.AddRange(argumentItems.Select(i => GetArgument(i, commandDescriptor)));
            commandDescriptor.Options.AddRange(optionItems.Select(i => GetOption(i, commandDescriptor)));
            commandDescriptor.SubCommands.AddRange(subCommandItems.Select(i => GetCommand(i, commandDescriptor)));

            return commandDescriptor;
        }

        protected CommandDescriptor GetCommand(Candidate candidate, SymbolDescriptorBase parentSymbolDescriptor)
        {
            var descriptor = new CommandDescriptor(parentSymbolDescriptor, candidate.Item);
            var ruleSet = Strategy.CommandRules;
            FillSymbol(descriptor, ruleSet, candidate, parentSymbolDescriptor);
            descriptor.TreatUnmatchedTokensAsErrors = ruleSet.TreatUnmatchedTokensAsErrorsRules.GetFirstOrDefaultValue<bool>(descriptor, candidate, parentSymbolDescriptor);
            return descriptor;

        }

        private ArgumentDescriptor GetArgument(Candidate candidate, SymbolDescriptorBase parentSymbolDescriptor)
        {
            var descriptor = new ArgumentDescriptor(parentSymbolDescriptor, candidate.Item);
            var ruleSet = Strategy.ArgumentRules;
            FillSymbol(descriptor, ruleSet, candidate, parentSymbolDescriptor);
            //descriptor.Arity = ruleSet.NameRules.GetFirstOrDefaultValue<string>(descriptor, candidate, parentSymbolDescriptor);
            //descriptor.DefaultValue = ruleSet.NameRules.GetFirstOrDefaultValue<string>(descriptor, candidate, parentSymbolDescriptor);
            descriptor.Required = ruleSet.RequiredRules.GetFirstOrDefaultValue<bool>(descriptor, candidate, parentSymbolDescriptor);
            descriptor.ArgumentType = GetArgumentType(candidate);
            return descriptor;
        }

        private OptionDescriptor GetOption(Candidate candidate, SymbolDescriptorBase parentSymbolDescriptor)
        {
            var descriptor = new OptionDescriptor(parentSymbolDescriptor, candidate.Item)
            {
                Arguments = new ArgumentDescriptor[] { GetArgument(candidate, parentSymbolDescriptor) }
            };
            var ruleSet = Strategy.ArgumentRules;
            FillSymbol(descriptor, ruleSet, candidate, parentSymbolDescriptor);
            //descriptor.Aliases = GetMatching(descriptor, Strategy.AliasRules, param, parentSymbolDescriptor);
            //descriptor.Arity = ruleSet.NameRules.GetFirstOrDefaultValue<string>(descriptor, candidate, parentSymbolDescriptor);
            //descriptor.DefaultValue = ruleSet.NameRules.GetFirstOrDefaultValue<string>(descriptor, candidate, parentSymbolDescriptor);
            descriptor.Required = ruleSet.RequiredRules.GetFirstOrDefaultValue<bool>(descriptor, candidate, parentSymbolDescriptor);
            //descriptor.ArgumentType = ruleSet.NameRules.GetFirstOrDefaultValue<string>(descriptor, candidate, parentSymbolDescriptor);
            return descriptor;
        }

        private void FillSymbol(SymbolDescriptorBase descriptor, RuleSetSymbol ruleSet, Candidate candidate, SymbolDescriptorBase parentSymbolDescriptor)
        {
            //var name = ruleSet.NameRules.GetFirstOrDefaultValue<string>(descriptor, candidate, parentSymbolDescriptor);
            //descriptor.Name = ruleSet.NameRules.MorphValue(descriptor, candidate, name, parentSymbolDescriptor);
            descriptor.Name = ruleSet.NameRules.GetFirstOrDefaultValue<string>(descriptor, candidate, parentSymbolDescriptor);
            descriptor.Description = ruleSet.DescriptionRules.GetFirstOrDefaultValue<string>(descriptor, candidate, parentSymbolDescriptor);
            descriptor.IsHidden = ruleSet.IsHiddenRules.GetFirstOrDefaultValue<bool>(descriptor, candidate, parentSymbolDescriptor);
        }
    }
}
