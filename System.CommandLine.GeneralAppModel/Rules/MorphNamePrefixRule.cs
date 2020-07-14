using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel
{
    public class MorphNamePrefixRule : RuleBase, IMorphNameRule
    {
        public MorphNamePrefixRule(string stringToAdd,  SymbolType symbolType = SymbolType.All) : base(symbolType)
        {
            StringToAdd = stringToAdd;
        }

        public string StringToAdd { get; }

        public string MorphName(string name, ISymbolDescriptor symbolDescriptor, IEnumerable<object> traits, ISymbolDescriptor parentSymbolDescriptor) 
            => StringToAdd + name;

        public override string RuleDescription<TIRuleSet>() 
            => $"add the {StringToAdd} prefix";
    }
}
