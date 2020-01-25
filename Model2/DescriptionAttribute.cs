using System;

namespace Model2.Args
{
    public class CmdDescriptionAttribute : Attribute
    {
        public string Description { get; }

        public CmdDescriptionAttribute(string description) 
            => Description = description;
    }
}