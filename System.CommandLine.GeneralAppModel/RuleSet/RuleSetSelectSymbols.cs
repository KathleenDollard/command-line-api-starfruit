using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.CommandLine.GeneralAppModel
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// In order to support remaining items (those not in the other two groups) being identifiable
    /// order matters, and the last can have a RemainingItemRule
    /// </remarks>
    public class RuleSetSelectSymbols
    {
        public RuleSet<IRuleGetItems> Rules { get; private set; } = new RuleSet<IRuleGetItems>();
        public List<string> NamesToIgnore { get; } = new List<string>();

        public IEnumerable<Candidate> GetItems(SymbolType symbolType,
                                               SymbolDescriptorBase commandDescriptor,
                                               IEnumerable<Candidate> candidates)
        {
            return Rules
                    .OfType<IRuleGetItems>()
                    .Where(x => x.SymbolType == symbolType)
                    .SelectMany(r => r.GetItems(candidates, commandDescriptor))
                    .ToList();
        }
    }
}
