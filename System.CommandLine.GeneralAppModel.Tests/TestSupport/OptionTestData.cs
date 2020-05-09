using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Linq;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public class OptionTestData
    {
        public IEnumerable<string> Aliases { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsHidden { get; set; }
        public bool Required { get; set; }
    }

    public static class OptionDataExtensions
    {
        public static Option CreateOption(this OptionTestData data)
        {
            var option = new Option(data.Name)
            {
                Description = data.Description,
                IsHidden = data.IsHidden,
                Required = data.Required
            };
            foreach (var alias in data.Aliases)
            {
                option.AddAlias(alias);
            }
            return option;
        }

        public static OptionDescriptor CreateDescriptor(this OptionTestData data) 
        => new OptionDescriptor
        {
            Name = data.Name,
            Description = data.Description,
            IsHidden = data.IsHidden,
            Aliases = data.Aliases,
            Required = data.Required
        };
    }

}
