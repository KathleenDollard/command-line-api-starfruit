using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;

namespace System.CommandLine.GeneralAppModel
{
    public class Strategy
    {
        public RuleSetSelectSymbols SelectSymbolRules { get; } = new RuleSetSelectSymbols();
        public RuleSetArgument ArgumentRules { get; } = new RuleSetArgument();
        public RuleSetOption OptionRules { get; } = new RuleSetOption();
        public RuleSetCommand CommandRules { get; } = new RuleSetCommand();

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
