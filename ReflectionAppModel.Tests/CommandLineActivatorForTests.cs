using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Descriptors;

namespace System.CommandLine.ReflectionAppModel.Tests
{
    /// <summary>
    /// This test allows creation of a command descriptor that can be retained for 
    /// test assertions. There is no other known purpose. 
    /// </summary>
    public partial class ActivatorTests
    {
        public class CommandLineActivatorForTests : CommandLineActivatorBase
        {
            private readonly CommandDescriptor commandDescriptor;

            public CommandLineActivatorForTests(CommandDescriptor commandDescriptor)
            {
                this.commandDescriptor = commandDescriptor;
            }

            public override CommandDescriptor GetCommandDescriptor<TRoot>(Strategy? strategy = null)
            {
                return commandDescriptor;
            }
        }
    }
}
