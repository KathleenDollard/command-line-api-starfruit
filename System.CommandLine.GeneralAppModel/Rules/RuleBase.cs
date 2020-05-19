namespace System.CommandLine.GeneralAppModel
{
    /// <summary>
    /// RuleBase defines a single rule - such as an ArgumentAttribute indicating that an item is an argument. 
    /// It will be most convenient if rules are derived from this class, and in early versions, this was
    /// required. However, the eventual intent is to have them interface based on capabilies. 
    /// <br/>
    /// Rules are contaiend in RuleSets, which are simply a group of rules - such as multiple attributes 
    /// and naming conventions any one of which the CLI developer can use
    /// <br/>
    /// RuleSets are contained in a Strategy. A strategy has named rulesets for specific decision points.
    /// </summary> 
    /// <remarks>
    /// The capabilities for rules are defined by a set of interfaces like IRuleSelectSymbols. 
    /// As we are early in development, more interfaces may be added. 
    /// A Rule will occasionally implement all three. 
    /// <br/>
    /// The usage of rules is defined in the intefaces like IRuleSetArgument.
    /// </remarks>
    public abstract class RuleBase : IRule
    {
        public RuleBase(SymbolType symbolType)
        {
            SymbolType = symbolType;
        }


        public SymbolType SymbolType { get; private protected set; }

        public abstract string RuleDescription<TIRuleSet>();

        /// <summary>
        /// Returns items where HasMatch is true. This is used for multi-item decision points like Aliases. 
        /// </summary>
        /// <param name="symbolDescriptor">The SymbolDescriptor for the thing the items belong to</param>
        /// <param name="items"> The items to evaluate.
        /// Some decision points (RuleSet usage) have a single item - such as a MethodInfo. Others have multiples,
        /// like Attributes. Thus, it's a collection. 
        /// </param>
        /// <remarks>
        /// TODO: Consider supplying a common implementation and making this virtual. 
        /// </remarks>
        /// <returns>All matching items</returns>

    } 
}
