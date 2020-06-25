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
                ManageGlobalJson.Find x => OutputAndReturnFind(),
                ManageGlobalJson.List x => OutputAndReturnList(),
                ManageGlobalJson.Update x => x.Invoke(),
                ManageGlobalJson.Check x => OutputAndReturnCheck(),
                _ => throw new NotImplementedException(),
            };
        }

        private static int OutputAndReturnFind()
        {
            Console.WriteLine("Find");
            return 5;
        }

        private static int OutputAndReturnList()
        {
            Console.WriteLine("List");
            return 7;
        }

        private static int OutputAndReturnCheck()
        {
            Console.WriteLine("Check");
            return 9;
        }
    }
}
