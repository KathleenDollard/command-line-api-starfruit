using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    public class RuleGroup<TRule> : IEnumerable<TRule>
        where TRule : IRule
    {

        private readonly List<TRule> _rules = new List<TRule>();
        public IEnumerable<TRule> Rules => _rules;

        public RuleGroup<TRule> Add(TRule rule)
        {
            _rules.Add(rule);
            return this;
        }

        public RuleGroup<TRule> AddRange(IEnumerable<TRule> rules)
        {
            foreach (var rule in rules)
            {
                _rules.Add(rule);
            }
            return this;
        }
        public IEnumerator<TRule> GetEnumerator()
            => ((IEnumerable<TRule>)_rules).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => ((IEnumerable<TRule>)_rules).GetEnumerator();

        public virtual (bool, TValue) TryGetFirstValue<TValue>(SymbolDescriptorBase symbolDescriptor,
                                                   Candidate candidate,
                                                   SymbolDescriptorBase parentSymbolDescriptor)
        {
            var traits = candidate.Traits;
            var getValueRules = Rules.OfType<IRuleGetValue<TValue>>();
            var results = getValueRules
                             .Select(r => r.GetFirstOrDefaultValue(symbolDescriptor, traits, parentSymbolDescriptor))
                             .Where(x => x.success);
            return results.Any()
                        ? results.First()
                        : (false, default(TValue));
        }

        public virtual T GetFirstOrDefaultValue<T>(SymbolDescriptorBase symbolDescriptor,
                                                   Candidate candidate,
                                                   SymbolDescriptorBase parentSymbolDescriptor)
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

        public virtual (bool success, T value) GetOptionalValue<T>(SymbolDescriptorBase symbolDescriptor,
                                             Candidate candidate,
                                             SymbolDescriptorBase parentSymbolDescriptor)
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

        public virtual IEnumerable<TValue> GetAllValues<TValue>(SymbolDescriptorBase symbolDescriptor,
                                            Candidate candidate,
                                            SymbolDescriptorBase parentSymbolDescriptor)
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

    }
}
