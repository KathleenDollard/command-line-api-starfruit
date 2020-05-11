﻿using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel
{
    // TODO: Clarify complex types here
    public class ComplexAttributeRule : NamedAttributeRule
    {
        public IEnumerable<string> PropertyNames { get; }
        public ComplexAttributeRule(string attributeName,  string[] propertyNames = null, SymbolType symbolType = SymbolType.All)
            : base(attributeName, "complex", null, symbolType)
        {
            PropertyNames = propertyNames;
        }
    }
}
