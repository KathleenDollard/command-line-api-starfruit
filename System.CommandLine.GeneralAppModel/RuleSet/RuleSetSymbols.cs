using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel
{
    public class RuleSetSymbols : IRuleSetSymbol
    {
        public bool Frozen { get; private set; }
        internal void Freeze()
            => Frozen = true;
        internal void UnFreeze()
            => Frozen = false;
        public bool CheckFrozen()
            => Frozen
                ? throw new InvalidOperationException("This operation can't be done at this time.")
                : true;

        public RuleSet<IRuleGetValues<string>> DescriptionRules { get; } = new RuleSet<IRuleGetValues<string>>();
        public RuleSet<IRuleGetValues<string>> NameRules { get; } = new RuleSet<IRuleGetValues<string>>();
        public RuleSet<IRuleAliases> AliasesRule { get; } = new RuleSet<IRuleAliases>();
        public RuleSet<IRuleGetValues<bool>> IsHiddenRule { get; } = new RuleSet<IRuleGetValues<bool>>();

        public IEnumerable<T> GetSymbols<T>(SymbolType requestedSymbolType, IEnumerable<T> items, SymbolDescriptorBase parentSymbolDescriptor)
        {
            throw new NotImplementedException();
        }

        public void AddDescriptionRule(IRuleGetValues<string> descriptionRule)
        {
            CheckFrozen();
            DescriptionRules.Add( descriptionRule);
        }

        public void AddNameRule(IRuleGetValues<string> nameRule)
        {
            CheckFrozen();
            NameRules .Add(nameRule);
        }

        public void AddAliasesRule(IRuleAliases aliasesRule)
        {
            CheckFrozen();
            AliasesRule.Add(aliasesRule);
        }

        public void AddHiddenRule(IRuleGetValues<bool> isHiddenRule)
        {
            CheckFrozen();
            IsHiddenRule.Add(isHiddenRule);
        }
    }
}