using System.CommandLine.ReflectionAppModel.Attributes;
using System.ComponentModel;

namespace System.CommandLine.ReflectionAppModel.DotnetCLI
{
    public class Restore : Dotnet
    { 
 
    }
}

//Usage: dotnet restore[options] <PROJECT | SOLUTION>

//Arguments:
//  <PROJECT | SOLUTION>   The project or solution file to operate on.If a file is not specified, the command will search the current directory for one.

//Options:
//  -h, --help Show command line help.
//  -s, --source<SOURCE> The NuGet package source to use for the restore.
//  -r, --runtime<RUNTIME_IDENTIFIER> The target runtime to restore packages for.
//  --packages<PACKAGES_DIR> The directory to restore packages to.
//  --disable-parallel Prevent restoring multiple projects in parallel.
//  --configfile<FILE> The NuGet configuration file to use.
//  --no-cache Do not cache packages and http requests.
//  --ignore-failed-sources Treat package source failures as warnings.
//  --no-dependencies Do not restore project-to-project references and only restore the specified project.
//  -f, --force Force all dependencies to be resolved even if the last restore was successful.
//                                       This is equivalent to deleting project.assets.json.
//  -v, --verbosity<LEVEL> Set the MSBuild verbosity level. Allowed values are q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic].
//  --interactive Allows the command to stop and wait for user input or action (for example to complete authentication).
//  --use-lock-file Enables project lock file to be generated and used with restore.
//  --locked-mode Don't allow updating project lock file.
//  --lock-file-path<LOCK_FILE_PATH> Output location where project lock file is written.By default, this is 'PROJECT_ROOT\packages.lock.json'.
//  --force-evaluate Forces restore to reevaluate all dependencies even if a lock file already exists.
