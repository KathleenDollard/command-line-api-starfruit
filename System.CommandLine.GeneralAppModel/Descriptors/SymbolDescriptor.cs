using System.Collections.Generic;
using System.CommandLine.Binding;
using System.Reflection;

namespace System.CommandLine.GeneralAppModel
{

    public class EmptySymbolDescriptor : ISymbolDescriptor
    {
        public EmptySymbolDescriptor()
        {
            OriginalName = string.Empty;
        }
        public SymbolType SymbolType { get; }
        public object? Raw { get; }
        public IEnumerable<string>? Aliases { get; }
        public string? Description { get; }

          public string? Name { get; }

        public string? CommandLineName { get; }

        public string OriginalName { get; }

        public bool IsHidden { get; set; }

        public  string Report(int tabsCount, VerbosityLevel verbosity)
            => "Empty SymbolDescriptor - used for testing";   
    }

    public abstract class SymbolDescriptor : ISymbolDescriptor
    {
        public static ISymbolDescriptor Empty = new EmptySymbolDescriptor();

        public SymbolDescriptor(ISymbolDescriptor parentSymbolDescriptorBase,
                                    string originalName,
                                    object? raw,
                                    SymbolType symbolType)
        {
            ParentSymbolDescriptorBase = parentSymbolDescriptorBase;
            Raw = raw;
            OriginalName = originalName;
            SymbolType = symbolType;
        }

        public abstract string ReportInternal(int tabsCount, VerbosityLevel verbosity);

        public virtual string Report(int tabsCount, VerbosityLevel verbosity)
        {
            string whitespace = CoreExtensions.NewLineWithTabs(tabsCount);
            string whitespace2 = CoreExtensions.NewLineWithTabs(tabsCount+1);
            return $"{whitespace}{Name}" +
                   $"{whitespace2}Kind:{SymbolType }" +
                   $"{whitespace2}Description:{Description }" +
                   $"{whitespace2}Aliases:{Aliases }" +
                   $"{whitespace2}IsHidden:{IsHidden  }" +
                   ReportInternal(tabsCount+1, verbosity) +
                   $"{whitespace2}Raw:{ReportRaw(Raw)}" +
                   $"{whitespace2}Symbol:{ReportBound(SymbolToBind)}";

            static string ReportRaw(object? raw)
            {
                return raw switch
                {
                    null => string.Empty,
                    Type t => $"Type: {t.Name}",
                    MethodInfo m => $"Method: {m.Name}",
                    PropertyInfo p => $"Property: {p.Name}",
                    ParameterInfo p => $"Parameter: {p.Name}",
                    _ => string.Empty
                };
            }

            static string ReportBound(ISymbol? symbolToBind)
            {
                return symbolToBind is null
                       ? string.Empty
                       : symbolToBind.GetType().Name;
            }
        }

        public ISymbol? SymbolToBind { get; private set; }

        internal void SetSymbol(ISymbol symbol)
            => SymbolToBind = symbol;

        /// <summary>
        /// Rules sometimes rely on the parent, although the only current known
        /// instance is Argument rules being different for Command and Option arguments
        /// <br/>
        /// The setting of this value makes depth first much easier, so that is the only option.
        /// If sibling evaluation is needed, plan a post processing step.
        /// </summary>
        public ISymbolDescriptor ParentSymbolDescriptorBase { get; }

        /// <summary>
        /// This is the underlying thing rules were evaluated against. For
        /// example MethodInfo, Type, ParameterInfo and PropertyInfo appear
        /// in the ReflectionAppModel. 
        /// </summary>
        public object? Raw { get; }
        public SymbolType SymbolType { get; }
        public IEnumerable<string>? Aliases { get; set; }
        // TODO: Understand raw aliases: public IReadOnlyList<string> RawAliases { get; }
        public string? Description { get; set; }

        /// <summary>
        /// The name as used to communicate with the end user. For example, an Arg suffix might be removed
        /// but the option prefix is not included.
        /// </summary>
        /// <remarks>
        /// The Name should not be ambiguous with an OriginalName and vice versa. For example, if you are 
        /// removing Arg suffixes via rules (a standard scenario), then having an BlahArg and a BlahArgArg 
        /// argument woudl not be legal. 
        /// </remarks>
        public virtual string? Name { get; set; }

        /// <summary>
        /// The name as used when System.CommandLine objects are created. This name includes option prefixes
        /// and is the name as it should appear in automated help. 
        /// </summary>
        public string? CommandLineName { get; set; }

        /// <summary>
        /// The original name in the model. This is used for DescriptionSource (which recognizes either Name or 
        /// OriginalName
        /// </summary>
        /// <remarks>
        /// The Name should not be ambiguous with an OriginalName and vice versa. For example, if you are 
        /// removing Arg suffixes via rules (a standard scenario), then having an BlahArg and a BlahArgArg 
        /// argument woudl not be legal. 
        /// </remarks>
        public string OriginalName { get; }

        public bool IsHidden { get; set; }
    }
}
