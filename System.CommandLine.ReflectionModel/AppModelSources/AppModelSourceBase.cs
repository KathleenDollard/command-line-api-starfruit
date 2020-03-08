using System;
using System.Collections.Generic;
using System.Text;

namespace System.CommandLine.ReflectionModel.AppModelSources
{
   public abstract class AppModelSourceBase
    {
        // Expected pattern: 
        //   Create new model source
        //   Setup strategies (or not for defaults)
        //   Call configure (possibly from another method) and use pattern matching
        public abstract bool Configure(Command command,
                                       object startingItem,
                                       object target = null);
    }
}
