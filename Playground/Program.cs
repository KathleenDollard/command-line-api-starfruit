using System;
using System.CommandLine;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.ReflectionAppModel;
using System.CommandLine.Invocation;

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
            System.Reflection.MethodInfo entryMethod = typeof(MyClass).GetMethod("Test");
            var descriptor = ReflectionAppModel.RootCommandDescriptor(strategy, entryMethod);
            var command = CommandMaker.MakeCommand(descriptor);
            command.Invoke(args); // not yet working
        }

        private static void Report(Strategy strategy)
        {
            Console.WriteLine(strategy.Report());
        }


    }
    public class MyClass
    {
        public  void Test(string nameArg, int A, string B, int C)
        {
            Console.WriteLine($"Name: {nameArg} A: {A} B: {B} C: {C}");
        }
    }
}
