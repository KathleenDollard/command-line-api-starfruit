using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Data.Common;
using System.Linq;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public static class TestDataExtensions
    {
        public static Command CreateCommand(this CommandTestData data)
        {
            var command = new Command(data.Name)
            {
                Description = data.Description,
                IsHidden = true
            };
            command.AddAliases(data.Aliases);
            return command;
        }

        public static CommandDescriptor CreateDescriptor(this CommandTestData data)
        {

            var command = new CommandDescriptor(null, data.Raw)
            {
                Name = data.Name,
                Description = data.Description,
                IsHidden = data.IsHidden,
                Aliases = data.Aliases,
            };
            if (!(data.Arguments is null))
                command.Arguments.AddRange(data
                                    .Arguments
                                    .Select(a => CreateDescriptor(a)));
            return command;
        }

        public static Option CreateOption(this OptionTestData data)
        {
            var option = new Option(data.Name)
            {
                Description = data.Description,
                IsHidden = data.IsHidden,
                Required = data.Required
            };
            option.AddAliases(data.Aliases);
            return option;
        }

        public static OptionDescriptor CreateDescriptor(this OptionTestData data)
            => new OptionDescriptor(null, data.Raw)
            {
                Name = data.Name,
                Description = data.Description,
                IsHidden = data.IsHidden,
                Aliases = data.Aliases,
                Required = data.Required
            };

        public static Argument CreateArgument(this ArgumentTestData data)
        {
            var arg = new Argument(data.Name)
            {
                ArgumentType = data.ArgumentType,
                Description = data.Description,
                IsHidden = data.IsHidden
            };
            arg.AddAliases(data.Aliases);
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
            var arg = new ArgumentDescriptor(null, data.Raw)
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

        public static void AddAliases(this Symbol symbol, IEnumerable<string> aliases)
        {
            if (aliases is null)
            {
                return;
            }
            foreach (var alias in aliases)
            {
                symbol.AddAlias(alias);
            }
        }
    }


}
