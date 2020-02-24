using System.Collections.Generic;

namespace System.CommandLine.ReflectionModel.ModelStrategies
{
    public abstract class ModelStrategies
    {
        public abstract IEnumerable<string> StrategyDescriptions { get; }

        public abstract void UseStandard();

    }
}