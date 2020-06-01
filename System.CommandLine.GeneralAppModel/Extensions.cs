using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    public static class CoreExtensions
    {
        public static void AddOptions(this Command command, IEnumerable<Option> options)
        {
            foreach (var option in options)
            {
                command.Add(option);
            }
        }

        public static void AddCommands(this Command command, IEnumerable<Command> subCommands)
        {
            foreach (var subCommand in subCommands)
            {
                command.Add(subCommand);
            }
        }

        public static void AddArguments(this Command command, IEnumerable<Argument> arguments)
        {
            foreach (var argument in arguments)
            {
                command.Add(argument);
            }
        }

        public static string NewLineWithTabs(int tabsCount)
        {
            return Environment.NewLine + new string(' ', tabsCount) + new string(' ', tabsCount) + new string(' ', tabsCount);
        }

        public static string ReportRuleGroup<TRule>(this RuleGroup<TRule> ruleGroup,int tabsCount, string what )
            where TRule :class, IRule
        {
            string whitespace = CoreExtensions.NewLineWithTabs(tabsCount);
            string indentedWhitespace = CoreExtensions.NewLineWithTabs(tabsCount + 1);
            return $"{ whitespace}To determine {what}  { string.Join("", ruleGroup.Select(r => indentedWhitespace + r.RuleDescription<TRule>() + $" ({r.GetType().Name})"))}";
        }

        public static string ProperAnOrA(this string s)
        {
            return new char[] { 'A', 'E', 'I', 'O', 'U', 'a', 'e', 'i', 'o', 'u' }.Contains(s.First())
                    ? "an " + s
                    : "a " + s;
        }
    }
}
