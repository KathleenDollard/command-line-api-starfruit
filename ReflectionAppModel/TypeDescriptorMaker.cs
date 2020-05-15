using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.Data;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.ReflectionAppModel
{
    internal class TypeDescriptorMaker : ReflectionAppModel<Type>
    {
        private readonly Type entryType;

        private TypeDescriptorMaker(Strategy strategy,
                              Type entryType,
                               object parentDataSource,
                               Type[] ommittedTypes = null)
            : base(strategy, entryType, parentDataSource, ommittedTypes)
        {
            _ = entryType ?? throw new ArgumentNullException(nameof(entryType));
            this.entryType = entryType;
        }

        public TypeDescriptorMaker(Strategy strategy,
                              Type entryType,
                              Type[] ommittedTypes = null)
            : this(strategy, entryType, null, ommittedTypes)
        { }


        //private ArityDescriptor GetArity(RuleSet<ArityDescriptor> arityRules,
        //                                 PropertyInfo prop,
        //                                 SymbolType symbolType)
        //{
        //    return null; // TODO
        //}

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
            return null;
        }

        protected override IEnumerable<Candidate> GetChildCandidates(object item) 
            => (IEnumerable<Candidate>)entryType.GetProperties()
                                .Select(x => GetChildCandidate(x));

  
        private Candidate GetChildCandidate(PropertyInfo propertyInfo)
        {
            var name = propertyInfo.Name;
            var candidate = new Candidate(propertyInfo);
            candidate.AddTraitRange(entryType.GetCustomAttributes(Context.IncludeBaseClassAttributes));
            candidate.AddTrait(name);
            candidate.AddTrait(new IdentityWrapper<string>(name));
            return candidate;
        }
    }
}
