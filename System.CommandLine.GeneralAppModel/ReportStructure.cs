using System;
using System.Collections.Generic;
using System.Text;

namespace System.CommandLine.GeneralAppModel
{
    public struct ReportStructure
    {
        public ReportStructure(string symbolName, string description, Type ruleType)
        {
            SymbolName = symbolName;
            Description = description;
            RuleType = ruleType;
        }

        public string SymbolName { get; }
        public string Description { get; }
        public Type RuleType { get; }
    }

    public struct DetailReportStructure
    {
        public DetailReportStructure(string detail, string description, Type ruleType)
        {
            Detail = detail;
            Description = description;
            RuleType = ruleType;
        }

        public string Detail { get; }
        public string Description { get; }
        public Type RuleType { get; }
    }
}
