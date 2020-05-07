using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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
        public static string Description<T>(this Expression<Func<Attribute, T>> expression)
        {
            var comma = ", ";
            return expression.Body switch
            {
                UnaryExpression _ => "",
                NewExpression newExpr => $"new {newExpr.Type.Name}({string.Join(comma, GetCtorArguments(newExpr))})",
                MemberExpression member => $"attribute.{member.Member.Name}",
                _ => "<Unknown>"
            };

            static IEnumerable<string> GetCtorArguments(NewExpression newExpr)
            {
                var ret = newExpr.Arguments
                            .Select(a => a switch
                                            { 
                                                MemberExpression member => "attribute." + member.Member.Name,
                                                _ => "<Unknown>"
                                            });
                            //.Select(a => "attribute." + a.Member.Name);
                return ret;
            }
        }
    }
}
