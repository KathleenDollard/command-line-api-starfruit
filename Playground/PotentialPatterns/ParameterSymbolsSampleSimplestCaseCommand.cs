using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.ReflectionAppModel;
using System.IO;

// SubCommands are via derived classes. This makes binding and accessing data in parent classes super easy.
// Options and arguments are via parameters on invoke properties. This probably won't work with "switch" style Main
// Starting with everything in attributes on properties of a class. 
// Descriptions are isolated, to make comparison and localization easier (they could be via a method as well)

namespace Playground
{

    public class ParameterSymbolsSampleSimplestCaseCommand
    {
        // Known to be argument by suffix, suffix removed in Argument name
        public DirectoryInfo StartPathArg { get; set; }

        [Aliases("v")]
        public VerbosityLevel Verbosity { get; set; }

        public class List : ParameterSymbolsSampleSimplestCaseCommand
        {
            public int Invoke([Aliases("o")] FileInfo output)
            { return default; }
        }

        public class Update : ParameterSymbolsSampleSimplestCaseCommand
        {
            public int Invoke(FileInfo filePathArg, string oldVersion, string newVersion, bool allowPrerelease,
                              RollForward rollForward)
            { return default; }
        }

        public class Check : ParameterSymbolsSampleSimplestCaseCommand
        {
            public int Invoke(bool sdkOnly)
            { return default; }
        }

        private const string startName = nameof(ManageGlobalJson);
        private const string listName = nameof(List);
        private const string updateName = nameof(Update);
        private const string checkName = nameof(Check);
        public Dictionary<string, string> HelpText = new Dictionary<string, string>
            {
                { startName, "Future global tool to manage global.json. See https://aka.ms/globaljson."},
                { startName + $"+{nameof(StartPathArg)}","Location where processing should begin." },
                { startName + $"+{nameof(Verbosity) }","Verbosity level:  q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic]." },
                { startName + $"+{listName}","List all global.json files in subdirectories, recursive." },
                { startName + $"+{listName}+Output","File to contain the output, if desired." },
                { startName + $"+{updateName}","Update the specified global.json" },
                { startName + $"+{updateName}+FilePath","Path to the global.json to update" },
                { startName + $"+{updateName}+OldVersion","Existing SDK version to update. This must be an exact match." },
                { startName + $"+{updateName}+NewVersion","New SDK version to use." },
                { startName + $"+{updateName}+AllowPrerelease","True to allow prerelease." },
                { startName + $"+{updateName}+RollForward","Rules for selecting an SDK version." },
                { startName + $"+{checkName}","Check that an appriorpiate SDK version is available." },
                { nameof(RollForward) + $"+{nameof(RollForward.Patch)}", "Uses the specified version. If not found, rolls forward to the latest patch level. If not found, fails. This value is the legacy behavior from the earlier versions of the SDK." },
                { nameof(RollForward) + $"+{nameof(RollForward.Feature)}", "Uses the latest patch level for the specified major, minor, and feature band. If not found, rolls forward to the next higher feature band within the same major/minor and uses the latest patch level for that feature band. If not found, fails." },
                { nameof(RollForward) + $"+{nameof(RollForward.Minor)}", "Uses the latest patch level for the specified major, minor, and feature band. If not found, rolls forward to the next higher feature band within the same major/minor version and uses the latest patch level for that feature band. If not found, rolls forward to the next higher minor and feature band within the same major and uses the latest patch level for that feature band. If not found, fails." },
                { nameof(RollForward) + $"+{nameof(RollForward.Major)}", "Uses the latest patch level for the specified major, minor, and feature band. If not found, rolls forward to the next higher feature band within the same major/minor version and uses the latest patch level for that feature band. If not found, rolls forward to the next higher minor and feature band within the same major and uses the latest patch level for that feature band. If not found, rolls forward to the next higher major, minor, and feature band and uses the latest patch level for that feature band. If not found, fails." },
                { nameof(RollForward) + $"+{nameof(RollForward.LatestPatch)}", "Uses the latest installed patch level that matches the requested major, minor, and feature band with a patch level and that is greater or equal than the specified value. If not found, fails." },
                { nameof(RollForward) + $"+{nameof(RollForward.LatestFeature)}", "Uses the highest installed feature band and patch level that matches the requested major and minor with a feature band that is greater or equal than the specified value. If not found, fails." },
                { nameof(RollForward) + $"+{nameof(RollForward.LatestMinor)}", "Uses the highest installed minor, feature band, and patch level that matches the requested major with a minor that is greater or equal than the specified value. If not found, fails." },
                { nameof(RollForward) + $"+{nameof(RollForward.LatestMajor)}", "Uses the highest installed .NET Core SDK with a major that is greater or equal than the specified value. If not found, fail." },
                { nameof(RollForward) + $"+{nameof(RollForward.Disable)}", "Doesn't roll forward. Exact match required." },

            };
    }
}
