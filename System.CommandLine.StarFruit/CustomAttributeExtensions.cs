using System.Linq;
using System.Reflection;

namespace System.CommandLine.StarFruit
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

        public static Range<int>
                GetArgumentCount(
                    this ICustomAttributeProvider item,
                    bool inherit = false)
        {
            var attr = item.GetCustomAttributes(typeof(CmdArgCountAttribute), inherit)
                        .OfType<CmdArgCountAttribute>()
                        .FirstOrDefault();
            
            return attr == null 
                        ? null
                        : Range.Create(attr.MinArgCount, attr.MaxArgCount);
        }

        public static Default<object> GetDefaultValue(this ICustomAttributeProvider item, bool inherit = false)
        {
            var attr = item.GetCustomAttributes(typeof(CmdDefaultValueAttribute), inherit)
                                .OfType<CmdDefaultValueAttribute>()
                                .FirstOrDefault();
            return attr == null
                      ? null
                      : Default.Create(attr.DefaultValue);
        }


        public static Range<int> GetRange<T>(
                    this ICustomAttributeProvider item,
                    bool inherit = false)
            where T : IComparable<T>
        {
            var attr = item.GetCustomAttributes(typeof(CmdRangeAttribute), inherit)
                        .OfType<CmdRangeAttribute>()
                        .FirstOrDefault();
            return attr == null
                        ? null
                        : Range.Create(attr.MinValue, attr.MaxValue);
        }

    }
}
