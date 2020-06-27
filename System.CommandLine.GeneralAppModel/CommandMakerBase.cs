using System.Collections.Generic;
using System.CommandLine.Binding;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.CommandLine.GeneralAppModel
{
    public abstract class CommandMakerBase<TRoot, TCommand, TOption, TArgument, TModelBinder, TItemTarget, TCommandTarget>
        where TModelBinder : class
    {
        protected abstract void FillCommandInternal(TCommand command, CommandDescriptor descriptor);
        protected abstract TOption MakeOption(OptionDescriptor descriptor);
        protected abstract TArgument MakeArgument(ArgumentDescriptor descriptor);
        protected abstract TModelBinder? MakeModelBinderInternal(CommandDescriptor commandDescriptor);
        protected abstract TModelBinder? GetModelBinderForType(Type type,  CommandDescriptor commandDescriptor);
        //protected abstract void BindSymbol<TSymbol>(TModelBinder modelBinder, TItemTarget target, TSymbol symbol)
        //    where TSymbol : Symbol;

        protected bool ValidateDescriptor(CommandDescriptor descriptor)
        {
            var (success, messages) = descriptor.ValidateRoot();
            if (!success)
            {
                throw new InvalidOperationException("There are errors in the definition of your CLI. See the Inner Exception.\n\t" +
                                                    string.Join("\n\t", messages.Select(x => x.Message)),
                                new DescriptorInvalidException(messages));
            }
            return true;
        }

 
        //protected TModelBinder? GetModelBinderForType(TModelBinder modelBinder, CommandDescriptor commandDescriptor)
        //{
        //    BindOptions(modelBinder, commandDescriptor);
        //    BindArguments(modelBinder, commandDescriptor);
        //    BindSubCommands(modelBinder, commandDescriptor);
        //    return modelBinder;
        //}

        //protected void BindOptions(TModelBinder modelBinder, CommandDescriptor commandDescriptor)
        //{
        //    // More validation
        //    var propertyOptions = commandDescriptor.Options
        //                            .Where(x => x.Raw is TItemTarget);
        //    foreach (var optionDescriptor in propertyOptions)
        //    {
        //        if (optionDescriptor.Raw is TItemTarget item) // already filtered
        //        {
        //            if (!(optionDescriptor.SymbolToBind is TOption option))
        //            {
        //                throw new InvalidOperationException("Unexpected binding source.");
        //            }
        //            BindOption(modelBinder, item, option);
        //        }
        //    }
        //}

        //protected void BindArguments(TModelBinder modelBinder, CommandDescriptor commandDescriptor)
        //{
        //    // More validation
        //    var propertyArguments = commandDescriptor.Arguments
        //                            .Where(x => x.Raw is TItemTarget);
        //    foreach (var propertyArgument in propertyArguments)
        //    {
        //        if (propertyArgument.Raw is TItemTarget item) // already filtered
        //        {
        //            if (!(propertyArgument.SymbolToBind is TArgument argument))
        //            {
        //                throw new InvalidOperationException("Unexpected binding source.");
        //            }
        //            BindArgument(modelBinder, item, argument);
        //        }
        //    }
        //}

        //protected void BindSubCommands(TModelBinder modelBinder, CommandDescriptor commandDescriptor)
        //{
        //    // More validation
        //    var commands = commandDescriptor.Arguments
        //                                 .Where(x => x.Raw is TCommandTarget);
        //    foreach (var propertyCommand in commands)
        //    {
        //        if (propertyCommand.Raw is TCommandTarget commandTarget) // already filtered
        //        {
        //            if (!(propertyCommand.SymbolToBind is TCommand command))
        //            {
        //                throw new InvalidOperationException("Unexpected binding source.");
        //            }
        //            BindSubCommand(modelBinder, commandTarget, command);
        //        }
        //    }
        //}

    }
}
