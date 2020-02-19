using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ArityAttributeStrategies = System.CommandLine.ReflectionModel.AttributeStrategies<System.CommandLine.ReflectionModel.ArityDescriptor>;

namespace System.CommandLine.ReflectionModel
{
    public class ArityDescriptor
    {
        public ArityDescriptor(int min, int max)
        {
            Min = min;
            Max = max;
            IsSet = true;
        }
        public ArityDescriptor()
        { }

        public int Min { get; }
        public int Max { get; }
        public bool IsSet { get; set; }
    }

    public class ArityStrategies
    {
        internal readonly ArityAttributeStrategies AttributeStrategies = new ArityAttributeStrategies();

        public ArityDescriptor GetArity(ParameterInfo parameterInfo)
            => GetArity(parameterInfo.GetCustomAttributes());

        public ArityDescriptor GetArity(PropertyInfo propertyInfo)
            => GetArity(propertyInfo.GetCustomAttributes());

        private ArityDescriptor GetArity(IEnumerable<Attribute> attributes)
        {
            (bool found, ArityDescriptor arityDescriptor) = AttributeStrategies.GetFirstValue(attributes, SymbolType.Argument);
            return found
                ? arityDescriptor
                : null;
        }
    }

    public static class ArityStrategiesExtensions
    {
        public static ArityStrategies AllStandard(this ArityStrategies arityStrategies)
             => arityStrategies.HasStandardAttribute();

        public static ArityStrategies HasStandardAttribute(this ArityStrategies arityStrategies)
        {
            arityStrategies.AttributeStrategies.Add<CmdArityAttribute>(a => new ArityDescriptor(((CmdArityAttribute)a).MinArgCount,
                                                                                                ((CmdArityAttribute)a).MaxArgCount));
            return arityStrategies;
        }
    }
}