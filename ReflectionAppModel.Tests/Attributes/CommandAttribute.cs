using System;
using System.Collections.Generic;
using System.Text;

namespace System.CommandLine.ReflectionAppModel.Tests
{
    public class CommandAttribute : Attribute
    {
        public CommandAttribute()
        {
        }

        public string Description { get; set; }
        public string Name { get; set; }
        public bool TreatUnmatchedTokensAsErrors { get; set; }
    }
}
