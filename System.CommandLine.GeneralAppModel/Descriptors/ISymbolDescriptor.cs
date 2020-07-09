namespace System.CommandLine.GeneralAppModel
{
    public interface ISymbolDescriptor
    {
        SymbolType SymbolType { get; }
        string Report(int tabsCount);
    }
}