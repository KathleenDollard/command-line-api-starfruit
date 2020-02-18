using System.Collections.Generic;
using System.CommandLine.ReflectionModel.Strategies;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.ReflectionModel
{
    public class CommandStrategies
    {
        // @jonseuitor How would we determine that a type is complex enough to be a subcommand (FileInfo, vs dotnet.New)
        internal readonly StringContentStrategies NameStrategies = new StringContentStrategies();
        internal readonly BoolAttributeStrategies AttributeStrategies = new BoolAttributeStrategies();
        internal readonly List<Func<Type, bool>> TypeStrategies = new List<Func<Type, bool>>();
        private SymbolType symbolType = SymbolType.All;

        public void AddTypeStrategy(Func<Type, bool> strategy)
                    => TypeStrategies.Add(strategy);

        public bool IsCommand(ParameterInfo parameterInfo)
             // order doesn't matter here, if anything is true, it's true
             => TypeStrategies.Any(s => s(parameterInfo.ParameterType))
                    ||
                    NameStrategies.AreAnyFound(parameterInfo.Name, symbolType)
                    ||
                    AttributeStrategies.AreAnyTrue(parameterInfo, symbolType);

        public bool IsCommand(PropertyInfo propertyInfo)
            // order doesn't matter here, if anything is true, it's true
            => TypeStrategies.Any(s => s(propertyInfo.PropertyType))
                    ||
                    NameStrategies
                    .AreAnyFound(propertyInfo.Name, symbolType)
                    ||
                    AttributeStrategies
                    .AreAnyTrue(propertyInfo, symbolType);
    }

    public static class IsCommandStrategiesExtensions
    {
        public static CommandStrategies AllStandard(this CommandStrategies commandStrategies)
            => commandStrategies
                    .HasStandardAttribute()
                    .HasStandardNaming()
                    .IsComplexType();


        public static CommandStrategies HasStandardNaming(this CommandStrategies commandStrategies)
        {
            commandStrategies.NameStrategies.Add(StringContentsStrategy.StringPosition.Suffix, "Command");
            return commandStrategies;
        }

        public static CommandStrategies HasStandardAttribute(this CommandStrategies commandStrategies)
        {
            commandStrategies.AttributeStrategies.Add<CmdCommandAttribute>();
            return commandStrategies;
        }


        public static CommandStrategies IsComplexType(this CommandStrategies commandStrategies)
        {
            commandStrategies.AddTypeStrategy(
                type => type.GetConstructor(new[] { typeof(string) }) == null
                        && type.FullName != "System." + type.Name
                        && !CommandMaker.ommittedTypes.Contains(type));
            return commandStrategies;
        }
    }
}