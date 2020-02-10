//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Reflection;
//using System.Xml;

//namespace System.CommandLine.ReflectionModel
//{
//    public class NameStrategies
//    {
//        private readonly List<Func<IEnumerable<Attribute>, string>> attributeStrategies = new List<Func<IEnumerable<Attribute>, string>>();

//        public void AddAttributeStrategy(Func<IEnumerable<Attribute>, string> strategy)
//         => attributeStrategies.Add(strategy);

//        public string Name(ParameterInfo parameterInfo)
//        {
//            // order does matter here, attributes win over Xml Docs
//            var names = attributeStrategies
//                                .Select(s => s(parameterInfo.GetCustomAttributes().OfType<Attribute>()))
//                                .Where(s => !(s is null));
//            return names.Any()
//                     ? names.First()
//                     : parameterInfo.Name;
//        }

//        public string Name(PropertyInfo propertyInfo)
//        {
//            // order does matter here, attributes win over Xml Docs
//            var names = attributeStrategies
//                                .Select(s => s(propertyInfo.GetCustomAttributes().OfType<Attribute>()))
//                                .Where(s => !(s is null));
//            return names.Any()
//                     ? names.First()
//                     : propertyInfo.Name;
//        }

//        public string Name(Type type)
//        {
//            // order does matter here, attributes win over Xml Docs
//            var names = attributeStrategies
//                                .Select(s => s(type.GetCustomAttributes().OfType<Attribute>()))
//                                .Where(s => !(s is null));
//            return names.Any()
//                     ? names.First()
//                     : type.Name;
//        }
//    }

//    public static class NameStrategiesExtensions
//    {
//        public static NameStrategies AllStandard(this NameStrategies nameStrategies)
//               => nameStrategies.HasStandardAttribute();

//        public static NameStrategies HasAttribute<T>(this NameStrategies argStrategies, Func<T, string> extractFunc)
//            where T : Attribute
//        {

//            argStrategies.AddAttributeStrategy(
//                attributes => attributes
//                                .OfType<T>()
//                                .Select(a => extractFunc(a))
//                                .FirstOrDefault());
//            return argStrategies;
//        }

//        public static NameStrategies HasStandardAttribute(this NameStrategies argStrategies)
//            => argStrategies
//                    .HasAttribute<CmdNameAttribute >(a => a.Name)
//                    .HasAttribute<CmdArgOptionBaseAttribute>(a => a.Name)
//                    .HasAttribute<CmdCommandAttribute>(a => a.Name)
//                    .HasAttribute<CmdArgOptionBaseAttribute>(a => a.Name);

//    }
//}