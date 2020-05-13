using System.CommandLine.Binding;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
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
 
    }
}
