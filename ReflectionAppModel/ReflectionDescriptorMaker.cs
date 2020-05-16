using System.CommandLine.Binding;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Reflection;
using System.Threading;

namespace System.CommandLine.ReflectionAppModel
{
    public  class ReflectionDescriptorMaker : DescriptorMakerBase
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
            var candidate = new Candidate(propertyInfo);
            candidate.AddTraitRange(propertyInfo.GetCustomAttributes(Context.IncludeBaseClassAttributes));
            candidate.AddTrait(name);
            candidate.AddTrait(new IdentityWrapper<string>(name));
            return candidate;
        }

        private static Candidate GetCandidateInternal(ParameterInfo parameterInfo)
        {
            var name = parameterInfo.Name;
            var candidate = new Candidate(parameterInfo);
            candidate.AddTraitRange(parameterInfo.GetCustomAttributes(Context.IncludeBaseClassAttributes));
            candidate.AddTrait(name);
            candidate.AddTrait(new IdentityWrapper<string>(name));
            return candidate;
        }

        private static Candidate GetCandidateInternal(MethodInfo methodItem)
        {
            var name = methodItem.Name;
            var candidate = new Candidate(methodItem);
            candidate.AddTraitRange(methodItem.GetCustomAttributes(Context.IncludeBaseClassAttributes));
            candidate.AddTrait(name);
            candidate.AddTrait(new IdentityWrapper<string>(name));
            return candidate;
        }

        private static Candidate GetCandidateInternal(Type typeItem)
        {
            var name = typeItem.Name;
            var candidate = new Candidate(typeItem);
            candidate.AddTraitRange(typeItem.GetCustomAttributes(Context.IncludeBaseClassAttributes));
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
