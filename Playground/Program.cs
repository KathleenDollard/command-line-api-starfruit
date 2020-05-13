using System;
using System.CommandLine;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.Invocation;

namespace UserStudyTest2
{
    class Program
    {
        static void Main(string[] args)
        {
            var strategy = new Strategy("Standard").SetStandardRules();
            Console.WriteLine(strategy.Report());
        }
    }
}
