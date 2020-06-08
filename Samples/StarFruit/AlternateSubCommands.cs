using System;
using System.Collections.Generic;
using System.Text;

namespace Playground
{
    public class RootCommand
    {

    }

    public class AlternateSubCommand : RootCommand
    {
        public int Invoke(string p1, int p2)
        {
            // stuff
            return 7;
        }
    }

    public class OtherSubCommands : RootCommand
    {
        public int Invoke(string p1)
        {
            // stuff
            return 7;
        }
    }
}
