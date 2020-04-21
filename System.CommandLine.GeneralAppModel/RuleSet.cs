using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

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

        public virtual T GetFirstOrDefault(SymbolType symbolType, params object[] items)
        {
            var flattenedItems = FlattenItems(items);
            foreach (var rule in Rules)
            {
                var (Success, Value) = rule.GetSingleOrDefault(symbolType, flattenedItems);
                if (Success)
                {
                    return Value;
                }
            }
            return default;
        }

        public virtual bool HasMatch(SymbolType symbolType, object[] items)
        {
            foreach (var rule in Rules)
            {
                var success = rule.HasMatch(symbolType, items);
                if (success)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual IEnumerable<T> GetAll(SymbolType symbolType, params object[] items)
        {
            var flattenedItems = FlattenItems(items);

            var list = new List<T>();
            foreach (var rule in Rules)
            {
                var values = rule.GetAllNonDefault(symbolType, flattenedItems);
                list.AddRange(values);
            }
            return list;
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

     
        private object[] FlattenItems(object[] items)
        {
            if (items.Any(i => i is IEnumerable))
            {
                var list = new List<object>();
                foreach (var item in items)
                {
                    if (item is IEnumerable subItems)
                    {
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
        }
    }
}
