using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel
{
    public interface IRuleOptionalValue<T> : IRule
    {
        (bool success, T value) GetOptionalValue(ISymbolDescriptor symbolDescriptor,
                                                 IEnumerable<object> item,
                                                 ISymbolDescriptor parentSymbolDescriptor);
    }
}