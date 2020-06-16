using System;
using System.Collections.Generic;
using System.Text;

namespace System.CommandLine.GeneralAppModel.Hosting
{
    public class DescriptorMakerContext : IDescriptorMakerContext
    {
        public DescriptorMakerContext(Strategy strategy)
        {
            Strategy = strategy;
        }

        public Strategy Strategy { get; }
    }
}