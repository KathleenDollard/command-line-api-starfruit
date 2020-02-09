using System;
using System.CommandLine.StarFruit;
using StarFruit.CLI;
using System.Threading.Tasks;

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
                Update update => DoWork(update),
                Dotnet cli => DoWork(cli),
                _ => throw new InvalidOperationException()
            };
        private static int Run(bool dryRun, bool force, string language, string name, string output, string project, string templateName) => throw new NotImplementedException();
        private static int Uninstall(Uninstall uninstall) => throw new NotImplementedException();
        private static int Install(Install install) => throw new NotImplementedException();

        private static int DoWork(Dotnet command)
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
            => await CommandLine.ExecuteMethodAsync<Dotnet>(typeof(Program).GetMethod("Main"), args);
    }
}
