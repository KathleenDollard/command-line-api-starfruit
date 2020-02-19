using System.Collections.Generic;

namespace System.CommandLine.ReflectionModel
{
    public abstract class ModelStrategies
    {
        public abstract IEnumerable<string> StrategyDescriptions { get; }
    }
}