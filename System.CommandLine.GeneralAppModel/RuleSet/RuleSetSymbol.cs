using System.Collections.Generic;
using System.Linq;
using System;

namespace System.CommandLine.GeneralAppModel
{
    public abstract class RuleSetSymbol : RuleSetBase, IRuleSetSymbol
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

        public RuleGroup<IRuleGetValues<string>> DescriptionRules { get; } = new RuleGroup<IRuleGetValues<string>>();

        /// <summary>
        /// Some NameRules will also morph the names. When morphing names, only those name rules with IRuleMorphValue<string>  will be used
        /// </summary>
        public RuleGroup<IRuleGetValues<string>> NameRules { get; } = new RuleGroup<IRuleGetValues<string>>();
        public RuleGroup<IRuleAliases> AliasesRules { get; } = new RuleGroup<IRuleAliases>();
        public RuleGroup<IRuleGetValues<bool>> IsHiddenRules { get; } = new RuleGroup<IRuleGetValues<bool>>();

        public IEnumerable<T> GetSymbols<T>(SymbolType requestedSymbolType, IEnumerable<T> items, SymbolDescriptorBase parentSymbolDescriptor)
        {
            throw new NotImplementedException();
        }

        public void AddDescriptionRule(IRuleGetValues<string> descriptionRule)
        {
            CheckFrozen();
            DescriptionRules.Add(descriptionRule);
        }

        public void AddNameRule(IRuleGetValues<string> nameRule)
        {
            CheckFrozen();
            NameRules.Add(nameRule);
        }

        public void AddAliasesRule(IRuleAliases aliasesRule)
        {
            CheckFrozen();
            AliasesRules.Add(aliasesRule);
        }

        public void AddHiddenRule(IRuleGetValues<bool> isHiddenRule)
        {
            CheckFrozen();
            IsHiddenRules.Add(isHiddenRule);
        }

        public override string Report(int tabsCount)
        {
            return $@"{CoreExtensions.NewLineWithTabs(tabsCount)} NameRules:  { string.Join("", NameRules.Select(r => CoreExtensions.NewLineWithTabs(tabsCount + 1) + r.RuleDescription))}
                     { CoreExtensions.NewLineWithTabs(tabsCount)} DescriptionRules:  { string.Join("", DescriptionRules.Select(r => CoreExtensions.NewLineWithTabs(tabsCount + 1) + r.RuleDescription))}
                     { CoreExtensions.NewLineWithTabs(tabsCount)} AliasesRules:  { string.Join("", AliasesRules.Select(r => CoreExtensions.NewLineWithTabs(tabsCount + 1) + r.RuleDescription))}
                     { CoreExtensions.NewLineWithTabs(tabsCount)} IsHiddenRules:  { string.Join("", IsHiddenRules.Select(r => CoreExtensions.NewLineWithTabs(tabsCount + 1) + r.RuleDescription))}";
        }
    }
}