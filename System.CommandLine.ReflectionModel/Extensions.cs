using System.Collections.Generic;

namespace System.CommandLine.ReflectionModel
{
    public static class CoreExtensions
    {
        public static  void AddOptions(this Command command, IEnumerable<Option> options)
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
    }
}
