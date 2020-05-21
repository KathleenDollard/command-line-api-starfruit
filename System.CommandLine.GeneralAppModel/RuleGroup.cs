using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    public class RuleGroup<T> : IEnumerable<IRule>
    {

        private readonly List<IRule> _rules = new List<IRule>();
        public IEnumerable<IRule> Rules => _rules;

        public RuleGroup<T> Add(IRule rule)
        {
            _rules.Add(rule);
            return this;
        }

        public RuleGroup<T> AddRange(IEnumerable<IRule> rules)
        {
            foreach (var rule in rules)
            {
                _rules.Add(rule);
            }
            return this;
        }
        public IEnumerator<IRule> GetEnumerator()
            => ((IEnumerable<IRule>)_rules).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => ((IEnumerable<IRule>)_rules).GetEnumerator();

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

        public virtual IEnumerable<T> GetAllValues<T>(SymbolDescriptorBase symbolDescriptor,
                                            Candidate candidate,
                                            SymbolDescriptorBase parentSymbolDescriptor)
        {
            var traits = candidate.Traits;
            var valueRules = Rules.OfType<IRuleGetValues<T>>().ToList();
            var values = new List<T>();
            foreach (var rule in valueRules)
            {
                values.AddRange(rule.GetAllValues(symbolDescriptor, traits, parentSymbolDescriptor));
            }
            return values;
        }

    }
}
