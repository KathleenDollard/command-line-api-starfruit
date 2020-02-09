using System;
using System.Collections.Generic;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.StarFruit.WinForms;
using System.Text;

namespace System.CommandLine.StarFruit
{
    public static class Extensions
    {
        //public static T GetInstance<T>(this ReflectionParser<T> reflectionParser, string args, bool isRoot = false)
        //    => ReflectionParser<T>.GetInstance(args, reflectionParser, isRoot);

        //public static T GetInstance<T>(this ReflectionParser<T> reflectionParser, string[] args, bool isRoot = false)
        //    => ReflectionParser<T>.GetInstance(args, reflectionParser, isRoot);


        public static CommandLineBuilder UseGui(
            this CommandLineBuilder builder)
        {
            builder.AddMiddleware(async (context, next) =>
            {
                if (context.ParseResult.Directives.Contains("gui"))
                {
                    var guiMaker = new GuiMaker();
                    guiMaker.Configure(context);
                    guiMaker.Show();
                }
                await next(context);
            }, MiddlewareOrder.Default);
            return builder;
        }

    }
}
