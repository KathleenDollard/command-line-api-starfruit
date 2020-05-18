using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel
{
    public interface IRuleOptionalValue<T>
    {
        (bool success, T value) GetOptionalValue(SymbolDescriptorBase symbolDescriptor,
                                                 IEnumerable<object> item,
                                                 SymbolDescriptorBase parentSymbolDescriptor);
    }
}