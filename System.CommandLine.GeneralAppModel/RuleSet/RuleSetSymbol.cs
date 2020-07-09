using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel
{
    public abstract class RuleSetSymbol : RuleSetBase
    {

        public RuleGroup<IRuleGetValue<string>> DescriptionRules { get; } = new RuleGroup<IRuleGetValue<string>>();

        /// <summary>
        /// Some NameRules will also morph the names. When morphing names, only those name rules with IRuleMorphValue<string>  will be used
        /// </summary>
        public RuleGroup<IRuleGetValue<string>> NameRules { get; } = new RuleGroup<IRuleGetValue<string>>();
        public RuleGroup<IRuleGetValues<string[]>> AliasRules { get; } = new RuleGroup<IRuleGetValues<string[]>>();
        public RuleGroup<IRuleGetValue<bool>> IsHiddenRules { get; } = new RuleGroup<IRuleGetValue<bool>>();

        public override void ReplaceAbstractRules(DescriptorMakerSpecificSourceBase tools)
        {
            DescriptionRules.ReplaceAbstractRules(tools);
            NameRules.ReplaceAbstractRules(tools);
            AliasRules.ReplaceAbstractRules(tools);
            IsHiddenRules.ReplaceAbstractRules(tools);
        }

        public override string Report(int tabsCount)
        {
            return NameRules.ReportRuleGroup(tabsCount, "the name")
                    + DescriptionRules.ReportRuleGroup(tabsCount, "the description")
                    + AliasRules.ReportRuleGroup(tabsCount, "aliases")
                    + IsHiddenRules.ReportRuleGroup(tabsCount, "whether to hide this in the CLI");
        }


    }
}