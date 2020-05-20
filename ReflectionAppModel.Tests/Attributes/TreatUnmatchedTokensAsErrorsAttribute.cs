using System;
using System.Collections.Generic;
using System.Text;

namespace System.CommandLine.ReflectionAppModel.Tests
{
    public class TreatUnmatchedTokensAsErrorsAttribute : Attribute
    {
        public TreatUnmatchedTokensAsErrorsAttribute()
        {
            Value = true;
        }

        public bool Value { get; set; }
    }
}
