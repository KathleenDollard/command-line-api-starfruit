using System.CommandLine.GeneralAppModel.Descriptors;

namespace System.CommandLine.GeneralAppModel.Tests.Maker
{
    public abstract class MakerCommandTestData
    {

        protected const string DummyArgumentName = "DummyArgumentName";
        protected const string DummyCommandName = "DummyCommandName";
        protected const string DummyRaw = "Explicit Descriptor";
        protected const string KebabDummyCommandName = "dummy-command-name";

        public MakerCommandTestData(CommandDescriptor descriptor)
        {
            Descriptor = descriptor;
        }

        public CommandDescriptor Descriptor { get; }
        public abstract void Check(Command command);

    }
}
