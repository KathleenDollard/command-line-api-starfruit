using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.IO;
using System.Text;

namespace DotnetCLI
{
    [Flags]
    public enum AuthenticationType
    {
        Base = 0b0001,
        Negotiate = 0b0010
    }

    public enum CacheLocations
    {
        // Make the default 0
        httpCache = 1,
        globaPackages,
        temp,
        all
    }

    public enum DetailLevel
    {
        // Make the default 0
        Detailed = 0,
        Short
    }

    public class NuGetCommand : DotnetCommand
    {
        public int Add_Source(string? sourcePathArg, string? name, string? userName, string? password,
                              string? storePassrwordsInPlainText, AuthenticationType validAuthenticationTypes,
                              FileInfo configfile)
        { return default; }

        public int Delete(string? packageIdArg, bool forceEnglishOutput, string source, bool nonInteractive,
                          string apiKey, bool noServiceEndpoint, bool interactive)
        { return default; }

        public int Disable_Source(string sourceNameArg, string? configFile)
        { return default; }

        public int Enable(string sourceNameArg, DetailLevel format, string? configFile)
        { return default; }

        public int List(string sourceNameArg, string? packageIdArg)
        { return default; }

        // A more consistent design would be to clear/list as subcommands to parallel Add:Source, etc. Not sure this is worth the break. Weakness in this design is specifying both shoul be an error.dotnet 
        public int Locals(CacheLocations cacheLocations, bool forceEnglishOutput,bool clear, bool list )
        { return default; }

        // Docs on root are poor, not clear what this shoudl look like
        public int Push(string? root, bool forceEnglishOutput, string source, string symbolSource, int timeout,
                        string symbolApiKey, bool disableBuffering, bool noSymbols, bool noServiceEndpoint,
                        bool interactive)
        { return default; }

        public int Remove(string? sourceName)
        { return default; }

        public int Update(string? sourceName)
        { return default; }
    }
}
