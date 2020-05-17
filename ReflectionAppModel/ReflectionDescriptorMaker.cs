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
    public class ReflectionDescriptorMaker : DescriptorMakerBase
    {
        protected ReflectionDescriptorMaker(Strategy strategy,
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
            var model = new ReflectionDescriptorMaker(strategy, entryMethod, ommittedTypes);
            return model.CommandFrom(null);
        }

        public static CommandDescriptor RootCommandDescriptor(Strategy strategy,
                                        Type entryType,
                                        Type[] ommittedTypes = null)
        {
            var model = new ReflectionDescriptorMaker(strategy, entryType, ommittedTypes);
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

        protected override IEnumerable<Candidate> GetChildCandidates(SymbolDescriptorBase commandDescriptor)
        {
            var item = commandDescriptor.Raw;
            return item switch
            {
                MethodInfo m => m.GetParameters().Select(p => GetCandidateInternal(p)),
                Type t => GetTypeChildren(Strategy, commandDescriptor, t, c => GetCandidate(c.Item)),
                _ => new List<Candidate>(),

            };

            static IEnumerable<Candidate> GetTypeChildren(Strategy strategy, SymbolDescriptorBase commandDescriptor, Type t, Func<Candidate, Candidate> fillCandidate)
            {
                var derivedTypes = strategy.GetCandidateRules.GetCandidates(commandDescriptor).Select(c=> fillCandidate(c));
                return derivedTypes.Union(t.GetProperties().Select(p => GetCandidateInternal(p)));
            }
        }


        protected override Type GetArgumentType(Candidate candidate)
            => candidate.Item switch
            {
                PropertyInfo prop => prop.PropertyType,
                ParameterInfo param => param.ParameterType,
                _ => null
            };

        protected override Candidate GetCandidate(object item)
           => item switch
           {
               Type typeItem => GetCandidateInternal(typeItem),
               MethodInfo methodItem => GetCandidateInternal(methodItem),
               ParameterInfo parameterInfo => GetCandidateInternal(parameterInfo),
               PropertyInfo propertyInfo => GetCandidateInternal(propertyInfo),
               _ => null
           };

        private static Candidate GetCandidateInternal(PropertyInfo propertyInfo)
        {
            var name = propertyInfo.Name;
            var candidate = new Candidate(propertyInfo, propertyInfo.Name);
            candidate.AddTraitRange(propertyInfo.GetCustomAttributes(Context.IncludeBaseClassAttributes));
            candidate.AddTrait(name);
            candidate.AddTrait(new IdentityWrapper<string>(name));
            return candidate;
        }

        private static Candidate GetCandidateInternal(ParameterInfo parameterInfo)
        {
            var name = parameterInfo.Name;
            var candidate = new Candidate(parameterInfo, parameterInfo.Name );
            candidate.AddTraitRange(parameterInfo.GetCustomAttributes(Context.IncludeBaseClassAttributes));
            candidate.AddTrait(name);
            candidate.AddTrait(new IdentityWrapper<string>(name));
            return candidate;
        }

        private static Candidate GetCandidateInternal(MethodInfo methodInfo)
        {
            var name = methodInfo.Name;
            var candidate = new Candidate(methodInfo, methodInfo.Name);
            candidate.AddTraitRange(methodInfo.GetCustomAttributes(Context.IncludeBaseClassAttributes));
            candidate.AddTrait(name);
            candidate.AddTrait(new IdentityWrapper<string>(name));
            return candidate;
        }

        private static Candidate GetCandidateInternal(Type type)
        {
            var name = type.Name;
            var candidate = new Candidate(type, type.Name);
            candidate.AddTraitRange(type.GetCustomAttributes(Context.IncludeBaseClassAttributes));
            candidate.AddTrait(name);
            candidate.AddTrait(new IdentityWrapper<string>(name));
            return candidate;
        }
    }

    public abstract class ReflectionDescriptorMaker<T> : ReflectionDescriptorMaker
        where T : ICustomAttributeProvider
    {
        private readonly object target;
        private readonly T entryPoint;

        protected ReflectionDescriptorMaker(Strategy strategy,
                                  T dataSource,
                                  object parentDataSource,
                                  Type[] ommittedTypes = null)
               : base(strategy, dataSource, parentDataSource, ommittedTypes)
        {
            entryPoint = dataSource;
        }

    }
}
