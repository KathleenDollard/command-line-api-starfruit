using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.GeneralAppModel
{
    public static class CommandMaker
    {
        public static RootCommand MakeRootCommand(CommandDescriptor descriptor)
        {
            var (success, messages) = descriptor.ValidateRoot();
            if (!success)
            {
                throw new InvalidOperationException("There are errors in the definition of your CLI. See the Inner Exception.\n\t" +
                                                    string.Join("\n\t", messages.Select(x=>x.Message)),
                                new DescriptorInvalidException(messages));
            }
            var command = new RootCommand(descriptor.Description ?? string.Empty);
            FillCommand(command, descriptor);
            return command;
        }

        public static Command MakeCommand(CommandDescriptor descriptor)
        {
            var _ = descriptor.Name ?? throw new InvalidOperationException("The name for a non-root command cannot be null");
            var subCommand = new Command(descriptor.Name, descriptor.Description);
            FillCommand(subCommand, descriptor);
            return subCommand;
        }

        public static void FillCommand(Command command, CommandDescriptor descriptor)
        {
            command.IsHidden = descriptor.IsHidden;
            command.TreatUnmatchedTokensAsErrors = descriptor.TreatUnmatchedTokensAsErrors;
            SetHandlerIfNeeded(command, descriptor);
            AddAliases(command, descriptor.Aliases);
            command.AddArguments(descriptor.Arguments
                                    .Select(a => MakeArgument(a)));
            command.AddOptions(descriptor.Options
                                    .Select(o => MakeOption(o)));
            command.AddCommands(descriptor.SubCommands
                                    .Select(c => MakeCommand(c)));
        }

        private static void SetHandlerIfNeeded(Command command, CommandDescriptor descriptor)
        {
            if (!(descriptor.InvokeMethod is null) )
            {
                var invokeMethodInfo = descriptor.InvokeMethod.GetInvokeMethod<MethodInfo>();
                command.Handler = CommandHandler.Create(invokeMethodInfo);
                return;
            }
            if (descriptor.Raw is MethodInfo rawMethodInfo)
            {
                command.Handler = CommandHandler.Create(rawMethodInfo);
            }
        }

        public static Option MakeOption(OptionDescriptor descriptor)
        {
            var _ = descriptor.Name ?? throw new InvalidOperationException("The name for an option cannot be null");
            var option = new Option("--" + descriptor.Name.ToKebabCase(), descriptor.Description);
            if (descriptor.Arguments.Any())
            {
                option.Argument = descriptor
                                   .Arguments
                                   .Select(a => MakeArgument(a))
                                   .FirstOrDefault();
            }
            AddAliases(option, descriptor.Aliases);

            option.IsHidden = descriptor.IsHidden;
            option.Required = descriptor.Required;
            return option;
        }

        private static void AddAliases(Symbol symbol, IEnumerable<string>? aliases)
        {
            if (aliases is null)
            {
                return;
            }
            foreach (var alias in aliases)
            {
                symbol.AddAlias(alias);
            }
        }

        public static Argument MakeArgument(ArgumentDescriptor descriptor)
        {
            var arg = new Argument(descriptor.Name);
            arg.ArgumentType = descriptor.ArgumentType.GetArgumentType<Type>(); // need work here for Roslyn source generation
            AddAliases(arg, descriptor.Aliases);
            if (descriptor.Arity != null)
            {
                arg.Arity = new ArgumentArity(descriptor.Arity.MinimumCount, descriptor.Arity.MaximumCount);
            }
            arg.Description = descriptor.Description;
            arg.IsHidden = descriptor.IsHidden;
            if (descriptor.DefaultValue != null)
            {
                arg.SetDefaultValue(descriptor.DefaultValue.DefaultValue);
            }
            return arg;
        }
    }
}
