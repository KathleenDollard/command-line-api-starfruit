using System.CommandLine.Binding;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.CommandLine.ReflectionAppModel
{
    public class ReflectionDescriptorMaker : DescriptorMakerBase
    {
        protected ReflectionDescriptorMaker(Strategy strategy,
                                     object dataSource,
                                     Type[]? ommittedTypes = null)
            : base(strategy, new ReflectionSpecificSource(), dataSource)
        {
            OmmittedTypes = ommittedTypes ?? commonOmmittedTypes;
        }

        public static CommandDescriptor RootCommandDescriptor(
                                     MethodInfo entryMethod,
                                     Type[]? ommittedTypes = null)
            => RootCommandDescriptor(Strategy.Standard, entryMethod, ommittedTypes);

        public static CommandDescriptor RootCommandDescriptor<TRoot>(
                                        Type[]? ommittedTypes = null)
               => RootCommandDescriptor(typeof(TRoot), ommittedTypes);

        public static CommandDescriptor RootCommandDescriptor<TRoot>(
                                Strategy strategy,
                                Type[]? ommittedTypes = null)
            => RootCommandDescriptor(strategy, typeof(TRoot), ommittedTypes);

        public static CommandDescriptor RootCommandDescriptor(
                                        Type entryType,
                                        Type[]? ommittedTypes = null)
            => RootCommandDescriptor(Strategy.Standard, entryType, ommittedTypes);

        public static CommandDescriptor RootCommandDescriptor(Strategy strategy,
                              MethodInfo entryMethod,
                              Type[]? ommittedTypes = null)
        {
            var model = new ReflectionDescriptorMaker(strategy, entryMethod, ommittedTypes);
            return model.CommandFrom(SymbolDescriptor.Empty);
        }

        public static CommandDescriptor RootCommandDescriptor(Strategy strategy,
                                        Type entryType,
                                        Type[]? ommittedTypes = null)
        {
            var model = new ReflectionDescriptorMaker(strategy, entryType, ommittedTypes);
            return model.CommandFrom(SymbolDescriptor.Empty);
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
