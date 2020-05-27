using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel
{
    public interface IRuleOptionalValue<T> : IRule
    {
        (bool success, T value) GetOptionalValue(SymbolDescriptorBase symbolDescriptor,
                                                 IEnumerable<object> item,
                                                 SymbolDescriptorBase parentSymbolDescriptor);
    }
}