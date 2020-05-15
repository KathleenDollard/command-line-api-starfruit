using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Tests;
using System.CommandLine.ReflectionAppModel.Tests.ModelCodeForTests;

namespace System.CommandLine.ReflectionAppModel.Tests.DotnetModel
{
    public class Dotnet : IHaveTypeTestData
    {
        public CommandTestData CommandDataFromType
            => new CommandTestData()
            {
                Name = nameof(Dotnet),
                Raw = typeof(Dotnet),
                SubCommands = new List<CommandTestData>
                {
                    (new Tool() as IHaveTypeTestData).CommandDataFromType,
                }
            };
    }

    public class Tool : Dotnet, IHaveTypeTestData
    {
        public new CommandTestData CommandDataFromType
            => new CommandTestData()
            {
                Name = nameof(Tool),
                Raw = typeof(Tool),
                SubCommands = new List<CommandTestData>
                {
                    (new Install() as IHaveTypeTestData) .CommandDataFromType,
                }
            };
    }

    public class Install : Tool, IHaveTypeTestData
    {
        public bool Global { get; set; }
        public bool Local { get; set; }
        public string ToolPath { get; set; }
        public string PackageIdArg { get; set; }

        public new CommandTestData CommandDataFromType
           => new CommandTestData()
           {
               Name = nameof(Install),
               Raw = typeof(Install),
               Arguments = new List<ArgumentTestData>
               { new ArgumentTestData
                    {
                       Name = nameof(PackageIdArg)[..^3],
                       Raw = ReflectionSupport.GetPropertyInfo<Install>(nameof(PackageIdArg)),
                       ArgumentType = typeof(string)
                    }
               },
               Options = new List<OptionTestData>
               { new OptionTestData
                    {
                        Name = nameof(Global),
                        Raw = ReflectionSupport.GetPropertyInfo<Install>(nameof(Global)),
                    },
                    new OptionTestData
                    {
                        Name = nameof(Local),
                        Raw = ReflectionSupport.GetPropertyInfo<Install>(nameof(Local)),
                    },
                    new OptionTestData
                    {
                        Name = nameof(ToolPath),
                        Raw = ReflectionSupport.GetPropertyInfo<Install>(nameof(ToolPath)),
                    },

               }
           };
    }

    //    Usage: dotnet tool install[options] <PACKAGE_ID>

    //Arguments:
    //  <PACKAGE_ID>   The NuGet Package Id of the tool to install.

    //Options:
    //  -g, --global Install the tool for the current user.
    //  --local Install the tool and add to the local tool manifest(default).
    //  --tool-path<PATH> The directory where the tool will be installed.The directory will be created if it does not exist.
    //  --version<VERSION> The version of the tool package to install.
    //  --configfile<FILE> The NuGet configuration file to use.
    //  --tool-manifest<PATH> Path to the manifest file.
    //  --add-source<SOURCE> Add an additional NuGet package source to use during installation.
    //  --framework<FRAMEWORK> The target framework to install the tool for.
    //  --disable-parallel Prevent restoring multiple projects in parallel.
    //  --ignore-failed-sources Treat package source failures as warnings.
    //  --no-cache Do not cache packages and http requests.
    //  --interactive Allows the command to stop and wait for user input or action (for example to complete authentication).
    //  -h, --help Show command line help.
    //  -v, --verbosity<LEVEL> Set the MSBuild verbosity level.Allowed values are q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic].
}
