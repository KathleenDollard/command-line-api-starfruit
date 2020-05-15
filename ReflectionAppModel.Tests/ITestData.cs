using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Tests;

namespace System.CommandLine.ReflectionAppModel.Tests
{
    internal  interface IHaveMethodTestData
    {
        IEnumerable<CommandTestData> CommandDataFromMethods { get; }
    }

    internal interface IHaveTypeTestData
    {
        CommandTestData CommandDataFromType { get; }
    }
}
