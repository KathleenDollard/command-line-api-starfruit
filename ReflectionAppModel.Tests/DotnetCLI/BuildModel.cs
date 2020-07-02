namespace System.CommandLine.ReflectionAppModel.DotnetCLI
{
    public class Build : Dotnet
    { 
    
    }

}


//Usage: dotnet build[options] <PROJECT | SOLUTION>

//Arguments:
//  <PROJECT | SOLUTION>   The project or solution file to operate on.If a file is not specified, the command will search the current directory for one.

//Options:
//  -h, --help Show command line help.
//  -o, --output<OUTPUT_DIR> The output directory to place built artifacts in.
//  -f, --framework<FRAMEWORK> The target framework to build for. The target framework must also be specified in the project file.
//  -c, --configuration<CONFIGURATION> The configuration to use for building the project.The default for most projects is 'Debug'.
//  -r, --runtime<RUNTIME_IDENTIFIER> The target runtime to build for.
//  --version-suffix<VERSION_SUFFIX> Set the value of the $(VersionSuffix) property to use when building the project.
//  --no-incremental Do not use incremental building.
//  --no-dependencies Do not build project-to-project references and only build the specified project.
//  /nologo, --nologo Do not display the startup banner or the copyright message.
//  --no-restore Do not restore the project before building.
//  --interactive Allows the command to stop and wait for user input or action (for example to complete authentication).
//  -v, --verbosity<LEVEL> Set the MSBuild verbosity level.Allowed values are q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic].
//  --force Force all dependencies to be resolved even if the last restore was successful.
//                         This is equivalent to deleting project.assets.json.
