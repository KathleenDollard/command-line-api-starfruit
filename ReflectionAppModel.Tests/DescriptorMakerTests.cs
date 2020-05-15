using System.CommandLine.GeneralAppModel;
using Xunit;
using System.CommandLine.ReflectionAppModel.Tests.ModelCodeForTests;
using System.CommandLine.ReflectionAppModel;

namespace System.CommandLine.ReflectionAppModel.Tests
{

    public class DescriptorMakerTests
    {

        private readonly Strategy strategy ;

        public DescriptorMakerTests()
        {
            strategy = new Strategy().SetReflectionRules();
        }

        [Fact]
        public void CanMakeSimplestCommandDescriptorFromMethodOnNamedModel() 
            => Utils.TestFirstMethodOnType<SimpleTypeWithMethodNoAtributes>(strategy);

        [Fact]
        public void CanMakeSimplestCommandDescriptorFromTypeOnNamedModel()
            => Utils.TestType<SimpleTypeNoAttributes>(strategy);

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