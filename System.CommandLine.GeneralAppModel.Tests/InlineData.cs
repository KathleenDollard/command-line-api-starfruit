namespace System.CommandLine.GeneralAppModel.Tests
{
    public class InlineData
    {
        public Type Type { get; }

        public InlineData(Type type)
        {
            Type = type;
        }
    }
    public class InlineMethod : InlineData
    {
        public string MethodName { get; }

        public InlineMethod(Type type, string methodName)
            : base(type)
        {
            MethodName = methodName;
        }
    }
    public class InlineType : InlineData
    {
        public InlineType(Type type) 
            : base(type)
        {
        }
    }
}