using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.ReflectionAppModel
{
    internal class TypeAppModel : ReflectionAppModel
    {
        private readonly object target;
        private readonly Type entryType;
        private readonly PropertyClassification propertyClassification;

        private TypeAppModel(Strategy strategy,
                               object dataSource,
                               object parentDataSource,
                               Type[] ommittedTypes = null)
            : base(strategy, dataSource, parentDataSource, ommittedTypes)
        { }

        public TypeAppModel(Strategy strategy,
                              Type entryType,
                              Type[] ommittedTypes = null)
            : this(strategy, entryType, null, ommittedTypes)
        {
            this.entryType = entryType;
            propertyClassification = new PropertyClassification(strategy, entryType.GetProperties());
        }


        protected override CommandDescriptor GetCommand()
        {
            _ = entryType ?? throw new ArgumentNullException(nameof(entryType));

            var command = BuildCommand();

            var arguments = propertyClassification.ArgumentItems
                                .Select(p => BuildArgument(p, SymbolType.Argument));

            return command;
        }

        protected override IEnumerable<ArgumentDescriptor> GetArguments()
        {
            return propertyClassification
                        .ArgumentItems
                        .Select(p => BuildArgument(p, SymbolType.Argument));

        }

        protected override IEnumerable<OptionDescriptor> GetOptions()
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<CommandDescriptor> GetSubCommands()
        {
            throw new NotImplementedException();
        }

        private CommandDescriptor BuildCommand()
             => new CommandDescriptor
             {
                 Name = GetValue(Strategy.NameRules, entryType, SymbolType.Command)
             };

        private ArgumentDescriptor BuildArgument(PropertyInfo prop, SymbolType symbolType)
        {
            return new ArgumentDescriptor
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
        }

        private OptionDescriptor BuildOption(PropertyInfo prop)
        {
            return new OptionDescriptor
            {
                Raw = prop,
                Name = GetValue(Strategy.NameRules, prop, SymbolType.Option),
                Aliases = GetAll(Strategy.AliasRules, prop, SymbolType.Option),
                Description = GetValue(Strategy.DescriptionRules, prop, SymbolType.Option),
                IsHidden = GetValue(Strategy.HiddenRules, prop, SymbolType.Option),
                Arguments = new ArgumentDescriptor[] { BuildArgument(prop, SymbolType.Option) },
                Required = GetValue(Strategy.RequiredRules, prop, SymbolType.Option)
            };
        }

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

        private T GetValue<T>(RuleSet<T> rules, PropertyInfo prop, SymbolType symbolType)
            => rules.GetFirstOrDefault(symbolType,
                                    prop.GetCustomAttributes(),
                                    prop.Name);

        private T GetValue<T>(RuleSet<T> rules, Type type, SymbolType symbolType)
            => rules.GetFirstOrDefault(symbolType,
                                            type.GetCustomAttributes(),
                                            type.Name,
                                            new IdentityWrapper<string>(type.Name));

        private IEnumerable<T> GetAll<T>(RuleSet<T> rules, PropertyInfo prop, SymbolType symbolType)
            => rules.GetAll(symbolType,
                                    prop.GetCustomAttributes(),
                                    prop.Name);


        private class PropertyClassification : ReflectionSourceClassification<PropertyInfo>
        {
            internal PropertyClassification(Strategy strategy, IEnumerable<PropertyInfo> sourceItems)
                : base(strategy, sourceItems)
            {
            }
        }
    }
}
