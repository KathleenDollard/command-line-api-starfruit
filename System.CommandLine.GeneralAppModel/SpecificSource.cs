using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel
{
    /// <summary>
    /// This is the base class for source specific code - such as reflection of Roslyn specific code. 
    /// GeneralAppModel should have no (zero, nada) dependencies on any specific type of source.
    /// </summary>
    public abstract class SpecificSource
    {
        public static SpecificSource Tools { get; internal set; }

        public abstract Candidate GetCandidate( object item);
        public abstract Type GetArgumentType(Candidate candidate);
        public abstract IEnumerable<Candidate> GetChildCandidates(Strategy strategy,
                                                                  SymbolDescriptorBase commandDescriptor);

         public abstract bool ComplexAttributeHasAtLeastOneProperty(IEnumerable<ComplexAttributeRule.NameAndType> propertyNamesAndTypes,
                                                                   object attribute);
        public abstract bool IsAttributeAMatch(string attributeName,
                                               SymbolDescriptorBase symbolDescriptor,
                                               object item,
                                               SymbolDescriptorBase parentSymbolDescriptor);
     
        public abstract bool DoesAttributeMatch(string attributeName, object a);
        public abstract  T GetAttributeProperty<T>(object attribute, string propertyName);
        public abstract  IEnumerable<T> GetAttributeProperties<T>(object attribute, string propertyName);
        public abstract (bool success, object value) GetAttributePropertyValue(object attribute, string propertyName);

    }
}
