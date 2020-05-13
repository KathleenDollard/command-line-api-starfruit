using FluentAssertions;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Tests;
using Xunit;
using System.CommandLine.ReflectionModel.Tests;
using ReflectionAppModel.Tests.TestSupport;
using System.Linq;
using System.Collections.Generic;
using System.CommandLine.ReflectionModel.Tests.ModelCodeForTests;

namespace System.CommandLine.ReflectionModel.Tests
{

    public class DescriptorMakerTests
    {
        public DescriptorMakerTests()
        {
            strategy = new Strategy().SetStandardRules();
        }

        private readonly Strategy strategy ;

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