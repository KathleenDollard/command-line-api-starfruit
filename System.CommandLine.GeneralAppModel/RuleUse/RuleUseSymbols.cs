using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel
{
    public class RuleUseSymbols : IRuleUseSymbol
    {
        public IRuleGetValues<string> DescriptionRule { get; }
        public IRuleGetValues<string> NameRule { get; }
        public IRuleGetValues<IEnumerable<string>> AliasesRule { get; }
        public IRuleGetValues<bool> IsHiddenRule { get; }

        public IEnumerable<T> GetSymbols<T>(SymbolType requestedSymbolType, IEnumerable<T> items, SymbolDescriptorBase parentSymbolDescriptor)
        {
            throw new NotImplementedException();
        }
    }
}