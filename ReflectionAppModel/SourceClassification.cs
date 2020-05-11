//using System;
//using System.Collections.Generic;
//using System.CommandLine.GeneralAppModel;
//using System.Linq;
//using System.Text;

//namespace System.CommandLine.ReflectionAppModel
//{
//    /// <summary>
//    /// This class supports rule based classification. It's current use is for Argument/Option/SubCommand
//    /// identification, but it was easier to give flexibility in operation order with a general solution. 
//    /// This is implemented as a separate class to facilitate caching. It is not Lazy on the assumption
//    /// that if you creat a classifier you are extremely likely to use its results.
//    /// <br/>
//    /// RuleSets are run in order and the Remaining property returns all values that do not match any passed
//    /// RuleSet. This will be common - for example the standard strategy identifies arguments and sub-commands
//    /// and treats everything else as an option.
//    /// </summary>
//    /// <typeparam name="T"></typeparam>
//    public abstract class SourceClassification<T>
//    {
//        private readonly IEnumerable<T> items;
//        private Dictionary<RuleSet<bool>, T> classifiedItems;

//        public SourceClassification(IEnumerable<T> items,
//                                    params RuleSet<bool>[] rulesets)
//        {
//            this.items = items;
//        }

//        public IEnumerable<T> MatchesRuleSet(RuleSet<bool> ruleSet)
//        {

//        }

//        public IEnumerable<T> Remaining()
//        {

//        }

//    }
//}


//namespace System.CommandLine.ReflectionAppModel
//{
//    /// <summary>
//    /// This class supports separating a list into those things used for Arguments, Options 
//    /// and SubCommands. Unless overridden in a derived class, the default is to recognize
//    /// ArgumentItems and OptionItems and the rest are SubCommands. 
//    /// </summary>
//    /// <typeparam name="T"></typeparam>
//    public abstract class SourceClassification<T>
//    {
//        internal SourceClassification(Strategy strategy, IEnumerable<T> sourceItems)
//        {
//            Classify(strategy, sourceItems);
//        }

//        protected virtual void Classify(Strategy strategy, IEnumerable<T> sourceItems)
//        {
//            ArgumentItems = sourceItems.Where(p => Match(p, strategy.ArgumentRules)).ToList();
//            SubCommandItems = sourceItems.Where(p => Match(p, strategy.CommandRules)).ToList();
//            OptionItems = sourceItems.Except(ArgumentItems).Except(SubCommandItems).ToList();
//        }

//        internal IEnumerable<T> ArgumentItems { get; private set; }
//        internal IEnumerable<T> OptionItems { get; private set; }
//        internal IEnumerable<T> SubCommandItems { get; private set; }

//        protected abstract bool Match(T item, RuleSet<string> rules);

//    }
//}
