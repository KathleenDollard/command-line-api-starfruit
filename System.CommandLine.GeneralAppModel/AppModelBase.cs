using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Linq;
using System.Threading;

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
    public abstract class AppModelBase
    {
        public AppModelBase(Strategy strategy, object dataSource, object parentDataSource = null)
        {
            Strategy = strategy;
            DataSource = dataSource;
            ParentDataSource = parentDataSource;
        }

        protected abstract IEnumerable<object> GetChildCandidates(object DataSource);
        protected abstract IEnumerable<object> GetDataCandidates(object DataSource);

        protected Strategy Strategy { get; }
        protected object DataSource { get; }
        protected object ParentDataSource { get; }

        protected (IEnumerable<object> optionItems, IEnumerable<object> subCommandItems, IEnumerable<object> argumentItems)
             ClassifyChildren(SymbolDescriptorBase commandDescriptor)
        {
            IEnumerable<object> optionItems = null;
            IEnumerable<object> subCommandItems = null;
            IEnumerable<object> argumentItems = null;

            var candidates = GetChildCandidates(DataSource);
            // TODO: Provide way to customize this order since the first match wins
            var symbolSelectionOrder = new SymbolType[] { SymbolType.Argument, SymbolType.Command, SymbolType.Option };
            foreach (var symbolType in symbolSelectionOrder)
            {
                switch (symbolType)
                {
                    case SymbolType.Option:
                        optionItems = Strategy.SelectSymbolRules.SelectSymbolRules
                                    .GetItemsForSymbol<object>(symbolType, candidates, commandDescriptor)
                                    .ToList();
                        candidates = candidates.Except(optionItems);
                        break;
                    case SymbolType.Command:
                        subCommandItems = Strategy.SelectSymbolRules.SelectSymbolRules
                                    .GetItemsForSymbol<object>(symbolType, candidates, commandDescriptor)
                                    .ToList();
                        candidates = candidates.Except(subCommandItems);
                        break;
                    case SymbolType.Argument:
                        argumentItems = Strategy.SelectSymbolRules.SelectSymbolRules
                                    .GetItemsForSymbol<object>(symbolType, candidates, commandDescriptor)
                                    .ToList();
                        candidates = candidates.Except(argumentItems);
                        break;
                }
            }
            return (optionItems, subCommandItems, argumentItems);
        }



        protected CommandDescriptor CommandFrom(SymbolDescriptorBase parentSymbolDescriptor)
        {
            var commandDescriptor = GetCommand(DataSource, parentSymbolDescriptor);
            var (optionItems, subCommandItems, argumentItems) = ClassifyChildren(commandDescriptor);

            commandDescriptor.Arguments.AddRange(argumentItems.Select(i => GetArgument(i, commandDescriptor)));
            commandDescriptor.Options.AddRange(argumentItems.Select(i => GetOption(i, commandDescriptor)));
            commandDescriptor.SubCommands.AddRange(argumentItems.Select(i => GetCommand(i, commandDescriptor)));

            return commandDescriptor;
        }
        protected CommandDescriptor GetCommand(object item, SymbolDescriptorBase parentSymbolDescriptor)
        {
            var dataCandidates = GetDataCandidates(DataSource);
            var descriptor = new CommandDescriptor(parentSymbolDescriptor, DataSource);
            var ruleSet = Strategy.CommandRules;
            descriptor.Name = ruleSet.NameRules.GetFirstOrDefaultValue<string>(descriptor, dataCandidates, parentSymbolDescriptor);
            descriptor.Description = ruleSet.DescriptionRules.GetFirstOrDefaultValue<string>(descriptor, dataCandidates, parentSymbolDescriptor);
            return descriptor;

        }

        private ArgumentDescriptor GetArgument(object item, SymbolDescriptorBase parentSymbolDescriptor)
        {
            var dataCandidates = GetDataCandidates(item);
            var descriptor = new ArgumentDescriptor(parentSymbolDescriptor, item);
            var ruleSet = Strategy.ArgumentRules;
            descriptor.Name = ruleSet.NameRules.GetFirstOrDefaultValue<string>(descriptor, dataCandidates, parentSymbolDescriptor);
            descriptor.Description = ruleSet.DescriptionRules.GetFirstOrDefaultValue<string>(descriptor, dataCandidates, parentSymbolDescriptor);
            descriptor.IsHidden = ruleSet.IsHiddenRule.GetFirstOrDefaultValue<bool>(descriptor, dataCandidates, parentSymbolDescriptor);
            //descriptor.Arity = ruleSet.NameRules.GetFirstOrDefaultValue<string>(descriptor, dataCandidates, parentSymbolDescriptor);
            //descriptor.DefaultValue = ruleSet.NameRules.GetFirstOrDefaultValue<string>(descriptor, dataCandidates, parentSymbolDescriptor);
            descriptor.Required = ruleSet.RequiredRule.GetFirstOrDefaultValue<bool>(descriptor, dataCandidates, parentSymbolDescriptor);
            //descriptor.ArgumentType = ruleSet.NameRules.GetFirstOrDefaultValue<string>(descriptor, dataCandidates, parentSymbolDescriptor);
            return descriptor;
        }

        private OptionDescriptor GetOption(object item, SymbolDescriptorBase parentSymbolDescriptor)
        {
            var dataCandidates = GetDataCandidates(item);
            var descriptor = new OptionDescriptor(parentSymbolDescriptor, item);
            //{
            //    Arguments = new ArgumentDescriptor[] { BuildArgument(param, parentSymbolDescriptor) }
            //};
            var ruleSet = Strategy.ArgumentRules;
            descriptor.Name = ruleSet.NameRules.GetFirstOrDefaultValue<string>(descriptor, dataCandidates, parentSymbolDescriptor);
            descriptor.Description = ruleSet.DescriptionRules.GetFirstOrDefaultValue<string>(descriptor, dataCandidates, parentSymbolDescriptor);
            descriptor.IsHidden = ruleSet.IsHiddenRule.GetFirstOrDefaultValue<bool>(descriptor, dataCandidates, parentSymbolDescriptor);
            //descriptor.Aliases = GetMatching(descriptor, Strategy.AliasRules, param, parentSymbolDescriptor);
            //descriptor.Arity = ruleSet.NameRules.GetFirstOrDefaultValue<string>(descriptor, dataCandidates, parentSymbolDescriptor);
            //descriptor.DefaultValue = ruleSet.NameRules.GetFirstOrDefaultValue<string>(descriptor, dataCandidates, parentSymbolDescriptor);
            descriptor.Required = ruleSet.RequiredRule.GetFirstOrDefaultValue<bool>(descriptor, dataCandidates, parentSymbolDescriptor);
            //descriptor.ArgumentType = ruleSet.NameRules.GetFirstOrDefaultValue<string>(descriptor, dataCandidates, parentSymbolDescriptor);
            return descriptor;
        }

        //protected abstract ArgumentDescriptor GetArgument(object item, SymbolDescriptorBase parentSymbolDescriptor);
        //protected abstract OptionDescriptor GetOption(object item, SymbolDescriptorBase parentSymbolDescriptor);
        //protected abstract CommandDescriptor GetCommand(object item, SymbolDescriptorBase parentSymbolDescriptor);}
    }
}
