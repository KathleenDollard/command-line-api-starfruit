using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;

namespace System.CommandLine.GeneralAppModel
{
    public class Strategy
    {
        public Storage Settable { get; } = new Storage();
        public IRuleSelectSymbols SelectSymbolRules { get; }
        public IRuleUseArgument ArgumentRules { get; }
        public IRuleUseOption OptionRules { get; }
        public IRuleUseCommand CommandRules { get; }

        public class Storage
        {
            public RuleUseSymbols SelectSymbolRules { get; }
            public RuleUseArgument ArgumentRules { get; }
            public RuleUseOption OptionRules { get; }
            public RuleUseCommand CommandRules { get; }

        }


        //public RuleSet NameRules { get; } = new RuleSet();
        //public RuleSet DescriptionRules { get; } = new RuleSet();
        //public RuleSet AliasRules { get; } = new RuleSet();
        //public RuleSet RequiredRules { get; } = new RuleSet();
        //public RuleSet HiddenRules { get; } = new RuleSet();
        //public RuleSet ArgumentRules { get; } = new RuleSet();
        //public RuleSet CommandRules { get; } = new RuleSet();
        //public RuleSet OptionRules { get; } = new RuleSet();
        //public RuleSet SubCommandRules { get; } = new RuleSet();
        //public RuleSet ArityRules { get; } = new RuleSet();
        //public RuleSet DefaultRules { get; } = new RuleSet();


    }
}
