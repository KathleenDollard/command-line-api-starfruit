using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.CommandLine.GeneralAppModel.MarkdownExtensions
{
    public static class ExtensionsMarkdown
    {

        public  static void Header(this StringBuilder sb, byte level, string text)
        {
            sb.AppendLine( $"{new string('#', level)} {text}");
        }

        public static string SentenceCase(this string s) 
            => !char.IsUpper(s[0])
                ? s.Substring(0, 1).ToUpperInvariant() + s.Substring(1)
                : s;


    }
}
