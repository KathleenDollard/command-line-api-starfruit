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
        {
            if (item is Type typeItem)
            {
                var name = typeItem.Name;
                var candidate = new Candidate(typeItem);
                candidate.AddTraitRange(typeItem.GetCustomAttributes(Context.IncludeBaseClassAttributes));
                candidate.AddTrait(name);
                candidate.AddTrait(new IdentityWrapper<string>(name));
                return candidate;
            }
            if (item is MethodInfo methodItem)
            {
                var name = methodItem.Name;
                var candidate = new Candidate(methodItem);
                candidate.AddTraitRange(methodItem.GetCustomAttributes(Context.IncludeBaseClassAttributes));
                candidate.AddTrait(name);
                candidate.AddTrait(new IdentityWrapper<string>(name));
                return candidate;
            }
            if (item is ParameterInfo parameterInfo)
            {
                var name = parameterInfo.Name;
                var candidate = new Candidate(parameterInfo);
                candidate.AddTraitRange(parameterInfo.GetCustomAttributes(Context.IncludeBaseClassAttributes));
                candidate.AddTrait(name);
                candidate.AddTrait(new IdentityWrapper<string>(name));
                return candidate;
            }
            if (item is PropertyInfo propertyInfo)
            {
                var name = propertyInfo.Name;
                var candidate = new Candidate(propertyInfo);
                candidate.AddTraitRange(propertyInfo.GetCustomAttributes(Context.IncludeBaseClassAttributes));
                candidate.AddTrait(name);
                candidate.AddTrait(new IdentityWrapper<string>(name));
                return candidate;
            }
            return null;
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
