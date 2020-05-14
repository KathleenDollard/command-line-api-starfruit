using System;
using System.CommandLine;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.ReflectionAppModel;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;

namespace UserStudyTest2
{
    class Program
    {
        static void Main(string[] args)
        {
            var strategy = new Strategy("Standard").SetStandardRules();
            //Report(strategy);
            Run(args, strategy);
        }

        private static void Run(string[] args, Strategy strategy)
        {
            System.Reflection.MethodInfo entryMethod = typeof(Program).GetMethod("Test");
            var descriptor = ReflectionAppModel.RootCommandDescriptor(strategy, entryMethod);
            var command = CommandMaker.MakeCommand(descriptor);
            //command.Handler = CommandHandler.Create<string, int, int>(Test);
            command.Invoke(args); 
        }

        private static void Report(Strategy strategy)
        {
            Console.WriteLine(strategy.Report());
        }

        public static void Test(string nameArg, int A, string B, int C)
        {
            Console.WriteLine($"Name: {nameArg} A: {A} B: {B} C: {C}");
        }
    }
}
