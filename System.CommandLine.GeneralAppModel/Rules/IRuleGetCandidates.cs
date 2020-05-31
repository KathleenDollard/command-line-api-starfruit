using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel
{
    public interface IRuleGetCandidates : IRule
    {
        IEnumerable<Candidate> GetCandidates(IEnumerable<Candidate> candidates,
                                   ISymbolDescriptor parentSymbolDescriptor);
   
    }

    public interface IRuleGetAvailableCandidates : IRule
    {
        IEnumerable<Candidate> GetChildCandidates(ISymbolDescriptor parentSymbolDescriptor);
    }

}
