using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    public abstract class RuleBase
    {
        public RuleBase(SymbolType symbolType)
        {
            SymbolType = symbolType;
        }

   
        public SymbolType SymbolType { get; private protected  set; }
        public abstract string RuleDescription { get; }
    }

    /// <summary>
    /// RuleBase defines a single rule - such as an ArgumentAttribute indicating that an item is an argument. 
    /// <br/>
    /// Rules are contaiend in RuleSets, which are simply a group of rules - such as multiple attributes and naming conventions any one of which the CLI developer can use
    /// <br/>
    /// RuleSets are contained in a Strategy. A strategy has named rulesets for specific decision points.
    /// </summary>
    /// <typeparam name="T">The type of items expected. This can be object.</typeparam>
    public abstract class RuleBase<T> : RuleBase
    {
        public RuleBase(SymbolType symbolType) : base(symbolType)
        { }

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
        protected abstract IEnumerable<object> GetMatchingItems(SymbolDescriptorBase parentSymbolDescriptor, IEnumerable<object> items);

        /// <summary>
        /// Determines whether any of the items match. 
        /// </summary>
        /// <param name="symbolDescriptor">The SymbolDescriptor for the thing the items belong to</param>
        /// <param name="items"> The items to evaluate.
        /// Some decision points (RuleSet usage) have a single item - such as a MethodInfo. Others have multiples,
        /// like Attributes. Thus, it's a collection. 
        /// </param>
        /// <returns>True if any matches were found.</returns>
        public abstract bool HasMatch(SymbolDescriptorBase parentSymbolDescriptor, IEnumerable<object> items);

        /// <summary>
        /// Returns matching items that are not the default. It is particularly important in attributes where 
        /// it is hard to tell whether a parameter was actually set - like "name" in [Option(name)]
        /// </summary>
        /// <param name="symbolDescriptor">The SymbolDescriptor for the thing the items belong to</param>
        /// <param name="items"> The items to evaluate.
        /// Some decision points (RuleSet usage) have a single item - such as a MethodInfo. Others have multiples,
        /// like Attributes. Thus, it's a collection. 
        /// </param>
        /// <returns>All matches that were not equal to the default for the type</returns>
        public virtual IEnumerable<object> GetAllNonDefault(SymbolDescriptorBase symbolDescriptor, IEnumerable<object> items)
              => GetMatchingItems(symbolDescriptor, items)
                       .Where(v => !v.Equals(default));

        public RuleBase<T> WithSymbolType(SymbolType symbolType)
        {
            var ret = (RuleBase<T>)MemberwiseClone();
            ret.SymbolType = symbolType;
            return ret;
        }

        /// <summary>
        /// Returns a tuple with success indicator and the value if found. 
        /// <br/>
        /// TODO: Consider naming. This is named Single, but returns First
        /// </summary>
        /// <param name="symbolDescriptor">The SymbolDescriptor for the thing the items belong to</param>
        /// <param name="items"> The items to evaluate.
        /// Some decision points (RuleSet usage) have a single item - such as a MethodInfo. Others have multiples,
        /// like Attributes. Thus, it's a collection. 
        /// </param>
        /// <returns>
        /// A tuple of (true, value) if an item was found. Otherwise a tuple (false, value). This lets you distinguish 
        /// between an item that is found and returns default, and not found. 
        /// </returns>
        public virtual (bool Success, T Value) GetSingleOrDefault(SymbolDescriptorBase symbolDescriptor, IEnumerable<object> items)
        {
            var matches = GetMatchingItems(symbolDescriptor, items)
                                .Where(v => !v.Equals(default));
            return matches.Any()
                       ? (true, (T)matches.FirstOrDefault())
                       : (false, default);
        }
    }
}
