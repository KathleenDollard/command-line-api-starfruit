using System;
using System.Collections.Generic;
using System.Text;

namespace System.CommandLine.StarFruit
{
    public static class Extensions
    {
        public static T GetInstance<T>(this ReflectionParser<T> reflectionParser, string args)
            => ReflectionParser<T>.GetInstance(args, reflectionParser);

        public static T GetInstance<T>(this ReflectionParser<T> reflectionParser, string[] args)
            => ReflectionParser<T>.GetInstance(args, reflectionParser);
    }
}
