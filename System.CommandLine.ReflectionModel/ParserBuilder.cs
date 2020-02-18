using System.CommandLine.Builder;
using System.Reflection;

namespace System.CommandLine.ReflectionModel
{
    public static class ParserBuilder
    {

        public static void ConfigureFromMethod(
                             this CommandLineBuilder builder,
                             MethodInfo method,
                             object target = null)
        {
            _ = builder ?? throw new ArgumentNullException(nameof(builder));
            _ = method ?? throw new ArgumentNullException(nameof(method));

            var commandMaker = new CommandMaker();
            commandMaker.UseDefaults();
            commandMaker.Configure(builder.Command, method);
        }
    }
}
