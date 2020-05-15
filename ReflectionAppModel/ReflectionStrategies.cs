using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.Text;

namespace System.CommandLine.ReflectionAppModel
{
    public static class ReflectionStrategies
    {
        public static Strategy SetReflectionRules(this Strategy strategy)
            => strategy
                .SetGeneralRules()
                .SetStandardAvailableCandidatesRules();
    }
}
