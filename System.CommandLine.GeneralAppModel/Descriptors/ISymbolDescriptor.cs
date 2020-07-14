using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel
{
    public interface ISymbolDescriptor
    {
        SymbolType SymbolType { get; }
        object? Raw { get; }
        IEnumerable<string>? Aliases { get; }
        //TODO: Understand raw aliases: public IReadOnlyList<string> RawAliases { get; }
        string? Description { get; }
        string? Name { get; }
        string? CommandLineName { get; }
        string OriginalName { get; }
        bool IsHidden { get; set; }
        string Report(int tabsCount, VerbosityLevel verbosity);
    }
}