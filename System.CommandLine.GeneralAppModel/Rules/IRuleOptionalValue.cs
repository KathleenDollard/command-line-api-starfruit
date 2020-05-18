using System;
using System.Collections.Generic;
using System.Text;

namespace System.CommandLine.GeneralAppModel.Rules
{
   public  interface IRuleOptionalValue<T> : IRule
    {
        (bool success, T value) GetOptionalValue(SymbolDescriptorBase symbolDescriptor,
                           IEnumerable<object> item,
                           SymbolDescriptorBase parentSymbolDescriptor);
    }
}
