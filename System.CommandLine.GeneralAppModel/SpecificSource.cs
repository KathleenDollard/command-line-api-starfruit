using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel
{
    /// <summary>
    /// This is the base class for source specific code - such as reflection of Roslyn specific code. 
    /// GeneralAppModel should have no (zero, nada) dependencies on any specific type of source.
    /// </summary>
    public abstract class SpecificSource
    {
        private static SpecificSource? tools;

        /// <summary>
        /// This provides a singleton for access to the methods technology specific layer (like Roslyn, Reflection or JSON)
        /// </summary>
        public static SpecificSource Tools
        {
            get
            {
                var _ = tools ?? throw new InvalidOperationException("Tools cannot be used before they are set");
                return tools;
            }
        }

        /// <summary>
        /// This provides a singleton for access to the methods technology specific layer (like Roslyn, Reflection or JSON)
        /// </summary>
        internal static void SetTools(SpecificSource value)
        {
            tools = value;
        }

        /// <summary>
        /// Wrap the item including supplying a default name and traits that will be used by rules
        /// </summary>
        /// <remarks>
        /// Candidates wrap the raw item providing technology specific data - especially default
        /// names and traits. Traits are things that can be used in rules. For example, .NET Attributes
        /// or labeled values (key value pairs).
        /// <br/>
        /// Candidates may align with Commands, Arguments, Options or OptionArguments.
        /// </remarks>
        /// <param name="item">The raw item</param>
        /// <returns>The raw item wrapped in a hydrated candidate object</returns>
        public abstract Candidate CreateCandidate(object item);

        /// <summary>
        /// The argument type.
        /// </summary>
        /// <param name="candidate">The candidate to retrieve the value from</param>
        /// <remarks>
        /// This be replaced by either a labelled trait or something else 
        /// a bit more flexible. Waiting to see what other needs their are,
        /// for example we may have custom rules that require running code in the technology.
        /// </remarks>
        /// <returns></returns>
        public abstract ArgTypeInfo GetArgTypeInfo(Candidate candidate);

        /// <summary>
        /// 
        /// <br/>
        /// If there is more than one return value that matches a given name, the first
        /// will be used, so if there is an ordering preference, such as the greatest number
        /// of parmeters, this order should be set in the derived implementation. 
        /// </summary>
        /// <param name="raw"></param>
        /// <returns></returns>
        public abstract IEnumerable<InvokeMethodInfo> GetAvailableInvokeMethodInfos( object? raw, SymbolDescriptor parentSymbolDescriptor , bool treatParametersAsSymbols);

        /// <summary>
        /// Given a command, return it's children - subcommands, options and arguments.
        /// </summary>
        /// <remarks>
        /// The use of strategies in this layer feels a bit wonky. The alternative seems 
        /// to be returning a very large list of candidates and having GeneralAppModelBase
        /// work out what's a candidate. I would prefer that, but hesitate to pass around 
        /// a list of all of the types in the assembly. I suspect this is a false concern. 
        /// </remarks>
        /// <param name="strategy">
        /// The strategy that contains the rules to use in determining which of technology 
        /// specific children (the raw children) should be considered candidates. This is 
        /// needed, for example, to determine which types should be considered.
        /// </param>
        /// <param name="commandDescriptor">
        /// The descriptor that contains the raw item used to determine potential candidates. 
        /// The descriptor is passed because in some cases information already supplied may
        /// be useful in determining child candidates.
        /// </param>
        /// <returns></returns>
        public abstract IEnumerable<Candidate> GetChildCandidates(Strategy strategy,
                                                                  SymbolDescriptor commandDescriptor);

        public virtual bool DoesTraitMatch<TAttribute, TTraitType>(
                         ISymbolDescriptor symbolDescriptor,
                         TTraitType trait,
                         ISymbolDescriptor parentSymbolDescriptor)
            => DoesTraitMatch<TAttribute, TTraitType>(string.Empty, symbolDescriptor, trait, parentSymbolDescriptor);

        public virtual bool DoesTraitMatch<TTraitType>(
                                string attributeName,
                                ISymbolDescriptor symbolDescriptor,
                                TTraitType trait,
                                ISymbolDescriptor parentSymbolDescriptor)
            => DoesTraitMatch(attributeName, string.Empty, symbolDescriptor, trait, parentSymbolDescriptor);


        public abstract bool DoesTraitMatch<TTraitType>(
                                string attributeName,
                                string propertyName,
                                ISymbolDescriptor symbolDescriptor,
                                TTraitType trait,
                                ISymbolDescriptor parentSymbolDescriptor);

        public abstract bool DoesTraitMatch<TAttribute, TTraitType>(
                         string propertyName,
                         ISymbolDescriptor symbolDescriptor,
                         TTraitType trait,
                         ISymbolDescriptor parentSymbolDescriptor);

        public virtual (bool success, TValue value) GetValue<TValue>(
                        string attributeName,
                        ISymbolDescriptor symbolDescriptor,
                        object trait,
                        ISymbolDescriptor parentSymbolDescriptor)
            => GetValue<TValue>(attributeName, string.Empty, symbolDescriptor, trait, parentSymbolDescriptor);


        public abstract (bool success, TValue value) GetValue<TValue>(
                       string attributeName,
                       string propertyName,
                       ISymbolDescriptor symbolDescriptor,
                       object trait,
                       ISymbolDescriptor parentSymbolDescriptor);

        public abstract (bool success, TValue value) GetValue<TAttribute, TValue>(
                        string propertyName,
                        ISymbolDescriptor symbolDescriptor,
                        object trait,
                        ISymbolDescriptor parentSymbolDescriptor);

        public abstract IEnumerable<TValue> GetAllValues<TValue>(
                        string attributeName,
                        string propertyName,
                        ISymbolDescriptor symbolDescriptor,
                        object trait,
                        ISymbolDescriptor parentSymbolDescriptor);

        public abstract IEnumerable<TValue> GetAllValues<TAttribute, TValue>(
                        string propertyName,
                        ISymbolDescriptor symbolDescriptor,
                        object trait,
                        ISymbolDescriptor parentSymbolDescriptor);

        public abstract IEnumerable<(string key, TValue value)> GetComplexValue<TValue>(
                        string attributeName,
                        ISymbolDescriptor symbolDescriptor,
                        object trait,
                        ISymbolDescriptor parentSymbolDescriptor);

        public abstract IEnumerable<(string key, TValue value)> GetComplexValue<TAttribute, TValue>(
                 ISymbolDescriptor symbolDescriptor,
                 object trait,
                 ISymbolDescriptor parentSymbolDescriptor);

    }
}
