using System;
using System.Collections.Generic;
using System.Text;

namespace System.CommandLine.GeneralAppModel.Descriptors
{
    public class ArityDescriptor
    {
        private int minimumNumberOfValues;
        private int maximumNumberOfValues;

        public bool IsSet { get; private set; }
        public int MinimumNumberOfValues
        {
            get => minimumNumberOfValues;
            set
            {
                minimumNumberOfValues = value;
                IsSet = true;
            }
        }

        public int MaximumNumberOfValues
        {
            get => maximumNumberOfValues;
            set
            {
                maximumNumberOfValues = value;
                IsSet = true;
            }
        }
    }
}
