using System;
using System.Collections.Generic;
using System.Text;

namespace Playground.PotentialPatterns
{
    public class MainWithSwitchDragonFruit
    {
        public static int Main1(ManageGlobalJson arg)
        {
            return arg switch
            {
                ManageGlobalJson.Find x => OutputAndReturn(x),
                ManageGlobalJson.List x => OutputAndReturn(x),
                ManageGlobalJson.Update x => x.Invoke(),
                ManageGlobalJson.Check x => OutputAndReturn(x),
                _ => throw new NotImplementedException(),
            };
        }

        private static int OutputAndReturn(object x)
        {
            Console.WriteLine(x.GetType().Name);
            return 7;
        }
    }
}
