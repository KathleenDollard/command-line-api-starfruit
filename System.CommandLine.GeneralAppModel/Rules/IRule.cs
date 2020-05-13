using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Text;

namespace System.CommandLine.GeneralAppModel
{
    public interface IRule
    { }

    public interface IRule<T> : IRule
    { }

    public interface IRuleGetItems : IRule
    {
        SymbolType SymbolType { get; }
        IEnumerable<Candidate> GetItems(IEnumerable<Candidate> candidates,
                                   SymbolDescriptorBase parentSymbolDescriptor);
    }

    public interface IRuleGetValue<T> : IRule
    {
        (bool success, T value) GetFirstOrDefaultValue(SymbolDescriptorBase symbolDescriptor,
                                 IEnumerable<object> item,
                                 SymbolDescriptorBase parentSymbolDescriptor);

    }

    public interface IRuleGetValues<T> : IRuleGetValue<T>
    {

        IEnumerable<T> GetAllValues(SymbolDescriptorBase symbolDescriptor,
                                    IEnumerable<object> item,
                                    SymbolDescriptorBase parentSymbolDescriptor);
    }

    public interface IRuleMorphValue<T> : IRule
    {
        T MorphValue(SymbolDescriptorBase symbolDescriptor,
                        object item,
                        T input,
                        SymbolDescriptorBase parentSymbolDescriptor);
    }

    public interface IRuleArity : IRule
    {
        (uint MinimumCount, uint MaximumCount) GetArity(SymbolDescriptorBase symbolDescriptor,
                                                        IEnumerable<object> item,
                                                        SymbolDescriptorBase parentSymbolDescriptor);
    }

    public interface IRuleAliases : IRule
    {
        // TODO: 
        //(uint MinimumCount, uint MaximumCount) GetArity(SymbolDescriptorBase symbolDescriptor,
        //                                                IEnumerable<object> item,
        //                                                SymbolDescriptorBase parentSymbolDescriptor);
    }
    public interface IRuleSetSymbol : IRule
    {
        RuleSet<IRuleGetValues<string>> DescriptionRules { get; }
        RuleSet<IRuleGetValues<string>> NameRules { get; }
        RuleSet<IRuleAliases> AliasesRule { get; }
        RuleSet<IRuleGetValues<bool>> IsHiddenRule { get; }
    }
    public interface IRuleSetArgument : IRuleSetSymbol
    {
        RuleSet<IRuleArity> ArityRule { get; }
        RuleSet<IRuleGetValues<bool>> RequiredRule { get; }

        /// <summary>
        /// The argument type is inferred in most cases. However, a JSON or other non-typed
        /// AppModel may need this. The name is intended to imply its use is rare.
        /// </summary>
        RuleSet<IRuleGetValues<Type>> SpecialArgumentTypeRule { get; }
        //IRuleGetValues<DefaultDescriptor> DefaultRule { get; }
    }
    public interface IRuleSetCommand : IRuleSetSymbol
    {
    }
    public interface IRuleSetOption : IRuleSetSymbol
    {
        RuleSet<IRuleGetValues<bool>> RequiredRule { get; }
        // TODO: We need a rule to recognize arguments
    }

}
