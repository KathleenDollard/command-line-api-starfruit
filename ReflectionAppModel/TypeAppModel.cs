using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Reflection;

namespace System.CommandLine.ReflectionAppModel
{
    internal class TypeAppModel : ReflectionAppModel<Type>
    {
        private readonly Type entryType;

        private TypeAppModel(Strategy strategy,
                              Type entryType,
                               object parentDataSource,
                               Type[] ommittedTypes = null)
            : base(strategy, entryType, parentDataSource, ommittedTypes)
        {
            _ = entryType ?? throw new ArgumentNullException(nameof(entryType));
            this.entryType = entryType;
            //SourceClassification = new AttributeClassification<PropertyInfo>(strategy, entryType.GetProperties());
        }

        public TypeAppModel(Strategy strategy,
                              Type entryType,
                              Type[] ommittedTypes = null)
            : this(strategy, entryType, null, ommittedTypes)
        { }

   
        private ArityDescriptor GetArity(RuleSet<ArityDescriptor> arityRules,
                                         PropertyInfo prop,
                                         SymbolType symbolType)
        {
            return null; // TODO
        }

        protected override IEnumerable<ICustomAttributeProvider> GetChildCandidates()
        {
            return entryType.GetProperties();
        }
    }
}
