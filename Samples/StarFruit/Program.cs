using System;
using System.CommandLine;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.ReflectionAppModel;
using System.Runtime.CompilerServices;

namespace StarFruit
{
    class Program
    {
        static void Main(string[] args)
        {
            var descriptor = ReflectionDescriptorMaker.RootCommandDescriptor<ManageGlobalJson>();
            var command = CommandMaker.MakeRootCommand(descriptor);
            command.Invoke(args);
        }

        static void Main1(ManageGlobalJson arg)
        {
            Host.Build(arg).Run(); // expects invocation methods are set, probably through methods or Invoke on type
        }

        static void Main2(ManageGlobalJson arg)
        {
            // Invoke only. No code ever runs here. There isn't even a Main. I think people will be confused. 
        }

        static void Main3(string[] args)
        {
            //Host.UseCommandLine(builder =>
            //{
            //    builder.UseHelp()
            //  }).Build(args).Run();
        }

        static int Main4(ManageGlobalJson arg)
        {
            return arg switch
            {
                _ => throw new NotImplementedException()
            };
        }

        private static object CustomizeBuilder()
        {
            throw new NotImplementedException();
        }
    }
}
