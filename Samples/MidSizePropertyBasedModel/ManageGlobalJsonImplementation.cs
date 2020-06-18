using System.CommandLine.ReflectionAppModel;
using System.IO;

namespace System.CommandLine.Samples

{
    public  class ManageGlobalJsonImplementation
    {
        public static int Find(DirectoryInfo startPathArg, VerbosityLevel verbosity)
        {
            Console.WriteLine($"Run Find from {startPathArg} with verbosity {verbosity}");
            return 0;
        }

        public static int List(DirectoryInfo startPathArg, VerbosityLevel verbosity, string output)
        {
            Console.WriteLine($"Run List from {startPathArg} with verbosity {verbosity}");
            return 0;
        }

        public static int Update(DirectoryInfo startPathArg,
                                 VerbosityLevel verbosity,
                                 FileInfo filePath,
                                 string oldVersion,
                                 string newVersion,
                                 bool allowPrerelease,
                                 RollForward rollForward)
        {
            var prereleaseText = allowPrerelease ? "allowing prerelease" : "disallowing prerelease";
            Console.WriteLine(@$"Run Update from {startPathArg} for {filePath} changing '{oldVersion}' to '{newVersion}' " +
                                    $"{prereleaseText} and rollforward to {rollForward} with verbosity {verbosity} ");
            return 7;
        }

        public static int Check(DirectoryInfo startPathArg, VerbosityLevel verbosity)
        {
            Console.WriteLine($"Run Check from {startPathArg} with verbosity {verbosity}");
            return 0;
        }
    }
}
