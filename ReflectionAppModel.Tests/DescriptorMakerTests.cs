using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Tests;
using Xunit;
using System.CommandLine.ReflectionAppModel.Tests.ModelCodeForTests;
using System.Linq;
using FluentAssertions;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.GeneralAppModel.Tests.ModelCodeForTests;

namespace System.CommandLine.ReflectionAppModel.Tests
{

    public class DescriptorTests
    {

        private readonly Strategy strategy;

        private const string Method = "Method";
        private const string Type = "Type";

        public DescriptorTests()
        {
            strategy = new Strategy()
                            .SetReflectionRules()
                            .CandidateNamesToIgnore("CommandDataFromMethods", "CommandDataFromType");
        }

        [Theory]
        [ClassData(typeof(EmptyCommand))]
        [ClassData(typeof(CommandWithDescription))]
        [ClassData(typeof(CommandWithSpecifiedName))]
        [ClassData(typeof(CommandWithTreatUnmatchedTokensAsErrors))]
        [ClassData(typeof(CommandWithIsHidden))]
        [ClassData(typeof(CommandWithOneArg))]
        [ClassData(typeof(CommandWithOneOption))]
        public void CommandViaClassData(string id, ClassData.For forSource, ClassData.CommandData commandData)
        {
            RunCommandTests(id, forSource, commandData);
        }

        [Theory]
        [ClassData(typeof(CommandWithOneSubCommand), Skip ="Need ComplexType parameter evaluation")]
        public void CommandViaClassDataBrokenTests(string id, ClassData.For forSource, ClassData.CommandData commandData)
        {
            RunCommandTests(id, forSource, commandData);
        }

        /// <summary>
        /// The Visual Studio test runner groups xUnit ClassData tests as one test. This is
        /// a pain when doing new tests. So I created the ability to group tests for no reason other
        /// than to make VS Test Runner use easier. Yeah, I know.
        /// </summary>
        /// <param name="forSource"></param>
        /// <param name="commandData"></param>
        private void RunCommandTests(string id, ClassData.For forSource, ClassData.CommandData commandData)
        {
            var actual = forSource switch
            {
                ClassData.ForMethod _ => ReflectionDescriptorMaker.RootCommandDescriptor(strategy, forSource.Type.GetMethodsOnDeclaredType().First()),
                ClassData.ForType t => ReflectionDescriptorMaker.RootCommandDescriptor(strategy, t.Type),
                _ => throw new InvalidOperationException("Unexpected data source for test")
            };

            var altName = forSource switch
            {
                ClassData.ForMethod m => m.MethodName,
                ClassData.ForType t => t.Type.Name,
                _ => "<Invalid Name>"
            };

            actual.Should().BeSameAs(commandData.WithAltName(altName).WithId(id));
        }

        //[Theory]
        //[InlineData(Method, typeof(SimpleTypeWithMethodNoAtributes), nameof(SimpleTypeWithMethodNoAtributes.DoSomething))]
        //[InlineData(Type, typeof(SimpleTypeNoAttributes))]
        //public void SimplestCommandDescriptorIsEmpty(string methodOrType, Type type, string name = null)
        //{
        //    var actual = methodOrType == Method
        //                 ? ReflectionDescriptorMaker.RootCommandDescriptor(strategy, type.GetMethodsOnDeclaredType().First())
        //                 : ReflectionDescriptorMaker.RootCommandDescriptor(strategy, type);
        //    name ??= type.Name;
        //    var sourceType = methodOrType == Method
        //                 ? typeof(MethodInfo)
        //                 : typeof(Type);

        //    actual.Should().BeEmpty()
        //                   .And.HaveName(name)
        //                   .And.HaveRawOfType(sourceType);
        //}



        //[Fact]
        //public void EmptyCommandDescriptorFromMethodIsEmpty()
        //{
        //    CommandDescriptor actual = GetFirstMethodDescriptor<SimpleTypeWithMethodNoAtributes>();

        //    // Jon: This approach to "Because" has some kind of weird output. Do you live with that or do something more here?
        //    //using var _ = new AssertionScope();
        //    actual.Should().BeOfType<CommandDescriptor>("Command");
        //    actual.Aliases.Should().BeEmpty("Aliases");
        //    actual.Arguments.Should().BeEmpty("Arguments");
        //    actual.Description.Should().BeNull("Description");
        //    actual.IsHidden.Should().BeFalse("IsHidden");
        //    actual.Name.Should().Be(nameof(SimpleTypeWithMethodNoAtributes.DoSomething) + "X", "Name");
        //    actual.Options.Should().BeEmpty();
        //    actual.ParentSymbolDescriptorBase.Should().NotBeNull();
        //    actual.Raw.Should().BeOfType<SimpleTypeWithMethodNoAtributes>("Raw");
        //    actual.SubCommands.Should().BeEmpty("SubCommands");
        //    //   actual.SymbolType.Should().Be(SymbolType.Command, "SymbolType");
        //    actual.TreatUnmatchedTokensAsErrors.Should().BeFalse("TreatUnmatchedTokensAsErrors");
        //}

        //[Fact]
        //public void CanGetCommandDescriptionFromMethodAttribute()
        //   => Utils.TestFirstMethodOnType<SimpleTypeWithMethodWithDescriptionAttribute>(new Strategy().SetGeneralRules());

        //[Fact]
        //public void CanGetArgumentFromNamedMethodParam()
        //   => Utils.TestFirstMethodOnType<MethodWithParameterNamedArgs>(strategy);

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