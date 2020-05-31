using System;
using System.Collections.Generic;
using System.Text;

namespace System.CommandLine.GeneralAppModel
{
    public class DisplayFor
    {
        private const string NullDisplay = "<null>";
        private const string EmptyDisplay = "<empty>";

        public static object Name(string? name)
        {
            return name is null
                ? NullDisplay
                : string.IsNullOrWhiteSpace(name)
                    ? EmptyDisplay
                    : name;
        }
    }
}
