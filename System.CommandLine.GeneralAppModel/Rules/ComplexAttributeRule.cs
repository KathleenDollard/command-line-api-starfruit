using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel
{

    /// <summary>
    /// This will be important for multi-part attributes like arity
    /// </summary>
    public class ComplexAttributeRule : NamedAttributeRule
    {
        public IEnumerable<string> PropertyNames { get; }
        public ComplexAttributeRule(string attributeName,  string[] propertyNames = null, SymbolType symbolType = SymbolType.All)
            : base(attributeName, "complex", null, symbolType)
        {
            PropertyNames = propertyNames;
        }
        public override string RuleDescription<TIRuleSet>()
          => $"ComplexAttribute Rule: {AttributeName} PropertyNames: {String.Join (", ",PropertyNames)}";
    }
}
