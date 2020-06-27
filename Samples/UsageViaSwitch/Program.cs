using System.CommandLine.ReflectionAppModel;

namespace System.CommandLine.Samples
{
    // Class and method are public for testing
    public class Program
    {
       public static int Main(string[] args)
        {
            var typedArg = new CommandLineActivator().CreateInstance<ManageGlobalJson>(args);
            return typedArg switch
            {
                ManageGlobalJson.Find x => OutputAndReturnFind(x),
                ManageGlobalJson.List x => OutputAndReturnList(x),
                ManageGlobalJson.Update x => x.Invoke(),
                ManageGlobalJson.Check x => OutputAndReturnCheck(x),
                _ => throw new NotImplementedException(),
            };
        }

        private static int OutputAndReturnFind(ManageGlobalJson.Find x)
        {
            Console.WriteLine("Find");
            return 5;
        }

        private static int OutputAndReturnList(ManageGlobalJson.List x)
        {
            Console.WriteLine("List");
            return 7;
        }

        private static int OutputAndReturnCheck(ManageGlobalJson.Check x)
        {
            Console.WriteLine("Check");
            return 9;
        }
    }
}
