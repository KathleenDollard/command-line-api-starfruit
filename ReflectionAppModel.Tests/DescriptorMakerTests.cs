using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Tests;
using Xunit;
using System.CommandLine.ReflectionAppModel.Tests.ModelCodeForTests;
using System.Linq;
using FluentAssertions;
using System.CommandLine.GeneralAppModel.Descriptors;
using FluentAssertions.Execution;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace System.CommandLine.ReflectionAppModel.Tests
{

    public class DescriptorMakerTests
    {

        private readonly Strategy strategy;

        private const string Method = "Method";
        private const string Type = "Type";

        public DescriptorMakerTests()
        {
            strategy = new Strategy()
                            .SetReflectionRules()
                            .CandidateNamesToIgnore("CommandDataFromMethods", "CommandDataFromType");
        }

        [Theory]
        [InlineData(Method, typeof(SimpleTypeWithMethodNoAtributes), nameof(SimpleTypeWithMethodNoAtributes.DoSomething))]
        [InlineData(Type, typeof(SimpleTypeNoAttributes))]
        public void SimplestCommandDescriptorIsEmpty(string methodOrType, Type type, string name = null)
        {
            var actual = methodOrType == Method
                         ? ReflectionDescriptorMaker.RootCommandDescriptor(strategy, type.GetMethodsOnDeclaredType().First())
                         : ReflectionDescriptorMaker.RootCommandDescriptor(strategy, type);
            name ??= type.Name;
            var sourceType = methodOrType == Method
                         ? typeof(MethodInfo)
                         : typeof(Type);

            actual.Should().BeEmpty()
                           .And.HaveName(name+"X")
                           .And.HaveRawOfType(sourceType);
        }

        //public static IEnumerable<object[]> GetExamples()
        //{

        //    foreach (var assertion in SimpleTypeWithMethodNoAtributes.GetAssertions())
        //    {
        //        yield return new object[] { typeof(SimpleTypeWithMethodNoAtributes), assertion };
        //    }
        //}

        [Fact]
        public void EmptyCommandDescriptorFromMethodIsEmpty()
        {
            CommandDescriptor actual = GetFirstMethodDescriptor<SimpleTypeWithMethodNoAtributes>();

            // Jon: This approach to "Because" has some kind of weird output. Do you live with that or do something more here?
            //using var _ = new AssertionScope();
            actual.Should().BeOfType<CommandDescriptor>("Command");
            actual.Aliases.Should().BeEmpty("Aliases");
            actual.Arguments.Should().BeEmpty("Arguments");
            actual.Description.Should().BeNull("Description");
            actual.IsHidden.Should().BeFalse("IsHidden");
            actual.Name.Should().Be(nameof(SimpleTypeWithMethodNoAtributes.DoSomething) + "X", "Name");
            actual.Options.Should().BeEmpty();
            actual.ParentSymbolDescriptorBase.Should().NotBeNull();
            actual.Raw.Should().BeOfType<SimpleTypeWithMethodNoAtributes>("Raw");
            actual.SubCommands.Should().BeEmpty("SubCommands");
            //   actual.SymbolType.Should().Be(SymbolType.Command, "SymbolType");
            actual.TreatUnmatchedTokensAsErrors.Should().BeFalse("TreatUnmatchedTokensAsErrors");
        }


        [Fact]
        public void EmptyCommandDescriptorFromTypeIsEmpty()
        {
            var actual = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeof(SimpleTypeNoAttributes));

            using var _ = new AssertionScope();
            //actual.Should().BeOfType<CommandDescriptor>("Command");
            //actual.Aliases.Should().BeEmpty("Aliases");
            //actual.Arguments.Should().BeEmpty("Arguments");
            //actual.Description.Should().BeNull("Description");
            //actual.IsHidden.Should().BeFalse("IsHidden");
            //actual.Name.Should().Be(nameof(SimpleTypeNoAttributes), "Name");
            actual.Options.Should().BeEmpty("Options");
            //actual.ParentSymbolDescriptorBase.Should().NotBeNull("ParentSymbolDescriptorBase");
            //actual.Raw.Should().BeOfType<SimpleTypeWithMethodNoAtributes>("Raw");
            //actual.SubCommands.Should().BeEmpty("SubCommands");
            //actual.SymbolType.Should().Be(SymbolType.Command, "SymbolType");
            //actual.TreatUnmatchedTokensAsErrors.Should().BeFalse("TreatUnmatchedTokensAsErrors");

            // Utils.TestType<SimpleTypeNoAttributes>(strategy);
        }

        [Fact]
        public void CanGetCommandDescriptionFromMethodAttribute()
           => Utils.TestFirstMethodOnType<SimpleTypeWithMethodWithDescriptionAttribute>(new Strategy().SetGeneralRules());

        [Fact]
        public void CanGetCommandDescriptionFromTypeAttribute()
            => Utils.TestType<SimpleTypeWithDescriptionAttribute>(strategy);

        [Fact]
        public void CanGetArgumentFromNamedMethodParam()
           => Utils.TestFirstMethodOnType<MethodWithParameterNamedArgs>(strategy);


        [Fact]
        public void CanGetArgumentWithArityFromNamedMethodParam()
           => Utils.TestFirstMethodOnType<MethodWithParameterNamedArgsWithArity>(strategy);

        [Fact]
        public void CanGetArgumentFromNamedProperty()
            => Utils.TestType<TypeWithPropertyNamedArgs>(strategy);

        [Fact]
        public void CanGetArgumentWithArityFromNamedProperty()
            => Utils.TestType<TypeWithPropertyNamedArgsWithArity>(strategy);

        [Fact]
        public void CanGetArgumentWithDefaultFromNamedProperty()
            => Utils.TestType<TypeWithPropertyNamedArgsWithDefault>(strategy);

        [Fact]
        public void CanGetCommandWithAliasesFromNamedProperty()
            => Utils.TestType<TypeWithPropertyNamedArgsWithAliases>(strategy);

        [Fact]
        public void CanGetOptionFromMethodParam()
            => Utils.TestFirstMethodOnType<MethodWithParameterOption>(strategy);

        [Fact]
        public void CanGetOptionFromPropertyProperty()
            => Utils.TestType<TypeWithPropertyOption>(strategy);

        [Fact]
        public void CanGetSubCommandFromInheritedType()
        => Utils.TestType<TypeWithDerivedTypeCommands_A>(strategy);


        private CommandDescriptor GetFirstMethodDescriptor<T>()
        {
            var type = typeof(T);
            var method = type.GetMethodsOnDeclaredType().First();
            var actual = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, method);
            return actual;
        }
    }
}