namespace System.CommandLine.Samples
{
    // Class and method are public for testing
    public class Program
    {
       public static int Main(string[] args)
        {
            return CommandLineActivator.Invoke<ManageGlobalJson>(args);
        }
    }
}
