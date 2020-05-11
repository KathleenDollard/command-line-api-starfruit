using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    public class RuleSet<T> : IEnumerable<RuleBase<T>>
    {

        private readonly List<RuleBase<T>> _rules = new List<RuleBase<T>>();
        public IEnumerable<RuleBase<T>> Rules => _rules;

        public RuleSet<T> Add(RuleBase<T> rule)
        {
            _rules.Add(rule);
            return this;
        }

        public IEnumerator<RuleBase<T>> GetEnumerator()
            => ((IEnumerable<RuleBase<T>>)_rules).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => ((IEnumerable<RuleBase<T>>)_rules).GetEnumerator();

        public virtual T GetFirstOrDefault(SymbolDescriptorBase symbolDescriptor, IEnumerable<object> items)
        {
            var flattenedItems = FlattenItems(items);
            foreach (var rule in Rules)
            {
                var (Success, Value) = rule.GetSingleOrDefault(symbolDescriptor, flattenedItems);
                if (Success)
                {
                    return Value;
                }
            }
            return default;
        }

        public virtual bool HasMatch(SymbolDescriptorBase symbolDescriptor, IEnumerable<object> items)
        {
            foreach (var rule in Rules)
            {
                var success = rule.HasMatch(symbolDescriptor, items);
                if (success)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual IEnumerable<T> GetMatching(SymbolDescriptorBase descriptor, 
                                             IEnumerable<object> items,
                                             SymbolDescriptorBase parentSymbolDescriptor)
        {
            var flattenedItems = FlattenItems(items);

            var list = new List<T>();
            foreach (var rule in Rules)
            {
                var values = rule.GetAllNonDefault(descriptor, flattenedItems);
                list.AddRange(values.OfType<T>());
            }
            return list.Distinct();
        }

        // Do we need GetAllFromFirstMatchingRule?


        //public abstract void UseStandard();
        public virtual IEnumerable<string> RuleDescriptions
            => Rules.Select(r => r.RuleDescription);

        internal void AddRange(Func<RuleSet<T>, RuleSet<T>> setStandardArgumentRules,
                               SymbolType symbolType)
        {
            var tempRules = new RuleSet<T>();
            setStandardArgumentRules(tempRules);
            foreach (var rule in tempRules)
            {
                Add(rule.WithSymbolType(symbolType));
            }
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
