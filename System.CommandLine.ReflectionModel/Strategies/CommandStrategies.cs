using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.ReflectionAppModel
{
    public class CommandStrategies
    {
        private readonly List<Func<string, bool>> nameStrategies = new List<Func<string, bool>>();
        private readonly List<Func<IEnumerable<Attribute>, bool>> attributeStrategies = new List<Func<IEnumerable<Attribute>, bool>>();

        public void AddNameStrategy(Func<string, bool> strategy)
            => nameStrategies.Add(strategy);

        public void AddAttributeStrategy(Func<IEnumerable<Attribute>, bool> strategy)
         => attributeStrategies.Add(strategy);

        public bool IsCommand(ParameterInfo parameterInfo)
             // order doesn't matter here, if anything is true, it's true
             => nameStrategies
                    .Any(s => s(parameterInfo.Name))
                    ||
                    attributeStrategies
                        .Any(s => s(parameterInfo.GetCustomAttributes().OfType<Attribute>()));

        public bool IsCommand(PropertyInfo propertyInfo)
            // order doesn't matter here, if anything is true, it's true
            => nameStrategies
                    .Any(s => s(propertyInfo.Name))
                    ||
                    attributeStrategies
                    .Any(s => s(propertyInfo.GetCustomAttributes().OfType<Attribute>()));
    }

    public static class IsCommandStrategiesExtensions
    {
        public static CommandStrategies AllStandard(this CommandStrategies commandStrategies)
            => commandStrategies.HasStandardAttribute().HasStandardSuffixes();

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

            commandStrategies.AddAttributeStrategy(attributes => attributes
                    .Where(a => a is T)
                    .Any());
            return commandStrategies;
        }

        public static CommandStrategies HasStandardAttribute(this CommandStrategies commandStrategies)
            => commandStrategies.HasAttribute<CmdCommandAttribute>();

    }
}