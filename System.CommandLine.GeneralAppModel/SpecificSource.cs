using System;
using System.Collections.Generic;
using System.Text;

namespace System.CommandLine.GeneralAppModel
{
    public abstract class SpecificSource
    {
        public static SpecificSource Tools { get; internal set; }

        public abstract Candidate GetCandidate( object item);
        public abstract Type GetArgumentType(Candidate candidate);
        public abstract IEnumerable<Candidate> GetChildCandidates(Strategy strategy, SymbolDescriptorBase commandDescriptor);
    }
}
