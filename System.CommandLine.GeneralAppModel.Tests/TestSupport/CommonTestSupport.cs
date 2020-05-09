using System;
using System.Collections.Generic;
using System.Text;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public class CommonTestSupport
    {
        public static void AddAliases(Symbol symbol, IEnumerable<string> aliases)
        {
            if (aliases is null)
            {
                return;
            }
            foreach (var alias in aliases)
            {
                symbol.AddAlias(alias);
            }
        }
    }
}
