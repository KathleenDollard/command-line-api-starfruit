// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// This was lifted from DragonFruit

using System.CommandLine.Builder;
using System.CommandLine.Parsing;
//using System.CommandLine.Rendering;
using System.Reflection;
using System.Threading.Tasks;

// This code should perhaps be in core or in an app model layer as it will be used for all app models that manage main parameters

namespace System.CommandLine.ReflectionModel
{
    public static class CommandLine
    {
        #region Changes in this section are to facilitate debugging during direct use.
        /// <summary>
        /// Finds and executes 'Program.Main', but with strong types.
        /// </summary>
        /// <param name="entryAssembly">The entry assembly</param>
        /// <param name="args">The string arguments.</param>
        /// <param name="entryPointFullTypeName">Explicitly defined entry point</param>
        /// <param name="xmlDocsFilePath">Explicitly defined path to xml file containing XML Docs</param>
        /// <param name="console">Output console</param>
        /// <returns>The exit code.</returns>
        public static async Task<int> ExecuteMethodAsync<T>(Action<CommandLineBuilder, MethodInfo, object> configureFunc,
                                                            MethodInfo mainMethodInfo,
                                                            string[] args,
                                                            IConsole console = null)
            => await InvokeMethodAsync(configureFunc,args, mainMethodInfo, "", null, console);

        /// <summary>
        /// Finds and executes 'Program.Main', but with strong types.
        /// </summary>
        /// <param name="entryAssembly">The entry assembly</param>
        /// <param name="args">The string arguments.</param>
        /// <param name="entryPointFullTypeName">Explicitly defined entry point</param>
        /// <param name="xmlDocsFilePath">Explicitly defined path to xml file containing XML Docs</param>
        /// <param name="console">Output console</param>
        /// <returns>The exit code.</returns>
        public static int ExecuteMethod<T>(Action<CommandLineBuilder, MethodInfo, object> configureFunc,
                                           MethodInfo mainMethodInfo,
                                           string[] args,
                                           IConsole console = null)
             => InvokeMethod(configureFunc,args, mainMethodInfo, "", null, console);
        #endregion

        public static async Task<int> InvokeMethodAsync(Action<CommandLineBuilder, MethodInfo, object> configureFunc,
                                                        string[] args,
                                                        MethodInfo method,
                                                        string xmlDocsFilePath = null,
                                                        object target = null,
                                                        IConsole console = null)
        {
            Parser parser = BuildParser(configureFunc,method, xmlDocsFilePath, target);

            return await parser.InvokeAsync(args, console);
        }

        public static int InvokeMethod(Action<CommandLineBuilder, MethodInfo, object> configureFunc,
                                       string[] args,
                                       MethodInfo method,
                                       string xmlDocsFilePath = null,
                                       object target = null,
                                       IConsole console = null)
        {
            Parser parser = BuildParser(configureFunc, method, xmlDocsFilePath, target);

            return parser.Invoke(args, console);
        }


        private static Parser BuildParser(Action<CommandLineBuilder, MethodInfo, object> configureFunc,
                                          MethodInfo method,
                                          string xmlDocsFilePath,
                                          object target)
        {
            var builder = new CommandLineBuilder();
            configureFunc(builder, method, target);
            builder.UseDefaults();
           // builder.UseDefaults()
           //.UseAnsiTerminalWhenAvailable();
            // .UseGui()
            return builder.Build();
        }

    }

}