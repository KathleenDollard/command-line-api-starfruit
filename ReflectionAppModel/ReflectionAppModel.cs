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
        private readonly Strategy strategy;

        protected ReflectionAppModel(Strategy strategy,
                                  object dataSource,
                                  object parentDataSource,
                                  Type[] ommittedTypes = null)
               : base(strategy, dataSource, parentDataSource)
        {
            OmmittedTypes = ommittedTypes ?? commonOmmittedTypes;
        }

        public static CommandDescriptor  RootCommandDescriptor(Strategy strategy,
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
}
