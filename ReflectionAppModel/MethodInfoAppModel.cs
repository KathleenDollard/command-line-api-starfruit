using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Linq;
using System.Reflection;

namespace System.CommandLine.ReflectionAppModel
{
    internal class MethodInfoAppModel : ReflectionAppModel
    {
        private readonly object target;
        private readonly MethodInfo entryMethod;
        private readonly ParameterClassification parameterClassification;

        private MethodInfoAppModel(Strategy strategy,
                               object dataSource,
                               object parentDataSource,
                               Type[] ommittedTypes = null)
            : base(strategy, dataSource, parentDataSource, ommittedTypes)
        { }

        public MethodInfoAppModel(Strategy strategy,
                              MethodInfo entryMethod,
                              Type[] ommittedTypes = null)
            : this(strategy, entryMethod, null, ommittedTypes)
        {
            this.entryMethod = entryMethod;
            parameterClassification = new ParameterClassification(strategy, entryMethod.GetParameters());
        }


        protected override CommandDescriptor GetCommand()
        {
            _ = entryMethod ?? throw new ArgumentNullException(nameof(entryMethod));

            var command = BuildCommand();

            var arguments = parameterClassification.ArgumentItems
                                .Select(p => BuildArgument(p, SymbolType.Argument));

            return command;
        }

        protected override IEnumerable<ArgumentDescriptor> GetArguments()
        {
            return parameterClassification
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
                Name = GetValue(Strategy.NameRules, entryMethod, SymbolType.Command)
            };

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
        {
            return new OptionDescriptor
            {
                Raw = param,
                Name = GetValue(Strategy.NameRules, param, SymbolType.Option),
                Aliases = GetAll(Strategy.AliasRules, param, SymbolType.Option),
                Description = GetValue(Strategy.DescriptionRules, param, SymbolType.Option),
                IsHidden = GetValue(Strategy.HiddenRules, param, SymbolType.Option),
                Arguments = new ArgumentDescriptor[] { BuildArgument(param, SymbolType.Option) },
                Required = GetValue(Strategy.RequiredRules, param, SymbolType.Option)
            };
        }

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

        private T GetValue<T>(RuleSet<T> rules, ParameterInfo param, SymbolType symbolType)
            => rules.GetFirstOrDefault(symbolType,
                                        param.GetCustomAttributes(),
                                        param.Name,
                                        param.Name);

        private T GetValue<T>(RuleSet<T> rules, MethodInfo method, SymbolType symbolType)
            => rules.GetFirstOrDefault(symbolType,
                                         method.GetCustomAttributes(),
                                         method.Name,
                                         new IdentityWrapper<string>(method.Name));

        private IEnumerable<T> GetAll<T>(RuleSet<T> rules, ParameterInfo param, SymbolType symbolType)
            => rules.GetAll(symbolType,
                                    param.GetCustomAttributes(),
                                    param.Name,
                                    param.Name);


        private class ParameterClassification : ReflectionSourceClassification<ParameterInfo>
        {
            internal ParameterClassification(Strategy strategy, IEnumerable<ParameterInfo> sourceItems)
                : base(strategy, sourceItems)
            {
            }
        }
    }

}
