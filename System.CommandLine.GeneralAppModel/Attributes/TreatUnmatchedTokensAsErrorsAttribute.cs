namespace System.CommandLine.GeneralAppModel
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Struct)]
    public class TreatUnmatchedTokensAsErrorsAttribute : Attribute
    {
        public TreatUnmatchedTokensAsErrorsAttribute(bool value = true)
        {
            Value = value;
        }

        public bool Value { get; set; }
    }
}
