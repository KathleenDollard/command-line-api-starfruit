namespace System.CommandLine.ReflectionAppModel.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Class | AttributeTargets.Struct |
                    AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property)]
    public class AliasesAttribute : Attribute
    {
        public AliasesAttribute(params string[] aliases)
        {
            Aliases = aliases;
        }

        public string[] Aliases { get; }
    }
}
