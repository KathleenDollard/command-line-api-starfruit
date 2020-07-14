using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel
{
    public class MorphNameSuffixRule : RuleBase, IMorphNameRule
    {
        public MorphNameSuffixRule(string stringToAdd, SymbolType symbolType = SymbolType.All) : base(symbolType)
        {
            StringToAdd = stringToAdd;
        }

        public string StringToAdd { get; }

        public string MorphName(string name, ISymbolDescriptor symbolDescriptor, IEnumerable<object> traits, ISymbolDescriptor parentSymbolDescriptor)
            => name + StringToAdd;

        public override string RuleDescription<TIRuleSet>()
            => $"add the {StringToAdd} suffix";
    }
}
