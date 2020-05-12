using System;
using System.Collections.Generic;
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
        public RuleSet<IRuleSelectSymbols> SelectSymbolRules { get; private set; } = new RuleSet<IRuleSelectSymbols>();
    }
}
