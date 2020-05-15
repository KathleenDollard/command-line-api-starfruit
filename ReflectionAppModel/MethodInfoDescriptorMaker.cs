using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.ReflectionAppModel
{
    internal class MethodInfoDescriptorMaker : ReflectionAppModel<MethodInfo>
    {
        private readonly MethodInfo entryMethod;

        private MethodInfoDescriptorMaker(Strategy strategy,
                               MethodInfo entryMethod,
                               object parentDataSource,
                               Type[] ommittedTypes = null)
            : base(strategy, entryMethod, parentDataSource, ommittedTypes)
        {
            _ = entryMethod ?? throw new ArgumentNullException(nameof(entryMethod));
            this.entryMethod = entryMethod;
        }

        public MethodInfoDescriptorMaker(Strategy strategy,
                              MethodInfo entryMethod,
                              Type[] ommittedTypes = null)
            : this(strategy, entryMethod, null, ommittedTypes)
        { }

        protected override Candidate GetCandidate(object item)
        {
            if (item is MethodInfo methodItem)
            {
                var name = methodItem.Name;
                var candidate = new Candidate(methodItem);
                candidate.AddTraitRange(methodItem.GetCustomAttributes(Context.IncludeBaseClassAttributes));
                candidate.AddTrait(name);
                candidate.AddTrait(new IdentityWrapper<string>(name));
                return candidate;
            }
            return null;
        }

        //protected override IEnumerable<Candidate> GetChildCandidates(object DataSource)
        //    => (IEnumerable<Candidate>)entryMethod.GetParameters()
        //                 .Select(x => GetChildCandidate(x));

        private Candidate GetChildCandidate(ParameterInfo parameterInfo)
        {
            var name = parameterInfo.Name;
            var candidate = new Candidate(parameterInfo);
            candidate.AddTraitRange(entryMethod.GetCustomAttributes(Context.IncludeBaseClassAttributes));
            candidate.AddTrait(name);
            candidate.AddTrait(new IdentityWrapper<string>(name));
            return candidate;
        }
    }
}
