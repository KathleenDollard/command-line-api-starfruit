using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    public class IdentityRule<T> : RuleBase, IRuleGetValue<T>
    {
        public IdentityRule(SymbolType symbolType = SymbolType.All)
              : base(symbolType)
        { }


        public (bool success, T value) GetFirstOrDefaultValue(SymbolDescriptorBase symbolDescriptor,
                                                              IEnumerable<object> items,
                                                              SymbolDescriptorBase parentSymbolDescriptor)
        {
            var matches = items.OfType<IdentityWrapper<T>>();
            if (matches.Any())
            {
                return (true, matches.First().Value);
            }
            return (false, default);
        }
         public override string RuleDescription<TIRuleSet>()
           => "The identity, usually the name.";
   }

    public class IdentityWrapper<T> : IdentityWrapper
    {
        public IdentityWrapper(T value)
        {
            Value = value;
        }
        public T Value { get; }
    }
}
