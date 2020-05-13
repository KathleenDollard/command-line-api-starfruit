using System.Collections.Generic;
using System.CommandLine.Binding;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace System.CommandLine.ReflectionAppModel
{
    public abstract class ReflectionAppModel : AppModelBase
    {
        // I'm not sure whether we shoudl inherit custom attributes, but it seems less breaking to later change false->true than the reverse
        protected readonly bool useBaseClassAttributes = false;

        protected ReflectionAppModel(Strategy strategy,
                                     object dataSource,
                                     object parentDataSource = null,
                                     Type[] ommittedTypes = null)
            : base(strategy, dataSource, parentDataSource)
        {
            OmmittedTypes = ommittedTypes ?? commonOmmittedTypes;
        }

        public static CommandDescriptor RootCommandDescriptor(Strategy strategy,
                                     MethodInfo entryMethod,
                                     Type[] ommittedTypes = null)
        {
            var model = new MethodInfoAppModel(strategy, entryMethod, ommittedTypes);
            return model.CommandFrom(null);
        }

        public static CommandDescriptor RootCommandDescriptor(Strategy strategy,
                                        Type entryType,
                                        Type[] ommittedTypes = null)
        {
            var model = new TypeAppModel(strategy, entryType, ommittedTypes);
            return model.CommandFrom(null);
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

        protected override Type GetArgumentType(Candidate candidate)
            => candidate.Item switch
            {
                PropertyInfo prop => prop.PropertyType,
                ParameterInfo param => param.ParameterType,
                _ => null
            };
    }

    public abstract class ReflectionAppModel<T> : ReflectionAppModel
        where T : ICustomAttributeProvider
    {
        private readonly object target;
        private readonly T entryPoint;

        protected ReflectionAppModel(Strategy strategy,
                                  T dataSource,
                                  object parentDataSource,
                                  Type[] ommittedTypes = null)
               : base(strategy, dataSource, parentDataSource, ommittedTypes)
        {
            entryPoint = dataSource;
        }

        //protected abstract IEnumerable<ICustomAttributeProvider> GetChildCandidates();

        //protected override CommandDescriptor GetCommand(SymbolDescriptorBase parentSymbolDescriptor)
        //{
        //    var descriptor = new CommandDescriptor(parentSymbolDescriptor, entryPoint);
        //    descriptor.Name = GetValue(descriptor, Strategy.CommandRules.NameRules, entryPoint, parentSymbolDescriptor);
        //    descriptor.Description = GetValue(descriptor, Strategy.DescriptionRules, entryPoint, parentSymbolDescriptor);
        //    return descriptor;
        //}

        //protected override IEnumerable<ArgumentDescriptor> GetArguments(SymbolDescriptorBase parentSymbolDescriptor)
        //{
        //    return GetMatching(parentSymbolDescriptor, Strategy.ArgumentRules, entryPoint, parentSymbolDescriptor);
        //    //return GetChildCandidates()
        //    //                       .Where(p => Match(parentSymbolDescriptor, p, Strategy.ArgumentRules))
        //    //                       .Select(p => BuildArgument(p, parentSymbolDescriptor));
        //}

        //protected override IEnumerable<CommandDescriptor> GetSubCommands(SymbolDescriptorBase parentSymbolDescriptor)
        //    => GetChildCandidates()
        //                .Where(p => Match(parentSymbolDescriptor, p, Strategy.SubCommandRules))
        //                .Select(p => BuildCommand(p, parentSymbolDescriptor));

        // protected override IEnumerable<OptionDescriptor> GetOptions(SymbolDescriptorBase parentSymbolDescriptor)
        //    => GetChildCandidates()
        //                .Where(p => Match(parentSymbolDescriptor, p, Strategy.OptionRules))
        //                .Select(p => BuildOption(p, parentSymbolDescriptor));

        //protected SourceClassification<TPropOrParam> SourceClassification { get; set; }

        //private CommandDescriptor BuildCommand(ICustomAttributeProvider p, SymbolDescriptorBase parentSymbolDescriptor)
        //{
        //    throw new NotImplementedException();
        //}

        //private ArgumentDescriptor BuildArgument(ICustomAttributeProvider param, SymbolDescriptorBase parentSymbolDescriptor)
        //{
        //    var descriptor = new ArgumentDescriptor(parentSymbolDescriptor, entryPoint);
        //    descriptor.Name = GetValue(descriptor, Strategy.NameRules, param, parentSymbolDescriptor);
        //    descriptor.Description = GetValue(descriptor, Strategy.DescriptionRules, param, parentSymbolDescriptor);
        //    descriptor.IsHidden = GetValue(descriptor, Strategy.HiddenRules, param, parentSymbolDescriptor);
        //    descriptor.Arity = GetArity(descriptor, Strategy.ArityRules, param, parentSymbolDescriptor);
        //    descriptor.DefaultValue = GetDefaultValue(descriptor, Strategy.DefaultRules, param, parentSymbolDescriptor);
        //    descriptor.Required = GetValue(descriptor, Strategy.RequiredRules, param, parentSymbolDescriptor);
        //    descriptor.ArgumentType = GetArgumentType(param);
        //    return descriptor;

        //    static Type GetArgumentType(ICustomAttributeProvider param)
        //    {
        //        return param switch
        //        {
        //            PropertyInfo p => p.PropertyType,
        //            ParameterInfo p => p.ParameterType,
        //            _ => throw new InvalidOperationException($"Unexpected argument type source {param.GetType().Name}")
        //        };
        //    }
        //}

        //private OptionDescriptor GetOption(ICustomAttributeProvider param, SymbolDescriptorBase parentSymbolDescriptor)
        //{
        //    var descriptor = new OptionDescriptor(parentSymbolDescriptor, param)
        //    {
        //        Arguments = new ArgumentDescriptor[] { BuildArgument(param, parentSymbolDescriptor) }
        //    };
        //    descriptor.Name = GetValue(descriptor, Strategy.NameRules, param, parentSymbolDescriptor);
        //    descriptor.Aliases = GetMatching(descriptor, Strategy.AliasRules, param, parentSymbolDescriptor);
        //    descriptor.Description = GetValue(descriptor, Strategy.DescriptionRules, param, parentSymbolDescriptor);
        //    descriptor.IsHidden = GetValue(descriptor, Strategy.HiddenRules, param, parentSymbolDescriptor);
        //    descriptor.Required = GetValue(descriptor, Strategy.RequiredRules, param, parentSymbolDescriptor);
        //    return descriptor;
        //}

        //private DefaultValueDescriptor GetDefaultValue(SymbolDescriptorBase descriptor,
        //                                        RuleSet<DefaultValueDescriptor> defaultRules,
        //                                        ICustomAttributeProvider param,
        //                                        SymbolDescriptorBase parentSymbolDescriptor)
        //{
        //    return null; // TODO
        //}

        //private ArityDescriptor GetArity(SymbolDescriptorBase descriptor,
        //                                 RuleSet<ArityDescriptor> arityRules,
        //                                 ICustomAttributeProvider param,
        //                                 SymbolDescriptorBase parentSymbolDescriptor)
        //{
        //    return null; // TODO
        //}

        //private IEnumerable<TRet> GetMatching<TRet>(SymbolDescriptorBase symbolDescriptor,
        //                                       RuleSet<TRet> rules,
        //                                       ICustomAttributeProvider reflectingOver,
        //                                       SymbolDescriptorBase parentSymbolDescriptor)
        //    => rules.GetMatching(symbolDescriptor, GetItems(symbolDescriptor, reflectingOver, parentSymbolDescriptor), parentSymbolDescriptor);

        //private TRet GetValue<TRet>(SymbolDescriptorBase symbolDescriptor,
        //                            RuleSet<TRet> rules,
        //                            ICustomAttributeProvider reflectingOver,
        //                            SymbolDescriptorBase parentSymbolDescriptor)
        //    => rules.GetFirstOrDefault(symbolDescriptor, GetItems(symbolDescriptor, reflectingOver, parentSymbolDescriptor));

        //private static object[] GetItems(SymbolDescriptorBase symbolDescriptor,
        //                                 ICustomAttributeProvider reflectingOver,
        //                                 SymbolDescriptorBase parentSymbolDescriptor,
        //                                 bool includeNameIdentity = true)
        //{
        //    string name = GetName(reflectingOver);
        //    var items = reflectingOver.GetCustomAttributes(useBaseClassAttributes)
        //                            .Append(name);
        //    if (includeNameIdentity)
        //    {
        //        items = items.Append(new IdentityWrapper<string>(name));
        //    }
        //    return items.ToArray();
        //}

        //private static string GetName(object item)
        //{
        //    return item switch
        //    {
        //        PropertyInfo p => p.Name,
        //        ParameterInfo p => p.Name,
        //        MethodInfo m => m.Name,
        //        Type t => t.Name,
        //        _ => throw new InvalidOperationException($"Unexpected reflected type {item.GetType().Name}"),
        //    };
        //}

        //protected bool Match(SymbolDescriptorBase symbolDescriptor, ICustomAttributeProvider reflectingOver, RuleSet<string> rules)
        //{
        //    return rules.HasMatch(symbolDescriptor, GetItems(null,
        //                                                   reflectingOver,
        //                                                   symbolDescriptor,
        //                                                   useBaseClassAttributes));
        //}


        //protected class AttributeClassification<TMatch> : SourceClassification<TMatch>
        //    where TMatch : ICustomAttributeProvider
        //{
        //    private readonly SymbolDescriptorBase parentSymbolDescriptor;

        //    internal AttributeClassification(Strategy strategy,
        //                                     IEnumerable<TMatch> sourceItems,
        //                                     SymbolDescriptorBase parentSymbolDescriptor)
        //        : base(strategy, sourceItems)
        //    {
        //        this.parentSymbolDescriptor = parentSymbolDescriptor;
        //    }

        //    protected override bool Match(TMatch reflectingOver, RuleSet<string> rules)
        //    {
        //        return rules.HasMatch(SymbolType.All, GetItems(null,
        //                                                       reflectingOver,
        //                                                       parentSymbolDescriptor,
        //                                                       useBaseClassAttributes));
        //    }

        //}
    }
}
