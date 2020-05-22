namespace System.CommandLine.ReflectionAppModel.Tests
{
    public class HiddenAttribute : Attribute 
    {
        public HiddenAttribute()
        {
            Value = true;
        }

        public bool Value { get; set; }
    }
}
