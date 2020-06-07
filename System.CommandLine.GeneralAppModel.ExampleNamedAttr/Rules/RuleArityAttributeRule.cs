using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Text;

namespace System.CommandLine.NamedAttributeRules
{
    public class RuleArity : AttributeWithComplexValueRule, IRuleArity
    {
        public RuleArity(string attributeName, string minCountPropertyName, string maxCountPropertyName)
            : base(attributeName, SymbolType.Argument)
        {
            PropertyNamesAndTypes.Add(new AttributeWithComplexValueRule.NameAndType(ArityDescriptor.MinimumCountName, propertyName: minCountPropertyName, propertyType: typeof(int)));
            PropertyNamesAndTypes.Add(new AttributeWithComplexValueRule.NameAndType(ArityDescriptor.MaximumCountName, propertyName: maxCountPropertyName, propertyType: typeof(int)));
        }
    }
}
