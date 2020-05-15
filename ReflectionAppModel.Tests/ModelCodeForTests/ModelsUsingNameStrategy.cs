using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Tests;
using System.ComponentModel;

namespace System.CommandLine.ReflectionAppModel.Tests.ModelCodeForTests
{
    public class SimpleTypeWithMethodNoAtributes : IHasTestData
    {
        public void DoSomething() { }

        IEnumerable<CommandTestData> IHasTestData.CommandDataFromMethods
            => new List<CommandTestData>
            {
                new CommandTestData()
                {
                    Name = nameof(DoSomething),
                    Raw = ReflectionSupport.GetMethodInfo<SimpleTypeWithMethodNoAtributes>(nameof(DoSomething))
                }
            };

        CommandTestData IHasTestData.CommandDataFromType
            => new CommandTestData()
            {
                Name = nameof(SimpleTypeWithMethodNoAtributes),
                Raw = typeof(SimpleTypeWithMethodNoAtributes)
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
                Name = nameof(SimpleTypeNoAttributes),
                Raw = typeof(SimpleTypeNoAttributes)
            };
    }

    public class SimpleTypeWithMethodWithDescriptionAttribute : IHasTestData
    {
        [Description("This is a great description 1")]
        public void DoSomething() { }

        IEnumerable<CommandTestData> IHasTestData.CommandDataFromMethods
            => new List<CommandTestData>
            {
                new CommandTestData()
                {
                    Name = nameof(DoSomething),
                    Description = "This is a great description 1",
                    Raw = ReflectionSupport.GetMethodInfo<SimpleTypeWithMethodWithDescriptionAttribute>(nameof(DoSomething))
               }
            };

        CommandTestData IHasTestData.CommandDataFromType
            => new CommandTestData()
            {
                Name = nameof(SimpleTypeWithMethodWithDescriptionAttribute),
                Raw = typeof(SimpleTypeWithMethodWithDescriptionAttribute)
            };
    }

    [Description("This is a great description 2")]
    public class SimpleTypeWithDescriptionAttribute : IHasTestData
    {
        IEnumerable<CommandTestData> IHasTestData.CommandDataFromMethods
            => new List<CommandTestData>
            { };

        CommandTestData IHasTestData.CommandDataFromType
            => new CommandTestData()
            {
                Name = nameof(SimpleTypeWithDescriptionAttribute),
                Description = "This is a great description 2",
                Raw = typeof(SimpleTypeWithDescriptionAttribute)
            };
    }

    public class MethodWithParameterNamedArgs : IHasTestData
    {
        public void DoSomething(string stringParamArg) { }

        IEnumerable<CommandTestData> IHasTestData.CommandDataFromMethods
            => new List<CommandTestData>
            {
                new CommandTestData()
                {
                    Raw = ReflectionSupport.GetMethodInfo<MethodWithParameterNamedArgs>(nameof(DoSomething)),
                    Name = nameof(DoSomething),
                    Arguments = new List<ArgumentTestData>
                    {
                        new ArgumentTestData
                        {
                            Raw = ReflectionSupport.GetParameterInfo<MethodWithParameterNamedArgs>(nameof(DoSomething), "stringParamArg"),
                            Name = "stringParam",
                            ArgumentType = typeof(string)
                        }
                    }
                }
            };

        CommandTestData IHasTestData.CommandDataFromType
            => new CommandTestData()
            {
                Name = nameof(MethodWithParameterNamedArgs),
                Raw = typeof(MethodWithParameterNamedArgs)
            };
    }

    public class TypeWithPropertyNamedArgs : IHasTestData
    {
        public string StringPropertyArg { get; set; }

        IEnumerable<CommandTestData> IHasTestData.CommandDataFromMethods
            => new List<CommandTestData>
            { };

        CommandTestData IHasTestData.CommandDataFromType
            => new CommandTestData()
            {
                Name = nameof(TypeWithPropertyNamedArgs),
                Raw = typeof(TypeWithPropertyNamedArgs),
                Arguments = new List<ArgumentTestData>
                { new ArgumentTestData
                    {
                       Name = nameof(StringPropertyArg)[..^3],
                       Raw = ReflectionSupport.GetPropertyInfo<TypeWithPropertyNamedArgs>(nameof(StringPropertyArg)),
                       ArgumentType = typeof(string)
                    }
                }
            };
    }

    public class MethodWithParameterOption : IHasTestData
    {
        public void DoSomething(string stringParam) { }

        IEnumerable<CommandTestData> IHasTestData.CommandDataFromMethods
            => new List<CommandTestData>
            {
                new CommandTestData()
                {
                    Raw = ReflectionSupport.GetMethodInfo<MethodWithParameterOption>(nameof(DoSomething)),
                    Name = nameof(DoSomething),
                    Options = new List<OptionTestData>
                    {
                        new OptionTestData
                        {
                            Raw = ReflectionSupport.GetParameterInfo<MethodWithParameterOption>(nameof(DoSomething), "stringParam"),
                            Name = "stringParam",
                            Arguments = new List<ArgumentTestData>
                            {
                                new ArgumentTestData
                                {
                                    Raw =  ReflectionSupport.GetParameterInfo<MethodWithParameterOption>(nameof(DoSomething), "stringParam"),
                                    Name = "stringParam",
                                    ArgumentType = typeof(string),
                                }
                            }
                        }
                    }
                }
            };

        CommandTestData IHasTestData.CommandDataFromType
            => new CommandTestData()
            {
                Name = nameof(MethodWithParameterOption),
                Raw = typeof(MethodWithParameterOption)
            };
    }

    public class TypeWithPropertyOption : IHasTestData
    {
        public string StringProperty { get; set; }

        IEnumerable<CommandTestData> IHasTestData.CommandDataFromMethods
            => new List<CommandTestData>
            { };

        CommandTestData IHasTestData.CommandDataFromType
            => new CommandTestData()
            {
                Name = nameof(TypeWithPropertyOption),
                Raw = typeof(TypeWithPropertyOption),
                Options = new List<OptionTestData>
                { new OptionTestData
                    {
                       Name = nameof(StringProperty),
                       Raw = ReflectionSupport.GetPropertyInfo<TypeWithPropertyOption>(nameof(StringProperty)),
                    }
                }
            };
    }


}
