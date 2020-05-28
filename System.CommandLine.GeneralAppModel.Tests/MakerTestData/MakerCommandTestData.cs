﻿using System.CommandLine.GeneralAppModel.Descriptors;

namespace System.CommandLine.GeneralAppModel.Tests.Maker
{
    public abstract class MakerCommandTestData
    {

        protected const string DummyArgumentName = "DummyArgumentName";
        protected const string DummyCommandName = "DummyCommandName";
        public MakerCommandTestData(CommandDescriptor descriptor)
        {
            Descriptor = descriptor;
        }

        public CommandDescriptor Descriptor { get; }
        public abstract void Check(Command command);

    }
}