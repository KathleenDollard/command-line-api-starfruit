using System.CommandLine.Binding;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.ReflectionAppModel
{
    public class CommandMakerSpecificSource : CommandMakerSpecificSourceBase
    {
        public override BinderWrapper MakeModelBinder(CommandDescriptor commandDescriptor)
        {
            return new BinderWrapper<ModelBinder>(commandDescriptor.Raw switch
            {
                Type t => GetModelBinder(t, commandDescriptor),
                MethodInfo m => throw new NotImplementedException(),
                _ => throw new InvalidOperationException()
            });
        }

        private ModelBinder GetModelBinder(Type commandType, CommandDescriptor commandDescriptor)
        {
            var modelBinder = new ModelBinder(commandType);

            // TODO: Consider contructor
            BindOptions(commandDescriptor, modelBinder);
            BindArguments(commandDescriptor, modelBinder);

            return modelBinder;
        }

        private static void BindOptions(CommandDescriptor commandDescriptor, ModelBinder modelBinder)
        {
            var propertyOptions = commandDescriptor.Options
                                    .Where(x => x.Raw is PropertyInfo);
            foreach (var optionDescriptor in propertyOptions)
            {
                if (optionDescriptor.Raw is PropertyInfo propertyInfo) // already filtered
                {
                    if (!(optionDescriptor.SymbolToBind is Option option))
                    {
                        throw new InvalidOperationException("Unexpected binding source.");
                    }
                    modelBinder.BindMemberFromValue(propertyInfo, option);
                }
            }
        }

        private static void BindArguments(CommandDescriptor commandDescriptor, ModelBinder modelBinder)
        {
            var propertyArguments = commandDescriptor.Arguments
                                    .Where(x => x.Raw is PropertyInfo);
            foreach (var propertyArgument in propertyArguments)
            {
                if (propertyArgument.Raw is PropertyInfo propertyInfo) // already filtered
                {
                    if (!(propertyArgument.SymbolToBind is Argument argument))
                    {
                        throw new InvalidOperationException("Unexpected binding source.");
                    }
                    modelBinder.BindMemberFromValue(propertyInfo, argument);
                }
            }
        }
    }
}
