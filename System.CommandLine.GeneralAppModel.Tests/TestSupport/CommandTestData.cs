using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public class CommandTestData
    {
        public object Raw { get; set; }
        public IEnumerable<string> Aliases { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsHidden { get; set; }
        public bool TreatUnmatchedTokensAsErrors { get; set; } = true;
        public IEnumerable<ArgumentTestData> Arguments { get; set; }
        public IEnumerable<OptionTestData> Options { get; set; }
    }
}
