using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    public class StringContentsRule : RuleBase, IRuleGetValue<string>, IRuleGetCandidates
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
            BeginsWith = 1,
            EndsWith,
            Contains
        }

        protected virtual string MorphValueInternal(SymbolDescriptorBase symbolDescriptor,
                         object item,
                         string input,
                         SymbolDescriptorBase parentSymbolDescriptor)
            => input;

        public virtual (bool success, string value) GetFirstOrDefaultValue(SymbolDescriptorBase symbolDescriptor,
                                                                   IEnumerable<object> items,
                                                                   SymbolDescriptorBase parentSymbolDescriptor)
        {
            var matches = items.OfType<string>()
                               .Where(x => DoesStringMatch(x, Position, CompareTo));
            if (matches.Any())
            {
                return (true, matches
                               .Select(x => MorphValueInternal(symbolDescriptor, x, x, parentSymbolDescriptor))
                               .First());
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
                StringPosition.BeginsWith => value.StartsWith(compareTo),
                StringPosition.EndsWith => value.EndsWith(compareTo),
                StringPosition.Contains => value.Contains(compareTo),
                _ => throw new ArgumentException("Unexpected position")
            };
        }
        public IEnumerable<Candidate> GetCandidates(IEnumerable<Candidate> items, SymbolDescriptorBase parentSymbolDescriptor) 
            => (IEnumerable<Candidate>)items
                            .Where(x => x.Traits
                                        .OfType<string>()
                                        .Where(x => DoesStringMatch(x, Position, CompareTo))
                                        .Any());

        public override string RuleDescription<TIRuleSet>()
         => typeof(IRuleGetValue<string>).IsAssignableFrom(typeof(TIRuleSet))
               ? $@"If {NameOrString} {Position.ToString().FriendlyFromPascal()} '{CompareTo}', remove '{CompareTo}'"
               : $@"the {NameOrString} {Position.ToString().FriendlyFromPascal()} '{CompareTo}'";

        protected virtual string NameOrString => "string";

    }

}
