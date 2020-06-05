namespace System.CommandLine.GeneralAppModel
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct |
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
