using System.Threading.Tasks;
using System.CommandLine.GeneralAppModel.Hosting;

namespace Playground.PotentialPatterns
{
    public class MainWithGenericHostInvokeNoConfiguration
    {
        public static async Task Main1(string[] args)
        {
            await GeneralAppModelHosting.RunAsync<ManageGlobalJson>(args);
        }
    }
}
