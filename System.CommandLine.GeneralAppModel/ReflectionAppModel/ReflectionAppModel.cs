﻿using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Binding;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace System.CommandLine.GeneralAppModel
{
    // This expects an instance per command to support caching
    // Currently depth first, didn't think of scnearios where it would matter
    public abstract class AppModelBase
    {
        public AppModelBase(Strategy strategy, object dataSource, object parentDataSource = null)
        {
            Strategy = strategy;
            DataSource = dataSource;
            ParentDataSource = parentDataSource;
        }

        protected Strategy Strategy { get; }
        protected object DataSource { get; }
        protected object ParentDataSource { get; }

        protected abstract IEnumerable<ArgumentDescriptor> GetArguments();
        protected abstract IEnumerable<CommandDescriptor> GetSubCommands();
        protected abstract IEnumerable<OptionDescriptor> GetOptions();
        protected abstract CommandDescriptor GetCommandDescriptor();

        private CommandDescriptor CommandFrom()
        {
            var arguments = GetArguments();
            var options = GetOptions();
            var subommands = GetSubCommands();

            var command = GetCommandDescriptor();
            command.Arguments.AddRange(arguments);
            command.Options.AddRange(options);
            command.SubCommands.AddRange(subommands);
            return command;
        }
    }

    public abstract class ReflectionAppModel : AppModelBase
    {
        public static RootCommand RootCommand(Strategy strategy,
                                              MethodInfo entryMethod,
                                              Type[] ommittedTypes = null)
            => GetRootCCommand(new MethodInfoAppModel(strategy, entryMethod, ommittedTypes));

        public static RootCommand RootCommand(Strategy strategy,
                                        Type entryType,
                                        Type[] ommittedTypes = null)
            => GetRootCCommand(new TypeAppModel(strategy, entryType, ommittedTypes));

        private static RootCommand GetRootCCommand(MethodInfoAppModel model)
        {
            var descriptor = model.GetCommandDescriptor();
            throw new NotImplementedException();
            //var builder = new NotYetCreatedBuilder(descriptor);
            //return builder.BuildCommand();
        }

        private static RootCommand GetRootCCommand(TypeAppModel model)
        {
            var descriptor = model.GetCommandDescriptor();
            throw new NotImplementedException();
            //var builder = new NotYetCreatedBuilder(descriptor);
            //return builder.BuildCommand();
        }

        // Get this from ServiceProvider
        protected ReflectionAppModel(Strategy strategy,
                                  object dataSource,
                                  object parentDataSource,
                                  Type[] ommittedTypes = null)
               : base(strategy, dataSource, parentDataSource)
        {
            OmmittedTypes = ommittedTypes ?? commonOmmittedTypes;
        }


        internal static readonly Type[] commonOmmittedTypes = new[]
                 {
                    typeof(IConsole),
                    typeof(InvocationContext),
                    typeof(BindingContext),
                    typeof(ParseResult),
                    typeof(CancellationToken),
                };

        protected Type[] OmmittedTypes { get; }
    }

    internal class MethodInfoAppModel : ReflectionAppModel
    {
        private readonly object target;
        private readonly Strategy strategy;
        private readonly MethodInfo entryMethod;
        private readonly ParameterClassification parameterClassification;

        private MethodInfoAppModel(Strategy strategy,
                               object dataSource,
                               object parentDataSource,
                               Type[] ommittedTypes = null)
            : base(strategy, dataSource, parentDataSource, ommittedTypes)
        { }

        public MethodInfoAppModel(Strategy strategy,
                              MethodInfo entryMethod,
                              Type[] ommittedTypes = null)
            : this(strategy, entryMethod, null, ommittedTypes)
        {
            this.entryMethod = entryMethod;
            this.strategy = strategy;
            parameterClassification = new ParameterClassification(strategy, entryMethod);
        }


        protected override IEnumerable<ArgumentDescriptor> GetArguments()
        {
            return   parameterClassification
                        .ArgumentParameters
                        .Select(p=>BuildArgument(p, SymbolType.Argument ));

        }
 
        protected override CommandDescriptor GetCommandDescriptor()
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<OptionDescriptor> GetOptions()
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<CommandDescriptor> GetSubCommands()
        {
            throw new NotImplementedException();
        }

        private ArgumentDescriptor BuildArgument(ParameterInfo param, SymbolType symbolType)
        {
            return new ArgumentDescriptor
            {
                Raw = param,
                Name = GetValue(strategy.NameRules, param, symbolType),
                Description = GetValue(strategy.DescriptionRules, param, symbolType),
                IsHidden = GetValue(strategy.HiddenRules, param, symbolType),
                Arity = GetArity(strategy.ArityRules, param, symbolType),
                ArgumentType = param.ParameterType,
                DefaultValue = GetDefaultValue(strategy.DefaultRules, param, symbolType),
                Required = GetValue(strategy.RequiredRules, param, symbolType)
            };
        }

        private OptionDescriptor BuildOption(ParameterInfo param)
        {
            return new OptionDescriptor
            {
                Raw = param,
                Name = GetValue(strategy.NameRules, param, SymbolType.Option),
                Aliases = GetAll(strategy.AliasRules, param, SymbolType.Option),
                Description = GetValue(strategy.DescriptionRules, param, SymbolType.Option),
                IsHidden = GetValue(strategy.HiddenRules, param, SymbolType.Option),
                Arguments = new ArgumentDescriptor[] { BuildArgument(param, SymbolType.Option )},
                Required = GetValue(strategy.RequiredRules, param, SymbolType.Option)
            };
        }

        private DefaultValueDescriptor GetDefaultValue(RuleSet<DefaultValueDescriptor> defaultRules,
                                                       ParameterInfo param,
                                                       SymbolType symbolType)
        {
            throw new NotImplementedException();
        }

        private ArityDescriptor GetArity(RuleSet<ArityDescriptor> arityRules,
                                         ParameterInfo param,
                                         SymbolType symbolType)
        {
            throw new NotImplementedException();
        }

        private T GetValue<T>(RuleSet<T> rules, ParameterInfo param, SymbolType symbolType) 
            => rules.GetFirstOrDefault(symbolType,
                                            param.GetCustomAttributes(),
                                            param.Name,
                                            param.Name);

        private IEnumerable<T> GetAll<T>(RuleSet<T> rules, ParameterInfo param, SymbolType symbolType)
            => rules.GetAll(symbolType,
                                    param.GetCustomAttributes(),
                                    param.Name,
                                    param.Name);


        private class ParameterClassification
        {
            internal ParameterClassification(Strategy strategy, MethodInfo methodInfo)
            {
                var parameters = methodInfo.GetParameters();
                ArgumentParameters = parameters.Where(p => MatchParameter(p, strategy.ArgumentRules));
                SubCommandParameters = parameters.Where(p => MatchParameter(p, strategy.CommandRules));
                OptionParameters = parameters.Except(ArgumentParameters).Except(SubCommandParameters).ToList();
            }
            internal IEnumerable<ParameterInfo> ArgumentParameters { get; }
            internal IEnumerable<ParameterInfo> OptionParameters { get; }
            internal IEnumerable<ParameterInfo> SubCommandParameters { get; }

            private bool MatchParameter(ParameterInfo param, RuleSet<string> rules)
            {
                return rules.HasMatch(SymbolType.All, param.GetCustomAttributes()
                                                        .Union(new object[] { new IdentityWrapper<string>(param.Name) })
                                                        .ToArray());
            }
        }

    }

    internal class TypeAppModel : ReflectionAppModel
    {
        private readonly object target;
        private readonly Strategy strategy;
        private readonly Type entryType;
        private readonly PropertyClassification propertyClassification;

        private TypeAppModel(Strategy strategy,
                               object dataSource,
                               object parentDataSource,
                               Type[] ommittedTypes = null)
            : base(strategy, dataSource, parentDataSource, ommittedTypes)
        { }

        public TypeAppModel(Strategy strategy,
                              Type entryType,
                              Type[] ommittedTypes = null)
            : this(strategy, entryType, null, ommittedTypes)
        {
            this.entryType = entryType;
            this.strategy = strategy;
            propertyClassification = new PropertyClassification(strategy, entryType);
        }


        protected override IEnumerable<ArgumentDescriptor> GetArguments()
        {
            return propertyClassification
                        .ArgumentProperties 
                        .Select(p => BuildArgument(p, SymbolType.Argument));

        }

        protected override CommandDescriptor GetCommandDescriptor()
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<OptionDescriptor> GetOptions()
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<CommandDescriptor> GetSubCommands()
        {
            throw new NotImplementedException();
        }

        private ArgumentDescriptor BuildArgument(PropertyInfo prop, SymbolType symbolType)
        {
            return new ArgumentDescriptor
            {
                Raw = prop,
                Name = GetValue(strategy.NameRules, prop, symbolType),
                Description = GetValue(strategy.DescriptionRules, prop, symbolType),
                IsHidden = GetValue(strategy.HiddenRules, prop, symbolType),
                Arity = GetArity(strategy.ArityRules, prop, symbolType),
                ArgumentType = prop.PropertyType,
                DefaultValue = GetDefaultValue(strategy.DefaultRules, prop, symbolType),
                Required = GetValue(strategy.RequiredRules, prop, symbolType)
            };
        }

        private OptionDescriptor BuildOption(PropertyInfo prop)
        {
            return new OptionDescriptor
            {
                Raw = prop,
                Name = GetValue(strategy.NameRules, prop, SymbolType.Option),
                Aliases = GetAll(strategy.AliasRules, prop, SymbolType.Option),
                Description = GetValue(strategy.DescriptionRules, prop, SymbolType.Option),
                IsHidden = GetValue(strategy.HiddenRules, prop, SymbolType.Option),
                Arguments = new ArgumentDescriptor[] { BuildArgument(prop, SymbolType.Option) },
                Required = GetValue(strategy.RequiredRules, prop, SymbolType.Option)
            };
        }

        private DefaultValueDescriptor GetDefaultValue(RuleSet<DefaultValueDescriptor> defaultRules,
                                                       PropertyInfo prop,
                                                       SymbolType symbolType)
        {
            throw new NotImplementedException();
        }

        private ArityDescriptor GetArity(RuleSet<ArityDescriptor> arityRules,
                                         PropertyInfo prop,
                                         SymbolType symbolType)
        {
            throw new NotImplementedException();
        }

        private T GetValue<T>(RuleSet<T> rules, PropertyInfo prop, SymbolType symbolType)
            => rules.GetFirstOrDefault(symbolType,
                                            prop.GetCustomAttributes(),
                                            prop.Name);

        private IEnumerable<T> GetAll<T>(RuleSet<T> rules, PropertyInfo prop, SymbolType symbolType)
            => rules.GetAll(symbolType,
                                    prop.GetCustomAttributes(),
                                    prop.Name);


        private class PropertyClassification
        {
            internal PropertyClassification(Strategy strategy, Type type)
            {
                var properties = type.GetProperties();
                ArgumentProperties = properties.Where(p => MatchProperty(p, strategy.ArgumentRules));
                SubCommandProperties = properties.Where(p => MatchProperty(p, strategy.CommandRules));
                OptionProperties = properties.Except(ArgumentProperties).Except(SubCommandProperties).ToList();
            }
            internal IEnumerable<PropertyInfo> ArgumentProperties { get; }
            internal IEnumerable<PropertyInfo> OptionProperties { get; }
            internal IEnumerable<PropertyInfo> SubCommandProperties { get; }

            private bool MatchProperty(PropertyInfo param, RuleSet<string> rules)
            {
                return rules.HasMatch(SymbolType.All, param.GetCustomAttributes()
                                                        .Union(new object[] { new IdentityWrapper<string>(param.Name) })
                                                        .ToArray());
            }
        }

    }

    //public class ReflectionAppModelOld : AppModelBase
    //{
    //    //private MethodInfo methodInfo;
    //    private object target;
    //    private bool supportEntryMethod;
    //    private bool supportEntryType;
    //    private Type[] ommittedTypes;
    //    private Type entryType;
    //    private MethodInfo entryMethod;

    //    private ReflectionAppModel(Strategy strategy,
    //                              object dataSource,
    //                              object parentDataSource,
    //                              Type[] ommittedTypes = null)
    //        : base(strategy, dataSource, parentDataSource)
    //    {
    //        this.ommittedTypes = ommittedTypes ?? commonOmmittedTypes;
    //    }

    //    public ReflectionAppModel(Strategy strategy,
    //                              Type entryType,
    //                              Type[] ommittedTypes = null)
    //        : this(strategy, entryType, null, ommittedTypes)
    //    {
    //        this.entryType = entryType;
    //    }

    //    public ReflectionAppModel(Strategy strategy,
    //                              MethodInfo entryMethod,
    //                              Type[] ommittedTypes = null)
    //        : this(strategy, entryMethod, null, ommittedTypes)
    //    {
    //        this.entryMethod = entryMethod;
    //    }

    //    // Get this from ServiceProvider
    //    internal static readonly Type[] commonOmmittedTypes = new[]
    //            {
    //                typeof(IConsole),
    //                typeof(InvocationContext),
    //                typeof(BindingContext),
    //                typeof(ParseResult),
    //                typeof(CancellationToken),
    //            };



    //    public CommandDescriptor GetCommandDescriptor(Strategy strategy, object commandData)
    //        => commandData switch
    //        {
    //            MethodInfo methodInfo => CommandFromMethodInfo(strategy, methodInfo),
    //            Type type => CommandFromType(strategy, type),
    //            _ => throw new ArgumentException(nameof(commandData))
    //        };

    //    private CommandDescriptor CommandFromType(Strategy ruleSet, Type type)
    //    {
    //        _ = type ?? throw new ArgumentNullException(nameof(type));
    //        throw new NotImplementedException();
    //    }


    //    private CommandDescriptor CommandFromMethodInfo(Strategy strategy, MethodInfo methodInfo)
    //    {
    //        _ = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));

    //        var parameters = methodInfo.GetParameters();

    //        var argumentParameters = parameters.Where(p => MatchParameter(p, strategy.ArgumentRules));
    //        var subCommandParameters = parameters.Where(p => MatchParameter(p, strategy.CommandRules));
    //        var optionParameters = parameters.Except(argumentParameters).Except(subCommandParameters).ToList();

    //        var arguments = argumentParameters
    //                        .Select(p => BuildArgumentDescriptor(strategy, p));

    //        //BuildArgument(GetName(param, SymbolType.Argument),
    //        //                 param.ParameterType,
    //        //                 GetDescription(param, SymbolType.Argument),
    //        //                 required,
    //        //                 StrategiesSet.ArityStrategies.GetArity(param));


    //    }

    //    private bool MatchParameter(ParameterInfo param, RuleSet<string> rules)
    //    {
    //        return rules.HasMatch(SymbolType.All, param.GetCustomAttributes()
    //                                                .Union(new object[] { new IdentityWrapper<string>(param.Name) })
    //                                                .ToArray());
    //    }

    //    private ArgumentDescriptor BuildArgumentDescriptor(Strategy strategy,
    //                                                       ParameterInfo param,
    //                                                       SymbolType symbolType)
    //    {
    //        return new ArgumentDescriptor
    //        {
    //            Raw = param,
    //            Name = GetString(strategy.NameRules, param, symbolType),
    //            Aliases = GetStrings(strategy.AliasRules, param, symbolType),
    //            Description = GetString(strategy.DescriptionRules, param, symbolType),
    //            IsHidden = GetBool(strategy.HiddenRules, param, symbolType),
    //            Arity = GetArity(strategy.ArityRules, param, symbolType),
    //            ArgumentType = param.ParameterType,
    //            DefaultValue = GetDefaultValue(strategy.DefaultRules, param, symbolType),
    //            Required = GetBool(strategy.RequiredRules, param, symbolType)
    //        };
    //    }

    //    private DefaultValueDescriptor GetDefaultValue(RuleSet<DefaultValueDescriptor> defaultRules,
    //                                                   ParameterInfo param)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    private ArityDescriptor GetArity(RuleSet<ArityDescriptor> arityRules, ParameterInfo param)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    private bool GetBool(BoolRuleSet rules, ParameterInfo param, SymbolType symbolType)
    //    {
    //        var (Success, RawValue) = rules.TryGetFirstMatch(symbolType,
    //                                                            param.GetCustomAttributes(),
    //                                                            param.Name,
    //                                                            param.Name);
    //        return Success
    //                ? ConvertToBool(RawValue)
    //                : false;
    //    }

    //    private string GetString(StringRuleSet rules, ParameterInfo param, SymbolType symbolType)
    //    {
    //        var (success, value) = rules.TryGetFirstMatch(symbolType,
    //                                                      param.GetCustomAttributes(),
    //                                                      param.Name,
    //                                                      param.Name);
    //        return success
    //                ? value.ToString()
    //                : null;
    //    }


    //    private IEnumerable<string> GetStrings(StringRuleSet rules, ParameterInfo param, SymbolType symbolType)
    //        => rules.GetAll<string>(symbolType,
    //                                param.GetCustomAttributes(),
    //                                param.Name,
    //                                param.Name);

    //    public void Configure(
    //          Command command,
    //          MethodInfo methodInfo,
    //          object target = null)
    //    {
    //        _ = command ?? throw new ArgumentNullException(nameof(command));
    //        _ = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
    //        this.target = target;

    //        var parameters = methodInfo.GetParameters();
    //        if (parameters.Count() == 1 && StrategiesSet.CommandStrategies.IsCommand(parameters.First()))
    //        {
    //            AddChildren(command, parameters.First().ParameterType);
    //        }
    //        else
    //        {
    //            AddChildren(command, methodInfo);
    //        };

    //        command.Handler = CommandHandler.Create(methodInfo, target);
    //        this.methodInfo = methodInfo;

    //    }

    //    public void Configure(
    //           Command command,
    //           Type type,
    //           object target = null)
    //    {
    //        _ = command ?? throw new ArgumentNullException(nameof(command));
    //        _ = type ?? throw new ArgumentNullException(nameof(type));

    //        // TODO: Differentiate between Main with single type and more complex scenarios (sometimes need to collapse root type)
    //        AddChildren(command, type);
    //    }

    //    public void AddChildren(Command command, MethodInfo method)
    //    {
    //        var parameters = method.GetParameters();
    //        var argumentParameters = parameters.Where(p => StrategiesSet.ArgumentStrategies.IsArgument(p)).ToList();
    //        var commandParameters = parameters.Where(p => StrategiesSet.CommandStrategies.IsCommand(p)).ToList();
    //        var optionParameters = parameters.Except(argumentParameters).Except(commandParameters).ToList();

    //        command.AddArguments(argumentParameters
    //                               .Select(p => BuildArgument(p)));
    //        command.AddCommands(commandParameters
    //                                .Select(p => BuildCommand(p)));
    //        command.AddOptions(optionParameters
    //                                .Select(p => BuildOption(p)));
    //    }

    //    public void AddChildren(Command command, Type type)
    //    {
    //        var properties = type.GetProperties();
    //        var argumentProperties = properties.Where(p => StrategiesSet.ArgumentStrategies.IsArgument(p));
    //        var commandProperties = properties.Where(p => StrategiesSet.CommandStrategies.IsCommand(p));
    //        var optionProperties = properties.Except(argumentProperties).Except(commandProperties);

    //        command.AddArguments(argumentProperties
    //                                .Select(p => BuildArgument(p))
    //                                .ToList());
    //        command.AddCommands(commandProperties
    //                                .Select(p => BuildCommand(p))
    //                                .ToList());
    //        command.AddOptions(optionProperties
    //                                .Select(p => BuildOption(p))
    //                                .ToList());

    //        command.AddCommands(StrategiesSet.SubCommandStrategies.GetCommandTypes(type)
    //                    .Select(t => BuildCommand(t))
    //                    .ToList());

    //    }

    //    public Option BuildOption(ParameterInfo param)
    //    {
    //        // TODO: DefaultValue strategy 
    //        bool? argumentRequired = StrategiesSet.RequiredStrategies.IsRequired(param, SymbolType.Argument); ;
    //        bool? optionRequired = StrategiesSet.RequiredStrategies.IsRequired(param, SymbolType.Option); ;
    //        ArityDescriptor arityDescriptor = StrategiesSet.ArityStrategies.GetArity(param);
    //        var argument = BuildArgument(GetName(param, SymbolType.Argument), param.ParameterType, GetDescription(param, SymbolType.Argument), argumentRequired, arityDescriptor);

    //        return BuildOption(GetName(param, SymbolType.Option), GetDescription(param, SymbolType.Option), optionRequired, argument);
    //    }

    //    public Option BuildOption(PropertyInfo prop)
    //    {
    //        // TODO: DefaultValue strategy 
    //        bool? argumentRequired = StrategiesSet.RequiredStrategies.IsRequired(prop, SymbolType.Argument); ;
    //        bool? optionRequired = StrategiesSet.RequiredStrategies.IsRequired(prop, SymbolType.Option); ;
    //        ArityDescriptor arityDescriptor = StrategiesSet.ArityStrategies.GetArity(prop);
    //        var argument = BuildArgument(GetName(prop, SymbolType.Argument), prop.PropertyType, GetDescription(prop, SymbolType.Argument), argumentRequired, arityDescriptor);

    //        return BuildOption(GetName(prop, SymbolType.Option), GetDescription(prop, SymbolType.Option), optionRequired, argument);
    //    }

    //    public Option BuildOption(string name, string description, bool? optionRequired, Argument argument)
    //    {
    //        var option = new Option("--" + name.ToKebabCase().ToLowerInvariant())
    //        {
    //            Argument = argument,
    //            Description = description,

    //            //Required = required
    //        };
    //        if (optionRequired.HasValue)
    //        {
    //            option.Required = optionRequired.Value;
    //        }
    //        return option;
    //    }

    //    public Argument BuildArgument(ParameterInfo param)
    //    {
    //        // TODO: DefaultValue strategy 
    //        var required = StrategiesSet.RequiredStrategies.IsRequired(param, SymbolType.Argument);
    //        return BuildArgument(GetName(param, SymbolType.Argument),
    //                             param.ParameterType,
    //                             GetDescription(param, SymbolType.Argument),
    //                             required,
    //                             StrategiesSet.ArityStrategies.GetArity(param));
    //    }

    //    public Argument BuildArgument(PropertyInfo prop)
    //    {
    //        // TODO: DefaultValue strategy 
    //        var required = StrategiesSet.RequiredStrategies.IsRequired(prop, SymbolType.Argument);
    //        return BuildArgument(GetName(prop, SymbolType.Argument),
    //                             prop.PropertyType,
    //                             GetDescription(prop, SymbolType.Argument),
    //                             required,
    //                             StrategiesSet.ArityStrategies.GetArity(prop));
    //    }

    //    public Argument BuildArgument(string name, Type type, string description, bool? required, ArityDescriptor arityDescriptor)
    //    {
    //        // @jonsequitor to review please - can arity have a lower bound and supply upper or is there another way to force required. Maybe an IsRequired method setting something internal. This is a lot of code that overrides core functionality
    //        var arg = new Argument(name.ToUpperInvariant())
    //        {
    //            Description = description,
    //            ArgumentType = type,
    //        };

    //        var isRequired = required.GetValueOrDefault();
    //        if (!(arityDescriptor is null) && arityDescriptor.IsSet)
    //        {
    //            var min = isRequired && arityDescriptor.Min <= 1
    //                        ? 1
    //                        : arityDescriptor.Min;
    //            arg.Arity = new ArgumentArity(min, arityDescriptor.Max);
    //        }
    //        else if (isRequired)
    //        {
    //            // TODO: This is probably not complete for collections!!!!!
    //            var max = !typeof(string).IsAssignableFrom(type) && typeof(IEnumerable).IsAssignableFrom(type)
    //                        ? byte.MaxValue
    //                        : 1;
    //            arg.Arity = new ArgumentArity(1, max);
    //        }
    //        // TODO: Set default value
    //        return arg;
    //    }

    //    public Command BuildCommand(ParameterInfo param)
    //    {
    //        var name = StrategiesSet.NameStrategies.Name(param, SymbolType.Command);
    //        var type = param.ParameterType;
    //        var description = StrategiesSet.DescriptionStrategies.Description(param, SymbolType.Command);

    //        return BuildCommand(name, description, type);
    //    }

    //    public Command BuildCommand(PropertyInfo prop)
    //    {
    //        var name = StrategiesSet.NameStrategies.Name(prop, SymbolType.Command);
    //        var type = prop.PropertyType;
    //        var description = StrategiesSet.DescriptionStrategies.Description(prop, SymbolType.Command);

    //        return BuildCommand(name, description, type);
    //    }

    //    public Command BuildCommand(Type type)
    //    {
    //        var name = StrategiesSet.NameStrategies.Name(type, SymbolType.Command);
    //        var description = StrategiesSet.DescriptionStrategies.Description(type, SymbolType.Command);

    //        return BuildCommand(name, description, type);
    //    }

    //    public Command BuildCommand(string name, string description, Type type)
    //    {

    //        var command = new Command(name.ToKebabCase().ToLowerInvariant())
    //        {
    //            Description = description,
    //        };
    //        command.Handler = CommandHandler.Create<InvocationContext>(
    //            ic =>
    //            {
    //                var parseResult = ic.ParseResult;
    //                if (ic.ParseResult.Errors.Count == 0)
    //                {
    //                    var binder = new ModelBinder(type);
    //                    var newObj = binder.CreateInstance(ic.BindingContext);
    //                    methodInfo.Invoke(null, new[] { newObj });
    //                }
    //            });

    //        AddChildren(command, type);

    //        return command;
    //    }

    //    private string GetDescription(ParameterInfo param, SymbolType symbolType) => StrategiesSet.DescriptionStrategies.Description(param, symbolType);
    //    private string GetName(ParameterInfo param, SymbolType symbolType) => StrategiesSet.NameStrategies.Name(param, symbolType);
    //    private string GetDescription(PropertyInfo prop, SymbolType symbolType) => StrategiesSet.DescriptionStrategies.Description(prop, symbolType);
    //    private string GetName(PropertyInfo prop, SymbolType symbolType) => StrategiesSet.NameStrategies.Name(prop, symbolType);




    //    //public void UseDefaults()
    //    //{
    //    //    StrategiesSet.ArgumentStrategies.UseStandard();
    //    //    StrategiesSet.CommandStrategies.UseStandard();
    //    //    StrategiesSet.ArityStrategies.UseStandard();
    //    //    StrategiesSet.DescriptionStrategies.UseStandard();
    //    //    StrategiesSet.NameStrategies.UseStandard();
    //    //    StrategiesSet.RequiredStrategies.UseStandard();
    //    //    StrategiesSet.SubCommandStrategies.UseStandard();
    //    //}
    //}
}
