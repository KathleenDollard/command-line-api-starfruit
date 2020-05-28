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

        public (bool success, uint minimumCount, uint maximumCount) GetArity(SymbolDescriptorBase descriptor,
                                                               IEnumerable<object> traits,
                                                               SymbolDescriptorBase parentSymbolDescriptor)
        {
            var (success, data) = GetFirstOrDefaultValue(descriptor, traits, parentSymbolDescriptor);
            if (!success )
            {
                return (false, default, default);
            }
            uint minCount = 0;
            uint maxCount = 0;
            if (data.TryGetValue(ArityDescriptor.MinimumCountName, out var objMinCount))
            {
                minCount = Conversions.To<uint>(objMinCount);
            }
            if (data.TryGetValue(ArityDescriptor.MaximumCountName, out var objMaxCount))
            {
                maxCount = Conversions.To<uint>(objMaxCount);
            }
            return (true, minCount, maxCount);
        }

    }
}
