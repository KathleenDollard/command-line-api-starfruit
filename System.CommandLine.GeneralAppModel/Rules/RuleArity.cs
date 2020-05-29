using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Text;

namespace System.CommandLine.GeneralAppModel.Rules
{
    public class RuleArity : AttributeWithComplexValueRule, IRuleArity
    {
        public RuleArity(string attributeName, string minCountPropertyName, string maxCountPropertyName)
            : base(attributeName, SymbolType.Argument)
        {
            PropertyNamesAndTypes = new List<AttributeWithComplexValueRule.NameAndType>()
            {
                        new AttributeWithComplexValueRule.NameAndType(ArityDescriptor.MinimumCountName,propertyName: minCountPropertyName, propertyType: typeof(int)),
                        new AttributeWithComplexValueRule.NameAndType(ArityDescriptor.MaximumCountName,propertyName: maxCountPropertyName, propertyType: typeof(int))
            };
        }
    }
}
