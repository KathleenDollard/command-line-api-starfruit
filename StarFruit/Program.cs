using System;
using System.CommandLine.ReflectionModel;
using System.CommandLine;
using System.Threading.Tasks;
using StarFruit.CLI;
using System.Linq;
using System.Runtime.CompilerServices;

namespace StarFruit
{
class Program
{
public static int Main(Dotnet args)
    => args switch
    {
        Run run => Run(run.DryRun, run.Force, run.Language, run.Name, run.Output, run.Project, run.TemplateName),
        Install install => Install(install),
        Uninstall uninstall => Uninstall(uninstall),
    List list => DoWork(list),
    Search search => DoWork(search),
    Update update => Update(update),
    Dotnet cli => DoWork(cli),
    _ => throw new InvalidOperationException()
};


        private static int Update(Update update) => DoWork<Update>(update);
        private static int Run(bool dryRun, bool force, string language, string name, string output, string project, string templateName) => throw new NotImplementedException();
        private static int Uninstall(Uninstall uninstall) => DoWork<Uninstall>(uninstall);
        private static int Install(Install install) => DoWork<Install>(install);

        private static int DoWork<T>(T command)
            where T : Dotnet
        {
            Console.WriteLine(command.GetType().Name);
            return 0;
        }

        //public static void Main(string[] args)
        //{
        //    var model = ReflectionParser<TemplateCli>.GetInstance(args);
        //    Main(model);
        //}

        public static async Task<int> Main(string[] args)
        {
            var methodInfo = typeof(Program).GetMethods()
                                            .Where(x => x.Name == "Main")
                                            .Where(x => x.GetParameters().First().ParameterType != typeof(string[]))
                                            .Single();
            return await CommandLine.ExecuteMethodAsync<Dotnet>(ParserBuilder.ConfigureFromMethod, methodInfo, args);
        }
    }
}
