using System;
using System.CommandLine.GeneralAppModel;
using System.Security.Cryptography.X509Certificates;

namespace UserStudyTest2
{
    class Program
    {
        static void Main(string[] args)
        {
            var strategy = new Strategy("Standard").SetStandardRules();
            //strategy.Report();
            strategy.InvokeMethod(typeof(Program).GetMethod("Test"), args);

            var instance = strategy.CreateInstance<MyClass>(args);
            Console.WriteLine($"From Type: Name: {instance.Name} A: {instance.A} B: {instance.B} C: {instance.C}");
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
        public string C { get; set; }
    }
}
