namespace System.CommandLine.ReflectionAppModel.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Struct)]
    public class TreatUnmatchedTokensAsErrorsAttribute : Attribute
    {
        public TreatUnmatchedTokensAsErrorsAttribute()
        {
            Value = true;
        }

        public bool Value { get; set; }
    }
}
