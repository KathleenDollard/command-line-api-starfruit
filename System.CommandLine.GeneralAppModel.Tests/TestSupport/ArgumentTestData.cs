using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public class ArgumentTestData
    {
        public IEnumerable<string> Aliases { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsHidden { get; set; }
        public int MinArityValues { get; set; }
        public int MaxArityValues { get; set; }
        public bool HasArity { get; set; }
        //public ArityDescriptor Arity { get; set; }
        public HashSet<string> AllowedValues { get; } = new HashSet<string>();
        public Type ArgumentType { get; set; }
        public object DefaultValue { get; set; }
        public bool HasDefault { get; set; }

        //public DefaultValueDescriptor DefaultValue { get; set; }
        public bool Required { get; set; }
    }

    public static class ArgumentDataExtensions
    {
        public static Argument CreateArgument(this ArgumentTestData data)
        {
            var arg = new Argument(data.Name)
            {
                ArgumentType = data.ArgumentType,
                Description = data.Description,
                IsHidden = data.IsHidden
            };
            CommonTestSupport.AddAliases(arg, data.Aliases);
            if (data.HasArity)
            {
                arg.Arity = new ArgumentArity(data.MinArityValues, data.MaxArityValues);
            }
            if (data.HasDefault)
            {
                arg.SetDefaultValue(data.DefaultValue);
            }
            return arg;
        }

        public static ArgumentDescriptor CreateDescriptor(this ArgumentTestData data)
        {
            var arg = new ArgumentDescriptor
            {
                Name = data.Name,
                ArgumentType = data.ArgumentType,
                Description = data.Description,
                IsHidden = data.IsHidden,
                Aliases = data.Aliases,
            };
            if (data.HasArity)
            {
                arg.Arity = new ArityDescriptor
                {
                    MinimumNumberOfValues = data.MinArityValues,
                    MaximumNumberOfValues = data.MaxArityValues
                };
            }
            if (data.HasDefault)
            {
                arg.DefaultValue = new DefaultValueDescriptor
                {
                    DefaultValue = data.DefaultValue
                };
            }
            return arg;
        }
    }
}
