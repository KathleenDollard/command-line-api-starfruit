using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Tests;

namespace System.CommandLine.ReflectionModel.Tests
{
    internal  interface IHasTestData
    {
        IEnumerable<CommandTestData> CommandDataFromMethods { get; }
        CommandTestData CommandDataFromType { get; }
    }
}
