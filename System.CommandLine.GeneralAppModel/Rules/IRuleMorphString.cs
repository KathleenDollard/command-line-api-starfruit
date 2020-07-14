using System;
using System.Collections.Generic;
using System.Text;

namespace System.CommandLine.GeneralAppModel
{
    public interface IMorphNameRule : IRule
    {
        string MorphName(string name,
                         ISymbolDescriptor symbolDescriptor,
                         IEnumerable<object> traits,
                         ISymbolDescriptor parentSymbolDescriptor);
    }
}
