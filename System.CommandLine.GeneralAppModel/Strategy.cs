using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.MarkdownExtensions;
using System.Text;

namespace System.CommandLine.GeneralAppModel
{
    public class Strategy
    {
        public static Strategy Full = new Strategy("Full").SetFullRules();
        public static Strategy Standard = new Strategy("Standard").SetStandardRules();

        public Strategy(string name = "")
        {
            Name = name;
        }

        public string Name { get; }
        public RuleSetDescriptorContext DescriptorContextRules { get; } = new RuleSetDescriptorContext();
        public RuleSetGetCandidatesRule GetCandidateRules { get; } = new RuleSetGetCandidatesRule();
        public RuleSetSelectSymbols SelectSymbolRules { get; } = new RuleSetSelectSymbols();
        public RuleSetArgument ArgumentRules { get; } = new RuleSetArgument();
        public RuleSetOption OptionRules { get; } = new RuleSetOption();
        public RuleSetArgument OptionArgumentRules { get; } = new RuleSetArgument(); // these aren't yet used
        public RuleSetCommand CommandRules { get; } = new RuleSetCommand();

        public string Report(ReportFormat format = ReportFormat.Text, VerbosityLevel verbosity = VerbosityLevel.Normal)
        {
            return format switch
            {
                ReportFormat.Text => TextReport(),
                ReportFormat.Markdown => MarkdownReport(verbosity),
                _ => throw new NotImplementedException()
            };

        }

        private string MarkdownReport(VerbosityLevel verbosity)
        {
            var typeNameHeader = "Type Name |";
            var sb = new StringBuilder();
            sb.Header(1, "Strategy");
            sb.AppendLine();
            ReportClassifyCandidates(sb, this, verbosity, typeNameHeader);
            ReportArgumentDetails(sb, this, verbosity, typeNameHeader);
            ReportOptionDetails(sb, this, verbosity, typeNameHeader);
            ReportSubCommandDetails(sb, this, verbosity, typeNameHeader);
            ReportContext(sb, this, verbosity, typeNameHeader);
            ReportSelectCandidates(sb, this, verbosity, typeNameHeader);

            return sb.ToString();

            static void ReportContext(StringBuilder sb, Strategy strategy, VerbosityLevel verbosity, string typeNameHeader)
            {
                sb.Header(2, "Context");
                sb.AppendLine();
                sb.AppendLine($"|Detail|Apply To|Rule|{(verbosity >= VerbosityLevel.Normal ? typeNameHeader : string.Empty)}");
                sb.AppendLine("|-|-|-|-|");
                var appliesTo = "Type";
                foreach (var st in strategy.DescriptorContextRules.GetRulesReportStructure())
                {
                    sb.AppendLine($"|{st.Detail }|{appliesTo}|{st.Description.SentenceCase() }|{(verbosity >= VerbosityLevel.Normal ? st.RuleType.NameWithGenericArguments() : string.Empty)}");
                }
            }

            static void ReportSelectCandidates(StringBuilder sb, Strategy strategy, VerbosityLevel verbosity, string typeNameHeader)
            {
                sb.Header(2, "Candidate selection");
                sb.AppendLine();
                sb.AppendLine($"|Symbol|Apply To|Rule|{(verbosity >= VerbosityLevel.Normal ? typeNameHeader : string.Empty)}");
                sb.AppendLine("|-|-|-|-|");
                var appliesTo = "Type";
                foreach (var st in strategy.GetCandidateRules.GetRulesReportStructure())
                {
                    sb.AppendLine($"|{st.SymbolName }|{appliesTo}|{st.Description.SentenceCase() }|{(verbosity >= VerbosityLevel.Normal ? st.RuleType.NameWithGenericArguments() : string.Empty)}");
                }
            }

            static void ReportClassifyCandidates(StringBuilder sb, Strategy strategy, VerbosityLevel verbosity, string typeNameHeader)
            {
                sb.Header(2, "Classify symbols");
                sb.AppendLine();
                sb.AppendLine("Candidates are first collected and then a candidate is classified...");
                sb.AppendLine();
                sb.AppendLine($"|as a|if it is a|...|{(verbosity >= VerbosityLevel.Normal ? typeNameHeader : string.Empty)}");
                sb.AppendLine("|-|-|-|-|");
                var appliesTo = "Type or Method";
                foreach (var st in strategy.SelectSymbolRules.GetRulesReportStructure())
                {
                    sb.AppendLine($"|{st.SymbolName }|{appliesTo}|{st.Description }|{(verbosity >= VerbosityLevel.Normal ? st.RuleType.NameWithGenericArguments() : string.Empty)}");
                }
            }

            static void ReportSymbolDetails(StringBuilder sb, Strategy strategy, VerbosityLevel verbosity, string typeNameHeader, SymbolType symbolType, string appliesTo, RuleSetSymbol ruleSet)
            {
                sb.Header(2, $"{symbolType} details");
                sb.AppendLine();
                sb.AppendLine($"To find each detail, use the first matching rule:");
                sb.AppendLine();

                sb.AppendLine($"|For the|if the|...|{(verbosity >= VerbosityLevel.Normal ? typeNameHeader : string.Empty)}");
                sb.AppendLine("|-|-|-|-|");
                var footnotes = new List<string>();
                foreach (var st in ruleSet.GetRulesReportStructure())
                {
                    var detail = GetFootnotes(st.Detail, footnotes);
                    sb.AppendLine($"|{detail }|{appliesTo}|{st.Description }|{(verbosity >= VerbosityLevel.Normal ? st.RuleType.NameWithGenericArguments() : string.Empty)}");
                }
                foreach (var footnote in footnotes)
                {
                    sb.AppendLine();
                    sb.AppendLine(footnote);
                }

            }

            static string GetFootnotes(string? detail, List<string> footnotes)
            {
                var stars = "";
                switch (detail)
                {
                    case "Description":
                        stars = $"({ new string('*', footnotes.Count + 1) })";
                        footnotes.Add($"{stars} The description can either come be a detail, or come from a description source.");
                        return $"Description{stars}";
                    case "Arity":
                        stars = $"({ new string('*', footnotes.Count + 1) })";
                        footnotes.Add($"{stars} Arity rarely needs to be specified because it is usually implied correctly from the argument type and whether it is Required..");
                        return $"Arity{stars}";
                    default:
                        return detail ?? string.Empty;
                };
            }

            static void ReportArgumentDetails(StringBuilder sb, Strategy strategy, VerbosityLevel verbosity, string typeNameHeader)
            {
                ReportSymbolDetails(sb, strategy, verbosity, typeNameHeader, SymbolType.Argument, "Property or Parameter", strategy.ArgumentRules);
            }

            static void ReportOptionDetails(StringBuilder sb, Strategy strategy, VerbosityLevel verbosity, string typeNameHeader)
            {
                ReportSymbolDetails(sb, strategy, verbosity, typeNameHeader, SymbolType.Option, "Property or Parameter", strategy.OptionRules);
            }

            static void ReportSubCommandDetails(StringBuilder sb, Strategy strategy, VerbosityLevel verbosity, string typeNameHeader)
            {
                ReportSymbolDetails(sb, strategy, verbosity, typeNameHeader, SymbolType.Command, "Type or Method", strategy.CommandRules);
            }
        }


        private string TextReport()
        {
            return $@"
Strategy: {Name}
   Classify symbols as:{ SelectSymbolRules.Report(2)}
   Argument details:{ ArgumentRules.Report(2)}
   Option details:{ OptionRules.Report(2)}
   Command details:{ CommandRules.Report(2)}
";
        }

        public enum ReportFormat
        {
            Text = 0,
            Markdown
        }

    }
}
