using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel
{
    public interface IRuleGetItems : IRule
    {
        SymbolType SymbolType { get; }
        IEnumerable<Candidate> GetItems(IEnumerable<Candidate> candidates,
                                   SymbolDescriptorBase parentSymbolDescriptor);
    }

}
