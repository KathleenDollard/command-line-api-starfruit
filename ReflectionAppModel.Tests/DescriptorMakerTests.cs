using System.CommandLine.GeneralAppModel;
using Xunit;
using System.CommandLine.ReflectionModel.Tests.ModelCodeForTests;
using FluentAssertions;
using FluentAssertions.Equivalency;

namespace System.CommandLine.ReflectionModel.Tests
{

    public class DescriptorMakerTests
    {

        private readonly Strategy strategy ;

        public DescriptorMakerTests()
        {
            strategy = new Strategy().SetStandardRules();
        }

        [Fact]
        public void CanMakeSimplestCommandDescriptorFromMethodOnNamedModel() 
            => Utils.TestFirstMethodOnType<SimpleTypeWithMethodNoAtributes>(strategy);

        [Fact]
        public void CanMakeSimplestCommandDescriptorFromTypeOnNamedModel()
            => Utils.TestType<SimpleTypeNoAttributes>(strategy);

        [Fact]
        public void CanGetCommandDescriptionFromMethodAttribute()
           => Utils.TestFirstMethodOnType<SimpleTypeWithMethodWithDescriptionAttribute>(new Strategy().SetStandardRules());

        [Fact]
        public void CanGetCommandDescriptionFromTypeAttribute()
            => Utils.TestType<SimpleTypeWithDescriptionAttribute>(strategy);

        [Fact]
        public void CanGetArgumentFromNamedMethodParam()
           => Utils.TestFirstMethodOnType<MethodWithParameterNamedArgs>(strategy);

        [Fact]
        public void CanGetArgumentFromNamedPropertyProperty()
            => Utils.TestType<TypeWithPropertyNamedArgs>(strategy);

        [Fact]
        public void CanGetOptionFromMethodParam()
            => Utils.TestFirstMethodOnType<MethodWithParameterOption>(strategy);

        [Fact]
        public void CanGetOptionFromPropertyProperty()
            => Utils.TestType<TypeWithPropertyOption>(strategy);


  
    }
}