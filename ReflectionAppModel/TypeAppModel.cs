using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.ReflectionAppModel
{
    internal class TypeAppModel : ReflectionAppModel<Type, PropertyInfo>
    {
        private readonly Type entryType;
        private readonly AttributeClassification<PropertyInfo> sourceClassification;

        private TypeAppModel(Strategy strategy,
                              Type entryType,
                               object parentDataSource,
                               Type[] ommittedTypes = null)
            : base(strategy, entryType, parentDataSource, ommittedTypes)
        {
            _ = entryType ?? throw new ArgumentNullException(nameof(entryType));
            this.entryType = entryType;
            sourceClassification = new AttributeClassification<PropertyInfo>(strategy,entryType.GetProperties());
        }

        public TypeAppModel(Strategy strategy,
                              Type entryType,
                              Type[] ommittedTypes = null)
            : this(strategy, entryType, null, ommittedTypes)
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

        private ArgumentDescriptor BuildArgument(PropertyInfo prop, SymbolType symbolType)
            => new ArgumentDescriptor
            {
                Raw = prop,
                Name = GetValue(Strategy.NameRules, prop, symbolType),
                Description = GetValue(Strategy.DescriptionRules, prop, symbolType),
                IsHidden = GetValue(Strategy.HiddenRules, prop, symbolType),
                Arity = GetArity(Strategy.ArityRules, prop, symbolType),
                ArgumentType = prop.PropertyType,
                DefaultValue = GetDefaultValue(Strategy.DefaultRules, prop, symbolType),
                Required = GetValue(Strategy.RequiredRules, prop, symbolType)
            };

        private OptionDescriptor BuildOption(PropertyInfo prop)
            => new OptionDescriptor
            {
                Raw = prop,
                Name = GetValue(Strategy.NameRules, prop, SymbolType.Option),
                Aliases = GetAll(Strategy.AliasRules, prop, SymbolType.Option),
                Description = GetValue(Strategy.DescriptionRules, prop, SymbolType.Option),
                IsHidden = GetValue(Strategy.HiddenRules, prop, SymbolType.Option),
                Arguments = new ArgumentDescriptor[] { BuildArgument(prop, SymbolType.Option) },
                Required = GetValue(Strategy.RequiredRules, prop, SymbolType.Option)
            };

        private DefaultValueDescriptor GetDefaultValue(RuleSet<DefaultValueDescriptor> defaultRules,
                                                       PropertyInfo prop,
                                                       SymbolType symbolType)
        {
            throw new NotImplementedException();
        }

        private ArityDescriptor GetArity(RuleSet<ArityDescriptor> arityRules,
                                         PropertyInfo prop,
                                         SymbolType symbolType)
        {
            throw new NotImplementedException();
        }
    }
}
