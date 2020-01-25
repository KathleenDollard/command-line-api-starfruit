using Model2.Args;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Model2
{
    public static class CustomAttributeExtensions
    {
       public static string GetDescription(this ICustomAttributeProvider item, bool inherit = false)
        {
            var descAttribute = item.GetCustomAttributes(typeof(CmdDescriptionAttribute), inherit)
                                .OfType<CmdDescriptionAttribute>()
                                .FirstOrDefault();
            return descAttribute?.Description;
        }

        public static (int minCount, int maxCount) 
                GetArgumentValues(
                    this ICustomAttributeProvider item, 
                    bool inherit = false)
        {
            var attr = item.GetCustomAttributes(typeof(CmdArgumentAttribute), inherit)
                        .OfType<CmdArgumentAttribute>()
                        .FirstOrDefault();
            return (attr.MinArgCount, attr.MaxArgCount);
        }

        public static object GetDefaultValue(this ICustomAttributeProvider item, bool inherit = false)
        {
            var attr = item.GetCustomAttributes(typeof(CmdDefaultValue), inherit)
                                .OfType<CmdDefaultValue>()
                                .FirstOrDefault();
            return attr?.DefaultValue;
        }
    }
}
