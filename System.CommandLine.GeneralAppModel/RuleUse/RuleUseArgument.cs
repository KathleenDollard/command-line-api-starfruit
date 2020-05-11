using System.CommandLine.GeneralAppModel.Descriptors;

namespace System.CommandLine.GeneralAppModel
{
    public class RuleUseArgument : RuleUseSymbols, IRuleUseArgument
    {
        public IRuleGetValues<ArityDescriptor> ArityRule { get; }
        public IRuleGetValues<bool> RequiredRule { get; }
        public IRuleGetValues<Type> SpecialArgumentType { get; }
    }
}