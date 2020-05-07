﻿using System.CommandLine.GeneralAppModel.Descriptors;

namespace System.CommandLine.GeneralAppModel
{
    public interface  IAppModel
    {
        CommandDescriptor GetCommandDescriptor(object commandData);
        //IEnumerable<CommandDescriptor> GetSubCommands(object commandData);
        //IEnumerable<ArgumentDescriptor> GetArguments(object commandData);
        //IEnumerable<OptionDescriptor> GetOptions(object commandData);
    }
}
