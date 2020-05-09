using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Tests;
using System.Text;
using Xunit.Abstractions;

namespace System.CommandLine.ReflectionModel.Tests.ModelCodeForTests
{
    public class SimpleTypeWithMethodNoAtributes : IHasTestData
    {
        public void DoSomething(string param1) { }

        IEnumerable<CommandTestData> IHasTestData.CommandDataFromMethods
            => new List<CommandTestData>
            {
                new CommandTestData()
                {
                    Name = "DoSomething"
                }
            };

        CommandTestData IHasTestData.CommandDataFromType
            => new CommandTestData()
            {
                Name = "SimplestCommandFromMethod"
            };
    }

    public class SimpleTypeNoAttributes : IHasTestData
    {
        IEnumerable<CommandTestData> IHasTestData.CommandDataFromMethods
            => new List<CommandTestData>
            { };

        CommandTestData IHasTestData.CommandDataFromType
            => new CommandTestData()
            {
                Name = "SimpleTypeNoAttributes"
            };
    }


}
