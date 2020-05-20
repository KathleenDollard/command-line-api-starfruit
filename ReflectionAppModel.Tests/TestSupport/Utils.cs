using FluentAssertions;
using FluentAssertions.Equivalency;
using System.Collections.Generic;
using System.CommandLine.Collections;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.GeneralAppModel.Tests;
using System.CommandLine.ReflectionAppModel.Tests.ModelCodeForTests;
using System.Linq;

namespace System.CommandLine.ReflectionAppModel.Tests
{

    internal static class Utils
    {
        private static Func<EquivalencyAssertionOptions<SymbolDescriptorBase>, EquivalencyAssertionOptions<SymbolDescriptorBase>> symbolOptions;

        static Utils()
        {
            symbolOptions = o => o.Excluding(ctx => ctx.SelectedMemberPath.EndsWith("ParentSymbolDescriptorBase"));
        }

        public static CommandTestData FromFirstMethod<T>()
            where T : new()
            => (Activator.CreateInstance<T>() as IHaveMethodTestData)
                    .CommandDataFromMethods
                    .First();

        public static IEnumerable<CommandTestData> FromAllMethod<T>()
            where T : new()
            => (Activator.CreateInstance<T>() as IHaveMethodTestData)
                    .CommandDataFromMethods;

        public static CommandTestData FromMethod<T>(string methodName)
            where T : new()
            => (Activator.CreateInstance<T>() as IHaveMethodTestData)
                    .CommandDataFromMethods
                    .Where(x => x.Name == methodName)
                    .FirstOrDefault();

        internal static CommandTestData FromType<T>()
           where T : IHaveTypeTestData, new()
           => (Activator.CreateInstance<T>() as IHaveTypeTestData)
                    .CommandDataFromType;

        internal static void TestType<T>(this Strategy strategy)
            where T : IHaveTypeTestData, new()
        {
            var type = typeof(T);

            var actual = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, type);
            var expected = Utils.FromType<T>().CreateDescriptor();

            //actual.Should().BeEquivalentTo(expected, symbolOptions);
            //WhyDoWeNeedTheseExtraChecks(actual, expected);
        }

        internal static void TestFirstMethodOnType<T>(this Strategy strategy)
            where T : IHaveMethodTestData, new()
        {

            var type = typeof(T);
            var method = type.GetMethodsOnDeclaredType().First();

            var actual = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, method);
            var expected = Utils.FromFirstMethod<T>().CreateDescriptor();

            //actual.Should().BeEquivalentTo(expected, symbolOptions);
            //WhyDoWeNeedTheseExtraChecks(actual, expected);

        }

        private static void WhyDoWeNeedTheseExtraChecks(CommandDescriptor actual, CommandDescriptor expected)
        {
            actual.Options.Should().BeEquivalentTo(expected.Options, symbolOptions);
            var actualOptions = actual.Options.ToArray();
            var expectedOptions = expected.Options.ToArray();
            for (int i = 0; i < actualOptions.Length; i++)
            {
                actualOptions[i].Should().BeEquivalentTo(expectedOptions[i], symbolOptions);
            }
            actual.Arguments.Should().BeEquivalentTo(expected.Arguments, symbolOptions);
            var actualArgs = actual.Arguments.ToArray();
            var expectedArgs = expected.Arguments.ToArray();
            for (int i = 0; i < actualArgs.Length; i++)
            {
                actualArgs[i].Arity.Should().BeEquivalentTo(expectedArgs[i].Arity);
            }
            actual.SubCommands.Should().BeEquivalentTo(expected.SubCommands, symbolOptions);
        }

        public static IEnumerable<Command> SubCommands(this Command command)
        {
            return command.Children.OfType<Command>();
        }

    }
}

