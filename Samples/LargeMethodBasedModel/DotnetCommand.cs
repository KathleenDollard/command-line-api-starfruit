using System.CommandLine.GeneralAppModel;

namespace DotnetCLI
{
    public class DotnetCommand
    {
        // Put these in Dotnet or Project?
        // Build
        // Clean
        // Pacl
        // Publish
        // Restore
        // Run
        // Store
        // Test
        // VSTest (why is this in help?)

        // dev-certs 
        // fsi               
        // sql-cache 
        // user-secrets 
        // watch

        // For help, I don't think this makes it into the CLI
        // --additionalprobingpath <path> 
        // --additional-deps<path>        
        // --depsfile                     
        // --fx-version<version>          
        // --roll-forward<setting>        
        // --runtimeconfig

        public bool Info { get; set; }
        public bool ListRuntimes { get; set; }
        public bool ListSdks { get; set; }
        public bool Version { get; set; }
        [Aliases("d")]
        public bool Diagnostics { get; set; }

        public int Help { get; set; }
    }
}
