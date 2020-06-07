using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Text;

namespace System.CommandLine.GeneralAppModel.Rules
{
    public class AttributeArityRule<TAttribute> 
        : AttributeWithComplexValueRule<TAttribute>, IRuleArity
    {
        public AttributeArityRule( string minCountPropertyName, string maxCountPropertyName)
            : base( SymbolType.Argument)
        {
            PropertyNamesAndTypes.Add(new NameAndType(ArityDescriptor.MinimumCountName, propertyName: minCountPropertyName, propertyType: typeof(int)));
            PropertyNamesAndTypes.Add(new NameAndType(ArityDescriptor.MaximumCountName, propertyName: maxCountPropertyName, propertyType: typeof(int)));
        }
    }
}
