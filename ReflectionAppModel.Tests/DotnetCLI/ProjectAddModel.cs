using System.CommandLine.ReflectionAppModel.Attributes;
using System.ComponentModel;
using System.IO;

namespace System.CommandLine.ReflectionAppModel.DotnetCLI
{
    //[Hidden]
    //public class Project : Dotnet
    //{ }

    public class Add : Dotnet
    {
        [Description("The project file to operate on. If a file is not specified, the command will search the current directory for one.")]
        public string? ProjectArg { get; set; }

        public class Project : Add
        {
            [Description("The paths to the projects to add as references.")]
            public string? ProjectPath { get; set; }

            [Description("Add the reference only when targeting a specific framework.")]
            public string? Framework { get; set; }

            [Description("Allows the command to stop and wait for user input or action (for example to complete authentication).")]
            public bool? Interactive { get; set; }
        }

        public class Package : Add
        {
            [Description("The package reference to add.")]
            public string? PackageNameArg { get; set; }

            [Description("The version of the package to add.")]
            public string? Version { get; set; }

            [Description("Add the reference only when targeting a specific framework.")]
            public string? Framework { get; set; }

            [Description("Add the reference without performing restore preview and compatibility check.")]
            public bool? NoRestore { get; set; }

            [Description("The NuGet package source to use during the restore.")]
            public string? Source { get; set; }

            [Description("The directory to restore packages to..")]
            [Option(ArgumentName = "PackageDir")]
            public DirectoryInfo? PackageDirectory { get; set; }

            [Description("Allows the command to stop and wait for user input or action (for example to complete authentication).")]
            public bool? Interactive { get; set; }
        }
    }
}

