using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Tests;
using System.ComponentModel;

namespace System.CommandLine.ReflectionAppModel.Tests.ModelCodeForTests
{
    public class SimpleTypeWithMethodNoAtributes : IHaveMethodTestData, IHaveTypeTestData
    {
        public void DoSomething() { }

        public IEnumerable<CommandTestData> CommandDataFromMethods
                 => new List<CommandTestData>
                 {
                new CommandTestData()
                {
                    Name = nameof(DoSomething),
                    Raw = ReflectionSupport.GetMethodInfo<SimpleTypeWithMethodNoAtributes>(nameof(DoSomething))
                }
                 };

        public CommandTestData CommandDataFromType
            => new CommandTestData()
            {
                Name = nameof(SimpleTypeWithMethodNoAtributes),
                Raw = typeof(SimpleTypeWithMethodNoAtributes)
            };
    }

    public class SimpleTypeNoAttributes : IHaveTypeTestData
    {
        public CommandTestData CommandDataFromType
            => new CommandTestData()
            {
                Name = nameof(SimpleTypeNoAttributes),
                Raw = typeof(SimpleTypeNoAttributes)
            };
    }

    public class SimpleTypeWithMethodWithDescriptionAttribute : IHaveMethodTestData, IHaveTypeTestData
    {
        [Description("This is a great description 1")]
        public void DoSomething() { }

        public IEnumerable<CommandTestData> CommandDataFromMethods
            => new List<CommandTestData>
            {
                new CommandTestData()
                {
                    Name = nameof(DoSomething),
                    Description = "This is a great description 1",
                    Raw = ReflectionSupport.GetMethodInfo<SimpleTypeWithMethodWithDescriptionAttribute>(nameof(DoSomething))
               }
            };

        public CommandTestData CommandDataFromType
            => new CommandTestData()
            {
                Name = nameof(SimpleTypeWithMethodWithDescriptionAttribute),
                Raw = typeof(SimpleTypeWithMethodWithDescriptionAttribute)
            };
    }

    [Description("This is a great description 2")]
    public class SimpleTypeWithDescriptionAttribute : IHaveTypeTestData
    {
        public CommandTestData CommandDataFromType
            => new CommandTestData()
            {
                Name = nameof(SimpleTypeWithDescriptionAttribute),
                Description = "This is a great description 2",
                Raw = typeof(SimpleTypeWithDescriptionAttribute)
            };
    }

    public class MethodWithParameterNamedArgs : IHaveMethodTestData, IHaveTypeTestData
    {
        public void DoSomething(string stringParamArg) { }

        public IEnumerable<CommandTestData> CommandDataFromMethods
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

        public CommandTestData CommandDataFromType
            => new CommandTestData()
            {
                Name = nameof(MethodWithParameterNamedArgs),
                Raw = typeof(MethodWithParameterNamedArgs)
            };
    }

    public class TypeWithPropertyNamedArgs : IHaveTypeTestData
    {
        public string StringPropertyArg { get; set; }

        public CommandTestData CommandDataFromType
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

    public class MethodWithParameterOption : IHaveMethodTestData, IHaveTypeTestData
    {
        public void DoSomething(string stringParam) { }

        public IEnumerable<CommandTestData> CommandDataFromMethods
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

        public CommandTestData CommandDataFromType
            => new CommandTestData()
            {
                Name = nameof(MethodWithParameterOption),
                Raw = typeof(MethodWithParameterOption)
            };
    }

    public class TypeWithPropertyOption : IHaveTypeTestData
    {
        public string StringProperty { get; set; }

        public CommandTestData CommandDataFromType
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

    public class TypeWithDerivedTypeCommands_B : TypeWithDerivedTypeCommands_A, IHaveTypeTestData
    {
        public CommandTestData CommandDataFromType
            => new CommandTestData()
            {
                Name = nameof(TypeWithDerivedTypeCommands_B),
                Raw = typeof(TypeWithDerivedTypeCommands_B),
            };
    }
    public class TypeWithDerivedTypeCommands_C : TypeWithDerivedTypeCommands_A, IHaveTypeTestData
    {
        public CommandTestData CommandDataFromType
            => new CommandTestData()
            {
                Name = nameof(TypeWithDerivedTypeCommands_C),
                Raw = typeof(TypeWithDerivedTypeCommands_C),
            };
    }
    public class TypeWithDerivedTypeCommands_A : IHaveTypeTestData
    {
        public CommandTestData CommandDataFromType
            => new CommandTestData()
            {
                Name = nameof(TypeWithDerivedTypeCommands_A),
                Raw = typeof(TypeWithDerivedTypeCommands_A),
                SubCommands = new List<CommandTestData>
                {
                    (new TypeWithDerivedTypeCommands_B() as IHaveTypeTestData).CommandDataFromType,
                    (new TypeWithDerivedTypeCommands_C() as IHaveTypeTestData).CommandDataFromType
                }
            };
    }


}
