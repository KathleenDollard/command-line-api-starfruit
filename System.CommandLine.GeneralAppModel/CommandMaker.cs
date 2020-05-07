//using System.Collections;
//using System.Collections.Generic;
//using System.CommandLine;
//using System.CommandLine.GeneralAppModel.Descriptors;
//using System.CommandLine.Parsing;
//using System.Data;
//using System.Linq;
//using System.Reflection;
//using System.Runtime.CompilerServices;
//using System.Threading;

//namespace System.CommandLine.GeneralAppModel
//{
//    public class CommandMaker
//    {
//        private readonly Strategy ruleSet;
//        private readonly IAppModel appModel;
//        public CommandMaker(Strategy ruleSet, IAppModel appModel)
//        {
//            this.ruleSet = ruleSet;
//            this.appModel = appModel;
//        }

//        public void Configure(
//              Command command,
//              object commandData)
//        {
//            _ = command ?? throw new ArgumentNullException(nameof(command));

//            var descriptor = appModel.GetCommandDescriptor (commandData);
//            ConfigureCommandFromDescriptor(command, descriptor);
//        }

//        private void ConfigureCommandFromDescriptor(Command command, CommandDescriptor descriptor)
//        {
//            command.Description = descriptor.Description;

//            command.AddOptions(descriptor.Options
//                    .Select(descriptor => GetOptionFromDescriptor(descriptor)));
//            command.AddArguments(descriptor.Arguments
//                    .Select(descriptor => GetArgumentFromDescriptor(descriptor)));
//            command.AddCommands(descriptor.SubCommands
//                    .Select(descriptor => GetCommandFromDescriptor(descriptor)));
//            //command.Handler = CommandHandler.Create<InvocationContext>(
//            //           ic =>
//            //           {
//            //               var parseResult = ic.ParseResult;
//            //               if (ic.ParseResult.Errors.Count == 0)
//            //               {
//            //                   var binder = new ModelBinder(type);
//            //                   var newObj = binder.CreateInstance(ic.BindingContext);
//            //                   methodInfo.Invoke(null, new[] { newObj });
//            //               }
//            //           });
//        }

//        private Argument GetArgumentFromDescriptor(ArgumentDescriptor descriptor)
//        {
//            var arg = new Argument(descriptor.Name.ToUpperInvariant())
//            {
//                Description = descriptor.Description,
//                ArgumentType = descriptor.ArgumentType,
//            };

//            SetArity(arg, descriptor.Required, descriptor.Arity, descriptor.ArgumentType);
//            SetDefault(arg, descriptor.DefaultValue);

//            return arg;

//            static void SetDefault(Argument arg, DefaultValueDescriptor defaultValueDescriptor)
//            {
//                if (defaultValueDescriptor.HasDefaultValue)
//                {
//                    arg.SetDefaultValue(defaultValueDescriptor.DefaultValue);
//                }
//            }

//            static void SetArity(Argument argument, bool? required, ArityDescriptor arityDescriptor, Type argumentType)
//            {
//                // @jonsequitor to review please - can arity have a lower bound and supply upper or is there another way to force required. Maybe an IsRequired method setting something internal. This is a lot of code that overrides core functionality

//                var isRequired = required.GetValueOrDefault();
//                if (!(arityDescriptor is null) && arityDescriptor.IsSet)
//                {
//                    var min = isRequired && arityDescriptor.MinimumNumberOfValues <= 1
//                                ? 1
//                                : arityDescriptor.MinimumNumberOfValues;
//                    argument.Arity = new ArgumentArity(min, arityDescriptor.MinimumNumberOfValues);
//                }
//                else if (isRequired)
//                {
//                    // TODO: This is probably not complete for collections!!!!!
//                    var max = !typeof(string).IsAssignableFrom(argumentType) && typeof(IEnumerable).IsAssignableFrom(argumentType)
//                                ? byte.MaxValue
//                                : 1;
//                    argument.Arity = new ArgumentArity(1, max);
//                }
//            }
//        }

//        private Option GetOptionFromDescriptor(OptionDescriptor descriptor)
//            => new Option("--" + descriptor.Name.ToKebabCase().ToLowerInvariant())
//            {
//                Argument = descriptor.Arguments
//                                        .Select(a => GetArgumentFromDescriptor(a))
//                                        .FirstOrDefault(),
//                Description = descriptor.Description,
//                Required = descriptor.Required
//            };

//        private Command GetCommandFromDescriptor(CommandDescriptor descriptor)
//        {
//            var command = new Command(descriptor.Name);
//            ConfigureCommandFromDescriptor(command, descriptor);
//            return command;
//        }

//    }
//}
