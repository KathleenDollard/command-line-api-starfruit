using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.GeneralAppModel.Tests;
using System.ComponentModel;
using System.Linq.Expressions;
using FluentAssertions;

namespace System.CommandLine.ReflectionAppModel.Tests.ModelCodeForTests
{
    public class SimpleTypeWithMethodNoAtributes //: IHaveMethodTestData, IHaveTypeTestData
    {
        public void DoSomething() { }

        //public static IEnumerable<Action<CommandDescriptor>> GetAssertions()
        //{
        //   yield return x=> x.Should().BeOfType<CommandDescriptor>("Command");
        //   yield return x=> x.Aliases.Should().BeEmpty("Aliases");
        //   yield return x=> x.Arguments.Should().BeEmpty("Arguments");
        //   yield return x=> x.Description.Should().BeNull("Description");
        //   yield return x=> x.IsHidden.Should().BeFalse("IsHidden");
        //   yield return x=> x.Name.Should().Be(nameof(SimpleTypeWithMethodNoAtributes.DoSomething) + "X", "Name");
        //   yield return x=> x.Options.Should().BeEmpty();
        //   yield return x=> x.ParentSymbolDescriptorBase.Should().NotBeNull();
        //   yield return x=> x.Raw.Should().BeOfType<SimpleTypeWithMethodNoAtributes>("Raw");
        //   yield return x=> x.SubCommands.Should().BeEmpty("SubCommands");
        //  // yield return x=> x.SymbolType.Should().Be(SymbolType.Command, "SymbolType");
        //   yield return x=> x.TreatUnmatchedTokensAsErrors.Should().BeFalse("TreatUnmatchedTokensAsErrors");

        //}

        //public IEnumerable<CommandTestData> CommandDataFromMethods
        //=> new List<CommandTestData>
        //{
        //    new CommandTestData()
        //    {
        //        Name = nameof(DoSomething),
        //        Raw = ReflectionSupport.GetMethodInfo<SimpleTypeWithMethodNoAtributes>(nameof(DoSomething)),
        //        IsHidden = false

        //    }
        //};

        //public CommandTestData CommandDataFromType
        //    => new CommandTestData()
        //    {
        //        Name = nameof(SimpleTypeWithMethodNoAtributes),
        //        Raw = typeof(SimpleTypeWithMethodNoAtributes),
        //        IsHidden = false
        //    };
    }

    public class SimpleTypeNoAttributes //: IHaveTypeTestData
    {
        //public CommandTestData CommandDataFromType
        //    => new CommandTestData()
        //    {
        //        Name = nameof(SimpleTypeNoAttributes),
        //        Raw = typeof(SimpleTypeNoAttributes),
        //        IsHidden = false
        //    };
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
                    Raw = ReflectionSupport.GetMethodInfo<SimpleTypeWithMethodWithDescriptionAttribute>(nameof(DoSomething)),
                    IsHidden = false
               }
            };

        public CommandTestData CommandDataFromType
            => new CommandTestData()
            {
                Name = nameof(SimpleTypeWithMethodWithDescriptionAttribute),
                Raw = typeof(SimpleTypeWithMethodWithDescriptionAttribute),
                IsHidden = false
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
                Raw = typeof(SimpleTypeWithDescriptionAttribute),
                IsHidden = false
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
                    IsHidden = false,
                    Arguments = new List<ArgumentTestData>
                    {
                        new ArgumentTestData
                        {
                            Raw = ReflectionSupport.GetParameterInfo<MethodWithParameterNamedArgs>(nameof(DoSomething), "stringParamArg"),
                            Name = "stringParam",
                            ArgumentType = typeof(string),
                            IsHidden = false
                        }
                    }
                }
            };

        public CommandTestData CommandDataFromType
            => new CommandTestData()
            {
                Name = nameof(MethodWithParameterNamedArgs),
                Raw = typeof(MethodWithParameterNamedArgs),
                IsHidden = false
            };
    }


    public class MethodWithParameterNamedArgsWithArity : IHaveMethodTestData, IHaveTypeTestData
    {
        public void DoSomething([Arity(MinimumCount = 1, MaximumCount = 3)] string stringParamArg) { }

        public IEnumerable<CommandTestData> CommandDataFromMethods
            => new List<CommandTestData>
            {
                new CommandTestData()
                {
                    Raw = ReflectionSupport.GetMethodInfo<MethodWithParameterNamedArgsWithArity>(nameof(DoSomething)),
                    Name = nameof(DoSomething),
                    IsHidden = false,
                    Arguments = new List<ArgumentTestData>
                    {
                        new ArgumentTestData
                        {
                            Raw = ReflectionSupport.GetParameterInfo<MethodWithParameterNamedArgsWithArity>(nameof(DoSomething), "stringParamArg"),
                            Name = "stringParam",
                            HasArity  = true,
                            MinArityValues = 1,
                            MaxArityValues = 3,
                            ArgumentType = typeof(string),
                            IsHidden = false
                        }
                    }
                }
            };

        public CommandTestData CommandDataFromType
            => new CommandTestData()
            {
                Name = nameof(MethodWithParameterNamedArgs),
                Raw = typeof(MethodWithParameterNamedArgs),
                IsHidden = false
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
                IsHidden = false,
                Arguments = new List<ArgumentTestData>
                { new ArgumentTestData
                    {
                       Name = nameof(StringPropertyArg)[..^3],
                       Raw = ReflectionSupport.GetPropertyInfo<TypeWithPropertyNamedArgs>(nameof(StringPropertyArg)),
                       ArgumentType = typeof(string),
                       IsHidden = false
                    }
                }
            };
    }

    public class TypeWithPropertyNamedArgsWithArity : IHaveTypeTestData
    {
        [Arity(MinimumCount = 0, MaximumCount = 2)]
        public string StringPropertyArg { get; set; }

        public CommandTestData CommandDataFromType
            => new CommandTestData()
            {
                Name = nameof(TypeWithPropertyNamedArgsWithArity),
                Raw = typeof(TypeWithPropertyNamedArgsWithArity),
                IsHidden = false,
                Arguments = new List<ArgumentTestData>
                { new ArgumentTestData
                    {
                       Name = nameof(StringPropertyArg)[..^3],
                       Raw = ReflectionSupport.GetPropertyInfo<TypeWithPropertyNamedArgsWithArity>(nameof(StringPropertyArg)),
                       HasArity  = true,
                       MinArityValues = 0,
                       MaxArityValues = 2,
                       ArgumentType = typeof(string),
                       IsHidden = false
                    }
                }
            };
    }

    public class TypeWithPropertyNamedArgsWithDefault : IHaveTypeTestData
    {
        [Default("xyz")]
        public string StringPropertyArg { get; set; }

        public CommandTestData CommandDataFromType
            => new CommandTestData()
            {
                Name = nameof(TypeWithPropertyNamedArgsWithDefault),
                Raw = typeof(TypeWithPropertyNamedArgsWithDefault),
                IsHidden = false,
                Arguments = new List<ArgumentTestData>
                { new ArgumentTestData
                    {
                       Name = nameof(StringPropertyArg)[..^3],
                       Raw = ReflectionSupport.GetPropertyInfo<TypeWithPropertyNamedArgsWithDefault>(nameof(StringPropertyArg)),
                       HasDefault=true,
                       DefaultValue = "xyz",
                       ArgumentType = typeof(string),
                       IsHidden = false
                    }
                }
            };
    }

    [Aliases("x", "y", "z")]
    public class TypeWithPropertyNamedArgsWithAliases : IHaveTypeTestData
    {
        public string StringPropertyArg { get; set; }

        public CommandTestData CommandDataFromType
            => new CommandTestData()
            {
                Name = nameof(TypeWithPropertyNamedArgsWithAliases),
                Raw = typeof(TypeWithPropertyNamedArgsWithAliases),
                IsHidden = false,
                Aliases = new string[] { "x", "y", "z" },
                Arguments = new List<ArgumentTestData>
                { new ArgumentTestData
                    {
                       Name = nameof(StringPropertyArg)[..^3],
                       Raw = ReflectionSupport.GetPropertyInfo<TypeWithPropertyNamedArgsWithAliases>(nameof(StringPropertyArg)),
                       HasDefault=true,
                       DefaultValue = "xyz",
                       ArgumentType = typeof(string),
                       IsHidden = false
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
                    IsHidden = false,
                    Options = new List<OptionTestData>
                    {
                        new OptionTestData
                        {
                            Raw = ReflectionSupport.GetParameterInfo<MethodWithParameterOption>(nameof(DoSomething), "stringParam"),
                            Name = "stringParam",
                            IsHidden = false,
                            Arguments = new List<ArgumentTestData>
                            {
                                new ArgumentTestData
                                {
                                    Raw =  ReflectionSupport.GetParameterInfo<MethodWithParameterOption>(nameof(DoSomething), "stringParam"),
                                    Name = "stringParam",
                                    ArgumentType = typeof(string),
                                    IsHidden = false
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
                Raw = typeof(MethodWithParameterOption),
                IsHidden = false
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
                IsHidden = false,
                Options = new List<OptionTestData>
                { new OptionTestData
                    {
                        Name = nameof(StringProperty),
                        Raw = ReflectionSupport.GetPropertyInfo<TypeWithPropertyOption>(nameof(StringProperty)),
                        IsHidden = false,
                        Arguments = new List<ArgumentTestData >
                        { new ArgumentTestData
                            {
                               Name = nameof(StringProperty),
                               Raw = ReflectionSupport.GetPropertyInfo<TypeWithPropertyOption>(nameof(StringProperty)),
                               ArgumentType = typeof(string),
                               IsHidden = false
                            }
                        }
                    }
                }
            };
    }

    public class TypeWithDerivedTypeCommands_B : TypeWithDerivedTypeCommands_A, IHaveTypeTestData
    {
        public new CommandTestData CommandDataFromType
            => new CommandTestData()
            {
                Name = nameof(TypeWithDerivedTypeCommands_B),
                Raw = typeof(TypeWithDerivedTypeCommands_B),
            };
    }
    public class TypeWithDerivedTypeCommands_C : TypeWithDerivedTypeCommands_A, IHaveTypeTestData
    {
        public new CommandTestData CommandDataFromType
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

    [TreatUnmatchedTokensAsErrors(false)]
    public class TypeWithTreatUnmatchedTokenAsErrors : IHaveTypeTestData
    {
        public CommandTestData CommandDataFromType
            => new CommandTestData()
            {
                Name = nameof(TypeWithDerivedTypeCommands_A),
                Raw = typeof(TypeWithDerivedTypeCommands_A),
                IsHidden = false,
                TreatUnmatchedTokensAsErrors = false
            };
    }

    [Hidden]
    public class TypeWithThingsSetToHidden : IHaveTypeTestData
    {
        [Hidden]
        public string StringProperty { get; set; }

        [Hidden]
        public string StringArg { get; set; }

        public CommandTestData CommandDataFromType
            => new CommandTestData()
            {
                Name = nameof(TypeWithDerivedTypeCommands_A),
                Raw = typeof(TypeWithDerivedTypeCommands_A),
                IsHidden = true,
                Options = new List<OptionTestData>
                    { new OptionTestData
                            {
                               Name = nameof(StringProperty),
                               Raw = ReflectionSupport.GetPropertyInfo<TypeWithPropertyOption>(nameof(StringProperty)),
                               IsHidden = true,
                            }
                    },
                Arguments = new List<ArgumentTestData>
                    { new ArgumentTestData
                            {
                               Name = nameof(StringProperty),
                               Raw = ReflectionSupport.GetPropertyInfo<TypeWithPropertyOption>(nameof(StringProperty)),
                               IsHidden = true,
                            }
                    }
            };
    }
}

