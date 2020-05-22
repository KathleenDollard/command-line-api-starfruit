namespace System.CommandLine.ReflectionAppModel.Tests
{
    public class RequiredAttribute : Attribute 
    {
        public RequiredAttribute()
        {
            Value = true;
        }

        public bool Value { get; set; }
    }
}
