namespace System.CommandLine.GeneralAppModel
{
    public class RuleSetCommand : RuleSetSymbol
    {
        public RuleGroup<IRuleGetValue<bool>> TreatUnmatchedTokensAsErrorsRules { get; } = new RuleGroup<IRuleGetValue<bool>>();
        public RuleGroup<IRuleOptionalValue<InvokeMethodInfo>> InvokeMethodRules { get; } = new RuleGroup<IRuleOptionalValue<InvokeMethodInfo>>();

        public override void ReplaceAbstractRules(DescriptorMakerSpecificSourceBase tools)
        {
            base.ReplaceAbstractRules(tools);
            TreatUnmatchedTokensAsErrorsRules.ReplaceAbstractRules(tools);
        }
    }
}