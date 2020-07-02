using Playground.PotentialPatterns;
using System.Threading.Tasks;

namespace UserStudyTest2
{
    class Program
    {
        static async Task Main(string[] args)
        {
             // Currently, args are not piped through, but 
             await MainWithGeneraicHostInvokeWithConfiguration.Main1(args);


        }

        //    var arg = "start Update start2 --allow-prerelease";
        //    var result = GetParseResult(arg);
        //private static ParseResult GetParseResult(string arg)
        //{
        //    var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor<ManageGlobalJson>(Strategy.Standard);
        //    var builder = new CommandLineBuilder();
        //    CommandMaker.FillCommand(builder.Command, descriptor);
        //    return builder.Build().Parse(arg);
        //}
    }
}
