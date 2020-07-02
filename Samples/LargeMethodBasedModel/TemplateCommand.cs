using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;

namespace DotnetCLI
{
    public class TemplateCommand : DotnetCommand
    {

        public int Install(string? packageIdArg, string? version, string? configfile, string? source, bool? interactive)
        { return default; }

        public int Uninstall(string? packageIdArg)
        { return default; }

        public int Update(string? packageIdArg, string? version, string? configfile, string? source, bool? interactive)
        { return default; }

        public int List(string? packageIdArg, bool checkForUpdates)
        { return default; }

        public int Run(string? templateName)
        { return default; }

        public int Search(string? searchStringArg, bool? detail, int skip, int take, bool? prerelease, bool? semVerLevel)
        { return default; }

        public int Restore(string? configfile, string? addSource, bool? interactive)
        { return default; }
    }
}
