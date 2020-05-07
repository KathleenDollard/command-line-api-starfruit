namespace System.CommandLine.ReflectionAppModel
{
    public enum SymbolType
    {
        Command = 0b0001,
        Option = 0b0010,
        Argument = 0b0100,
        All = 0b0111
    }
}
