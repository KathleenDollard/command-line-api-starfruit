using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.Linq;
using System.Text;

namespace System.CommandLine.ReflectionAppModel
{
    /// <summary>
    /// This class supports separating a list into those things used for Arguments, Options 
    /// and SubCommands. Unless overridden in a derived class, the default is to recognize
    /// ArgumentItems and OptionItems and the rest are SubCommands. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SourceClassification<T>
    {
        internal SourceClassification(Strategy strategy, IEnumerable<T> sourceItems)
        {
            Classify(strategy, sourceItems);
        }

        protected virtual void Classify(Strategy strategy, IEnumerable<T> sourceItems)
        {
            ArgumentItems = sourceItems.Where(p => Match(p, strategy.ArgumentRules)).ToList();
            SubCommandItems = sourceItems.Where(p => Match(p, strategy.CommandRules)).ToList();
            OptionItems = sourceItems.Except(ArgumentItems).Except(SubCommandItems).ToList();
        }

        internal IEnumerable<T> ArgumentItems { get; private set; }
        internal IEnumerable<T> OptionItems { get; private set; }
        internal IEnumerable<T> SubCommandItems { get; private set; }

        protected abstract bool Match(T item, RuleSet<string> rules);

    }
}
