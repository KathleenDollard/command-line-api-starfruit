using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;

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

        protected Strategy Strategy { get; }
        protected object DataSource { get; }
        protected object ParentDataSource { get; }

        protected abstract IEnumerable<ArgumentDescriptor> GetArguments( SymbolDescriptorBase parentSymbolDescriptor);
        protected abstract IEnumerable<CommandDescriptor> GetSubCommands(SymbolDescriptorBase parentSymbolDescriptor);
        protected abstract IEnumerable<OptionDescriptor> GetOptions(SymbolDescriptorBase parentSymbolDescriptor);
        protected abstract CommandDescriptor GetCommand(SymbolDescriptorBase parentSymbolDescriptor);

        protected CommandDescriptor CommandFrom(SymbolDescriptorBase parentSymbolDescriptor)
        {
            var commandDescriptor = GetCommand(parentSymbolDescriptor);
            commandDescriptor.Arguments.AddRange(GetArguments(commandDescriptor));
            commandDescriptor.SubCommands.AddRange(GetSubCommands(commandDescriptor));
            commandDescriptor.Options.AddRange(GetOptions(commandDescriptor));
            return commandDescriptor;
        }
    }
}
