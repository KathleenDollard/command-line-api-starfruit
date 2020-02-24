using System;
using System.Collections.Generic;
using System.CommandLine.ReflectionModel.Strategies;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.ReflectionModel.ModelStrategies
{
    public class CommandStrategies : ModelStrategies
    {
        // @jonseuitor How would we determine that a type is complex enough to be a subcommand (FileInfo, vs dotnet.New)
        internal readonly StringContentStrategies NameStrategies = new StringContentStrategies();
        internal readonly BoolAttributeStrategies AttributeStrategies = new BoolAttributeStrategies();
        internal readonly List<IsCommandTypeStrategy> TypeStrategies = new List<IsCommandTypeStrategy>();
        private readonly SymbolType symbolType = SymbolType.All;

        public bool IsCommand(ParameterInfo parameterInfo)
             // order doesn't matter here, if anything is true, it's true
             => TypeStrategies.Any(s => s.IsCommand(parameterInfo.ParameterType))
                    ||
                    NameStrategies.AreAnyFound(parameterInfo.Name, symbolType)
                    ||
                    AttributeStrategies.AreAnyTrue(parameterInfo, symbolType);

        public bool IsCommand(PropertyInfo propertyInfo)
            // order doesn't matter here, if anything is true, it's true
            => TypeStrategies.Any(s => s.IsCommand(propertyInfo.PropertyType))
                    ||
                    NameStrategies
                    .AreAnyFound(propertyInfo.Name, symbolType)
                    ||
                    AttributeStrategies
                    .AreAnyTrue(propertyInfo, symbolType);

        public override IEnumerable<string> StrategyDescriptions
            => AttributeStrategies.StrategyDescriptions
               .Union(NameStrategies.StrategyDescriptions)
               .Union(TypeStrategies.Select(s => s.StrategyDescription));
    }

    public abstract class IsCommandTypeStrategy : StrategyBase
    {
        public IsCommandTypeStrategy()
        : base(SymbolType.All)

        { }
        public abstract bool IsCommand(Type type);
    }

    public class IsCommandComplexTypeStrategy : IsCommandTypeStrategy
    {
        public override bool IsCommand(Type type)
            => type.GetConstructor(new[] { typeof(string) }) == null
                     && type.FullName != "System." + type.Name
                     && !CommandMaker.ommittedTypes.Contains(type);

        public override string StrategyDescription => "Complex Type Strategy";
    }

    public static class IsCommandStrategiesExtensions
    {
        public static CommandStrategies AllStandard(this CommandStrategies commandStrategies)
            => commandStrategies
                    .HasStandardAttribute()
                    .HasStandardNaming()
                    .IsComplexType();

        public static CommandStrategies IsComplexType(this CommandStrategies commandStrategies)
        {
            commandStrategies.TypeStrategies.Add(new IsCommandComplexTypeStrategy());
            return commandStrategies;
        }

        public static CommandStrategies HasStandardNaming(this CommandStrategies commandStrategies)
        {
            commandStrategies.NameStrategies.Add(StringContentStrategy.StringPosition.Suffix, "Command");
            return commandStrategies;
        }

        public static CommandStrategies HasStandardAttribute(this CommandStrategies commandStrategies)
        {
            commandStrategies.AttributeStrategies.Add<CmdCommandAttribute>();
            return commandStrategies;
        }



    }
}