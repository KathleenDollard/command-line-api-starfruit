using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Text;

namespace System.CommandLine.GeneralAppModel
{
    public interface IRuleSelectSymbols
    {
        IEnumerable<T> GetSymbols<T>(SymbolType requestedSymbolType,
                                     IEnumerable<T> items,
                                     SymbolDescriptorBase parentSymbolDescriptor);
    }

    public interface IRuleGetValue<T>
    {
        T GetFirstOrDefaultValue(SymbolDescriptorBase symbolDescriptor,
                                 IEnumerable<object> item,
                                 SymbolDescriptorBase parentSymbolDescriptor);

    }

    public interface IRuleGetValues<T> : IRuleGetValue<T>
    {

        IEnumerable<T> GetAllValues(SymbolDescriptorBase symbolDescriptor,
                                    IEnumerable<object> item,
                                    SymbolDescriptorBase parentSymbolDescriptor);
    }

    public interface IRuleMorphValue
    {
        T MorphValue<T>(SymbolDescriptorBase symbolDescriptor,
                        object item,
                        T input,
                        SymbolDescriptorBase parentSymbolDescriptor);
    }

    public interface IRuleArity
    {
        (uint MinimumCount, uint MaximumCount) GetArity(SymbolDescriptorBase symbolDescriptor,
                                                        IEnumerable<object> item,
                                                        SymbolDescriptorBase parentSymbolDescriptor);
    }

    public interface IRuleUseSymbol : IRuleSelectSymbols
    {
        IRuleGetValues<string> DescriptionRule { get; }
        IRuleGetValues<string> NameRule { get; }
        IRuleGetValues<IEnumerable<string>> AliasesRule { get; }
        IRuleGetValues<bool> IsHiddenRule { get; }
    }

    public interface IRuleUseArgument : IRuleUseSymbol
    {
        IRuleGetValues<ArityDescriptor> ArityRule { get; }
        IRuleGetValues<bool> RequiredRule { get; }

        /// <summary>
        /// The argument type is inferred in most cases. However, a JSON or other non-typed
        /// AppModel may need this. The name is intended to imply its use is rare.
        /// </summary>
        IRuleGetValues<Type> SpecialArgumentType { get; }
        //IRuleGetValues<DefaultDescriptor> DefaultRule { get; }
    }
    public interface IRuleUseCommand : IRuleUseSymbol
    {
    }
    public interface IRuleUseOption : IRuleUseSymbol
    {
        IRuleGetValues<bool> RequiredRule { get; }
        // TODO: We need a rule to recognize arguments
    }

}
