//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;

//namespace System.CommandLine.GeneralAppModel
//{
//    public static class ReflectionAppModelExtensions
//    {
//        public static bool MatchAttributes(this BoolRuleSet isBoolRules,
//                                           ParameterInfo parameter,
//                                           string item,
//                                           bool defaultValue = default)
//            => isBoolRules.MatchAttributes(parameter.CustomAttributes, item, defaultValue);
//        public static bool MatchAttributes(this BoolRuleSet isBoolRules,
//                                           PropertyInfo propertyInfo,
//                                           string item,
//                                           bool defaultValue = default)
//            => isBoolRules.MatchAttributes(propertyInfo.CustomAttributes, item, defaultValue);
//        public static bool MatchAttributes(this BoolRuleSet isBoolRules,
//                                           Type type,
//                                           string item,
//                                           bool defaultValue = default)
//            => isBoolRules.MatchAttributes(type.CustomAttributes, item, defaultValue);
//        public static bool MatchAttributes(this BoolRuleSet isBoolRules,
//                                           IEnumerable<CustomAttributeData> attributes,
//                                           string item,
//                                           bool defaultValue = default)
//        {
//            return isBoolRules.RuleOperand == BoolRuleSet.Operand.Any
//                        ? attributes.Any(attributePredicate)
//                        : attributes.All(attributePredicate);

//            bool attributePredicate(CustomAttributeData attribute)
//            {
//                return isBoolRules.Rules
//                        .Any(r => MatchBoolRule(r, attribute, item, defaultValue));
//            }
//        }
//        public static bool MatchBoolRule(RuleBase ruleBase,
//                                         CustomAttributeData attribute,
//                                         string item,
//                                         bool defaultValue = default)
//          => ruleBase switch
//          {
//              BoolAttributeRule rule => MatchItem(rule, attribute),
//              StringContentsRule rule => MatchItem(rule, item),
//              _ => throw new ArgumentException(nameof(ruleBase))
//          };

//        public static bool MatchItem(BoolAttributeRule rule, CustomAttributeData attribute)
//        {
//            if (attribute.AttributeType.Name == rule.AttributeName)
//            {
//                var args = attribute.NamedArguments
//                               .Where(na => na.MemberName == rule.PropertyName);
//                return !args.Any()
//                    ? true
//                    : args.First().TypedValue.ArgumentType != typeof(bool)
//                        ? true
//                        : (bool)args.First().TypedValue.Value;
//            }
//            return false;
//        }
//        public static bool MatchItem(StringContentsRule rule, string item)
//           => string.IsNullOrEmpty(item) ? false : rule.IsFound(item);

//        public static string MatchStringAttributes(this StringRuleSet stringRules, ParameterInfo parameter)
//            => stringRules.MatchStringAttributes(parameter.CustomAttributes);
//        public static string MatchStringAttributes(this StringRuleSet stringRules, PropertyInfo propertyInfo)
//            => stringRules.MatchStringAttributes(propertyInfo.CustomAttributes);
//        public static string MatchStringAttributes(this StringRuleSet stringRules, Type type)
//            => stringRules.MatchStringAttributes(type.CustomAttributes);
//        public static string MatchStringAttributes(this StringRuleSet stringRules, IEnumerable<CustomAttributeData> attributes)
//        {
//            return stringRules.Rules
//                           .Select(r => r.MatchStringAttribute(p.CustomAttributes))
//}



//        //=> p.CustomAttributes
//        //        .Where(a => isRules.Rules
//        //                    .OfType<BoolAttributeRule>()
//        //                    .Any(r => MatchStringAttribute(a, r)))
//        //        .Any();








//        public static string MatchRule(RuleBase ruleBase, IEnumerable<CustomAttributeData> attributes, string item, string defaultValue = default)
//        {
//            return ruleBase switch
//            {
//                StringAttributeRule rule => MatchAttributes(rule, attributes),
//                IdentityRule _ => item,
//                _ => throw new ArgumentException(nameof(ruleBase))
//            };
//        }

//        public static T MatchRule<T>(RuleBase ruleBase, IEnumerable<CustomAttributeData> attributes, T defaultValue = default)
//            => rule switch
//            {
//                ComplexAttributeRule complexRule => ,
//                AttributeRule attributeRule =>,
//                LabelRule labelRule => ,
//                _ => throw new ArgumentException(nameof(rule))
//            };

//        public static string MatchAttributes(StringAttributeRule rule, IEnumerable<CustomAttributeData> attributes)
//        {
//            foreach (var attrib in attributes)
//            {
//                var s = MatchItem(rule, attrib);
//                if (!string.IsNullOrWhiteSpace(s))
//                {
//                    return s;
//                }
//            }
//            return null;
//        }

//        public static string MatchItem(StringAttributeRule rule, CustomAttributeData attribute)
//            => attribute.AttributeType.Name == rule.AttributeName
//                ? attribute.NamedArguments
//                               .FirstOrDefault(na => na.MemberName == rule.PropertyName)
//                               .TypedValue.Value as string
//                : null;

 





//        public static string MatchStringAttributeRule(RuleBase rule, IEnumerable<Attribute> attributes)
//        {

//            if (attribute.AttributeType.Name == rule.AttributeName)
//            {
//                // Add Support for constructor arguments
//                var args = attribute.NamedArguments
//                            .Where(na => na.MemberName == rule.PropertyName);
//                return !args.Any()
//                    ? true
//                    : args.First().TypedValue.ArgumentType != typeof(bool)
//                        ? true
//                        : (bool)args.First().TypedValue.Value;
//            }
//            return false;
//        }

//        public static string MatchItemAttributeRule(RuleBase rule, IEnumerable<Attribute> attributes)
//        {

//            if (attribute.AttributeType.Name == rule.AttributeName)
//            {
//                // Add Support for constructor arguments
//                var args = attribute.NamedArguments
//                            .Where(na => na.MemberName == rule.PropertyName);
//                return !args.Any()
//                    ? true
//                    : args.First().TypedValue.ArgumentType != typeof(bool)
//                        ? true
//                        : (bool)args.First().TypedValue.Value;
//            }
//            return false;
//        }
//    }
//}
