using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel
{
    public abstract class SymbolDescriptorBase
    {
        public SymbolDescriptorBase(SymbolDescriptorBase parentSymbolDescriptorBase,
                                    object raw,
                                    SymbolType symbolType)
        {
            ParentSymbolDescriptorBase = parentSymbolDescriptorBase;
            Raw = raw;
            SymbolType = symbolType;
        }

        /// <summary>
        /// Rules sometimes rely on the parent, although the only current known
        /// instance is Argument rules being different for Command and Option arguments
        /// <br/>
        /// The setting of this value makes depth first much easier, so that is the only option.
        /// If sibling evaluation is needed, plan a post processing step.
        /// </summary>
        public SymbolDescriptorBase ParentSymbolDescriptorBase { get; }

        /// <summary>
        /// This is the underlying thing rules were evaluated against. For
        /// example MethodInfo, Type, ParameterInfo and PropertyInfo appear
        /// in the ReflectionAppModel. 
        /// </summary>
        public object Raw { get; }
        public SymbolType SymbolType { get; }
        public IEnumerable<string> Aliases { get; set; }
        // TODO: Understand raw aliases: public IReadOnlyList<string> RawAliases { get; }
        public string Description { get; set; }
        public virtual string Name { get; set; }
        public bool IsHidden { get; set; }

        // Rules sometimes rely on other rules, such as knowing how an 
        // Argument was determined to be an argument affects the name.
        // For example removing "Argument".
        // This is also extremely valuable during debugging.
        public IEnumerable<RuleBase> AppliedAliasRules { get; } = new List<RuleBase>();
        public IEnumerable<RuleBase> AppliedDescriptionRules { get; } = new List<RuleBase>();
        public IEnumerable<RuleBase> AppliedNameRules { get; } = new List<RuleBase>();
        public IEnumerable<RuleBase> AppliedIsHiddenRules { get; } = new List<RuleBase>();

    }
}
