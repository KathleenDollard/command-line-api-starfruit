using CommandLineAppModel2.Args;
using Model2;
using System;

namespace CommandLineAppModel2
{
    class Program
    {


        static int Main(TemplateCli args)
            => args switch
            {
                Run run => DoWork(run),
                Install install => DoWork(install),
                Uninstall uninstall => DoWork(uninstall),
                List list => DoWork(list),
                Search search => DoWork(search),
                Update update => DoWork(update),
                _ => throw new InvalidOperationException()
            };


        private static int DoWork(TemplateCli command)
        {
            Console.WriteLine(command.GetType().Name);
            return 0;
        }

        static void Main(string[] args)
            => Main(ReflectionParser<TemplateCli>.GetInstance(args));
    }
}
