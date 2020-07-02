using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;

namespace DotnetCLI
{
    public class ToolCommand : DotnetCommand
    {

        public int Install(string? packageIdArg, [Aliases("--g")] bool? global, bool? local, string? toolPath,
                            string? version, [Option(ArgumentName = "File")] string? configfile,
                            [Option(ArgumentName = "Path")] string? toolManifest,
                            [Option(ArgumentName = "Source")] string? addSource,
                            string? framework, bool? disableParallel,
                            bool? ignoreFailedSource, bool? noCache, bool? interactive)
        { return default; }

        public int Uninstall(string? packageIdArg, [Aliases("--g")] bool? global, bool? local, string? toolPath,
              [Option(ArgumentName = "Path")] string? toolManifest)
        { return default; }

        public int Update(string? packageIdArg, [Aliases("--g")] bool? global, bool? local, string? toolPath,
                          string? version, [Option(ArgumentName = "File")] string? configfile,
                          [Option(ArgumentName = "Path")] string? toolManifest,
                          [Option(ArgumentName = "Source")] string? addSource,
                          string? framework, bool? disableParallel,
                          bool? ignoreFailedSource, bool? noCache, bool? interactive)
        { return default; }

        public int List(string? packageIdArg, bool checkForUpdates, [Aliases("--g")] bool? global, bool? local, string? toolPath)
        { return default; }

        public int Run(string? commandNameArg)
        { return default; }

        public int Search(string? searchStringArg, bool? detail, int skip, int take, bool? prerelease, bool? semVerLevel)
        { return default; }

        public int Restore([Option(ArgumentName = "File")] string? configfile,
                           [Option(ArgumentName = "Path")] string? toolManifest,
                           [Option(ArgumentName = "Source")] string? addSource, bool? disableParallel,
                           bool? ignoreFailedSource, bool? noCache, bool? interactive)
        { return default; }


        private const string startName = nameof(ToolCommand);
        public Dictionary<string, string> HelpText = new Dictionary<string, string>
        {
            { $"{startName}+{nameof(Install)}" , "Install global or local tool. local tools are added to manifest and restored."},
            { $"{startName}+{nameof(Uninstall)}" , "Uninstall a global tool or local tool."},
            { $"{startName}+{nameof(Update)}" , "Update a global tool."},
            { $"{startName}+{nameof(List)}" , ""},
            { $"{startName}+{nameof(Run)}" , ""},
            { $"{startName}+{nameof(Search)}" , ""},
            { $"{startName}+{nameof(Restore)}" , ""},
        };
    }
}
