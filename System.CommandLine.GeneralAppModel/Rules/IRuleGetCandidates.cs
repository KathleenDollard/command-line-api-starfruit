using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel
{
    public interface IRuleGetCandidates : IRule
    {
        SymbolType SymbolType { get; }
        IEnumerable<Candidate> GetCandidates(IEnumerable<Candidate> candidates,
                                   SymbolDescriptorBase parentSymbolDescriptor);
   
    }

    public interface IRuleGetAvailableCandidates : IRule
    {
        IEnumerable<Candidate> GetChildCandidates(SymbolDescriptorBase parentSymbolDescriptor);
    }

}
