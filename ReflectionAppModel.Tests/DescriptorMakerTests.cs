﻿using System.CommandLine.GeneralAppModel;
using Xunit;
using System.CommandLine.ReflectionAppModel.Tests.ModelCodeForTests;

namespace System.CommandLine.ReflectionAppModel.Tests
{

    public class DescriptorMakerTests
    {

        private readonly Strategy strategy ;

        public DescriptorMakerTests()
        {
            strategy = new Strategy()
                            .SetReflectionRules()
                            .CandidateNamesToIgnore("CommandDataFromMethods", "CommandDataFromType");
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
        public void CanGetOptionFromMethodParam()
            => Utils.TestFirstMethodOnType<MethodWithParameterOption>(strategy);

        [Fact]
        public void CanGetOptionFromPropertyProperty()
            => Utils.TestType<TypeWithPropertyOption>(strategy);

        [Fact]
        public void CanGetSubCommandFromInheritedType()
        => Utils.TestType<TypeWithDerivedTypeCommands_A>(strategy);

    }
}