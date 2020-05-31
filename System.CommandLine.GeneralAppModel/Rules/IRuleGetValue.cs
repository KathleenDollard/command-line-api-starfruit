using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel
{
    public interface IRuleGetValue<T> : IRule
    {
        (bool success, T value) GetFirstOrDefaultValue(ISymbolDescriptor symbolDescriptor,
                                 IEnumerable<object> item,
                                 ISymbolDescriptor parentSymbolDescriptor);

    }

    public interface IRuleGetValues<T> : IRuleGetValue<T>
    {

        IEnumerable<T> GetAllValues(ISymbolDescriptor symbolDescriptor,
                                    IEnumerable<object> traits,
                                    ISymbolDescriptor parentSymbolDescriptor);
    }

}
