using System.Linq;
using System.Reflection;

namespace System.CommandLine.ReflectionAppModel
{
    public static class CustomAttributeExtensions
    {
        public static T GetCustomAttribute<T>(this ICustomAttributeProvider item, bool inherit = false)
            where T : Attribute
            => item.GetCustomAttributes(typeof(T), inherit)
                     .OfType<T>()
                     .FirstOrDefault();

        public static Range<int> GetRange<T>(
                    this ICustomAttributeProvider item,
                    bool inherit = false)
            where T : IComparable<T>
        {
            CmdRangeAttribute attr = item.GetCustomAttributes(typeof(CmdRangeAttribute), inherit)
                        .OfType<CmdRangeAttribute>()
                        .FirstOrDefault();
            return attr == null
                        ? null
                        : Range.Create(attr.MinValue, attr.MaxValue);
        }

    }
}
