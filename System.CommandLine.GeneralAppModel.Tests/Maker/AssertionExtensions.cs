namespace System.CommandLine.GeneralAppModel.Tests.Maker
{
    public static class AssertionExtensions
    {
        public static CommandAssertions Should(this Command instance)
        {
            return new CommandAssertions(instance);
        }

        public static OptionAssertions Should(this Option instance)
        {
            return new OptionAssertions(instance);
        }

        public static ArgumentAssertions Should(this Argument instance)
        {
            return new ArgumentAssertions(instance);
        }
    }
}
