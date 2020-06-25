using System.Collections.Generic;
using System.CommandLine.Binding;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.GeneralAppModel
{
    // CommandMaker does not use the "SpecificSource" pattern that DescriptorMakers use because 
    // the Descriptor simplifes the problem so we can make an interface or base class. 

    // TODO: All the validation should be in the validate process, or each step should a base class check followed by a call to an abstract internal impl
    public class CommandMaker : CommandMakerBase<RootCommand, Command, Option, Argument, ModelBinder, PropertyInfo, Type>
    {
        public static RootCommand MakeRootCommand(CommandDescriptor descriptor)
            => MakeCommandInternal(new RootCommand(), descriptor);

        public static Command MakeCommand(CommandDescriptor descriptor)
        {
            var _ = descriptor.Name ?? throw new InvalidOperationException("The name for a non-root command cannot be null");

            return MakeCommandInternal(new Command(descriptor.Name), descriptor);
        }

        public static ModelBinder MakeModelBinder<TCommand>(CommandDescriptor descriptor)
            where TCommand : Command
        {
            var maker = new CommandMaker();
            var _ = maker.ValidateDescriptor(descriptor); // method throws
            return maker.MakeModelBinder(descriptor);
        }

        public static void FillCommand(Command command, CommandDescriptor descriptor)
        {
            var maker = new CommandMaker();
            maker.FillCommandInternal(command, descriptor);
        }

        private static TCommand MakeCommandInternal<TCommand>(TCommand command, CommandDescriptor descriptor)
             where TCommand : Command
        {
            var maker = new CommandMaker();
            var _ = maker.ValidateDescriptor(descriptor); // method throws
            maker.FillCommandInternal(command, descriptor);
            return command;
        }

        protected override void FillCommandInternal(Command command, CommandDescriptor descriptor)
        {
            command.Description = descriptor.Description;
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
            descriptor.SetSymbol(command);
        }

        private static void SetHandlerIfNeeded(Command command, CommandDescriptor descriptor)
        {
            if (!(descriptor.InvokeMethod is null))
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

        protected override Option MakeOption(OptionDescriptor descriptor)
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
            descriptor.SetSymbol(option);
            return option;
        }

        protected override Argument MakeArgument(ArgumentDescriptor descriptor)
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
            descriptor.SetSymbol(arg);
            return arg;
        }

        // Basic logic:
        //    Set modelbinder when the binding context is created.
        //    Create and check the parse result
        //    Create an instance from the parse result
        //    Use that instance to call invoke or the method. Check if Invocatin handles this better. 
        protected override ModelBinder? MakeModelBinder(CommandDescriptor commandDescriptor)
        {
            return commandDescriptor.Raw switch
            {
                Type t => GetModelBinderForType(new ModelBinder(t), commandDescriptor),
                MethodInfo m => null,
                _ => throw new InvalidOperationException()
            };
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

        protected override void BindOption(ModelBinder modelBinder, PropertyInfo target, Option option)
        {
            modelBinder.BindMemberFromValue(target, option);
        }

        protected override void BindArgument(ModelBinder modelBinder, PropertyInfo target, Argument argument)
        {
            modelBinder.BindMemberFromValue(target, argument);
        }

        protected override void BindSubCommand(ModelBinder modelBinder, Type target, Command option)
        {
            throw new NotImplementedException();
        }
    }
}
