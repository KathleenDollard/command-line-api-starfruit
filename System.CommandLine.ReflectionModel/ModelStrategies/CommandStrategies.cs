using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.ReflectionModel
{
    public class CommandStrategies
    {
        // @jonseuitor How would we determine that a type is complex enough to be a subcommand (FileInfo, vs dotnet.New)
        private readonly List<Func<string, bool>> nameStrategies = new List<Func<string, bool>>();
        internal readonly BoolAttributeStrategies AttributeStrategies = new BoolAttributeStrategies();

        public void AddNameStrategy(Func<string, bool> strategy)
            => nameStrategies.Add(strategy);

        public bool IsCommand(ParameterInfo parameterInfo)
             // order doesn't matter here, if anything is true, it's true
             => nameStrategies
                    .Any(s => s(parameterInfo.Name))
                    ||
                    AttributeStrategies
                        .AreAnyTrue(parameterInfo);

        public bool IsCommand(PropertyInfo propertyInfo)
            // order doesn't matter here, if anything is true, it's true
            => nameStrategies
                    .Any(s => s(propertyInfo.Name))
                    ||
                    AttributeStrategies
                    .AreAnyTrue(propertyInfo);
    }

    public static class IsCommandStrategiesExtensions
    {
        public static CommandStrategies AllStandard(this CommandStrategies commandStrategies)
            => commandStrategies
                    .HasStandardAttribute()
                    .HasStandardSuffixes();

        public static CommandStrategies HasSuffix(this CommandStrategies commandStrategies, params string[] suffixes)
        {
            if (suffixes.Any())
            {
                commandStrategies.AddNameStrategy(name => suffixes
                        .Any(s => name.EndsWith(s)));
            }
            return commandStrategies;
        }

        public static CommandStrategies HasStandardSuffixes(this CommandStrategies commandStrategies)
            => commandStrategies.HasSuffix("Command");

        public static CommandStrategies HasAttribute<T>(this CommandStrategies commandStrategies)
            where T : Attribute
        {

            commandStrategies.AttributeStrategies.Add<T>();
            return commandStrategies;
        }

        public static CommandStrategies HasStandardAttribute(this CommandStrategies commandStrategies)
            => commandStrategies.HasAttribute<CmdCommandAttribute>();

    }
}