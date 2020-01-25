﻿using System;
using System.CommandLine.StarFruit;
using StarFruit.CLI;

namespace StarFruit
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
                TemplateCli cli => DoWork(cli),
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
