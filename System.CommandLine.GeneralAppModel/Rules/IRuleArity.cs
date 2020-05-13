using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel
{
    public interface IRuleArity : IRule
    {
        (uint MinimumCount, uint MaximumCount) GetArity(SymbolDescriptorBase symbolDescriptor,
                                                        IEnumerable<object> item,
                                                        SymbolDescriptorBase parentSymbolDescriptor);
    }

}
