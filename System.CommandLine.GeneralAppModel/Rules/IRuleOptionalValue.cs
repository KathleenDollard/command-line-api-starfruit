using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.CommandLine.GeneralAppModel
{
    public interface IRuleOptionalValue<T> : IRule
    {
        // TODO: How do I say T may be null when Success is false
        (bool success, T value) GetOptionalValue(ISymbolDescriptor symbolDescriptor,
                                                 IEnumerable<object> item,
                                                 ISymbolDescriptor parentSymbolDescriptor);
    }
}