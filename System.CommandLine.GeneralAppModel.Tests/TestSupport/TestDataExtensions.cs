using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.Parsing;
using System.Linq;
using System.Security.Cryptography;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public static class TestDataExtensions
    {
        public static CommandDescriptor CreateDescriptor(this CommandTestData data)
        {
            return CreateDescriptor(data, SymbolDescriptor.Empty );
        }

        public static OptionDescriptor CreateDescriptor(this OptionTestData data, ISymbolDescriptor parentSymbolDescriptor)
        {
            var option = new OptionDescriptor(SymbolDescriptor.Empty, data.Raw)
            {
                Name = data.Name,
                Description = data.Description,
                IsHidden = data.IsHidden,
                Aliases = data.Aliases ?? new List<string>(),
                Required = data.Required,
            };
            if (!(data.Arguments is null))
                {
                option.Arguments.AddRange(data.Arguments.Select(a => CreateDescriptor(a, option)));
            }

            return option;
        }

        public static ArgumentDescriptor CreateDescriptor(this ArgumentTestData data, ISymbolDescriptor parentSymbolDescriptor)
        {
            var _ = data.ArgumentType ?? throw new InvalidOperationException("ArgumentType cannot be null");
            var arg = new ArgumentDescriptor(new ArgTypeInfo(data.ArgumentType), parentSymbolDescriptor, data.Raw)
            {
                Name = data.Name,
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

        public static CommandDescriptor CreateDescriptor(this CommandTestData data, ISymbolDescriptor parentSymbolDescriptor)
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
    }


}
