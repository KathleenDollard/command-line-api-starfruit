﻿namespace System.CommandLine.ReflectionAppModel.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class ArgumentAttribute : Attribute
    {
        private int minimumCount;
        private int maximumCount;
        private object defaultValue;

        public ArgumentAttribute()
        {
        }

        public string Description { get; set; }
        public string[] Aliases { get; set; }
        public string Name { get; set; }
        public bool IsHidden { get; set; }
        public Type ArgumentType { get; set; }
        public bool Required { get; set; }

        public bool IsAritySet { get; private set; }

        //public int MinimumValuesAllowed
        //{
        //    get => minimumCount;
        //    set
        //    {
        //        IsAritySet = true;
        //        minimumCount = value;
        //    }
        //}
        //public int MaximumValuesAllowed
        //{
        //    get => maximumCount;
        //    set
        //    {
        //        IsAritySet = true;
        //        maximumCount = value;
        //    }
        //}

        //public bool IsDefaultSet { get; private set; }

        //public object DefaultValue
        //{
        //    get => defaultValue;
        //    set
        //    {
        //        IsDefaultSet = true;
        //        defaultValue = value;
        //    }
        //}
 
    }
}