namespace System.CommandLine.ReflectionAppModel.Attributes
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
