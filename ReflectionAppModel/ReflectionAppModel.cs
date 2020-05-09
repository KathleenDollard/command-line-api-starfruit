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
        internal const bool useBaseClassAttributes = false;

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
           => GetRootCommandDescriptor(new MethodInfoAppModel(strategy, entryMethod, ommittedTypes));

        public static CommandDescriptor RootCommandDescriptor(Strategy strategy,
                                        Type entryType,
                                        Type[] ommittedTypes = null)
            => GetRootCommandDescriptor(new TypeAppModel(strategy, entryType, ommittedTypes));

        private static CommandDescriptor GetRootCommandDescriptor(MethodInfoAppModel model)
        {
            return model.GetCommand();
        }

        private static CommandDescriptor GetRootCommandDescriptor(TypeAppModel model)
        {
            return model.GetCommand();
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

    public abstract class ReflectionAppModel<T, TPropOrParam> : ReflectionAppModel
        where TPropOrParam : ICustomAttributeProvider
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


        protected override CommandDescriptor GetCommand()
            => new CommandDescriptor
            {
                Name = GetValue(Strategy.NameRules, entryPoint, SymbolType.Command)
            };

        protected IEnumerable<TRet> GetAll<TRet>(RuleSet<TRet> rules, ICustomAttributeProvider reflectingOver, SymbolType symbolType)
            => rules.GetAll(symbolType, GetItems(reflectingOver));

        protected TRet GetValue<TRet>(RuleSet<TRet> rules, ICustomAttributeProvider reflectingOver, SymbolType symbolType)
            => rules.GetFirstOrDefault(symbolType, GetItems(reflectingOver));

        private static object[] GetItems(ICustomAttributeProvider reflectingOver, bool includeNameIdentity = true)
        {
            string name = GetName(reflectingOver);
            var items = reflectingOver.GetCustomAttributes(useBaseClassAttributes)
                                    .Append(name);
            if (includeNameIdentity)
            {
                items = items.Append(new IdentityWrapper<string>( name));
            }
            return items.ToArray();
        }

        private static string GetName(object item)
        {
            return item switch
            {
                PropertyInfo p => p.Name,
                ParameterInfo p => p.Name,
                MethodInfo m => m.Name,
                Type t => t.Name,
                _ => throw new InvalidOperationException($"Unexpected reflected type {item.GetType().Name}"),
            };
        }


        internal class AttributeClassification<TMatch> : SourceClassification<TMatch>
            where TMatch : ICustomAttributeProvider
        {
            internal AttributeClassification(Strategy strategy, IEnumerable<TMatch> sourceItems)
                : base(strategy, sourceItems)
            { }

            protected override bool Match(TMatch reflectingOver, RuleSet<string> rules)
            {
                return rules.HasMatch(SymbolType.All, GetItems(reflectingOver,
                                                               useBaseClassAttributes));
            }

        }
    }
}
