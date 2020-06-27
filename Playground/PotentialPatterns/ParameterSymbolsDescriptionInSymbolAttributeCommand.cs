using System.CommandLine.GeneralAppModel;
using System.CommandLine.ReflectionAppModel;
using System.IO;

// SubCommands are via derived classes. This makes binding and accessing data in parent classes super easy.
// Options and arguments are via parameters on invoke methods. This probably won't work with "switch" style Main. Data can still be provided on parent classes based on properties (doesn't work yet).
// Using a symbol (Command/Argument/Option) based attribute. 
// Description in that attribute.

namespace Playground
{
    [Command(Description = "Future global tool to manage global.json. See https://aka.ms/globaljson.")]
    public class ParameterSymbolsDescriptionInSymbolAttributeCommand
    {
        [Argument(Description = "Location where processing should begin.")]
        public DirectoryInfo StartPath { get; set; }

        [Option(Aliases = new string[] { "v" }, Description = "Verbosity level: q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic].")]
        public VerbosityLevel Verbosity { get; set; }

        [Command(Description = "List all global.json files in subdirectories, recursive.")]
        public class List : ParameterSymbolsDescriptionInSymbolAttributeCommand
        {
            public int Invoke([Option(Aliases = new string[] { "o" }, Description ="File to contain the output, if desired.")]
                              FileInfo output)
            { return default; }
        }

        [Command(Description = "Update the specified global.json")]
        public class Update : ParameterSymbolsDescriptionInSymbolAttributeCommand
        {
            public int Invoke([Argument(Description = "Path to the global.json to update")]
                              FileInfo filePath,
                              [Option(Description = "Existing SDK version to update. This must be an exact match.")]
                              string oldVersion,
                              [Option(Description = "New SDK version to use.")]
                              string newVersion,
                              [Option(Description = "True to allow prerelease.")]
                              bool allowPrerelease,
                              [Option(Description = "Rules for selecting an SDK version.")]
                              RollForward rollForward)
            { return default; }
        }

        [Command(Description = "Check that an appriorpiate SDK version is available.")]
        public class Check : ParameterSymbolsDescriptionInSymbolAttributeCommand
        {
            public int Invoke([Option(Description = "Uses the specified version. If not found, rolls forward to the latest patch level. If not found, fails. This value is the legacy behavior from the earlier versions of the SDK.")]
                              bool sdkOnly)
            { return default; }
        }

    }
}
