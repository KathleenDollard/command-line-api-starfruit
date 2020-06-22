using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.Text;

namespace System.CommandLine.ReflectionAppModel
{
   public  class ReflectionCommandMaker : CommandMaker
    {

        protected ReflectionCommandMaker()
            : base(new CommandMakerSpecificSource )
        {

        }
    }
}
