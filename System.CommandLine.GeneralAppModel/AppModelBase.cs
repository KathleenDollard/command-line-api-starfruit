using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;

namespace System.CommandLine.GeneralAppModel
{
    // This expects an instance per command to support caching
    // Currently depth first, didn't think of scnearios where it would matter
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

        protected abstract IEnumerable<ArgumentDescriptor> GetArguments();
        protected abstract IEnumerable<CommandDescriptor> GetSubCommands();
        protected abstract IEnumerable<OptionDescriptor> GetOptions();
        protected abstract CommandDescriptor GetCommand();

        private CommandDescriptor CommandFrom()
        {
            var command = GetCommand();
            command.Arguments.AddRange(GetArguments());
            command.Options.AddRange(GetOptions());
            command.SubCommands.AddRange(GetSubCommands());
            return command;
        }
    }
}
