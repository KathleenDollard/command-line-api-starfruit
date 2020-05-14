using System;
using System.CommandLine;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.ReflectionAppModel;
using System.CommandLine.Parsing;
using System.CommandLine.Binding;
using System.CommandLine.Invocation;
using System.Reflection;

namespace UserStudyTest2
{
    public static class Extensions
    {
        public static T CreateInstance<T>(this Strategy strategy ,string[] args)
            where T : new()
        {
            var type = typeof(T);
            var descriptor = ReflectionAppModel.RootCommandDescriptor(strategy, type);
            var command = CommandMaker.MakeCommand(descriptor);
            var binder = new ModelBinder(type);
            var bindingContext = new BindingContext(command.Parse(args));
            // TODO: Jon: Binder is not filling instance
            return (T)binder.CreateInstance(bindingContext);
        }

        public static int InvokeMethod(this Strategy strategy, MethodInfo methodInfo, string[] args)
        {
            System.Reflection.MethodInfo entryMethod = typeof(Program).GetMethod("Test");
            var descriptor = ReflectionAppModel.RootCommandDescriptor(strategy, entryMethod);
            var command = CommandMaker.MakeCommand(descriptor);
            command.Handler = CommandHandler.Create(methodInfo);
            command.Invoke(args);
            return 0;
        }

        public static void Report(this Strategy strategy)
        {
            Console.WriteLine(strategy.Report());
        }

    }
}
