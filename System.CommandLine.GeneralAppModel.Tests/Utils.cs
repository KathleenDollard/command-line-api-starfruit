using FluentAssertions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public static class Utils
    {

        public const string EmptyRawForTest = "";

        public static (bool success, string message) CompareLists<T>(this IEnumerable<T> list1, IEnumerable<T> list2, string name)
        {
            var a1 = list1.ToArray();
            var a2 = list2.ToArray();
            if (a1.Length != a2.Length)
            {
                return (false, $"The length of {name} {list1.Count()} does not equal {list2.Count()}");
            }
            for (int i = 0; i < list1.Count(); i++)
            {
                if (Equals(a1[i], a2[i]))
                {
                    return (false, $"Position {i} of {name} is {a1}, while {a2} was expected");
                }

            }
            return (true, string.Empty);
        }

        public static void CheckRule<TRule>(this IRule rule, SymbolType symbolType)
           where TRule : IRule
        {
            rule.Should().BeOfType<TRule>();
            rule.SymbolType.Should().IncludeSymbolType(symbolType);
        }

        public static void CheckNamePatternRule(this IRule rule, SymbolType symbolType, StringContentsRule.StringPosition position, string compareTo)
        {
            rule.CheckRule<NameEndsWithRule>(symbolType);
            var typeRule = rule as NameEndsWithRule;
            var _ = typeRule ?? throw new InvalidOperationException("Unhandled rule type");
            typeRule.Position.Should().Be(position);
            typeRule.CompareTo.Should().Be(compareTo);
        }


        public static void CheckAttributeRule<TAttribute>(this IRule rule, SymbolType symbolType)
        {
            rule.CheckRule<AttributeRule<TAttribute>>(symbolType);
            var typeRule = rule as AttributeRule<TAttribute>;
            var _ = typeRule ?? throw new InvalidOperationException("Unhandled rule type");
            typeRule.Should().BeOfType<AttributeRule<TAttribute>>();
        }

        public static bool CompareDistinctEnumerable<T>(IEnumerable<T> expected, IEnumerable<T> actual, bool nullAndEmptyRelaxed = false)
        {
            if ((expected is null) && (actual is null))
            {
                return true;
            }
            if ((expected is null) || (actual is null))
            {
                // one, but not both are null
                return nullAndEmptyRelaxed
                        ? (expected?.Count() == 0 || actual.Count() == 0) // if they are both, later code finds them equal
                        : false;
            }
            var expectedCount = expected.Count();
            if ((expectedCount != actual.Count()) || (expectedCount != actual.Distinct().Count()) || (expectedCount != expected.Distinct().Count()))
            {
                return false;
            }
            var matchFailures = expected.Where(x => !actual.Where(y => Equals(x, y)).Any());
            return !matchFailures.Any();
        }

        public static void CheckIsOfTypeRule(this IRule rule, SymbolType symbolType, Type type)
        {
            rule.Should().BeAssignableTo<IsOfTypeRule>();
            rule.SymbolType.Should().IncludeSymbolType(symbolType);
            var typedRule = rule as IsOfTypeRule;
            var _ = typedRule ?? throw new InvalidOperationException("Unhandled rule type");
            typedRule.Type.Should().Be(type);
        }


        public static void CheckDerivedFromRule(this IRule rule, string? assembly, string? namespaceName, bool ignoreNamespace)
        {
            rule.Should().BeAssignableTo<DerivedFromRule>();
            var typedRule = rule as DerivedFromRule;
            var _ = typedRule ?? throw new InvalidOperationException("Unhandled rule type");
            typedRule.AssemblyName.Should().Be(assembly);
            typedRule.NamespaceName.Should().Be(namespaceName);
            typedRule.IgnoreNamespace.Should().Be(ignoreNamespace);

        }

        public static void CheckRules(this IRule[] actual, RuleGroupTestData descriptionData)
        {
            var symbolType = descriptionData.SymbolType;
            if (descriptionData.Rules is null)
            {
                actual.Should().HaveCount(0);
                return;
            }
            var expectedRules = descriptionData.Rules.ToArray();
            actual.Should().HaveCount(expectedRules.Length);
            for (int i = 0; i < actual.Length; i++)
            {
                switch (actual[i])
                {
                    case NameEndsWithRule r:
                        var np = expectedRules[i] as NamePatternTestData;
                        var _ = np ?? throw new InvalidOperationException("Unexpecte TestData type");
                        r.CheckNamePatternRule(symbolType, np.Position, np.CompareTo);
                        break;
                    //case AttributeWithPropertyValueRule<string> r:
                    //    var naps = expectedRules[i] as NamedAttributeWithPropertyTestData;
                    //    var _2 = naps ?? throw new InvalidOperationException("Unexpecte TestData type");
                    //    r.CheckNamedAttributeWithPropertyRule(symbolType, naps.AttributeName, naps.PropertyName, typeof(string));
                    //    break;
                    //case AttributeWithPropertyValueRule<bool> r:
                    //    var rapb = expectedRules[i] as NamedAttributeWithPropertyTestData;
                    //    var _3 = rapb ?? throw new InvalidOperationException("Unexpecte TestData type");
                    //    r.CheckNamedAttributeWithPropertyRule(symbolType, rapb.AttributeName, rapb.PropertyName, typeof(bool));
                    //    break;
                    //case AttributeRule r:
                    //    var na = expectedRules[i] as NamedAttributeTestData;
                    //    var _4 = na ?? throw new InvalidOperationException("Unexpecte TestData type");
                    //    r.CheckNamedAttributeRule(symbolType, na.AttributeName);
                    //    break;
                    default:
                        throw new InvalidOperationException("Add strongly typed attribute rules");

                }
            }
        }

        public static string DisplayEqualsFailure<T>(SymbolType symbolType, string name, T expected, T actual)
        {
            return ($@"Expected {name} for {symbolType} to be {DisplayString(expected)}, but found {DisplayString(actual)}");
        }

        public static string DisplayString<T>(T input)
        {
            return input switch
            {
                null => "<null>",
                string s => $@"""{s}""",
                IEnumerable e => e.OfType<object>().Count() == 0
                                   ? "<Empty>"
                                   : "\n" + string.Join('\n', Members(e)),
                _ => input.ToString() ?? string.Empty
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
