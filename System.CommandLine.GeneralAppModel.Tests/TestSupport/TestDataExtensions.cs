using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.Parsing;
using System.Linq;
using System.Security.Cryptography;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public static class TestDataExtensions
    {
        /// <summary>
        /// To be used in MakerTests that test the translation of a 
        /// descriptor into a System.CommandLine Symbol
        /// </summary>
        /// <param name="data">Test data for the command. This is hard coded somewhere</param>
        /// <returns>A System.CommandLine command</returns>
        public static Command CreateCommand(this CommandTestData data)
        {
            var command = new Command(data.Name)
            {
                Description = data.Description,
                IsHidden = true
            };
            command.AddAliases(data.Aliases);
            if (!(data.Arguments is null))
            {
                command.AddArguments(data.Arguments.Select(x => CreateArgument(x)));
            }
            if (!(data.Options is null))
            {
                command.AddOptions(data.Options.Select(x => CreateOption(x)));
            }
            if (!(data.SubCommands is null))
            {
                command.AddCommands (data.SubCommands.Select(x => CreateCommand(x)));
            }
            return command;
       }

        /// <summary>
        /// To be used in DescriptorMakerTests that test the creation of a
        /// descriptor from raw data like reflection
        /// </summary>
        /// <param name="data">Test data for the command. This is hard coded somewhere</param>
        /// <returns>An AppModel descriptor</returns>
        public static CommandDescriptor CreateDescriptor(this CommandTestData data)
        {
            return CreateDescriptor(data, null);
        }

        public static Option CreateOption(this OptionTestData data)
        {
            var option = new Option("--" + data.Name.ToKebabCase())
            {
                Description = data.Description,
                IsHidden = data.IsHidden,
                Required = data.Required
            };
            option.AddAliases(data.Aliases);
            return option;
        }

        public static OptionDescriptor CreateDescriptor(this OptionTestData data, SymbolDescriptorBase parentSymbolDescriptor)
        {
            var option = new OptionDescriptor(null, data.Raw)
            {
                Name = data.Name,
                Description = data.Description,
                IsHidden = data.IsHidden,
                Aliases = data.Aliases ?? new List<string>(),
                Required = data.Required,
            };
            option.Arguments = data.Arguments == null
                                   ? null
                                   : data.Arguments.Select(a => CreateDescriptor(a, option));
            return option;
        }

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

        public static ArgumentDescriptor CreateDescriptor(this ArgumentTestData data, SymbolDescriptorBase parentSymbolDescriptor)
        {
            var arg = new ArgumentDescriptor(parentSymbolDescriptor, data.Raw)
            {
                Name = data.Name,
                ArgumentType = data.ArgumentType,
                Description = data.Description,
                IsHidden = data.IsHidden,
                Aliases = data.Aliases ?? new List<string>(),
            };
            if (data.HasArity)
            {
                arg.Arity = new ArityDescriptor
                {
                    MinimumCount = data.MinArityValues,
                    MaximumCount = data.MaxArityValues
                };
            }
            if (data.HasDefault)
            {
                arg.DefaultValue = new DefaultValueDescriptor(data.DefaultValue);
            }
            return arg;
        }

         public static CommandDescriptor CreateDescriptor(this CommandTestData data, SymbolDescriptorBase parentSymbolDescriptor)
        {
            var command = new CommandDescriptor(parentSymbolDescriptor, data.Raw)
            {
                Name = data.Name,
                Description = data.Description,
                IsHidden = data.IsHidden,
                Aliases = data.Aliases ?? new List<string>(),
            };
           if (!(data.Arguments is null))
                command.Arguments.AddRange(data
                                    .Arguments
                                    .Select(a => CreateDescriptor(a, command)));
            if (!(data.Options is null))
                command.Options.AddRange(data
                                    .Options
                                    .Select(a => CreateDescriptor(a, command)));
            if (!(data.SubCommands is null))
                command.SubCommands.AddRange(data
                                    .SubCommands
                                    .Select(a => CreateDescriptor(a, command)));    
            return command;
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
