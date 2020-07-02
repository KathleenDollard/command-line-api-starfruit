using System.CommandLine.GeneralAppModel;
using System.CommandLine.ReflectionAppModel;
using System.IO;

// SubCommands are via derived classes. This makes binding and accessing data in parent classes super easy.
// Options and arguments are via properties. There may or may not be an Invoke method, depending on Main style
// Using a symbol (Command/Argument/Option) based attribute. 
// Description in that attribute.

namespace Playground
{
    [Command(Description = "Future global tool to manage global.json. See https://aka.ms/globaljson.")]
    public class PropertySymbolsDescriptionInSymbolAttributeCommand
    {
        [Argument( Description = "Location where processing should begin.")]
        public DirectoryInfo StartPath { get; set; }

        [Option(Aliases = new string[] { "v" }, Description = "Verbosity level: q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic].")]
        public VerbosityLevel Verbosity { get; set; }

        [Command(Description = "List all global.json files in subdirectories, recursive.")]
        public class List : PropertySymbolsDescriptionInSymbolAttributeCommand
        {
            [Option(Aliases = new string[] { "o" }, Description ="File to contain the output, if desired.")]
            public FileInfo Output { get; set; }
        }

        [Command(Description = "Update the specified global.json")]
        public class Update : PropertySymbolsDescriptionInSymbolAttributeCommand
        {
            [Argument(Description = "Path to the global.json to update")]
            public FileInfo FilePath { get; set; }
            [Option( Description = "Existing SDK version to update. This must be an exact match.")]
            public string OldVersion { get; set; }
            [Option(Description = "New SDK version to use.")]
            public string NewVersion { get; set; }
            [Option(Description = "True to allow prerelease.")]
            public bool AllowPrerelease { get; set; }
            [Option(Description = "Rules for selecting an SDK version.")]
            public RollForward RollForward { get; set; }

            public int Invoke()
            {
                return ManageGlobalJsonImplementation.Update(StartPath, Verbosity, FilePath, OldVersion, NewVersion, AllowPrerelease, RollForward);
            }
        }

        [Command(Description = "Check that an appriorpiate SDK version is available.")]
        public class Check : PropertySymbolsDescriptionInSymbolAttributeCommand
        {
            [Option(Description = "Uses the specified version. If not found, rolls forward to the latest patch level. If not found, fails. This value is the legacy behavior from the earlier versions of the SDK.")]
            public bool SdkOnly { get; set; }
        }

    }
}
