namespace System.CommandLine.ReflectionAppModel.Tests
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
