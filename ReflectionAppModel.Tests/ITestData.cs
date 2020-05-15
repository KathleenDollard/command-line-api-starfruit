using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Tests;

namespace System.CommandLine.ReflectionAppModel.Tests
{
    internal  interface IHasTestData
    {
        IEnumerable<CommandTestData> CommandDataFromMethods { get; }
        CommandTestData CommandDataFromType { get; }
    }
}
