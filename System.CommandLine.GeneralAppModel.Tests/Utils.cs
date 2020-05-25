using FluentAssertions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public static class Utils
    {
        public static string CompareLists<T>(this IEnumerable<T> list1, IEnumerable<T> list2, string name)
        {
            var a1 = list1.ToArray();
            var a2 = list2.ToArray();
            if (a1.Length != a2.Length)
            {
                return $"The length of {name} {list1.Count()} does not equal {list2.Count()}";
            }
            for (int i = 0; i < list1.Count(); i++)
            {
                if (!(a1[i].Equals(a2[i])))
                {
                    return $"Position {i} of {name} is {a1}, while {a2} was expected";
                }

            }
            return null;
        }

        public static void CheckRule<TRule>(this IRule rule, SymbolType symbolType)
           where TRule : IRule
        {
            rule.Should().BeOfType<TRule>();
            rule.SymbolType.Should().IncludeSymbolType(symbolType);
        }

        public static void CheckNamePatternRule(this IRule rule, SymbolType symbolType, StringContentsRule.StringPosition position, string compareTo)
        {
            rule.CheckRule<NamePatternRule>(symbolType);
            var typeRule = rule as NamePatternRule;
            typeRule.Position.Should().Be(position);
            typeRule.CompareTo.Should().Be(compareTo);
        }

        public static void CheckNamedAttributeRule(this IRule rule, SymbolType symbolType, string attributeName)
        {
            rule.CheckRule<AttributeRule>(symbolType);
            var typeRule = rule as AttributeRule;
            typeRule.AttributeName.Should().Be(attributeName);
        }

        public static void CheckNamedAttributeWithPropertyRule(this IRule rule, SymbolType symbolType, string attributeName, string propertyName, Type type)
        {
            rule.Should().BeAssignableTo<AttributeWithPropertyRule>();
            rule.SymbolType.Should().IncludeSymbolType(symbolType);
            var typedRule = rule as AttributeWithPropertyRule;
            typedRule.AttributeName.Should().Be(attributeName);
            typedRule.PropertyName.Should().Be(propertyName);
            typedRule.Type.Should().Be(type);
        }

        public static bool CompareDistinctEnumerable<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            var expectedCount = expected.Count();
            if ((expectedCount != actual.Count()) || (expectedCount != actual.Distinct().Count()) || (expectedCount != expected.Distinct().Count()))
            {
                return false;
            }
            var matchFailures = expected.Where(x => !actual.Where(y => y.Equals(x)).Any());
            return !matchFailures.Any();
        }

        public static void CheckIsOfTypeRule(this IRule rule, SymbolType symbolType, Type type)
        {
            rule.Should().BeAssignableTo<IsOfTypeRule>();
            rule.SymbolType.Should().IncludeSymbolType(symbolType);
            var typedRule = rule as IsOfTypeRule;
            typedRule.Type.Should().Be(type);

        }

        public static void CheckRules(this IRule[] actual, RuleGroupTestData descriptionData)
        {
            var symbolType = descriptionData.SymbolType;
            var expectedRules = descriptionData.Rules.ToArray();
            actual.Should().HaveCount(expectedRules.Length);
            for (int i = 0; i < actual.Length; i++)
            {
                switch (actual[i])
                {
                    case NamePatternRule r:
                        var np = expectedRules[i] as NamePatternTestData;
                        r.CheckNamePatternRule(symbolType, np.Position, np.CompareTo);
                        break;
                    case AttributeWithPropertyRule<string> r:
                        var naps = expectedRules[i] as NamedAttributeWithPropertyTestData;
                        r.CheckNamedAttributeWithPropertyRule(symbolType, naps.AttributeName, naps.PropertyName, typeof(string));
                        break;
                    case AttributeWithPropertyRule<bool> r:
                        var rapb = expectedRules[i] as NamedAttributeWithPropertyTestData;
                        r.CheckNamedAttributeWithPropertyRule(symbolType, rapb.AttributeName, rapb.PropertyName, typeof(bool));
                        break;
                    case AttributeRule r:
                        var na = expectedRules[i] as NamedAttributeTestData;
                        r.CheckNamedAttributeRule(symbolType, na.AttributeName);
                        break;

                }
            }
        }

        public static string DisplayEqualsFailure<T>(SymbolType symbolType, string name,T expected, T actual  )
        {
               return ($@"Expected {name} for {symbolType} to be {DisplayString(expected)}, but found {DisplayString(actual)}");
        }

        public static string DisplayString<T>(T input)
        {
            return input switch
            {
                null => "<null>",
                string s => $@"""{s}""",
                IEnumerable e => "\n" + string.Join('\n', Members(e)),
                _ => input.ToString()
            };

            static IEnumerable<string> Members(IEnumerable list)
            {
                var ret = new List<string>();
                foreach (var item in list)
                {
                    ret.Add(DisplayString(item));
                }
                return ret;
            }
        }
    }
}
