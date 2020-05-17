using System;
using System.Collections.Generic;
using System.Text;

namespace System.CommandLine.ReflectionAppModel.Tests
{
    public class ArityAttribute : Attribute
    {
        public ArityAttribute()
        {
        }

        public int MinimumCount { get; set; }
        public int MaximumCount { get; set; }
    }
}
