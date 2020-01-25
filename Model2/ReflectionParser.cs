using Model2.Args;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Binding;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Linq;
using System.Reflection;

namespace Model2
{
    public class ReflectionParser<T>
    {
        private  Dictionary<Type, List<Type>> _types;

        private  Dictionary<Type, List<Type>> Types
        {
            get
            {
                if (_types == null)
                {
                    _types = new Dictionary<Type, List<Type>>();
                    var ts = typeof(T).Assembly.GetExportedTypes();
                    foreach (var t in ts)
                    {
                        if (t.BaseType != typeof(object))
                        {
                            if (_types.ContainsKey(t.BaseType))
                            {
                                _types[t.BaseType].Add(t);
                                continue;
                            }
                            _types.Add(t.BaseType, new List<Type>() { t });
                        }
                    }
                }
                return _types;
            }
        }

        public static T GetInstance(string[] args)
        {
            var reflectionParser = new ReflectionParser<T>();
            var parser = reflectionParser.CreateParser();
            parser.Invoke(args);
            return reflectionParser.Result;
        }

        public  T Result { get; private set; }

        public  Parser CreateParser()
        {
            var command = GetCommand(typeof(T), true);
            var parser = new Parser(command);
            return parser;
        }

        public  Command GetCommand(Type type, bool isRoot = false)
        {
            var (argProperties, optionProperties) = GetProperties(type);
            var command = CreateCommand(type, isRoot);
            command.Handler = CommandHandler.Create<BindingContext>(
                bc =>
                {
                    var binder = new ModelBinder(type);
                    Result = (T)binder.CreateInstance(bc);
                });
            foreach (var property in argProperties)
            {
                command.AddArgument(GetArgument(property));
            }

            foreach (var property in optionProperties)
            {
                command.AddOption(GetOption(property));
            }

            if (Types.ContainsKey(type))
            {
                foreach (var childType in Types[type])
                {
                    command.AddCommand(GetCommand(childType));
                }
            }
            return command;
        }

        private  Option GetOption(PropertyInfo property)
            => new Option(property.Name)
            {
                Argument = new Argument()
                {
                    ArgumentType = property.PropertyType
                },
                Description = CustomAttributeExtensions.GetDescription(property)
            };

        private  Argument GetArgument(PropertyInfo property)
        {
            var values = property.GetArgumentValues();
            var argument = new Argument(property.Name)
            {
                ArgumentType = property.PropertyType,
                Description = property.GetDescription(),
                Arity = new ArgumentArity(values.minCount, values.maxCount ),
            };
            var defaultValue = property.GetDefaultValue();
            return argument;
        }

        private  (IEnumerable<PropertyInfo>, IEnumerable<PropertyInfo>) GetProperties(Type type)
        {
            var memberBindingFlags = BindingFlags.DeclaredOnly |
                                     BindingFlags.Instance |
                                     BindingFlags.Public;
            var properties = type
                            .GetProperties(memberBindingFlags)
                            .Where(x => x.DeclaringType != typeof(object));
            var argProperties = properties
                            .Where(x => !(x.GetCustomAttribute<CmdArgumentAttribute>() is null));
            var optionProperties = properties.Except(argProperties);
            return (argProperties, optionProperties);
        }

        private static Command CreateCommand(Type type, bool isRoot)
        {
            var desc = CustomAttributeExtensions.GetDescription(type);
            return isRoot
                    ? new RootCommand(desc)
                    : new Command(type.Name, desc);
        }

 
    }
}
