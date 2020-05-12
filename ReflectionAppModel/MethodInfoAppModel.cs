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

        protected override IEnumerable<object> GetChildCandidates(object DataSource)
        {
            return entryMethod.GetParameters ();
        }

        protected override IEnumerable<object> GetDataCandidates(object DataSource)
        {
            string name = entryMethod.Name;
            var items = new List<object>();
            items.AddRange(entryMethod.GetCustomAttributes(useBaseClassAttributes));
            items.Add(name);
            //if (includeNameIdentity)
            //{
            items.Add(new IdentityWrapper<string>(name));
            //}
            return items.ToArray();
        }

        //protected override IEnumerable<CommandDescriptor> GetSubCommands(SymbolDescriptorBase parentSymbolDescriptor)
        //{
        //    return new List<CommandDescriptor>();
        //}

        //protected override IEnumerable<ICustomAttributeProvider> GetChildCandidates()
        //{
        //    return entryMethod.GetParameters();
        //}
    }
}
