using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.IO;

namespace DotnetCLI
{
    public class ProjectCommand : DotnetCommand
    {
        public string? ProjectArg { get; set; }

        public int Add_Reference(string projectPathArg,
                                [Aliases("-f")] string? framework,
                                bool interactive)
        { return default; }

        [Alias("n", "noRestore")]
        [Alias("n", "noRestore")]
        public int Add_Package(string packageNameArg, string? version, string? framework, bool? noRestore,
                               string? source, DirectoryInfo? packageDirectory, bool interactive)
        { return default; }

        public int Remove_Reference(string projectPathArg, string? framework)
        { return default; }

        public int Remove_Package(string packageNameArg, bool interactive)
        { return default; }

        public int List_Package(string? framework, string? source, bool outdated, bool deprecated,
                                bool includeTransitive, bool includePrerelease, bool highestPatch, bool highestMinor,
                                FileInfo? config, bool interactive)
        { return default; }

        public int List_Reference()
        { return default; }

        private const string startName = nameof(ProjectCommand);
        public Dictionary<string, string> HelpText = new Dictionary<string, string>
    {
        { $"{startName}+{nameof(ProjectArg)}" , "The project file to operate on. If a file is not specified, the command will search the current directory for one."},
        { $"{startName}+{nameof(Add_Reference)}+ProjectPathArg" , "The paths to the projects to add as references."},
        { $"{startName}+{nameof(Add_Reference)}+Framework" , "Add the reference only when targeting a specific framework."},
        { $"{startName}+{nameof(Add_Reference)}+Interactive" , "Allows the command to stop and wait for user input or action (for example to complete authentication)."},
        { $"{startName}+{nameof(Add_Package)}+PackageNameArg" , "The package reference to add."},
        { $"{startName}+{nameof(Add_Package)}+Version" , "The version of the package to add."},
        { $"{startName}+{nameof(Add_Package)}+Framework" , "Add the reference only when targeting a specific framework."},
        { $"{startName}+{nameof(Add_Package)}+NoRestore" , "Add the reference without performing restore preview and compatibility check."},
        { $"{startName}+{nameof(Add_Package)}+Source" , "The NuGet package source to use during the restore."},
        { $"{startName}+{nameof(Add_Package)}+ArgumentName" , "The directory to restore packages to."},
        { $"{startName}+{nameof(Add_Package)}+Interactive" , "Allows the command to stop and wait for user input or action (for example to complete authentication)."},
        { $"{startName}+{nameof(Remove_Reference)}+ProjectPathArg" , "The paths to the referenced projects to remove."},
        { $"{startName}+{nameof(Remove_Reference)}+Interactive" , "Allows the command to stop and wait for user input or action (for example to complete authentication)."},
        { $"{startName}+{nameof(Remove_Package)}+PackageNameArg" , "The package reference to remove."},
        { $"{startName}+{nameof(Remove_Package)}+Interactive" , "Allows the command to stop and wait for user input or action (for example to complete authentication)."},
        { $"{startName}+{nameof(Add_Reference)}+ProjectPath" , ""},
        { $"{startName}+{nameof(Add_Reference)}+ProjectPath" , ""},
    };
    }
}
