using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
    public class AllowedValuesAttribute : Attribute
    {
        public AllowedValuesAttribute(params object[] allowedValues)
        {
            AllowedValues = allowedValues;
        }

        public object[] AllowedValues { get; }

    }
}
