namespace System.CommandLine.GeneralAppModel.Tests.Maker
{
    public static class MakerAssertionsExtensions
    {
        public static MakerCommandAssertions Should(this Command instance)
        {
            return new MakerCommandAssertions(instance);
        }

        public static MakerOptionAssertions Should(this Option instance)
        {
            return new MakerOptionAssertions(instance);
        }

        public static MakerArgumentAssertions Should(this Argument instance)
        {
            return new MakerArgumentAssertions(instance);
        }
    }
}
