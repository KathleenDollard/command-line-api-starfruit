using System;
using System.Collections.Generic;
using System.CommandLine.ReflectionModel.Strategies;
using System.Reflection;



namespace System.CommandLine.ReflectionModel.ModelStrategies
{
    public class IsRequiredStrategies : ModelStrategies
    {
        internal readonly BoolAttributeStrategies AttributeStrategies = new BoolAttributeStrategies();

        public bool? IsRequired(ParameterInfo parameterInfo, SymbolType symbolType)
            => AttributeStrategies.AreAnyTrue(parameterInfo, symbolType);

        public bool? IsRequired(PropertyInfo propertyInfo, SymbolType symbolType)
            => AttributeStrategies.AreAnyTrue(propertyInfo, symbolType);

        public override IEnumerable<string> StrategyDescriptions
             => AttributeStrategies.StrategyDescriptions;

        public override void UseStandard()
        {
            AttributeStrategies.Add<CmdRequiredAttribute>();
            AttributeStrategies.Add<CmdOptionAttribute>(a => ((CmdOptionAttribute)a).OptionRequired, SymbolType.Option);
            AttributeStrategies.Add<CmdOptionAttribute>(a => ((CmdOptionAttribute)a).ArgumentRequired, SymbolType.Argument);
            AttributeStrategies.Add<CmdArgumentAttribute>(a => ((CmdArgumentAttribute)a).Required);
        }
    }
}
