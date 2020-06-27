using System.Diagnostics.CodeAnalysis;
using System.CommandLine.Binding;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Linq;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Reflection;
using System.CommandLine;
using System.Security.Cryptography;

namespace System.CommandLine.GeneralAppModel
{
    public abstract class CommandLineActivatorBase
    {
        protected abstract CommandDescriptor GetCommandDescriptor<TRoot>(Strategy? strategy = null);

        public int Invoke<TRoot>(string[] args, Strategy? strategy = null)
        {
            var descriptor = GetCommandDescriptor<TRoot>(strategy);
            var command = CommandMaker.MakeRootCommand(descriptor);
            var parser = new CommandLineBuilder(command)
                                .UseDefaults()
                                .Build();
            var parseResult = parser.Parse(args);
            if (parseResult.Errors.Any())
            {
                // TODO: Figure out strategy here
                Console.WriteLine("Errors!");
            }
            return command.Invoke(args);
        }

        [return: MaybeNull]
        public TRoot CreateInstance<TRoot>(string[] args, Strategy? strategy = null, string? commandName = null)
        {
            return CreateInstance<TRoot>(string.Join(" ", args), strategy, commandName);
        }

        [return: MaybeNull]
        public TRoot CreateInstance<TRoot>(string args, Strategy? strategy = null, string? commandName = null)
        {
            var descriptor = GetCommandDescriptor<TRoot>(strategy);
            var command = commandName is null
                            ? CommandMaker.MakeRootCommand(descriptor)
                            : CommandMaker.MakeCommand(descriptor);
            var parser = new CommandLineBuilder(command)
                                .UseDefaults()
                                .Build();
            var parseResult = parser.Parse(args);
            if (parseResult.Errors.Any())
            {
                // TODO: Figure out strategy here
                Console.WriteLine("Errors!");
            }

            var commandDescriptor = GetCommandDescriptor(descriptor, parseResult.CommandResult.Command);
            var _ = commandDescriptor ?? throw new InvalidOperationException("Descriptor for command not found.");
            var binder = CommandMaker.MakeModelBinder(commandDescriptor);
            var bindingContext = new BindingContext(command.Parse(args));
            return (TRoot)binder.CreateInstance(bindingContext);
        }

        //private static ModelBinder GetBinder(CommandDescriptor commandDescriptor)
        //{
        //    if (commandDescriptor is null)
        //    {
        //        throw new InvalidOperationException("Cannot create instance unless bound to a type.");
        //    }
        //    if (!(commandDescriptor.Raw is Type type))
        //    {
        //        throw new InvalidOperationException("Cannot create instance when bound to a method.");
        //    } 
        //    var binder  = new ModelBinder(type);

        //    foreach (var desc in commandDescriptor.Arguments )
        //    {
        //        if (!(desc.Raw is PropertyInfo propertyInfo))
        //        {
        //            throw new InvalidOperationException("Argument not backed by a property on the type.");
        //        }
        //        if (!(desc.SymbolToBind is IValueDescriptor valueDescriptor ))
        //        {
        //            throw new InvalidOperationException("Property not backed by something recognized as producing a value.");
        //        }
        //        binder.BindMemberFromValue(propertyInfo, valueDescriptor);
        //    }

        //    return binder;
        //}

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

