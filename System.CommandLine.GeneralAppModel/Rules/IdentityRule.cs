using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    public class IdentityRule<T> : RuleBase, IRuleGetValue<T>
    {
        public IdentityRule(SymbolType symbolType = SymbolType.All)
              : base(symbolType)
        { }

        public override string RuleDescription
            => "Identity Rule: ";

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
