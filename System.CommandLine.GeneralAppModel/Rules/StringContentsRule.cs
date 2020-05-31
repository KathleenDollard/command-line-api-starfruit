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

        protected virtual string MorphValueInternal(ISymbolDescriptor symbolDescriptor,
                                                    object item,
                                                    string input,
                                                    ISymbolDescriptor parentSymbolDescriptor)
        {
            if (!(input is string s) || s is null)
            {
                return string.Empty;
            }
            if (!DoesStringMatch(input, Position, CompareTo))
            {
                return input;
            }
            if (Position == StringPosition.BeginsWith)
                return s.Substring(CompareTo.Length);
            else if (Position == StringPosition.EndsWith)
                return s.Substring(0, s.Length - CompareTo.Length);
            else if (Position == StringPosition.Contains)
                return s.Replace(CompareTo, "");
            else
                throw new ArgumentException("Unexpected position");
        }

        public virtual (bool success, string value) GetFirstOrDefaultValue(ISymbolDescriptor symbolDescriptor,
                                                                           IEnumerable<object> traits,
                                                                           ISymbolDescriptor parentSymbolDescriptor)
        {
            var matches = traits.OfType<IdentityWrapper<string>>()
                               .Where(x => DoesStringMatch(x.Value, Position, CompareTo));
            if (matches.Any())
            {
                return (true, matches
                               .Select(x => MorphValueInternal(symbolDescriptor, x, x.Value, parentSymbolDescriptor))
                               .First());
            }
            return (false, string.Empty);


        }



        protected bool DoesStringMatch(string value, StringPosition position, string compareTo)
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

        public IEnumerable<Candidate> GetCandidates(IEnumerable<Candidate> candidates, ISymbolDescriptor parentSymbolDescriptor)
            => candidates
                    .Where(x => x.Traits
                                .OfType<IdentityWrapper<string>>()
                                .Where(x => DoesStringMatch(x.Value, Position, CompareTo))
                                .Any());

        public override string RuleDescription<TIRuleSet>()
         => typeof(IRuleGetValue<string>).IsAssignableFrom(typeof(TIRuleSet))
               ? $@"If {NameOrString} {Position.ToString().FriendlyFromPascal()} '{CompareTo}', remove '{CompareTo}'"
               : $@"the {NameOrString} {Position.ToString().FriendlyFromPascal()} '{CompareTo}'";

        protected virtual string NameOrString => "string";

    }

}
