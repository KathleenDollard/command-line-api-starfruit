using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    public class IdentityRule<T> : RuleBase<T>
    {
        public IdentityRule(SymbolType symbolType = SymbolType.All)
              : base(symbolType)
        { }

        public override string RuleDescription
            => "Identity Rule";

        public override bool HasMatch(SymbolDescriptorBase symbolDescriptor, IEnumerable<object> items)
        {
            return items
                    .Any();

        }

        protected override IEnumerable<T> GetMatchingItems(SymbolDescriptorBase symbolDescriptor, IEnumerable<object> items)
        {
            return SymbolType != SymbolType.All && SymbolType != symbolDescriptor.SymbolType
                ? Array.Empty<T>()
                : items
                    .OfType<IdentityWrapper<T>>()
                    .Select(w => w.Value);
        }
    }

    /// <summary>
    /// The identity wrapper distinguishes the intent of supporting the identity rule from the intent of 
    /// supporting StringContentRules and supports non-string identity types. "Identity" here is the sense 
    /// of "Just use this value" not the sense of "This value is the name", although it often is the name.
    /// </summary>
    public class IdentityWrapper
    { }

    public class IdentityWrapper<T> : IdentityWrapper 
    {
        public IdentityWrapper(T value)
        {
            Value = value;
        }
        public T Value { get;  }
    }
}
