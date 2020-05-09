using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Tests;
using System.Text;

namespace System.CommandLine.ReflectionModel.Tests
{
   internal  interface IHasTestData
    {
        IEnumerable<CommandTestData> CommandDataFromMethods { get; }
        CommandTestData CommandDataFromType { get; }
    }
}
