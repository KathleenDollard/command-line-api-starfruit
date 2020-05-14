using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.Parsing;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    public static class CommandMaker
    {
        public static Command Make(CommandDescriptor commandDescriptor)
        {
            var command = new RootCommand(commandDescriptor.Description);
            FillCommand(command, commandDescriptor);
            return command;
        }

        public static Command MakeCommand(CommandDescriptor descriptor)
        {
            var subCommand = new Command(descriptor.Name, descriptor.Description);
            FillCommand(subCommand, descriptor);
            return subCommand;
        }

        private static void FillCommand(Command command, CommandDescriptor descriptor)
        {
            command.IsHidden = true;
            AddAliases(command, descriptor.Aliases);
            command.AddArguments(descriptor.Arguments
                                    .Select(a => MakeArgument(a)));
            command.AddOptions(descriptor.Options
                                    .Select(o => MakeOption(o)));
            command.AddCommands(descriptor.SubCommands
                                    .Select(c => MakeCommand(c)));
        }

        public static Option MakeOption(OptionDescriptor descriptor)
        {
            var option = new Option("--" + descriptor.Name.ToKebabCase(), descriptor.Description);
            if (!(descriptor.Arguments is null))
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

        private static void AddAliases(Symbol symbol, IEnumerable<string> aliases)
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
            arg.ArgumentType = descriptor.ArgumentType;
            AddAliases(arg, descriptor.Aliases);
            if (descriptor.Arity != null)
            {
                arg.Arity = new ArgumentArity(descriptor.Arity.MinimumNumberOfValues, descriptor.Arity.MaximumNumberOfValues);
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
