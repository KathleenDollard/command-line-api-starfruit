using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.ReflectionModel
{
    public class ArgumentStrategies
    {
        private readonly List<Func<string, bool>> nameStrategies = new List<Func<string, bool>>();
        private readonly List<Func<IEnumerable<Attribute>, bool>> attributeStrategies = new List<Func<IEnumerable<Attribute>, bool>>();

        public void AddNameStrategy(Func<string, bool> strategy)
            => nameStrategies.Add(strategy);

        public void AddAttributeStrategy(Func<IEnumerable<Attribute>, bool> strategy)
            => attributeStrategies.Add(strategy);

        public bool IsArgument(ParameterInfo parameterInfo)
            // order doesn't matter here, if anything is true, it's true
            => nameStrategies
                     .Any(s => s(parameterInfo.Name))
                   ||
                   attributeStrategies
                     .Any(s => s(parameterInfo.GetCustomAttributes().OfType<Attribute>()));

        public bool IsArgument(PropertyInfo propertyInfo)
            // order doesn't matter here, if anything is true, it's true
            => nameStrategies
                    .Any(s => s(propertyInfo.Name))
                    ||
                    attributeStrategies
                    .Any(s => s(propertyInfo.GetCustomAttributes().OfType<Attribute>()));

    }

    public static class IsArgumentStrategiesExtensions
    {
        public static ArgumentStrategies AllStandard(this ArgumentStrategies argumentStrategies)
            => argumentStrategies.HasStandardAttribute().HasStandardSuffixes();

        public static ArgumentStrategies HasSuffix(this ArgumentStrategies argStrategies, params string[] suffixes)
        {
            if (suffixes.Any())
            {
                argStrategies.AddNameStrategy(name => suffixes
                        .Any(s => name.EndsWith(s)));
            }
            return argStrategies;
        }

        public static ArgumentStrategies HasStandardSuffixes(this ArgumentStrategies argStrategies)
            => argStrategies.HasSuffix("Arg", "Argument");

        public static ArgumentStrategies HasAttribute<T>(this ArgumentStrategies argStrategies)
            where T : Attribute
        {

            argStrategies.AddAttributeStrategy(attributes => attributes
                    .Where(a => a is T)
                    .Any());
            return argStrategies;
        }

        public static ArgumentStrategies HasStandardAttribute(this ArgumentStrategies argStrategies)
            => argStrategies.HasAttribute<CmdArgumentAttribute>();

    }

}
