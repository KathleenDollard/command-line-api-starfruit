using System.CommandLine.GeneralAppModel;
using System.Linq;

// Simplest entry point.
// This can be split up, for example so the strategy or command tree can be modified
// This may need work to correctly communicate parser issues - maybe as simple as a try/catch
// This code could easily be put into an MSBuild task (like DragonFruit). In that case identifying the type needs to be worked out. 

namespace Playground.PotentialPatterns
{
    public class MainWithInvoke
    {
        public static int Main1(string[] args)
        {
            if (!args.Any())
            {
                args = "start Update start2 --allow-prerelease".Split(',');
            }
            return Strategy.Standard.InvokeMethod<ManageGlobalJson>(args);
        }
    }
}
