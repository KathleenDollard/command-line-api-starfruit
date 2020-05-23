using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel
{
    public interface IRuleGetValue<T> : IRule
    {
        (bool success, T value) GetFirstOrDefaultValue(SymbolDescriptorBase symbolDescriptor,
                                 IEnumerable<object> item,
                                 SymbolDescriptorBase parentSymbolDescriptor);

    }

    public interface IRuleGetValues<T> : IRuleGetValue<T>
    {

        IEnumerable<T> GetAllValues(SymbolDescriptorBase symbolDescriptor,
                                    IEnumerable<object> traits,
                                    SymbolDescriptorBase parentSymbolDescriptor);
    }

}
