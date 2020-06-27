using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Linq;
using System.Reflection;

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

        public static string ReportRuleGroup<TRule>(this RuleGroup<TRule> ruleGroup, int tabsCount, string what)
            where TRule : class, IRule
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

        public static void Assert(this List<ValidationFailureInfo> messages, bool isOK, string id, string path, string message)
        {
            if (!isOK)
            {
                messages.Add(new ValidationFailureInfo(id, path, message));
            }
        }

        public static SymbolDescriptor? GetOptionOrArgument(this CommandDescriptor commandDescriptor, PropertyInfo propertyInfo)
        {
            SymbolDescriptor symbol = commandDescriptor.Options
                                        .Where(x => ((PropertyInfo?)x.Raw)?.Name == propertyInfo.Name)
                                        .FirstOrDefault();
            if (!(symbol is null))
            {
                return symbol;
            }
            symbol = commandDescriptor.Arguments
                           .Where(x => ((PropertyInfo?)x.Raw)?.Name == propertyInfo.Name)
                           .FirstOrDefault();
            if (!(symbol is null))
            {
                return symbol;
            }
            if (commandDescriptor.ParentSymbolDescriptorBase is CommandDescriptor parentDescriptor)
            {
                return GetOptionOrArgument(parentDescriptor, propertyInfo);
            }
            return null;
        }
    }
}
