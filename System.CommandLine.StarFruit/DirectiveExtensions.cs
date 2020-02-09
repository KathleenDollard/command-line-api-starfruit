//using System;
//using System.Collections.Generic;
//using System.CommandLine.Builder;
//using System.CommandLine.Parsing;
//using System.Text;

//namespace System.CommandLine.StarFruit
//{
//    public static class DirectiveExtensions
//    {
//        public static string Gui(this ParseResult result)
//        {
//            return result.CommandResult.Command.Name + "|" + result.TextToMatch();
//        }

//        public static CommandLineBuilder UseGuid(this CommandLineBuilder builder)
//        {
//            builder.AddMiddleware(async (context, next) =>
//            {
//                var helpOptionTokens = new HashSet<string>();
//                var prefixes = context.Parser.Configuration.Prefixes;
//                if (prefixes == null)
//                {
//                    helpOptionTokens.Add("-h");
//                    helpOptionTokens.Add("/h");
//                    helpOptionTokens.Add("--help");
//                    helpOptionTokens.Add("-?");
//                    helpOptionTokens.Add("/?");
//                }
//                else
//                {
//                    string[] helpOptionNames = { "help", "h", "?" };
//                    foreach (var helpOption in helpOptionNames)
//                    {
//                        foreach (var prefix in prefixes)
//                        {
//                            helpOptionTokens.Add($"{prefix}{helpOption}");
//                        }
//                    }
//                }

//                if (!ShowHelp(context, helpOptionTokens))
//                {
//                    await next(context);
//                }
//            }, MiddlewareOrderInternal.HelpOption);

//            return builder;
//        }
//    }
//}
