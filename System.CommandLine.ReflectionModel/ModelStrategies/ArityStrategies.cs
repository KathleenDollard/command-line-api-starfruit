using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.Reflection;
using ArityAttributeStrategies = System.CommandLine.ReflectionModel.Strategies.AttributeStrategies<System.CommandLine.GeneralAppModel.ArityDescriptor>;

namespace System.CommandLine.ReflectionModel.ModelStrategies
{

    public class ArityStrategies : ModelStrategies
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


        public override IEnumerable<string> StrategyDescriptions
         => AttributeStrategies.StrategyDescriptions;
        public override void UseStandard() 
            => AttributeStrategies.Add<CmdArityAttribute>(
                            a => new ArityDescriptor(((CmdArityAttribute)a).MinArgCount,
                                                     ((CmdArityAttribute)a).MaxArgCount));
    }
}