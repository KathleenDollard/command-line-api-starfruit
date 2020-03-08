using System.Collections.Generic;

namespace System.CommandLine.ReflectionModel.Strategies
{
    public abstract class RuleStrategies
    {
        public abstract IEnumerable<string> StrategyDescriptions { get; }
    }
}