using System;
using System.Collections.Generic;
using System.CommandLine.ReflectionAppModel;
using System.Text;

namespace System.CommandLine.StarFruit
{
    public class StarFruitCommandMaker : CommandMaker
    {
        public StarFruitCommandMaker() 
            => this.UseDefaults();
    }
}
