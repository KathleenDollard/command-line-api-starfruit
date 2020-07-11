using System;
using System.Collections.Generic;
using System.Text;

namespace System.CommandLine.GeneralAppModel
{
  public static  class ExtensionsString
    {
        public static string WithoutAttributeSuffix(this string s)
            => s.EndsWith("Attribute")
                ? s.Substring(0, s.Length - 9)
                : s;
    }
}
