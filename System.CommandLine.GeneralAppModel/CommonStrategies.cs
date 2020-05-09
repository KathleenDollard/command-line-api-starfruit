using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.CommandLine.GeneralAppModel
{
    public static class CommonStrategies
    {
        public static Strategy SetAllStandardRules(this Strategy strategy) 
            => strategy
                .SetStandardArityRules()
                .SetStandardArgumentRules()
                .SetStandardCommandRules()
                .SetStandardDescriptionRules()
                .SetStandardNameRules()
                .SetStandardRequiredRules()
                .SetStandardSubCommandRules();
    }
}
