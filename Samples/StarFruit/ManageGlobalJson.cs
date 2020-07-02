using System;
using System.Collections.Generic;
using System.CommandLine.ReflectionAppModel;
using System.CommandLine.ReflectionAppModel.Attributes;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace StarFruit
{
    /// <summary>
    /// </summary>
    /// <remarks>
    /// Parts of this tool is not trivial unless we expose the SDK selection via an API. 
    /// The actual rules for SDK selection depend on the highest SDK version installed 
    /// (or ever installed) on the machine.
    /// </remarks>
    public class ManageGlobalJson
    {
        public DirectoryInfo StartPathArg { get; set; }

        [Aliases("v")]
        public VerbosityLevel Verbosity { get; set; }

        public class Find : ManageGlobalJson
        {
            public void Invoke(FileInfo FilePathArg, string OldVersion)
            { }
        }

        public class List : ManageGlobalJson
        {
            [Aliases("o")]
            public FileInfo Output { get; set; }
        }

        public class Update : ManageGlobalJson
        {
            public FileInfo FilePathArg { get; set; }
            public string OldVersion { get; set; }
            public string NewVersion { get; set; }
            public bool AllowPrerelease { get; set; }
            public RollForward RollForward { get; set; }

            public int Invoke()
            {
                Console.WriteLine($"From Update.Invoke. RollForward={RollForward}");
                return 42;
            }
        }

        public class Check : ManageGlobalJson
        {
            public bool SdkOnly { get; set; }
        }


        private const string startName = nameof(ManageGlobalJson);
        private const string findName = nameof(Find);
        private const string listName = nameof(List);
        private const string updateName = nameof(Update);
        private const string checkName = nameof(Check);
        public Dictionary<string, string> HelpText = new Dictionary<string, string>
            {
                { startName, "Future global tool to manage global.json. See https://aka.ms/globaljson."},
                { startName + $"+{nameof(StartPathArg)}","Location where processing should begin." },
                { startName + $"+{nameof(Verbosity) }","Verbosity level:  q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic]." },
                { startName + $"+{findName}","Find the current global.json and the SDK it contains." },
                { startName + $"+{listName}","List all global.json files in subdirectories, recursive." },
                { startName + $"+{updateName}","Update the specified global.json" },
                { startName + $"+{updateName}+{nameof(Update.FilePathArg)}","Path to the global.json to update" },
                { startName + $"+{updateName}+{nameof(Update.OldVersion)}","Existing SDK version to update. This must be an exact match." },
                { startName + $"+{updateName}+{nameof(Update.NewVersion)}","New SDK version to use." },
                { startName + $"+{updateName}+{nameof(Update.AllowPrerelease)}","True to allow prerelease." },
                { startName + $"+{updateName}+{nameof(Update.RollForward)}","Rules for selecting an SDK version." },
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

    public enum RollForward
    {
        Patch,   // Uses the specified version. If not found, rolls forward to the latest patch level. If not found, fails. This value is the legacy behavior from the earlier versions of the SDK.
        Feature, // Uses the latest patch level for the specified major, minor, and feature band. If not found, rolls forward to the next higher feature band within the same major/minor and uses the latest patch level for that feature band. If not found, fails.
        Minor,  // Uses the latest patch level for the specified major, minor, and feature band. If not found, rolls forward to the next higher feature band within the same major/minor version and uses the latest patch level for that feature band. If not found, rolls forward to the next higher minor and feature band within the same major and uses the latest patch level for that feature band. If not found, fails.
        Major,  // Uses the latest patch level for the specified major, minor, and feature band. If not found, rolls forward to the next higher feature band within the same major/minor version and uses the latest patch level for that feature band. If not found, rolls forward to the next higher minor and feature band within the same major and uses the latest patch level for that feature band. If not found, rolls forward to the next higher major, minor, and feature band and uses the latest patch level for that feature band. If not found, fails.
        LatestPatch, // Uses the latest installed patch level that matches the requested major, minor, and feature band with a patch level and that is greater or equal than the specified value. If not found, fails.
        LatestFeature,//  Uses the highest installed feature band and patch level that matches the requested major and minor with a feature band that is greater or equal than the specified value. If not found, fails.
        LatestMinor, // Uses the highest installed minor, feature band, and patch level that matches the requested major with a minor that is greater or equal than the specified value. If not found, fails.
        LatestMajor, // Uses the highest installed .NET Core SDK with a major that is greater or equal than the specified value. If not found, fail.
        Disable, // Doesn't roll forward. Exact match required.
    }
}
