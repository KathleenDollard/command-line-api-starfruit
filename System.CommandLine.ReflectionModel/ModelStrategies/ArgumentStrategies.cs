using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.CommandLine.ReflectionModel.Strategies;


namespace System.CommandLine.ReflectionModel.ModelStrategies
{
    public class ArgumentStrategies : ModelStrategies
    {
        internal readonly StringContentStrategies NameStrategies = new StringContentStrategies();
        internal readonly BoolAttributeStrategies AttributeStrategies = new BoolAttributeStrategies();
        private readonly SymbolType symbolType = SymbolType.All;

        public void AddNameStrategy(StringContentStrategy.StringPosition position, string compareTo)
           => NameStrategies.Add(position, compareTo);

        public bool IsArgument(ParameterInfo parameterInfo)
            // order doesn't matter here, if anything is true, it's true
            => NameStrategies.AreAnyFound(parameterInfo.Name, symbolType)
                   ||
                   AttributeStrategies.AreAnyTrue(parameterInfo, symbolType);

        public bool IsArgument(PropertyInfo propertyInfo)
                 // order doesn't matter here, if anything is true, it's true
                 => NameStrategies.AreAnyFound(propertyInfo.Name, symbolType)
                   ||
                   AttributeStrategies.AreAnyTrue(propertyInfo, symbolType);

        public override IEnumerable<string> StrategyDescriptions
            => AttributeStrategies.StrategyDescriptions
               .Union(NameStrategies.StrategyDescriptions);

        public override void UseStandard()
        {
            NameStrategies.Add(StringContentStrategy.StringPosition.Suffix, "Arg");
            NameStrategies.Add(StringContentStrategy.StringPosition.Suffix, "Argument");
            AttributeStrategies.Add<CmdArgumentAttribute>();
        }
    }
}
