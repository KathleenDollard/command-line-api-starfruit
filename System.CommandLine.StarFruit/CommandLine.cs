// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// This was lifted from DragonFruit

using System.Collections.Generic;
using System.CommandLine.Binding;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.CommandLine.Rendering;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;


namespace System.CommandLine.StarFruit
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
        public static async Task<int> ExecuteMethodAsync<T>(
            MethodInfo mainMethodInfo,
            string[] args,
            IConsole console = null)
            => await InvokeMethodAsync(args, mainMethodInfo, "", null, console);

        /// <summary>
        /// Finds and executes 'Program.Main', but with strong types.
        /// </summary>
        /// <param name="entryAssembly">The entry assembly</param>
        /// <param name="args">The string arguments.</param>
        /// <param name="entryPointFullTypeName">Explicitly defined entry point</param>
        /// <param name="xmlDocsFilePath">Explicitly defined path to xml file containing XML Docs</param>
        /// <param name="console">Output console</param>
        /// <returns>The exit code.</returns>
        public static int ExecuteMethod<T>(
             MethodInfo mainMethodInfo,
             string[] args,
             IConsole console = null)
             => InvokeMethod(args, mainMethodInfo, "", null, console);
        #endregion

        public static async Task<int> InvokeMethodAsync(
            string[] args,
            MethodInfo method,
            string xmlDocsFilePath = null,
            object target = null,
            IConsole console = null)
        {
            Parser parser = BuildParser(method, xmlDocsFilePath, target);

            return await parser.InvokeAsync(args, console);
        }

        public static int InvokeMethod(
            string[] args,
            MethodInfo method,
            string xmlDocsFilePath = null,
            object target = null,
            IConsole console = null)
        {
            Parser parser = BuildParser(method, xmlDocsFilePath, target);

            return parser.Invoke(args, console);
        }

        private static Parser BuildParser(MethodInfo method,
            string xmlDocsFilePath,
            object target)
        {
            var builder = new CommandLineBuilder()
                .ConfigureRootCommandFromMethod(method, target)
                .UseDefaults();
                //.UseGui()
                //.UseAnsiTerminalWhenAvailable();

            return builder.Build();
        }

    }


    public static class CommandLine2
    {
        //private static readonly string[] _argumentParameterNames =
        // {
        //    "arguments",
        //    "argument",
        //    "args"
        //};

        public static CommandLineBuilder ConfigureRootCommandFromMethod(
            this CommandLineBuilder builder,
            MethodInfo method,
            object target = null)
        {
            _ = builder ?? throw new ArgumentNullException(nameof(builder));
            _ = method ?? throw new ArgumentNullException(nameof(method));

            // TODO: Figure out how to configure this and select correct app model
            var commandMaker = new StarFruitCommandMaker();
            commandMaker.Configure(builder.Command, method);

            return builder;
        }
        //public static void ConfigureFromMethod(
        //    this Command command,
        //    MethodInfo method,
        //    object target = null)
        //{
        //    if (command == null)
        //    {
        //        throw new ArgumentNullException(nameof(command));
        //    }

        //    if (method == null)
        //    {
        //        throw new ArgumentNullException(nameof(method));
        //    }

        //    foreach (var option in method.BuildOptions())
        //    {
        //        command.AddOption(option);
        //    }

        //    if (method.GetParameters()
        //              .FirstOrDefault(p => _argumentParameterNames.Contains(p.Name)) is ParameterInfo argsParam)
        //    {
        //        var argument = new Argument
        //        {
        //            ArgumentType = argsParam.ParameterType,
        //            Name = argsParam.Name
        //        };

        //        if (argsParam.HasDefaultValue)
        //        {
        //            if (argsParam.DefaultValue != null)
        //            {
        //                argument.SetDefaultValue(argsParam.DefaultValue);
        //            }
        //            else
        //            {
        //                argument.SetDefaultValueFactory(() => null);
        //            }
        //        }

        //        command.AddArgument(argument);
        //    }

        //    command.Handler = CommandHandler.Create(method, target);
        //}

        //public static string BuildAlias(this IValueDescriptor descriptor)
        //{
        //    if (descriptor == null)
        //    {
        //        throw new ArgumentNullException(nameof(descriptor));
        //    }

        //    return BuildAlias(descriptor.ValueName);
        //}

        //internal static string BuildAlias(string parameterName)
        //{
        //    if (string.IsNullOrWhiteSpace(parameterName))
        //    {
        //        throw new ArgumentException("Value cannot be null or whitespace.", nameof(parameterName));
        //    }

        //    return parameterName.Length > 1
        //               ? $"--{parameterName.ToKebabCase()}"
        //               : $"-{parameterName.ToLowerInvariant()}";
        //}

        //public static IEnumerable<Option> BuildOptions(this MethodInfo methodInfo)
        //{
        //    var descriptor = HandlerDescriptor.FromMethodInfo(methodInfo);

        //    var omittedTypes = new[]
        //                       {
        //                           typeof(IConsole),
        //                           typeof(InvocationContext),
        //                           typeof(BindingContext),
        //                           typeof(ParseResult),
        //                           typeof(CancellationToken),
        //                       };

        //    foreach (var option in descriptor.ParameterDescriptors
        //                                     .Where(d => !omittedTypes.Contains(d.Type))
        //                                     .Where(d => !_argumentParameterNames.Contains(d.ValueName))
        //                                     .Select(p => p.BuildOption()))
        //    {
        //        yield return option;
        //    }
        //}

        //public static Option BuildOption(this ParameterDescriptor parameter)
        //{
        //    var argument = new Argument
        //    {
        //        ArgumentType = parameter.Type
        //    };

        //    if (parameter.HasDefaultValue)
        //    {
        //        argument.SetDefaultValueFactory(parameter.GetDefaultValue);
        //    }

        //    var option = new Option(
        //        parameter.BuildAlias(),
        //        parameter.ValueName)
        //    {
        //        Argument = argument
        //    };

        //    return option;
        //}

    }
}
