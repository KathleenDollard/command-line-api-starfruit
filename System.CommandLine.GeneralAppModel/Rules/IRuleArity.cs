using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel
{
    public interface IRuleArity : IRule
    {
        (bool success, uint minimumCount, uint maximumCount) GetArity(SymbolDescriptorBase symbolDescriptor,
                                                        IEnumerable<object> item,
                                                        SymbolDescriptorBase parentSymbolDescriptor);

        interface IFoo { }
    }

}
