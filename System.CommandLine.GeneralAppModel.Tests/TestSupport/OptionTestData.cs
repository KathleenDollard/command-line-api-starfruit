using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public class OptionTestData
    {
        public object? Raw { get; set; }
        public IEnumerable<string>? Aliases { get; set; } 
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsHidden { get; set; }
        public bool Required { get; set; }

        public IEnumerable<ArgumentTestData>? Arguments { get; set; }
    }
}
