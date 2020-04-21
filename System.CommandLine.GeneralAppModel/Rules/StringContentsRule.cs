using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    public class StringContentsRule : IdentityRule<string>
    {
        public StringContentsRule(StringPosition position,
                                 string compareTo,
                                 SymbolType symbolType = SymbolType.All)
        : base(symbolType)
        {
            Position = position;
            CompareTo = compareTo;
        }

        public StringPosition Position { get; }
        public string CompareTo { get; }

        public override bool HasMatch(SymbolType symbolType ,object[] items)
        {
            return items
                    .OfType<IdentityWrapper<string>>()
                    .Select(w => Check(w.Value, Position, CompareTo))
                    .Where(x=>x)
                    .Any();

            static bool Check(string value,StringPosition position, string compareTo)
            {
                if (value is null)
                {
                    return false;
                }
                return position switch
                {
                    StringPosition.Prefix => value.StartsWith(compareTo),
                    StringPosition.Suffix => value.EndsWith(compareTo),
                    StringPosition.Contains => value.Contains(compareTo),
                    _ => throw new ArgumentException("Unexpected position")
                };
            }
        }

        private string Strip(string s)
        {
            return s is null
                ? null
                : (Position switch
                {
                    StringPosition.Prefix => s.Substring(CompareTo.Length),
                    StringPosition.Suffix => s.Substring(s.Length, s.Length - CompareTo.Length),
                    StringPosition.Contains => s.Replace(CompareTo, ""),
                    _ => throw new ArgumentException("Unexpected position")
                });
        }

        public enum StringPosition
        {
            Prefix = 1,
            Suffix,
            Contains
        }

        protected override IEnumerable<string> GetMatchingItems(SymbolType symbolType, object[] items)
        {
            return SymbolType != SymbolType.All && SymbolType != symbolType
                ? Array.Empty<string>()
                : items
                    .OfType<IdentityWrapper<string>>()
                    .Select(w => Conversions.To<string>(Strip( w.Value.ToString())));
        }

        public override string RuleDescription
            => $"String Contents: {Position} - '{CompareTo}'";
    }
}
