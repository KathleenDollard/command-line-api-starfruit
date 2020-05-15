using System;
using System.Collections.Generic;
using System.Text;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public class TreatUnmatchedTokensAsErrorsAttribute : Attribute 
    {
        public TreatUnmatchedTokensAsErrorsAttribute(bool value)
        {
            Value = value;
        }

        public bool Value { get; }
    }
}
