using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.GeneralAppModel
{
    public class RuleGroup<TRule> : IEnumerable<TRule>
        where TRule : class, IRule
    {

        private readonly List<TRule> _rules = new List<TRule>();
        public IEnumerable<TRule> Rules => _rules;

        public RuleGroup<TRule> Add(TRule rule)
        {
            _rules.Add(rule);
            return this;
        }

        public IEnumerator<TRule> GetEnumerator()
            => ((IEnumerable<TRule>)_rules).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => ((IEnumerable<TRule>)_rules).GetEnumerator();

        [return: MaybeNull]
        public virtual T GetFirstOrDefaultValue<T>(ISymbolDescriptor symbolDescriptor,
                                                   Candidate candidate,
                                                   ISymbolDescriptor parentSymbolDescriptor)
        {
            var traits = candidate.Traits;

            // A frequent trouble spot for strategies is that their rules don't match OfType in the following line. 
            // This can be because they inadvertently have the wrong T, or they don't support IRuleGetValue.
            // Strong typing in strategies is planned to reduce this issue.
            var valueRules = Rules.OfType<IRuleGetValue<T>>().ToList();
            foreach (var rule in valueRules)
            {
                var (Success, Value) = rule.GetFirstOrDefaultValue(symbolDescriptor, traits, parentSymbolDescriptor);
                if (Success)
                {
                    return Value;
                }
            }
            return default;
        }

        public virtual (bool success, T value) GetOptionalValue<T>(ISymbolDescriptor symbolDescriptor,
                                             Candidate candidate,
                                             ISymbolDescriptor parentSymbolDescriptor)
        {
            var traits = candidate.Traits;

            // A frequent trouble spot for strategies is that their rules don't match OfType in the following line. 
            // This can be because they inadvertently have the wrong T, or they don't support IRuleGetValue.
            // Strong typing in strategies is planned to reduce this issue.
            var valueRules = Rules.OfType<IRuleOptionalValue<T>>().ToList();
            foreach (var rule in valueRules)
            {
                var (success, value) = rule.GetOptionalValue(symbolDescriptor, traits, parentSymbolDescriptor);
                if (success)
                {
                    return (success, value);
                }
            }
            return default;
        }

        public virtual IEnumerable<TValue> GetAllValues<TValue>(ISymbolDescriptor symbolDescriptor,
                                                                Candidate candidate,
                                                                ISymbolDescriptor parentSymbolDescriptor)
        {
            var traits = candidate.Traits;
            var valueRules = Rules.OfType<IRuleGetValues<TValue>>().ToList();
            var values = new List<TValue>();
            foreach (var rule in valueRules)
            {
                values.AddRange(rule.GetAllValues(symbolDescriptor, traits, parentSymbolDescriptor));
            }
            return values;
        }
        public void ReplaceAbstractRules(DescriptorMakerSpecificSourceBase tools)
        {
            var abstractRules = this.Where(r => typeof(IAbstractRule).IsAssignableFrom(r.GetType()));
            var toReplace = new List<(TRule oldRule, TRule newRule)>();
            foreach (var abstractRule in abstractRules)
            {
                Type? derivedType = GetNewRuleType(tools, abstractRule);
                if (derivedType is null)
                { continue; } // this isn't an error because the specific layer might not support this rule
                TRule derivedRule = CreateNewRule(abstractRule, derivedType);
                toReplace.Add((abstractRule, derivedRule));
            }
            ReplaceRules(toReplace);
            return;

            void ReplaceRules(List<(TRule oldRule, TRule newRule)> toReplace)
            {
                foreach (var (oldRule, newRule) in toReplace)
                {
                    var pos = _rules.IndexOf(oldRule);
                    _rules.RemoveAt(pos);
                    _rules.Insert(pos, newRule);

                }
            }

            static TRule CreateNewRule(TRule abstractRule, Type? derivedType)
            {
                // Abstract and derived types must have the same parameter list
                var ctor = abstractRule.GetType().GetConstructors().FirstOrDefault(); // we might want to look for longest parameter list
                var parameters = ctor.GetParameters();
                var arguments = new object[parameters.Length];
                var flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;
                for (int i = 0; i < parameters.Length; i++)
                {
                    var prop = abstractRule.GetType().GetProperty(parameters[i].Name, flags);
                    arguments[i] = prop.GetValue(abstractRule);
                }
                var derivedRuleObject = Activator.CreateInstance(derivedType, arguments);
                var derivedRule = (derivedRuleObject as TRule) ?? throw new InvalidOperationException("Derived rule not of expected type");
                return derivedRule;
            }

            static Type GetNewRuleType(DescriptorMakerSpecificSourceBase tools, TRule abstractRule)
            {
                // We might want to cache this
                var assembly = tools.GetType().Assembly;
                var derivedTypes = assembly.GetTypes()
                                    .Where(t => abstractRule.GetType().IsAssignableFrom(t));
                var derivedType = derivedTypes.FirstOrDefault();
                return derivedType;
            }
        }
    }
}
