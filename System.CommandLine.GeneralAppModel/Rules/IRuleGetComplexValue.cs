using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel
{
    public interface IRuleGetComplexValue : IRuleGetValue<Dictionary<string, object>>
    {
        (bool success, Dictionary<string, object> value) GetComplexValue(SymbolDescriptor symbolDescriptor,
                                 IEnumerable<object> item,
                                 SymbolDescriptor parentSymbolDescriptor);

    }

}
