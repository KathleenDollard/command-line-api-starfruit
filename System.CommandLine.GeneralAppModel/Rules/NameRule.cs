using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel
{
    /// <summary>
    /// This class exists to give a better name for the StringContentsRule
    /// </summary>
    public class NamePatternRule : StringContentsRule
    {
        public NamePatternRule(StringPosition position,
                        string compareTo,
                        SymbolType symbolType = SymbolType.All)
             : base(position, compareTo, symbolType) { }

        protected override string NameOrString => "name";

        protected override string MorphValueInternal(SymbolDescriptorBase symbolDescriptor,
                      object item,
                      string input,
                      SymbolDescriptorBase parentSymbolDescriptor)
         => MorphValue(symbolDescriptor, item, input, parentSymbolDescriptor);

        public string MorphValue(SymbolDescriptorBase symbolDescriptor,
                         object item,
                         string input,
                         SymbolDescriptorBase parentSymbolDescriptor)
        {
            if (!(input is string s) || s is null)
            {
                return null;
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


    }
}
