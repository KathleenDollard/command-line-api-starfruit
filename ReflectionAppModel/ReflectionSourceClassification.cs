using ReflectionAppModel;
using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System.CommandLine.ReflectionAppModel
{
    internal class ReflectionSourceClassification<T> : SourceClassification<T>
       where T : ICustomAttributeProvider
    {
        internal ReflectionSourceClassification(Strategy strategy, IEnumerable<T> sourceItems)
            : base(strategy,sourceItems)
        {     }
   
        protected override bool Match(T item, RuleSet<string> rules)
        {
            // I'm not sure whether we shoudl inherit custom attributes, but it seems less breaking to later change false->true than the reverse
            return rules.HasMatch(SymbolType.All, item.GetCustomAttributes(false)
                                                    .Union(new object[] { new IdentityWrapper<string>(GetName(item)) })
                                                    .ToArray());
        }

        private string GetName(T item)
        {
            return item switch
            {
                PropertyInfo p => p.Name,
                ParameterInfo p => p.Name,
                Type p => p.Name,
                _ => throw new InvalidOperationException("Unexpected reflected type"),
            };
        }
    }
}
