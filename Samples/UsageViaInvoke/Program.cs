namespace System.CommandLine.Samples
{
    class Program
    {
        static int Main(string[] args)
        {
            return CommandLineActivator.Invoke<ManageGlobalJson>(args);
        }
    }
}
