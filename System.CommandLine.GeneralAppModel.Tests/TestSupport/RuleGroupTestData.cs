using System;
using System.Collections.Generic;
using System.Text;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public class RuleGroupTestData
    {
        public SymbolType SymbolType { get; set; }
        public List<RuleBaseTestData> Rules {get; set;}

    }

    public class RuleBaseTestData
    {
    }

    public class NamedAttributeTestData:RuleBaseTestData
    {
        public NamedAttributeTestData(string attributeName)
        {
            AttributeName = attributeName;
        }
        public string AttributeName { get; set; }

 
    }

    public class NamedAttributeWithPropertyTestData : NamedAttributeTestData
    {
       public NamedAttributeWithPropertyTestData(string attributeName, string propertyName, Type type) : base(attributeName)
        {
            PropertyName = propertyName;
            Type = type;
        }  
        
        public string PropertyName { get;  }
        public Type Type { get; }
    }

    public class NamePatternTestData : RuleBaseTestData
    {
        public string CompareTo { get; set; }

        public NamePatternTestData(StringContentsRule.StringPosition position, string compareTo)
        {
            CompareTo = compareTo;
            Position = position;
        }

        public StringContentsRule.StringPosition Position  { get; set; }
    }
}
