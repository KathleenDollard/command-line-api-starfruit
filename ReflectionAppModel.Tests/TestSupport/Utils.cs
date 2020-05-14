using FluentAssertions;
using FluentAssertions.Equivalency;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Tests;
using System.Linq;

namespace System.CommandLine.ReflectionModel.Tests
{

    internal static class Utils
    {
        private static Func<EquivalencyAssertionOptions<SymbolDescriptorBase>, EquivalencyAssertionOptions<SymbolDescriptorBase>> symbolOptions;

        static Utils()
        {
            symbolOptions = o => o.Excluding(x => x.ParentSymbolDescriptorBase);
        }

        public static CommandTestData FromFirstMethod<T>()
            where T : new()
            => (Activator.CreateInstance<T>() as IHasTestData)
                    .CommandDataFromMethods
                    .First();

        public static IEnumerable<CommandTestData> FromAllMethod<T>()
            where T : new()
            => (Activator.CreateInstance<T>() as IHasTestData)
                    .CommandDataFromMethods;

        public static CommandTestData FromMethod<T>(string methodName)
            where T : new()
            => (Activator.CreateInstance<T>() as IHasTestData)
                    .CommandDataFromMethods
                    .Where(x => x.Name == methodName)
                    .FirstOrDefault();

        internal static CommandTestData FromType<T>()
           where T : IHasTestData, new()
           => (Activator.CreateInstance<T>() as IHasTestData)
                    .CommandDataFromType;

        internal static void TestType<T>(this Strategy strategy)
        where T : IHasTestData, new()
        {
            var type = typeof(T);

            var actual = ReflectionAppModel.ReflectionAppModel.RootCommandDescriptor(strategy, type);
            var expected = Utils.FromType<T>().CreateDescriptor();

            actual.Should().BeEquivalentTo(expected, symbolOptions);
        }

        internal static void TestFirstMethodOnType<T>(this Strategy strategy)
            where T : IHasTestData, new()
        {

            var type = typeof(T);
            var method = type.GetMethodsOnDeclaredType().First();

            var actual = ReflectionAppModel.ReflectionAppModel.RootCommandDescriptor(strategy, method);
            var expected = Utils.FromFirstMethod<T>().CreateDescriptor();

            actual.Should().BeEquivalentTo(expected, symbolOptions);
        }
    }
}
