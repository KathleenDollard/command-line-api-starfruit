    using Playground.PotentialPatterns;
using System.Threading.Tasks;

namespace UserStudyTest2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await MainWithGeneraicHostInvokeWithConfiguration.Main1("playground start update start2 baz".Split(' '));
        }
    }
}
