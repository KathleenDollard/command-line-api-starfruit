namespace System.CommandLine.ReflectionAppModel.Tests
{
    public  class AliasesAttribute : Attribute
    {
        public AliasesAttribute(params string[] aliases)
        {
            Aliases = aliases;
        }

        public string[] Aliases { get; }
    }
}
