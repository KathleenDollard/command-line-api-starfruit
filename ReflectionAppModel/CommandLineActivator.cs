using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.Invocation;
using System.CommandLine.ReflectionAppModel;
using System.Reflection;
using System.Text;

namespace System.CommandLine
{
    public static class CommandLineActivator
    {
        public static int Invoke<TRoot>(string[] args, Strategy? strategy = null)
        {
            strategy ??= Strategy.Standard;
            MethodInfo entryMethod = typeof(TRoot).GetMethod("Invoke");
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, entryMethod);
            var command = CommandMaker.MakeRootCommand(descriptor);
            command.Handler = CommandHandler.Create(entryMethod);
            command.Invoke(args);
            return 0;
        }
}
}
