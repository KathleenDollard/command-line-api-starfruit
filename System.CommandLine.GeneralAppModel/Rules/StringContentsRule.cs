using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    public class StringContentsRule : RuleBase, IRuleGetValue<string>, IRuleMorphValue<string>, IRuleGetItems
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

        public enum StringPosition
        {
            Prefix = 1,
            Suffix,
            Contains
        }

        public string MorphValue(SymbolDescriptorBase symbolDescriptor,
                                object item,
                                string input,
                                SymbolDescriptorBase parentSymbolDescriptor)
        {
            if (!(input is string s) || s is null)
            {
                return null;
            }
            if (!DoesStringMatch(input, Position, CompareTo ))
            {
                return input;
            }
            if (Position == StringPosition.Prefix)
                return s.Substring(CompareTo.Length);
            else if (Position == StringPosition.Suffix)
                return s.Substring(0, s.Length - CompareTo.Length);
            else if (Position == StringPosition.Contains)
                return s.Replace(CompareTo, "");
            else
                throw new ArgumentException("Unexpected position");
        }

        public (bool success, string value) GetFirstOrDefaultValue(SymbolDescriptorBase symbolDescriptor,
                                                                   IEnumerable<object> items,
                                                                   SymbolDescriptorBase parentSymbolDescriptor)
        {
            var matches = items.OfType<string>()
                               .Where(x => DoesStringMatch(x, Position, CompareTo));
            if (matches.Any())
            {
                return (true, matches.First());
            }
            return (false, default);


        }

        public bool DoesStringMatch(string value, StringPosition position, string compareTo)
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
        public IEnumerable<Candidate> GetItems(IEnumerable<Candidate> items, SymbolDescriptorBase parentSymbolDescriptor) 
            => (IEnumerable<Candidate>)items
                            .Where(x=>x.Traits
                                        .OfType<string>()
                                        .Where(x => DoesStringMatch(x, Position, CompareTo))
                                        .Any());

        public override string RuleDescription
            => $"String Contents Rule: {Position} - '{CompareTo}'";
    }

}
