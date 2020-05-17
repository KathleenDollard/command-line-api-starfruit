using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public  static class  Utils
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

        public static IEnumerable<MethodInfo> GetMethodsOnDeclaredType(this Type type)
           => type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

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
            rule.CheckRule<NamedAttributeRule>(symbolType);
            var typeRule = rule as NamedAttributeRule;
            typeRule.AttributeName .Should().Be(attributeName);
        }

        public static void CheckNamedAttributeWithPropertyRule(this IRule rule, SymbolType symbolType, string attributeName, string propertyName, Type type)
        {
            rule.Should().BeOfType(type);
            rule.SymbolType.Should().IncludeSymbolType(symbolType);
            var typeRule = rule as NamedAttributeWithPropertyRule;
            typeRule.AttributeName.Should().Be(attributeName);
            typeRule.PropertyName.Should().Be(propertyName);
        }

    }
}
