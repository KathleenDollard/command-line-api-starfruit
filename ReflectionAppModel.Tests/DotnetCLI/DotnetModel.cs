using System.CommandLine.ReflectionAppModel.Attributes;
using System.ComponentModel;

namespace System.CommandLine.ReflectionAppModel.DotnetCLI
{
    public class Dotnet
    {
        [Description("Install the tool for the current user.")]
        [Aliases("--v")]
        public VerbosityLevel verbosity { get; set; }

    }
}
