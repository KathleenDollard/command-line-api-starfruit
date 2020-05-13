namespace System.CommandLine.GeneralAppModel
{
    public interface IRuleMorphValue<T> : IRule
    {
        T MorphValue(SymbolDescriptorBase symbolDescriptor,
                        object item,
                        T input,
                        SymbolDescriptorBase parentSymbolDescriptor);
    }

}
