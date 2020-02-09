using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.ReflectionAppModel
{
    public class ArityStrategies
    {
        private readonly List<Func<IEnumerable<Attribute>, (int min, int max)?>> attributeStrategies = new List<Func<IEnumerable<Attribute>, (int min, int max)?>>();

        public void AddAttributeStrategy(Func<IEnumerable<Attribute>, (int min, int max)?> strategy)
            => attributeStrategies.Add(strategy);

        public (int min, int max)? MinMax(ParameterInfo parameterInfo)
            => attributeStrategies
                                .Select(s => s(parameterInfo.GetCustomAttributes().OfType<Attribute>()))
                                .Where(s => !(s is null))
                                .FirstOrDefault();

        public (int min, int max)? MinMax(PropertyInfo propertyInfo)
            => attributeStrategies
                        .Select(s => s(propertyInfo.GetCustomAttributes().OfType<Attribute>()))
                        .Where(s => !(s is null))
                        .FirstOrDefault();
    }

    public static class ArityStrategiesExtensions
    {
        public static ArityStrategies AllStandard(this ArityStrategies arityStrategies)
             => arityStrategies.HasStandardAttribute();

        public static ArityStrategies HasAttribute<T>(this ArityStrategies arityStrategies, Func<T, (int min, int max)?> extractFunc)
            where T : Attribute
        {

            arityStrategies.AddAttributeStrategy(
                attributes => attributes
                                .OfType<T>()
                                .Select(a => extractFunc(a))
                                .FirstOrDefault());
            return arityStrategies;
        }

        public static ArityStrategies HasStandardAttribute(this ArityStrategies argStrategies)
            => argStrategies.HasAttribute<CmdArityAttribute>(a => (a.MinArgCount, a.MaxArgCount ));

    }
}