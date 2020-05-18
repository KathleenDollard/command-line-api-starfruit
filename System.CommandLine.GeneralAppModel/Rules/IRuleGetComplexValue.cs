using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel
{
    public interface IRuleGetComplexValue : IRuleGetValue<Dictionary<string, object>>
    {
        (bool success, Dictionary<string, object> value) GetComplexValue(SymbolDescriptorBase symbolDescriptor,
                                 IEnumerable<object> item,
                                 SymbolDescriptorBase parentSymbolDescriptor);

    }

}
