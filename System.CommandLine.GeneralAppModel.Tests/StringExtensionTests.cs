using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public class StringExtensionTests
    {
        [Theory]
        [InlineData("abcd", "abcd")]
        [InlineData("Abcd", "abcd")]
        [InlineData("aBcd", "a bcd")]
        [InlineData("abcD", "abc d")]
        [InlineData("ABCD", "a b c d")]
        [InlineData("", "")]
        [InlineData(null, null)]
        public void FriendlyToPascalProducesCorrectOutput(string s, string expected)
        {
            s.FriendlyFromPascal().Should().Be(expected);
        }
    }
}
