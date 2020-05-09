using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.Linq;
using System.Text;

namespace ReflectionAppModel
{
    internal abstract class SourceClassification<T>
    {
        internal SourceClassification(Strategy strategy, IEnumerable<T> sourceItems)
        {
            ArgumentItems = sourceItems.Where(p => Match(p, strategy.ArgumentRules));
            SubCommandItems = sourceItems.Where(p => Match(p, strategy.CommandRules));
            OptionItems = sourceItems.Except(ArgumentItems).Except(SubCommandItems).ToList();
        }
        internal IEnumerable<T> ArgumentItems { get; }
        internal IEnumerable<T> OptionItems { get; }
        internal IEnumerable<T> SubCommandItems { get; }

        protected abstract bool Match(T item, RuleSet<string> rules);
    }
}
