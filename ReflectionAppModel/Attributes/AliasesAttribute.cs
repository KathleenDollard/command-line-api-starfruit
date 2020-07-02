//using System.Linq;

//namespace System.CommandLine.ReflectionAppModel.Attributes
//{
//    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct |
//                    AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property)]
//    public class AliasesAttribute : Attribute
//    {
//        public AliasesAttribute(params string[] aliases)
//        {
//            if (aliases.Any() && aliases.First().Contains(","))
//            {
//                aliases = aliases.First().Split(',');
//            }
//            Aliases = aliases;
//        }

//        public string[] Aliases { get; }
//    }
//}
