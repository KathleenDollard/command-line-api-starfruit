using System.Collections;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Rules;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    public class RuleSet<T> : IEnumerable<IRule>
    {

        private readonly List<IRule> _rules = new List<IRule>();
        public IEnumerable<IRule> Rules => _rules;

        public RuleSet<T> Add(IRule rule)
        {
            _rules.Add(rule);
            return this;
        }

        public RuleSet<T> AddRange(IEnumerable<IRule> rules)
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

        public IEnumerable<T> GetItemsForSymbol<T>(SymbolType requestedSymbolType,
                             IEnumerable<T> items,
                             SymbolDescriptorBase parentSymbolDescriptor)
        {
            var symbolRules = Rules.OfType<IRuleSelectSymbols>();
            var matches = symbolRules
                            .SelectMany(rule => rule.GetItemsForSymbol(requestedSymbolType, items, parentSymbolDescriptor))
                            .Distinct();
            return matches;
        }

        public virtual T GetFirstOrDefaultValue<T>(SymbolDescriptorBase symbolDescriptor,
                                                   IEnumerable<object> items,
                                                   SymbolDescriptorBase parentSymbolDescriptor)
        {
            var flattenedItems = FlattenItems(items);
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

        //public class RuleSet : IEnumerable<RuleBase>
        //{

        //    private readonly List<RuleBase> _rules = new List<RuleBase>();
        //    public IEnumerable<RuleBase> Rules => _rules;

        //    public RuleSet Add(RuleBase rule)
        //    {
        //        _rules.Add(rule);
        //        return this;
        //    }

        //    public IEnumerator<RuleBase> GetEnumerator()
        //        => ((IEnumerable<RuleBase>)_rules).GetEnumerator();

        //    IEnumerator IEnumerable.GetEnumerator()
        //        => ((IEnumerable<RuleBase>)_rules).GetEnumerator();

        //public virtual T GetFirstOrDefaultValue<T>(SymbolDescriptorBase symbolDescriptor, IEnumerable<object> items)
        //{
        //    var flattenedItems = FlattenItems(items);
        //    foreach (var rule in Rules)
        //    {
        //        var (Success, Value) = rule.GetFirstOrDefaultValue<T>(symbolDescriptor, flattenedItems);
        //        if (Success)
        //        {
        //            return Value;
        //        }
        //    }
        //    return default;
        //}

        //public virtual IEnumerable<T> GetAllValues<T>(SymbolDescriptorBase symbolDescriptor, IEnumerable<object> items)
        //{
        //    var flattenedItems = FlattenItems(items);
        //    return Rules
        //        .OfType<IValueRule<T>>()
        //        .SelectMany(r => r.GetAllValues(symbolDescriptor, flattenedItems))
        //        .Distinct();
        //}

        //public virtual bool IsMatch(SymbolDescriptorBase symbolDescriptor, IEnumerable<object> items)
        //{
        //    foreach (var rule in Rules)
        //    {
        //        var success = rule.IsMatch(symbolDescriptor, items);
        //        if (success)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //public virtual IEnumerable<T> GetNonDefaultMatches<T>(SymbolDescriptorBase descriptor,
        //                                     IEnumerable<T> items,
        //                                     SymbolDescriptorBase parentSymbolDescriptor)
        //{
        //    var flattenedItems = FlattenItems(items);

        //    var list = new List<T>();
        //    foreach (var rule in Rules)
        //    {
        //        var values = rule.GetNonDefaultMatches<T>(descriptor, flattenedItems);
        //        list.AddRange(values.OfType<T>());
        //    }
        //    return list.Distinct();
        //}

        //// Do we need GetAllFromFirstMatchingRule?


        //public abstract void UseStandard();
        //public virtual IEnumerable<string> RuleDescriptions
        //    => Rules.Select(r => r.RuleDescription);

        //internal void AddRange(Func<RuleSet, RuleSet> setStandardArgumentRules,
        //                       SymbolType symbolType)
        //{
        //    var tempRules = new RuleSet();
        //    setStandardArgumentRules(tempRules);
        //    foreach (var rule in tempRules)
        //    {
        //        Add(rule.WithSymbolType(symbolType));
        //    }
        //}



    }
}
