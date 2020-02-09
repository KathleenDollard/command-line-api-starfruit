using System.CommandLine.Binding;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace System.CommandLine.ReflectionAppModel
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
        public ArityStrategies ArityStrategies { get; } = new ArityStrategies();
        public DescriptionStrategies DescriptionStrategies { get; } = new DescriptionStrategies();
        public NameStrategies NameStrategies { get; } = new NameStrategies();

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
        }


        public Option BuildOption(ParameterInfo param)
        {
            var name = NameStrategies.Name(param);
            var description = DescriptionStrategies.Description(param);
            var arityMinMax = ArityStrategies.MinMax(param);
            // TODO: DefaultValue strategy 
            // TODO: Required strategy 
            var required = false;
            var argument = BuildArgument(name, description, required, arityMinMax);

            return BuildOption(name, description, argument);

        }

        public Option BuildOption(PropertyInfo prop)
        {
            var name = NameStrategies.Name(prop);
            var description = DescriptionStrategies.Description(prop);
            var arityMinMax = ArityStrategies.MinMax(prop);
            // TODO: DefaultValue strategy 
            // TODO: Required strategy 
            var required = false;
            var argument = BuildArgument(name, description, required, arityMinMax);

            return BuildOption(name, description, argument);
        }

        public Option BuildOption(string name, string description, Argument argument)
        {
            var option = new Option("--" + name.ToKebabCase().ToLowerInvariant())
            {
                Argument = argument,
                Description = description,

                //Required = required
            };
            return option;
        }

        public Argument BuildArgument(ParameterInfo param)
        {
            var name = NameStrategies.Name(param);
            var description = DescriptionStrategies.Description(param);
            var arityMinMax = ArityStrategies.MinMax(param);
            // TODO: DefaultValue strategy 
            // TODO: Required strategy 
            var required = false;

            return BuildArgument(name, description, required, arityMinMax);
        }

        public Argument BuildArgument(PropertyInfo prop)
        {
            var name = NameStrategies.Name(prop);
            var description = DescriptionStrategies.Description(prop);
            var arityMinMax = ArityStrategies.MinMax(prop);
            // TODO: DefaultValue strategy 
            // TODO: Required strategy 
            var required = false;

            return BuildArgument(name, description, required, arityMinMax);
        }

        public Argument BuildArgument(string name, string description, bool required, (int min, int max)? arityMinMax)
        {
            var arg = new Argument(name.ToUpperInvariant())
            {
                Description = description,
                //Required = required
            };
            if (arityMinMax.HasValue)
            {
                arg.Arity = new ArgumentArity(arityMinMax.Value.min, arityMinMax.Value.max);
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

        public Command BuildCommand(string name, string description, Type type)
        {

            var command = new Command(name.ToKebabCase().ToLowerInvariant())
            {
                Description = description,
            };
            AddChildren(command, type);
            return command;
        }

        //if (method.GetParameters()
        //          .FirstOrDefault(p => _argumentParameterNames.Contains(p.Name)) is ParameterInfo argsParam)
        //{
        //    var argument = new Argument
        //    {
        //        ArgumentType = argsParam.ParameterType,
        //        Name = argsParam.Name
        //    };

        //    if (argsParam.HasDefaultValue)
        //    {
        //        if (argsParam.DefaultValue != null)
        //        {
        //            argument.SetDefaultValue(argsParam.DefaultValue);
        //        }
        //        else
        //        {
        //            argument.SetDefaultValueFactory(() => null);
        //        }
        //    }

        //    command.AddArgument(argument);
        //}

        //command.Handler = CommandHandler.Create(method, target);
        //}

        //public Option BuildOption(ParameterDescriptor parameter)
        //{
        //    var argument = new Argument
        //    {
        //        ArgumentType = parameter.Type
        //    };

        //    if (parameter.HasDefaultValue)
        //    {
        //        argument.SetDefaultValueFactory(parameter.GetDefaultValue);
        //    }

        //    var option = new Option(
        //        parameter.BuildAlias(),
        //        parameter.ValueName)
        //    {
        //        Argument = argument
        //    };

        //    return option;
        //}
    }

    public static class CommandMakerExtensions
    {
        public static CommandMaker UseDefaults(this CommandMaker commandMaker)
        {
            commandMaker.ArgumentStrategies.AllStandard();
            commandMaker.CommandStrategies.AllStandard();
            commandMaker.ArityStrategies.AllStandard();
            commandMaker.DescriptionStrategies.AllStandard();
            return commandMaker;
        }
    }
}
