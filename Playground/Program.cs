using Playground;
using System;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.ReflectionAppModel;
using System.Security.Cryptography.X509Certificates;

namespace UserStudyTest2
{
    class Program
    {
        // Old debug: --name "Hello" --a 7 --b 13 --c 17
        static int Main(string[] args)
        {
            return ManageGlobalJsonProgram.Main2(args);

            //var strategy = new Strategy("Standard").SetReflectionRules();

            //Console.WriteLine(strategy.Report());

            //strategy.InvokeMethod(typeof(Program).GetMethod("Test"), args);

            //var instance = strategy.CreateInstance<MyClass>(args);
            //Console.WriteLine($"From Type: Name: {instance.Name} A: {instance.A} B: {instance.B} C: {instance.C}");
        }

        public static void Test(string name, int a, string b, int c)
        {
            Console.WriteLine($"From Method: Name: {name} A: {a} B: {b} C: {c}");
        }
    }

    public class MyClass
    {
        public string Name { get; set; }
        public int A { get; set; }
        public string B { get; set; }
        public int C { get; set; }
    }
}
