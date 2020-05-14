﻿namespace System.CommandLine.GeneralAppModel
{
    public interface IRuleSetArgument : IRuleSetSymbol
    {
        RuleGroup<IRuleArity> ArityRules { get; }
        RuleGroup<IRuleGetValues<bool>> RequiredRules { get; }

        /// <summary>
        /// The argument type is inferred in most cases. However, a JSON or other non-typed
        /// AppModel may need this. The name is intended to imply its use is rare.
        /// </summary>
        RuleGroup<IRuleGetValues<Type>> SpecialArgumentTypeRules { get; }
        //IRuleGetValues<DefaultDescriptor> DefaultRule { get; }
    }

}
