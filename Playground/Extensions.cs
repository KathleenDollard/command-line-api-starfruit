using System;
using System.CommandLine;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.ReflectionAppModel;
using System.CommandLine.Parsing;
using System.CommandLine.Binding;
using System.CommandLine.Invocation;
using System.Reflection;

namespace System.CommandLine.GeneralAppModel
{
    public static class Extensions
    {
        public static T CreateInstance<T>(this Strategy strategy ,string[] args)
            where T : new()
        {
            var type = typeof(T);
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, type);
            var command = CommandMaker.MakeRootCommand(descriptor);
            var binder = new ModelBinder(type);
            var bindingContext = new BindingContext(command.Parse(args));
            return (T)binder.CreateInstance(bindingContext);
        }
    }
}
