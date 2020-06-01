using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.GeneralAppModel
{
    public abstract class RuleSetBase
    {
        public abstract string Report(int tabsCount);

        public abstract void ReplaceAbstractRules(SpecificSource tools);

    }
}
