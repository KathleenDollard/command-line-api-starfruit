﻿using System.Collections.Generic;

namespace System.CommandLine.GeneralAppModel
{
    public static class StringExtensions
    {
        public static string FriendlyFromPascal(this string s)
        {
            if (s is null)
            {
                return string.Empty;
            }
            char space = (char)32;
            var start = s.ToCharArray();
            var friendly = new List<char>();
            for (int i = 0; i < s.Length; i++)
            {
                var c = start[i];
                if (char.IsUpper(c))
                {
                    friendly.Add(space);
                    c = char.ToLower(c);
                }
                friendly.Add(c);
            }
            return new string(friendly.ToArray()).Trim();
        }
    }
}
