using FluentAssertions;
using FluentAssertions.Equivalency;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.GeneralAppModel.Tests;
using System.CommandLine.ReflectionAppModel.Tests.ModelCodeForTests;
using System.Linq;

namespace System.CommandLine.ReflectionAppModel.Tests
{

    internal static class Utils
    {
        private static Func<EquivalencyAssertionOptions<SymbolDescriptor>, EquivalencyAssertionOptions<SymbolDescriptor>> symbolOptions;

        static Utils()
        {
            symbolOptions = o => o.Excluding(ctx => ctx.SelectedMemberPath.EndsWith("ParentSymbolDescriptorBase"));
        }

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

    }
}

