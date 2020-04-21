//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace System.CommandLine.GeneralAppModel
//{
//    public static class RuleSetExtensions
//    {
//        public static (bool Success, object Value) TryGetFirstMatch(this RuleSet rules,
//                                                             SymbolType symbolType,
//                                                             IEnumerable<Attribute> attributes = null,
//                                                             string stringValue = null,
//                                                             object item = null)
//        {
//            var useRules = rules
//                            .Where(r => r.SymbolType == SymbolType.All || r.SymbolType == symbolType);
//            foreach (var rule in useRules)
//            {
//                var (Success, Value) = rule switch
//                {
//                    AttributeRule attributeRule => attributeRule.TryGetFirstMatch(attributes, symbolType),
//                    StringContentsRule stringRule => stringRule.TryGetMatch(stringValue, symbolType),
//                    IdentityRule itemRule => (true, item),
//                    _ => throw new InvalidOperationException("Unknown rule type")
//                };
//                if (Success)
//                {
//                    return (Success, Value);
//                }
//            }
//            return (false, null);
//        }



//        public static IEnumerable<T> GetAll<T>(this RuleSet rules,
//                                                          SymbolType symbolType,
//                                                          IEnumerable<Attribute> attributes = null,
//                                                          string stringValue = null,
//                                                          object item = null)
//        {
//            var list = new List<T>();
//            var useRules = rules
//                            .Where(r => r.SymbolType == SymbolType.All || r.SymbolType == symbolType);
//            foreach (var rule in useRules)
//            {
//                var values = rule switch
//                {
//                    AttributeRule attributeRule => attributeRule.GetAll<T>(attributes, symbolType),
//                    StringContentsRule stringRule => stringRule.GetStringMatches<T>(stringValue, symbolType),
//                    IdentityRule itemRule => new T[] { Conversions.To<T>(item) },
//                    _ => throw new InvalidOperationException("Unknown rule type")
//                };
//                list.AddRange(values);
//            }
//            return list;
//        }


//        public static (bool Success, object Value) TryGetFirstMatch(this AttributeRule rule,
//                                                             IEnumerable<Attribute> attributes,
//                                                             SymbolType symbolType)
//        {
//            return rule.SymbolType != SymbolType.All && rule.SymbolType != symbolType
//                ? ((bool Success, object Value))(false, null)
//                : GetResult(attributes
//                            .Where(a => a.GetType().Name.Equals(rule.AttributeName)), rule.PropertyName);

//            (bool, object) GetResult(IEnumerable<Attribute> matches, string propertyName)
//            {
//                return matches.Any()
//                    ? (true, GetProperty(matches.First(), rule.PropertyName))
//                    : (false, null);
//            }
//        }

//        public static (bool Success, object Value) TryGetMatch(this StringContentsRule rule,
//                                                               string stringValue,
//                                                               SymbolType symbolType)
//        {
//            return symbolType != SymbolType.All && symbolType != rule.SymbolType
//                ? ((bool Success, object Value))(false, null)
//                : rule.IsFound(stringValue)
//                    ? (true, rule.Strip(stringValue))
//                    : (false, null);
//        }

//        public static IEnumerable<T> GetAll<T>(this AttributeRule rule,
//                                                             IEnumerable<Attribute> attributes,
//                                                             SymbolType symbolType)
//        {
//            return rule.SymbolType != SymbolType.All && rule.SymbolType != symbolType
//                ? null
//                : GetValues(attributes.Where(a => a.GetType().Name.Equals(rule.AttributeName)),
//                        rule.PropertyName);

//            IEnumerable<T> GetValues(IEnumerable<Attribute> matches, string propertyName)
//            {
//                return matches
//                        .Select(a => GetProperty<T>(a, rule.PropertyName));
//            }
//        }

//        public static IEnumerable<T> GetStringMatches<T>(this StringContentsRule rule,
//                                          string stringValue,
//                                          SymbolType symbolType)
//        {
//            return ListOrEmpty(GetStringMatch<T>(rule, stringValue, symbolType));

//            static IEnumerable<T> ListOrEmpty(string value)
//            {
//                return value is null
//                        ? Array.Empty<T>()
//                        : (new T[] { (T)(object)value });
//            }
//        }

//        public static string GetStringMatch<T>(this StringContentsRule rule,
//                                               string stringValue,
//                                               SymbolType symbolType)
//        {
//            return (rule.SymbolType != SymbolType.All && rule.SymbolType != symbolType)
//                    || typeof(T) != typeof(string)
//                         ? null
//                        : rule.IsFound(stringValue)
//                            ? rule.Strip(stringValue)
//                            : null;
//        }

//        private static object GetProperty(Attribute attribute, string propertyName)
//            => attribute.GetType()
//                .GetProperty(propertyName)
//                .GetValue(attribute);

//        private static T GetProperty<T>(Attribute attribute, string propertyName)
//            => Conversions.To<T>(attribute.GetType()
//                                .GetProperty(propertyName)
//                                .GetValue(attribute));
//    }
//}
