using FluentAssertions;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Tests;
using Xunit;
using System.CommandLine.ReflectionModel.Tests;
using ReflectionAppModel.Tests.TestSupport;
using System.Linq;
using System.Collections.Generic;
using System.CommandLine.ReflectionModel.Tests.ModelCodeForTests;

namespace System.CommandLine.ReflectionModel.Tests.Maker
{

    public class DescriptorMakerTests
    {
        private IEnumerable<Type> modelTypes;
        private readonly string modelNamespace = "ReflectionAppModel.Tests.Models";

        public DescriptorMakerTests()
        {
            modelTypes = GetType()
                .Assembly
                .GetTypes()
                .Where(t => t.Namespace == modelNamespace)
                .ToList();
        }

        [Fact]
        public void CanMakeSimplestCommandDescriptorFromMethodOnNamedModel() 
            => TestFirstMethodOnType<SimpleTypeWithMethodNoAtributes>(new Strategy().SetAllStandardRules());

        [Fact]
        public void CanMakeSimplestCommandDescriptorFromTypeOnNamedModel()
            => TestType<SimpleTypeNoAttributes>(new Strategy().SetAllStandardRules());

        private void TestType<T>(Strategy strategy)
            where T : IHasTestData, new()
        {
            var type = typeof(T);

            var actual = ReflectionAppModel.ReflectionAppModel.RootCommandDescriptor(strategy, type);
            var expected = ModelData.FromType<T>();

            actual.Should().BeEquivalentTo(expected);
        }

        private void TestFirstMethodOnType<T>(Strategy strategy )
            where T : IHasTestData, new()
        {
            var type = typeof(T);
            var method = type.GetMethodsOnDeclaredType().First();

            var actual = ReflectionAppModel.ReflectionAppModel.RootCommandDescriptor(strategy, method);
            var expected = ModelData.FromFirstMethod<T>();

            actual.Should().BeEquivalentTo(expected);
        }



    }
}