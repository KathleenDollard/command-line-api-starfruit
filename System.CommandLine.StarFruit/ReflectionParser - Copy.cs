//using System.Collections.Generic;
//using System.CommandLine;
//using System.CommandLine.Binding;
//using System.CommandLine.Builder;
//using System.CommandLine.Invocation;
//using System.CommandLine.Parsing;
//using System.Linq;
//using System.Reflection;

//namespace System.CommandLine.StarFruit
//{
//    public class ReflectionParser<T>
//    {
//        private Dictionary<Type, List<Type>> _types;

//        private Dictionary<Type, List<Type>> Types
//        {
//            get
//            {
//                if (_types == null)
//                {
//                    _types = new Dictionary<Type, List<Type>>();
//                    Type[] ts = typeof(T).Assembly.GetExportedTypes();
//                    foreach (Type t in ts)
//                    {
//                        if (t.BaseType != typeof(object))
//                        {
//                            if (_types.ContainsKey(t.BaseType))
//                            {
//                                _types[t.BaseType].Add(t);
//                                continue;
//                            }
//                            _types.Add(t.BaseType, new List<Type>() { t });
//                        }
//                    }
//                }
//                return _types;
//            }
//        }

//        // TODO: Add configuration, particularly unhandled tokens here
//        public static ReflectionParser<T> GetReflectionParser()
//            => new ReflectionParser<T>();

//        // TODO: Ask Jon how to collapse these overloads. 
//        public static T GetInstance(string[] args, bool isRoot = false)
//        {
//            ReflectionParser<T> reflectionParser = GetReflectionParser();
//            return GetInstance(args, reflectionParser, isRoot);
//        }

//        public static T GetInstance(string args, bool isRoot = false)
//        {
//            ReflectionParser<T> reflectionParser = GetReflectionParser();
//            return GetInstance(args, reflectionParser, isRoot);
//        }

//        internal static T GetInstance(string args,
//                                      ReflectionParser<T> reflectionParser,
//                                      bool isRoot)
//            => GetInstanceInternal(p => p.Invoke(args), reflectionParser, isRoot);

//        internal static T GetInstance(string[] args,
//                                      ReflectionParser<T> reflectionParser,
//                                      bool isRoot)
//            => GetInstanceInternal(p => p.Invoke(args), reflectionParser, isRoot);

//        private static T GetInstanceInternal(Action<Parser> parseAction,
//                                             ReflectionParser<T> reflectionParser,
//                                             bool isRoot = false)
//        {
//            Parser parser = reflectionParser.CreateParser(isRoot);
//            parseAction(parser);
//            if (reflectionParser.ParseResult != null && reflectionParser.ParseResult.Errors.Any())
//            {
//                // For now, throw
//                throw new ArgumentException(reflectionParser.ParseResult.Errors.ToString());
//            }
//            return reflectionParser.Result;
//        }

//        public T Result { get; private set; }

//        public ParseResult ParseResult { get; private set; }

//        private Parser CreateParser(bool isRoot = false)
//        {
//            Command command = GetCommand(isRoot);
//            var parser = new CommandLineBuilder(command)
//                       .UseVersionOption()
//                       .UseHelp()
//                       .UseParseDirective()
//                       .UseGui()
//                       .UseDebugDirective()
//                       .UseSuggestDirective()
//                       .RegisterWithDotnetSuggest()
//                       .UseTypoCorrections()
//                       .UseExceptionHandler()
//                       .CancelOnProcessTermination()
//                       .Build();

//            return parser;
//        }

//        public Command GetCommand(bool isRoot = false)
//            => GetCommand(typeof(T), isRoot);

//        public Command GetCommand(Type type, bool isRoot = false)
//        {
//            var (argProperties, optionProperties) = GetProperties(type);
//            Command command = CreateCommand(type, isRoot);
//            command.Handler = CommandHandler.Create<BindingContext>(
//                bc =>
//                {
//                    ParseResult = bc.ParseResult;
//                    if (bc.ParseResult.Errors.Count == 0)
//                    {
//                        var binder = new ModelBinder(type);
//                        Result = (T)binder.CreateInstance(bc);
//                    }
//                });
//            foreach (PropertyInfo property in argProperties)
//            {
//                command.AddArgument(GetArgument(property));
//            }

//            foreach (PropertyInfo property in optionProperties)
//            {
//                command.AddOption(GetOption(property));
//            }

//            if (Types.ContainsKey(type))
//            {
//                foreach (Type childType in Types[type])
//                {
//                    command.AddCommand(GetCommand(childType));
//                }
//            }
//            return command;
//        }

//        // TODO: Look for AddPrefix style method for the property
//        private Option GetOption(PropertyInfo property)
//        {
//            CmdOptionAttribute attr = property.GetCustomAttribute<CmdOptionAttribute>();
//            var name = property.Name;
//            var argumentRequired = attr is null
//                            ? false
//                            : attr.ArgumentRequired;
//            var optionRequired = attr is null
//                 ? false
//                 : attr.OptionRequired;
//            return new Option("--" + name.ToKebabCase().ToLowerInvariant())
//            {
//                Argument = GetArgument(property, attr?.Description, argumentRequired, attr?.DefaultValue),
//                Description = attr?.Description,
//                Required = optionRequired
//            };
//        }

//        private Argument GetArgument(PropertyInfo property)
//        {
//            CmdArgumentAttribute attr = property.GetCustomAttribute<CmdArgumentAttribute>();
//            return GetArgument(property, attr.Description, attr.Required, attr.DefaultValue);
//        }

//        private Argument GetArgument(PropertyInfo property, string description, bool required, object defaultValue)
//        {
//            var name = property.Name.ToKebabCase();
//            var argument = new Argument(name)
//            {
//                ArgumentType = property.PropertyType,
//                Description = description,
//                Arity = GetArity(property, required),
//            };

//            if (typeof(System.Collections.ICollection).IsAssignableFrom(property.PropertyType))
//            {
//                defaultValue = new object[] { defaultValue };
//            }
//            SetDefaultArgumentValue(argument, defaultValue);
//            AddRangeValidator(argument, property);
//            return argument;
//        }

//        private void SetDefaultArgumentValue(Argument argument, object defaultValue)
//        {
//            if (defaultValue == null)
//            {
//                return;
//            }
//            argument.SetDefaultValue(defaultValue);
//        }

//        private void AddRangeValidator(Argument argument, ICustomAttributeProvider attributeProvider)
//        {
//            Range range = attributeProvider.GetRange<int>(); // temp hardcoding of type
//            if (range is null)
//            {
//                return;
//            }
//            //argument.AddValidator(symbol =>
//            //                        symbol.Tokens
//            //                               Select(t=>t.Value, range.)
//        }

//        private IArgumentArity GetArity(ICustomAttributeProvider attributeProvider, bool required)
//        {
//            CmdArityAttribute arityAttribute = attributeProvider.GetCustomAttribute<CmdArityAttribute>();
//            var minArgCount = required
//                                ? 1
//                                : 0;
//            minArgCount = arityAttribute is null || arityAttribute.MinArgCount < minArgCount
//                                ? minArgCount
//                                : arityAttribute.MinArgCount;
//            var maxArgCount = arityAttribute is null || arityAttribute.MaxArgCount < minArgCount
//                           ? minArgCount
//                           : arityAttribute.MaxArgCount;
//            return arityAttribute is null
//                    ? null
//                    : new ArgumentArity(minArgCount, maxArgCount);
//        }

//        private (IEnumerable<PropertyInfo>, IEnumerable<PropertyInfo>) GetProperties(Type type)
//        {
//            BindingFlags memberBindingFlags = BindingFlags.DeclaredOnly |
//                                     BindingFlags.Instance |
//                                     BindingFlags.Public;
//            IEnumerable<PropertyInfo> properties = type
//                            .GetProperties(memberBindingFlags)
//                            .Where(x => x.DeclaringType != typeof(object));
//            IEnumerable<PropertyInfo> argProperties = properties
//                            .Where(x => !(x.GetCustomAttribute<CmdArgumentAttribute>() is null));
//            IEnumerable<PropertyInfo> optionProperties = properties.Except(argProperties);
//            return (argProperties, optionProperties);
//        }

//        private static Command CreateCommand(Type type, bool isRoot)
//        {
//            CmdCommandAttribute commandAttribute = type.GetCustomAttribute<CmdCommandAttribute>();
//            var desc = commandAttribute?.Description;
//            return isRoot
//                    ? new RootCommand(desc)
//                    : new Command(type.Name.ToKebabCase().ToLowerInvariant(), desc);
//        }


//    }
//}
