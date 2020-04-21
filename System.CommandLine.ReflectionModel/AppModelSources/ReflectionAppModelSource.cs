using System;
using System.Collections;
using System.Collections.Generic;
using System.CommandLine.Binding;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.CommandLine.ReflectionModel.Strategies;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System.CommandLine.ReflectionModel.AppModelSources
{
    public class ReflectionAppModelSource : AppModelSourceBase
    {

        private readonly MethodInfo methodInfo;
        private object target;
        public StrategiesSet StrategiesSet { get; } = new StrategiesSet();

        public override bool Configure(Command command,
                                       object startingItem,
                                       object target = null) 
            => startingItem switch
                    {
                        MethodInfo methodInfo => Configure(command, methodInfo, target),
                        Type type => Configure(command, type, target),
                        _ => throw new ArgumentException(nameof(startingItem))
                    };

        public bool Configure(
              Command command,
              MethodInfo methodInfo,
              object target = null)
        {
            _ = command ?? throw new ArgumentNullException(nameof(command));
            _ = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
            this.target = target;

            var parameters = methodInfo.GetParameters();
            if (parameters.Count() == 1
                && StrategiesSet.CommandStrategies.IsCommand(parameters.First()))
            {
                AddChildren(command, parameters.First().ParameterType);
            }
            else
            {
                AddChildren(command, methodInfo);
            };

            command.Handler = CommandHandler.Create(methodInfo, target);
            return true;
        }

        public bool Configure(
                Command command,
                Type type,
                object target = null)
        {
            _ = command ?? throw new ArgumentNullException(nameof(command));
            _ = type ?? throw new ArgumentNullException(nameof(type));

            // TODO: Differentiate between Main with single type and more complex scenarios (sometimes need to collapse root type)
            AddChildren(command, type);

            return true;
        }

        public void AddChildren(Command command, MethodInfo method)
        {
            var parameters = method.GetParameters();
            var argumentParameters = parameters.Where(p => StrategiesSet.ArgumentStrategies.IsArgument(p)).ToList();
            var commandParameters = parameters.Where(p => StrategiesSet.CommandStrategies.IsCommand(p)).ToList();
            var optionParameters = parameters.Except(argumentParameters).Except(commandParameters).ToList();

            command.AddArguments(argumentParameters
                                   .Select(p => BuildArgument(p)));
            command.AddCommands(commandParameters
                                    .Select(p => BuildCommand(p)));
            command.AddOptions(optionParameters
                                    .Select(p => BuildOption(p)));
        }

        public void AddChildren(Command command, Type type)
        {
            var properties = type.GetProperties();
            var argumentProperties = properties.Where(p => StrategiesSet.ArgumentStrategies.IsArgument(p));
            var commandProperties = properties.Where(p => StrategiesSet.CommandStrategies.IsCommand(p));
            var optionProperties = properties.Except(argumentProperties).Except(commandProperties);

            command.AddArguments(argumentProperties
                                    .Select(p => BuildArgument(p))
                                    .ToList());
            command.AddCommands(commandProperties
                                    .Select(p => BuildCommand(p))
                                    .ToList());
            command.AddOptions(optionProperties
                                    .Select(p => BuildOption(p))
                                    .ToList());

            command.AddCommands(StrategiesSet.SubCommandStrategies.GetCommandTypes(type)
                        .Select(t => BuildCommand(t))
                        .ToList());

        }

        public Option BuildOption(ParameterInfo param)
        {
            // TODO: DefaultValue strategy 
            bool? argumentRequired = StrategiesSet.RequiredStrategies.IsRequired(param, SymbolType.Argument); ;
            bool? optionRequired = StrategiesSet.RequiredStrategies.IsRequired(param, SymbolType.Option); ;
            ArityDescriptor arityDescriptor = StrategiesSet.ArityStrategies.GetArity(param);
            var argument = BuildArgument(GetName(param, SymbolType.Argument), param.ParameterType, GetDescription(param, SymbolType.Argument), argumentRequired, arityDescriptor);

            return BuildOption(GetName(param, SymbolType.Option), GetDescription(param, SymbolType.Option), optionRequired, argument);
        }

        public Option BuildOption(PropertyInfo prop)
        {
            // TODO: DefaultValue strategy 
            bool? argumentRequired = StrategiesSet.RequiredStrategies.IsRequired(prop, SymbolType.Argument); ;
            bool? optionRequired = StrategiesSet.RequiredStrategies.IsRequired(prop, SymbolType.Option); ;
            ArityDescriptor arityDescriptor = StrategiesSet.ArityStrategies.GetArity(prop);
            var argument = BuildArgument(GetName(prop, SymbolType.Argument), prop.PropertyType, GetDescription(prop, SymbolType.Argument), argumentRequired, arityDescriptor);

            return BuildOption(GetName(prop, SymbolType.Option), GetDescription(prop, SymbolType.Option), optionRequired, argument);
        }

        public Option BuildOption(string name, string description, bool? optionRequired, Argument argument)
        {
            var option = new Option("--" + name.ToKebabCase().ToLowerInvariant())
            {
                Argument = argument,
                Description = description,

                //Required = required
            };
            if (optionRequired.HasValue)
            {
                option.Required = optionRequired.Value;
            }
            return option;
        }

        public Argument BuildArgument(ParameterInfo param)
        {
            // TODO: DefaultValue strategy 
            var required = StrategiesSet.RequiredStrategies.IsRequired(param, SymbolType.Argument);
            return BuildArgument(GetName(param, SymbolType.Argument),
                                 param.ParameterType,
                                 GetDescription(param, SymbolType.Argument),
                                 required,
                                 StrategiesSet.ArityStrategies.GetArity(param));
        }

        public Argument BuildArgument(PropertyInfo prop)
        {
            // TODO: DefaultValue strategy 
            var required = StrategiesSet.RequiredStrategies.IsRequired(prop, SymbolType.Argument);
            return BuildArgument(GetName(prop, SymbolType.Argument),
                                 prop.PropertyType,
                                 GetDescription(prop, SymbolType.Argument),
                                 required,
                                 StrategiesSet.ArityStrategies.GetArity(prop));
        }

        public Argument BuildArgument(string name, Type type, string description, bool? required, ArityDescriptor arityDescriptor)
        {
            // @jonsequitor to review please - can arity have a lower bound and supply upper or is there another way to force required. Maybe an IsRequired method setting something internal. This is a lot of code that overrides core functionality
            var arg = new Argument(name.ToUpperInvariant())
            {
                Description = description,
                ArgumentType = type,
            };

            var isRequired = required.GetValueOrDefault();
            if (!(arityDescriptor is null) && arityDescriptor.IsSet)
            {
                var min = isRequired && arityDescriptor.Min <= 1
                            ? 1
                            : arityDescriptor.Min;
                arg.Arity = new ArgumentArity(min, arityDescriptor.Max);
            }
            else if (isRequired)
            {
                // TODO: This is probably not complete for collections!!!!!
                var max = !typeof(string).IsAssignableFrom(type) && typeof(IEnumerable).IsAssignableFrom(type)
                            ? byte.MaxValue
                            : 1;
                arg.Arity = new ArgumentArity(1, max);
            }
            // TODO: Set default value
            return arg;
        }

        public Command BuildCommand(ParameterInfo param)
        {
            var name = StrategiesSet.NameStrategies.Name(param, SymbolType.Command);
            var type = param.ParameterType;
            var description = StrategiesSet.DescriptionStrategies.Description(param, SymbolType.Command);

            return BuildCommand(name, description, type);
        }

        public Command BuildCommand(PropertyInfo prop)
        {
            var name = StrategiesSet.NameStrategies.Name(prop, SymbolType.Command);
            var type = prop.PropertyType;
            var description = StrategiesSet.DescriptionStrategies.Description(prop, SymbolType.Command);

            return BuildCommand(name, description, type);
        }

        public Command BuildCommand(Type type)
        {
            var name = StrategiesSet.NameStrategies.Name(type, SymbolType.Command);
            var description = StrategiesSet.DescriptionStrategies.Description(type, SymbolType.Command);

            return BuildCommand(name, description, type);
        }

        public Command BuildCommand(string name, string description, Type type)
        {

            var command = new Command(name.ToKebabCase().ToLowerInvariant())
            {
                Description = description,
            };
            command.Handler = CommandHandler.Create<InvocationContext>(
                ic =>
                {
                    var parseResult = ic.ParseResult;
                    if (ic.ParseResult.Errors.Count == 0)
                    {
                        var binder = new ModelBinder(type);
                        var newObj = binder.CreateInstance(ic.BindingContext);
                        methodInfo.Invoke(null, new[] { newObj });
                    }
                });

            AddChildren(command, type);

            return command;
        }

        private string GetDescription(ParameterInfo param, SymbolType symbolType) => StrategiesSet.DescriptionStrategies.Description(param, symbolType);
        private string GetName(ParameterInfo param, SymbolType symbolType) => StrategiesSet.NameStrategies.Name(param, symbolType);
        private string GetDescription(PropertyInfo prop, SymbolType symbolType) => StrategiesSet.DescriptionStrategies.Description(prop, symbolType);
        private string GetName(PropertyInfo prop, SymbolType symbolType) => StrategiesSet.NameStrategies.Name(prop, symbolType);

    }
}
