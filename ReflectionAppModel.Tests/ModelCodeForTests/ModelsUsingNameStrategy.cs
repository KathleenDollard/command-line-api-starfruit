using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.GeneralAppModel.Tests;
using System.CommandLine.GeneralAppModel.Tests.ModelCodeForTests;
using System.ComponentModel;
using System.Linq.Expressions;
using FluentAssertions;

namespace System.CommandLine.ReflectionAppModel.Tests.ModelCodeForTests
{

    public class EmptyCommand : ClassData
    {
        public EmptyCommand()
            : base(
                  new CommandData
                  { },
                  new For[]
                  {
                      new ForType(typeof(FromEmptyType)),
                      new ForMethod(typeof(FromMethodNoParameters),nameof(FromMethodNoParameters.DoSomething ))
                  })
        { }

        public class FromMethodNoParameters
        {
            public void DoSomething() { }
        }

        public class FromEmptyType { }
    }

    public class CommandWithDescription : ClassData
    {
        const string desc = "This is a great description";
        public CommandWithDescription()
            : base(
                  new CommandData
                  {
                      Description = desc
                  },
                  new For[]
                  {
                      new ForType(typeof(FromTypeWithDescriptionAttribute)),
                      new ForMethod(typeof(FromMethodWithDescriptionAttribute),nameof(FromMethodWithDescriptionAttribute.DoSomething )),
                      new ForType(typeof(FromTypeWithCommandAttribute)),
                      new ForMethod(typeof(FromMethodWithCommandAttribute),nameof(FromMethodWithCommandAttribute.DoSomething ))
                 })
        { }

        public class FromMethodWithDescriptionAttribute
        {
            [Description(desc)]
            public void DoSomething() { }
        }

        [Description(desc)]
        public class FromTypeWithDescriptionAttribute { }

        public class FromMethodWithCommandAttribute
        {
            [Command(Description = desc)]
            public void DoSomething() { }
        }

        [Command(Description = desc)]
        public class FromTypeWithCommandAttribute { }
    }

    public class CommandWithSpecifiedName : ClassData
    {
        const string name = "Fred";
        public CommandWithSpecifiedName()
            : base(
                  new CommandData
                  {
                      Name = name
                  },
                  new For[]
                  {
                      new ForType(typeof(FromTypeWithCommandAttribute)),
                      new ForMethod(typeof(FromMethodWithCommandAttribute),nameof(FromMethodWithCommandAttribute.DoSomething ))
                  })
        { }

        public class FromMethodWithCommandAttribute
        {
            [Command(Name = name)]
            public void DoSomething() { }
        }

        [Command(Name = name)]
        public class FromTypeWithCommandAttribute { }
    }

    public class CommandWithTreatUnmatchedTokensAsErrors : ClassData
    {
        public CommandWithTreatUnmatchedTokensAsErrors()
            : base(
                  new CommandData
                  {
                      TreatUnmatchedTokensAsErrors = true
                  },
                  new For[]
                  {
                      new ForType(typeof(FromTypeWithAttribute)),
                      new ForMethod(typeof(FromMethodWithAttribute),nameof(FromMethodWithAttribute.DoSomething )),
                      new ForType(typeof(FromTypeWithAttributeValue)),
                      new ForMethod(typeof(FromMethodWithAttributeValue),nameof(FromMethodWithAttributeValue.DoSomething )),
                      new ForType(typeof(FromTypeWithCommandAttribute)),
                      new ForMethod(typeof(FromMethodWithCommandAttribute),nameof(FromMethodWithCommandAttribute.DoSomething ))
                  })
        { }

        public class FromMethodWithAttribute
        {
            [TreatUnmatchedTokensAsErrors]
            public void DoSomething() { }
        }

        [TreatUnmatchedTokensAsErrors]
        public class FromTypeWithAttribute { }

        public class FromMethodWithAttributeValue
        {
            [TreatUnmatchedTokensAsErrors(Value = true)]
            public void DoSomething() { }
        }

        [TreatUnmatchedTokensAsErrors(Value = true)]
        public class FromTypeWithAttributeValue { }

        public class FromMethodWithCommandAttribute
        {
            [Command(TreatUnmatchedTokensAsErrors=true)]
            public void DoSomething() { }
        }

        [Command(TreatUnmatchedTokensAsErrors = true)]
        public class FromTypeWithCommandAttribute { }
    }

    [TreatUnmatchedTokensAsErrors(Value = false)]
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

}

