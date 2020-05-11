using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Reflection;

namespace System.CommandLine.ReflectionAppModel
{
    internal class MethodInfoAppModel : ReflectionAppModel<MethodInfo>
    {
        private readonly MethodInfo entryMethod;

        private MethodInfoAppModel(Strategy strategy,
                              MethodInfo entryMethod,
                               object parentDataSource,
                               Type[] ommittedTypes = null)
            : base(strategy, entryMethod, parentDataSource, ommittedTypes)
        {
            _ = entryMethod ?? throw new ArgumentNullException(nameof(entryMethod));
            this.entryMethod = entryMethod;
            //SourceClassification = new AttributeClassification<ParameterInfo>(strategy, entryMethod.GetParameters());
        }

        public MethodInfoAppModel(Strategy strategy,
                              MethodInfo entryMethod,
                              Type[] ommittedTypes = null)
            : this(strategy, entryMethod, null, ommittedTypes)
        { }

        protected override IEnumerable<CommandDescriptor> GetSubCommands(SymbolDescriptorBase parentSymbolDescriptor)
        {
            return new List<CommandDescriptor>();
        }

        protected override IEnumerable<ICustomAttributeProvider> GetChildCandidates()
        {
            return entryMethod.GetParameters();
        }
    }
}
