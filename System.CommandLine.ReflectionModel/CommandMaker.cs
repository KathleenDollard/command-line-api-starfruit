using System.Collections;
using System.CommandLine.Binding;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.CommandLine.ReflectionModel;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace System.CommandLine.ReflectionModel
{
    public class CommandMaker
    {
        // Get this from ServiceProvider
        private readonly Type[] omittedTypes = new[]
                           {
                                   typeof(IConsole),
                                   typeof(InvocationContext),
                                   typeof(BindingContext),
                                   typeof(ParseResult),
                                   typeof(CancellationToken),
                               };

        public ArgumentStrategies ArgumentStrategies { get; } = new ArgumentStrategies();
        public CommandStrategies CommandStrategies { get; } = new CommandStrategies();
        public SubCommandStrategies SubCommandStrategies { get; } = new SubCommandStrategies();
        public ArityStrategies ArityStrategies { get; } = new ArityStrategies();
        public DescriptionStrategies DescriptionStrategies { get; } = new DescriptionStrategies();
        public NameStrategies NameStrategies { get; } = new NameStrategies();
        public IsRequiredStrategies RequiredStrategies { get; } = new IsRequiredStrategies();

        public void Configure(
              Command command,
              MethodInfo method,
              object target = null)
        {
            _ = command ?? throw new ArgumentNullException(nameof(command));
            _ = method ?? throw new ArgumentNullException(nameof(method));

            // TODO: Differentiate between Main with single type and more complex scenarios (sometimes need to collapse root type)
            AddChildren(command, method);
            command.Handler = CommandHandler.Create(method, target);
        }

        public void Configure(
               Command command,
               Type type,
               object target = null)
        {
            _ = command ?? throw new ArgumentNullException(nameof(command));
            _ = type ?? throw new ArgumentNullException(nameof(type));

            // TODO: Differentiate between Main with single type and more complex scenarios (sometimes need to collapse root type)
            AddChildren(command, type);
        }

        public void AddChildren(Command command, MethodInfo method)
        {
            var parameters = method.GetParameters();
            var argumentParameters = parameters.Where(p => ArgumentStrategies.IsArgument(p));
            var commandParameters = parameters.Where(p => CommandStrategies.IsCommand(p));
            var optionParameters = parameters.Except(argumentParameters).Except(commandParameters);

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
            var argumentProperties = properties.Where(p => ArgumentStrategies.IsArgument(p));
            var commandProperties = properties.Where(p => CommandStrategies.IsCommand(p));
            var optionProperties = properties.Except(argumentProperties).Except(commandProperties);

            command.AddArguments(argumentProperties
                                    .Select(p => BuildArgument(p)));
            command.AddCommands(commandProperties
                                    .Select(p => BuildCommand(p)));
            command.AddOptions(optionProperties
                                    .Select(p => BuildOption(p)));

            var subCommands = SubCommandStrategies.GetCommandTypes(type)
                        .Select(t => BuildCommand(t));
            command.AddCommands(subCommands);
        }


        public Option BuildOption(ParameterInfo param)
        {
            // TODO: DefaultValue strategy 
            bool? argumentRequired = RequiredStrategies.IsRequired(param, SymbolType.Argument); ;
            bool? optionRequired = RequiredStrategies.IsRequired(param, SymbolType.Option); ;
            (int min, int max)? arityMinMax = ArityStrategies.MinMax(param);
            var argument = BuildArgument(GetName(param), param.ParameterType, GetDescription(param), argumentRequired, arityMinMax);

            return BuildOption(GetName(param), GetDescription(param), optionRequired, argument);
        }

        public Option BuildOption(PropertyInfo prop)
        {
            // TODO: DefaultValue strategy 
            bool? argumentRequired = RequiredStrategies.IsRequired(prop, SymbolType.Argument); ;
            bool? optionRequired = RequiredStrategies.IsRequired(prop, SymbolType.Option); ;
            (int min, int max)? arityMinMax = ArityStrategies.MinMax(prop);
            var argument = BuildArgument(GetName(prop), prop.PropertyType, GetDescription(prop), argumentRequired, arityMinMax);

            return BuildOption(GetName(prop), GetDescription(prop), optionRequired,  argument);
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
            var required = RequiredStrategies.IsRequired(param, SymbolType.Argument);
            return BuildArgument(GetName(param), param.ParameterType, GetDescription(param), required, ArityStrategies.MinMax(param));
        }

        public Argument BuildArgument(PropertyInfo prop)
        {
            // TODO: DefaultValue strategy 
            var required = RequiredStrategies.IsRequired(prop, SymbolType.Argument);
            return BuildArgument(GetName(prop), prop.PropertyType, GetDescription(prop), required, ArityStrategies.MinMax(prop));
        }

        public Argument BuildArgument(string name, Type type, string description, bool? required, (int min, int max)? arityMinMax)
        {
            // @jonsequitor to review please - can arity have a lower bound and supply upper or is there another way to force required. Maybe an IsRequired method setting something internal. This is a lot of code that overrides core functionality
            var arg = new Argument(name.ToUpperInvariant())
            {
                Description = description,
                ArgumentType = type,
            };

            var isRequired = required.GetValueOrDefault();
            if (arityMinMax.HasValue)
            {
                var min = isRequired && arityMinMax.Value.min <= 1
                            ? 1
                            : arityMinMax.Value.min;
                arg.Arity = new ArgumentArity(min, arityMinMax.Value.max);
            }
            else if (isRequired)
            {
                // TODO: This is probably not complete for collections!!!!!
                var max = !typeof(string).IsAssignableFrom(type) && typeof(IEnumerable).IsAssignableFrom(type)
                            ? byte.MaxValue
                            : 1;
                arg.Arity = new ArgumentArity(1,max);
            }
            // TODO: Set default value
            return arg;
        }

        public Command BuildCommand(ParameterInfo param)
        {
            var name = NameStrategies.Name(param);
            var type = param.ParameterType;
            var description = DescriptionStrategies.Description(param);

            return BuildCommand(name, description, type);
        }

        public Command BuildCommand(PropertyInfo prop)
        {
            var name = NameStrategies.Name(prop);
            var type = prop.PropertyType;
            var description = DescriptionStrategies.Description(prop);

            return BuildCommand(name, description, type);
        }

        public Command BuildCommand(Type type)
        {
            var name = NameStrategies.Name(type);
            var description = DescriptionStrategies.Description(type);

            return BuildCommand(name, description, type);
        }

        public Command BuildCommand(string name, string description, Type type)
        {

            var command = new Command(name.ToKebabCase().ToLowerInvariant())
            {
                Description = description,
            };
            AddChildren(command, type);

            return command;
        }

        private string GetDescription(ParameterInfo param) => DescriptionStrategies.Description(param);
        private string GetName(ParameterInfo param) => NameStrategies.Name(param);
        private string GetDescription(PropertyInfo prop) => DescriptionStrategies.Description(prop);
        private string GetName(PropertyInfo prop) => NameStrategies.Name(prop);

    }

    public static class CommandMakerExtensions
    {
        public static CommandMaker UseDefaults(this CommandMaker commandMaker)
        {
            commandMaker.ArgumentStrategies.AllStandard();
            commandMaker.CommandStrategies.AllStandard();
            commandMaker.ArityStrategies.AllStandard();
            commandMaker.DescriptionStrategies.AllStandard();
            commandMaker.RequiredStrategies.AllStandard();
            commandMaker.SubCommandStrategies.AllStandard();
            return commandMaker;
        }
    }
}
