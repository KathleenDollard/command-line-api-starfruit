using System.Collections.Generic;
using System.CommandLine.Parsing;

namespace System.CommandLine.GeneralAppModel
{
    public class MorphNameCasingRule : RuleBase, IMorphNameRule
    {
        public MorphNameCasingRule(StringCasing stringCasing, SymbolType symbolType = SymbolType.All) : base(symbolType)
        {
            Casing = stringCasing;
        }

        public StringCasing Casing { get; }

        public string MorphName(string name, ISymbolDescriptor symbolDescriptor, IEnumerable<object> traits, ISymbolDescriptor parentSymbolDescriptor)
        {
            if (name is null)
            {
                return name;
            }
            // Order of operations matter below. Kebab and anything else acting on caps must be first
            name = Casing.HasFlag(StringCasing.Kebab)
                    ? name.ToKebabCase()
                    : name;
            name = Casing.HasFlag(StringCasing.Lower)
                    ? name.ToLower()
                    : name;
            name = Casing.HasFlag(StringCasing.Upper)
                    ? name.ToUpper()
                    : name;

            return name;
        }

        public override string RuleDescription<TIRuleSet>()
            => $"change to {Casing} casing";

        [Flags]
        public enum StringCasing
        {
            None = 0,
            Lower = 0b0001,
            Upper = 0b0010,
            Kebab = 0b0100,

            LowerKebab = 0b0101
        }
    }
}
