using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.Text;

namespace Playground.PotentialPatterns
{
    public class MainWithSwitch
    {
        public static int Main1(string[] args)
        {
            var typedArg =  Strategy.Standard.CreateInstance<ManageGlobalJson>(args);
            // System.CommandLine.CommandLine.CreateInstance<T>(args, [Strategy])
            return typedArg switch
            {
                ManageGlobalJson.Find x => OutputAndReturnFind(),
                ManageGlobalJson.List x => OutputAndReturnList(),
                ManageGlobalJson.Update x => x.Invoke(),
                ManageGlobalJson.Check x => OutputAndReturnCheck(),
                _ => throw new NotImplementedException(),
            };
        }

        private static int OutputAndReturnFind()
        {
            Console.WriteLine("Find");
            return 5;
        }

        private static int OutputAndReturnList()
        {
            Console.WriteLine("List");
            return 7;
        }

        private static int OutputAndReturnCheck()
        {
            Console.WriteLine("Check");
            return 9;
        }
    }
}
