using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.ReflectionAppModel
{
    internal class MethodInfoAppModel : ReflectionAppModel<MethodInfo, ParameterInfo>
    {
        private readonly MethodInfo entryMethod;
        private readonly AttributeClassification<ParameterInfo> sourceClassification;

        private MethodInfoAppModel(Strategy strategy,
                              MethodInfo entryMethod,
                               object parentDataSource,
                               Type[] ommittedTypes = null)
            : base(strategy, entryMethod, parentDataSource, ommittedTypes)
        {
            _ = entryMethod ?? throw new ArgumentNullException(nameof(entryMethod));
            this.entryMethod = entryMethod;
            sourceClassification = new AttributeClassification<ParameterInfo>(strategy, entryMethod.GetParameters());
        }

        public MethodInfoAppModel(Strategy strategy,
                              MethodInfo entryMethod,
                              Type[] ommittedTypes = null)
            : this(strategy, entryMethod, null, ommittedTypes)
        {
        }

        protected override IEnumerable<ArgumentDescriptor> GetArguments()
        {
            return sourceClassification
                        .ArgumentItems
                        .Select(p => BuildArgument(p, SymbolType.Argument));

        }

        protected override IEnumerable<OptionDescriptor> GetOptions()
        {
            return sourceClassification
                        .OptionItems
                        .Select(p => BuildOption(p));
        }

        protected override IEnumerable<CommandDescriptor> GetSubCommands()
        {
            throw new NotImplementedException();
        }

        private ArgumentDescriptor BuildArgument(ParameterInfo param, SymbolType symbolType)
            => new ArgumentDescriptor
            {
                Raw = param,
                Name = GetValue(Strategy.NameRules, param, symbolType),
                Description = GetValue(Strategy.DescriptionRules, param, symbolType),
                IsHidden = GetValue(Strategy.HiddenRules, param, symbolType),
                Arity = GetArity(Strategy.ArityRules, param, symbolType),
                ArgumentType = param.ParameterType,
                DefaultValue = GetDefaultValue(Strategy.DefaultRules, param, symbolType),
                Required = GetValue(Strategy.RequiredRules, param, symbolType)
            };

        private OptionDescriptor BuildOption(ParameterInfo param)
            => new OptionDescriptor
            {
                Raw = param,
                Name = GetValue(Strategy.NameRules, param, SymbolType.Option),
                Aliases = GetAll(Strategy.AliasRules, param, SymbolType.Option),
                Description = GetValue(Strategy.DescriptionRules, param, SymbolType.Option),
                IsHidden = GetValue(Strategy.HiddenRules, param, SymbolType.Option),
                Arguments = new ArgumentDescriptor[] { BuildArgument(param, SymbolType.Option) },
                Required = GetValue(Strategy.RequiredRules, param, SymbolType.Option)
            };

        private DefaultValueDescriptor GetDefaultValue(RuleSet<DefaultValueDescriptor> defaultRules,
                                                       ParameterInfo param,
                                                       SymbolType symbolType)
        {
            throw new NotImplementedException();
        }

        private ArityDescriptor GetArity(RuleSet<ArityDescriptor> arityRules,
                                         ParameterInfo param,
                                         SymbolType symbolType)
        {
            throw new NotImplementedException();
        }
    }
}
