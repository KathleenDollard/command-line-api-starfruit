using System;
using System.CommandLine.GeneralAppModel;
using System.CommandLine.ReflectionAppModel;
using System.IO;
using UserStudyTest2;

namespace Playground
{
    public class ManageGlobalJsonProgram
    {
        // REVIEWERS Comment: 
        // I expect if we go down this route we will have only the Func passed to invoke in Main

        // REVIEWERS Issue: 
        // * I dislike the idea of using the System.CommandLine invoke on unfamiliar codebases as I think getting your bearings would be difficult.
        // * I like being able to see the entry point and have isolation here between the command line arg structure and the actual running code. 
        // * But, having a point of isolation in the Main results in either ceremony or inefficiency. 
        //   * There are 3 appraoches here. They aren't playing field because some subcommands have more options/arguments
        //   * The approach => ManageGlobalJsonImplementation.Find(find.StartPathArg, find.Verbosity) will result in a very long parameter list in the middle of Main.
        //   * The approach => MapAndRun<ManageGlobalJson.List>(list) uses reflection. System.CommandLine already used reflection to get here. I think there may be cleaner implementations.
        //   * The approach => update.Invoke() with mapping in the args puts a dependency between the args and the implmenation, and a long parameter list there.
        //   * The approach => Check(check.StartPathArg , check.Verbosity ) is pretty close to directly calling the implementation
        // So, thoughts? We either have coupling between the CLI definition and implementation, we're going to have a nasty list of paramters somewhere (or reflection). No? Where shoudl that be?
        public static int Main2(string[] args)
        {
            Strategy strategy = new Strategy("Full").SetReflectionRules();
            return strategy.Invoke((Func<ManageGlobalJson, int>)(args
                => (args switch
                    {
                        ManageGlobalJson.Find find => ManageGlobalJsonImplementation.Find(find.StartPathArg, find.Verbosity),
                        ManageGlobalJson.List list => list.MapAndRun(Utils.MethodInfo<ManageGlobalJsonImplementation>("List")),
                        ManageGlobalJson.Update update => update.Invoke(),
                        ManageGlobalJson.Check check => Check(check.StartPathArg, check.Verbosity),
                        ManageGlobalJson entry => Error("You must use a subcommand"),
                        _ => throw new InvalidOperationException("Unexpected args type")
                    })), 
                    args);
        }

        private static int Error(string message)
        {
            Console.WriteLine(message);
            return 1;
        }

        private static int Check(DirectoryInfo startPathArg, VerbosityLevel verbosity)
        {
            ManageGlobalJsonImplementation.Check(startPathArg, verbosity);
            return 0;
        }

    }
}
