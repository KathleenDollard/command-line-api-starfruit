using System.CommandLine.Invocation;
using System.CommandLine.ReflectionAppModel;
using System.Reflection;

namespace System.CommandLine.GeneralAppModel
{
    public static  class PatternSupportingExtensions
    {
        public static int InvokeMethod<TRoot>(this Strategy strategy,string[] args)
        {
            // This is wrong
            MethodInfo entryMethod = typeof(TRoot).GetMethod("Invoke");
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, entryMethod);
            var command = CommandMaker.MakeRootCommand(descriptor);
            command.Handler = CommandHandler.Create(entryMethod);
            command.Invoke(args);
            return 0;
        }
    }
}
