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

        public IEnumerable<Candidate> GetItems(IEnumerable<Candidate> candidates,
                                          SymbolDescriptorBase parentSymbolDescriptor)
        {
            var symbolRules = Rules.OfType<IRuleGetCandidates>();
            var matches = symbolRules
                            .SelectMany(rule => rule.GetCandidates(candidates, parentSymbolDescriptor))
                            .Distinct();
            return matches;
        }

        public virtual T GetFirstOrDefaultValue<T>(SymbolDescriptorBase symbolDescriptor,
                                                   Candidate candidate,
                                                   SymbolDescriptorBase parentSymbolDescriptor)
        {
            var traits = candidate.Traits;
            var flattenedItems = FlattenItems(traits);
            var valueRules = Rules.OfType<IRuleGetValue<T>>();
            foreach (var rule in valueRules)
            {
                var (Success, Value) = rule.GetFirstOrDefaultValue(symbolDescriptor, flattenedItems, parentSymbolDescriptor );
                if (Success)
                {
                    return Value;
                }
            }
            return default;
        }

        public virtual T MorphValue<T>(SymbolDescriptorBase symbolDescriptor,
                                       Candidate candidate,
                                       T value,
                                       SymbolDescriptorBase parentSymbolDescriptor)
        {
            T retValue = value;
            foreach (var rule in Rules.OfType<IRuleMorphValue<T>>())
            {
                retValue  = rule.MorphValue(symbolDescriptor, candidate, value, parentSymbolDescriptor);
                if (!value .Equals( retValue))
                {
                    return retValue;
                }
            }
            return value;
        }


        private IEnumerable<object> FlattenItems(IEnumerable<object> items)
        {
            if (items.Any(i => ShouldFlatten(i)))
            {
                var list = new List<object>();
                foreach (var item in items)
                {
                    if (ShouldFlatten(item))
                    {
                        var subItems = item as IEnumerable;
                        foreach (var subItem in subItems)
                        {
                            list.Add(subItem);
                        }
                    }
                    else
                    {
                        list.Add(item);
                    }
                }
                return list.ToArray();
            }
            return items;

            static bool ShouldFlatten(object i)
            {
                return (i is IEnumerable) && i.GetType() != typeof(string);
            }
        }

    }
}
