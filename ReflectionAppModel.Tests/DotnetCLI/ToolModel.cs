using System.CommandLine.GeneralAppModel;

namespace System.CommandLine.ReflectionAppModel.DotnetCLI
{
    public class Tool : Dotnet
    {

        public class Install : Tool
        {
            [Description("The NuGet Package Id of the tool to install")]
            public string? PackageIdArg { get; set; }

            [Description("Install the tool for the current user.")]
            [Aliases("--g")]
            public bool? Global { get; set; }

            [Description("Install the tool and add to the local tool manifest(default).")]
            public bool? Local { get; set; }

            [Description("The directory where the tool will be installed.The directory will be created if it does not exist.")]
            public string? ToolPath { get; set; }

            [Description("The version of the tool package to install.")]
            public string? Version { get; set; }

            [Description("The NuGet configuration file to use.")]
            [Option(ArgumentName = "File")]
            public string? Configfile { get; set; }

            [Description("Path to the manifest file.")]
            [Option(ArgumentName = "Path")]
            public string? ToolManifest { get; set; }

            [Description("Add an additional NuGet package source to use during installation.")]
            [Option(ArgumentName = "Source")]
            public string? AddSource { get; set; }

            [Description("The target framework to install the tool for.")]
            [Option(ArgumentName = "Framework")]
            public string? Framework { get; set; }

            [Description("Prevent restoring multiple projects in parallel.")]
            public bool? DisableParallel { get; set; }

            [Description("Treat package source failures as warnings.")]
            public bool? IgnoreFailedSource { get; set; }

            [Description("Do not cache packages and http requests.")]
            public bool? NoCache { get; set; }

            [Description("Allows the command to stop and wait for user input or action (for example to complete authentication).")]
            public bool? Interactive { get; set; }
        }

        public class Uninstall : Tool
        {
            [Description("The NuGet Package Id of the tool to uninstall")]
            public string? PackageIdArg { get; set; }

            [Description("Uninstall the tool from the current user's tools directory.")]
            public bool? Global { get; set; }

            [Description("Uninstall the tool and remove it from the local tool manifest.")]
            public bool? Local { get; set; }

            [Description("The directory containing the tool to uninstall.")]
            public string? ToolPath { get; set; }

            [Description("Path to the manifest file.")]
            [Option(ArgumentName = "Path")]
            public string? ToolManifest { get; set; }
        }

        public class Update : Tool
        {
            [Description("The NuGet Package Id of the tool to update")]
            public string? PackageIdArg { get; set; }

            [Description("Update the tool in the current user's tools directory.")]
            [Aliases("--g")]
            public bool? Global { get; set; }

            [Description("Update the tool and the local tool manifest.")]
            public bool? Local { get; set; }

            [Description("The directory containing the tool to update.")]
            public string? ToolPath { get; set; }

            [Description("The version range of the tool package to update to.")]
            public string? Version { get; set; }

            [Description("TThe NuGet configuration file to use.")]
            [Option(ArgumentName = "File")]
            public string? Configfile { get; set; }

            [Description("Path to the manifest file.")]
            [Option(ArgumentName = "Path")]
            public string? ToolManifest { get; set; }

            [Description("Add an additional NuGet package source to use during the update.")]
            [Option(ArgumentName = "Source")]
            public string? AddSource { get; set; }

            [Description("The target framework to update the tool for.")]
            [Option(ArgumentName = "Framework")]
            public string? Framework { get; set; }

            [Description("Prevent restoring multiple projects in parallel.")]
            public bool? DisableParallel { get; set; }

            [Description("Treat package source failures as warnings.")]
            public bool? IgnoreFailedSource { get; set; }

            [Description("Do not cache packages and http requests.")]
            public bool? NoCache { get; set; }

            [Description("Allows the command to stop and wait for user input or action (for example to complete authentication).")]
            public bool? Interactive { get; set; }
        }

        public class List : Tool
        {

            [Description("List tools installed for the current user.")]
            public bool? Global { get; set; }

            [Description("List tools in the local tool manifest.")]
            public bool? Local { get; set; }

            [Description("The directory containing the tool to list.")]
            public string? ToolPath { get; set; }

        }

        public class Run : Tool
        {
            [Description("The command name of the tool to run.")]
            public string? CommandNameArg { get; set; }
        }

        public class Search : Tool
        {
            [Description("Search term from package id or package description. Require at least one character.")]
            public string? SearchStringArg { get; set; }

            [Description("Show detail result of the query.")]
            public bool? Detail { get; set; }

            [Description("The number of results to skip, for pagination.")]
            public int Skip { get; set; }

            [Description("The number of results to return, for pagination.")]
            public int Take { get; set; }

            [Description("Include prerelease packages when true.")]
            public bool? Prerelease { get; set; }

            [Description("A SemVer 1.0.0 version string.")]
            public bool? SemVerLevel { get; set; }
        }

        public class Restore : Tool
        {
            [Description("The NuGet configuration file to use.")]
            [Option(ArgumentName = "File")]
            public string? Configfile { get; set; }

            [Description("Path to the manifest file.")]
            [Option(ArgumentName = "Path")]
            public string? ToolManifest { get; set; }

            [Description("Add an additional NuGet package source to use during installation.")]
            [Option(ArgumentName = "Source")]
            public string? AddSource { get; set; }

            [Description("Prevent restoring multiple projects in parallel.")]
            public bool? DisableParallel { get; set; }

            [Description("Treat package source failures as warnings.")]
            public bool? IgnoreFailedSource { get; set; }

            [Description("Do not cache packages and http requests.")]
            public bool? NoCache { get; set; }

            [Description("Allows the command to stop and wait for user input or action (for example to complete authentication).")]
            public bool? Interactive { get; set; }
        }
    }
}
