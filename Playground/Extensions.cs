using System;
using System.CommandLine;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.ReflectionAppModel;
using System.CommandLine.Parsing;
using System.CommandLine.Binding;
using System.CommandLine.Invocation;
using System.Reflection;
using System.CommandLine.GeneralAppModel.Descriptors;

namespace System.CommandLine.GeneralAppModel
{
    public static class Extensions
    {
        //public static T CreateInstance<T>(this Strategy strategy ,string[] args)
        //    where T : new()
        //{
        //    var type = typeof(T);
        //    var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, type);
        //    var rootCommand = CommandMaker.MakeRootCommand(descriptor);
        //    var bindingContext = new BindingContext(rootCommand.Parse(args));
        //    var calledCommandDescriptor = FindDescriptorForCommand(bindingContext.ParseResult.CommandResult.Command);
        //    ModelBinder binder = new ModelBinder()
        //    bindingContext.AddModelBinder(binder);
        //    return (T)binder.CreateInstance(bindingContext);
        //}

        private static CommandDescriptor?  FindDescriptorForCommand(CommandDescriptor parentDescriptor,ICommand command)
        {
           if (parentDescriptor == command)
            {
                return parentDescriptor;
            }
           foreach (var descriptor in parentDescriptor.SubCommands )
            {
                var match = FindDescriptorForCommand(descriptor, command);
                if (!(match is null))
                {
                    return match;
                }
            }
            return null;
        }
    }
}
