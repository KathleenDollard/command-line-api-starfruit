using System.Diagnostics.CodeAnalysis;
using System.CommandLine.Binding;
using System.CommandLine.Builder;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.Parsing;
using System.CommandLine.ReflectionAppModel;
using System.Linq;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace System.CommandLine
{
    public static class CommandLineActivator
    {
        public static int Invoke<TRoot>(string[] args, Strategy? strategy = null)
        {
            strategy ??= Strategy.Standard;
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeof(TRoot));
            var command = CommandMaker.MakeRootCommand(descriptor);
            var parser = new CommandLineBuilder(command)
                                .UseDefaults()
                                .Build();
            var parseResult = parser.Parse(args);
            if (parseResult.Errors.Any())
            {
                Console.WriteLine("Errors!");
            }
            return command.Invoke(args);
        }

        // [return: MaybeNull] Not finding the local one
        public static TRoot CreateInstance<TRoot>(string[] args, Strategy? strategy = null)
        {
            strategy ??= Strategy.Standard;
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor(strategy, typeof(TRoot));
            var command = CommandMaker.MakeRootCommand(descriptor);
            var parser = new CommandLineBuilder(command)
                                .UseDefaults()
                                .Build();
            var parseResult = parser.Parse(args);
            if (parseResult.Errors.Any())
            {
                Console.WriteLine("Errors!");
            }
            var commandDescriptor = GetCommandDescriptor(descriptor, parseResult.CommandResult.Command);
            if (commandDescriptor is null)
            {
                throw new InvalidOperationException("Cannot create instance unless bound to a type.");
            }
            var typeOrMethodInfo = commandDescriptor.Raw as Type;
            if (typeOrMethodInfo is null)
            {
                throw new InvalidOperationException("Cannot create instance when bound to a method.");
            }
            var binder = new ModelBinder(typeOrMethodInfo);
            var bindingContext = new BindingContext(command.Parse(args));
            return (TRoot)binder.CreateInstance(bindingContext);
        }

        private static CommandDescriptor? GetCommandDescriptor(CommandDescriptor descriptor, ICommand command)
        {
            if (descriptor.SymbolToBind == command)
            {
                return descriptor;
            }
            foreach (var subDescriptor in descriptor.SubCommands)
            {
                var subCommand = GetCommandDescriptor(subDescriptor, command);
                if (!(subCommand is null))
                {
                    return subCommand;
                }
            }
            return null;
        }
    }
}
