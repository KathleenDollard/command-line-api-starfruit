using FluentAssertions;
using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.CommandLine.GeneralAppModel.Tests.Maker;
using System.Text;
using Xunit;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public class DescriptorValidationTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("\t")]
        public void DescriptorValidationThrowsIfSubCommandNameNullOrWhitespace(string? name)
        {
            var descriptor = new CommandDescriptor(SymbolDescriptor.Empty, null);
            descriptor.SubCommands.Add(new CommandDescriptor(SymbolDescriptor.Empty, null)
            { Name = name });

            var ex = Assert.Throws<InvalidOperationException>(() => CommandMaker.MakeRootCommand(descriptor));
            ex.InnerException.Should().BeOfType<DescriptorInvalidException>();
            var validationException = ex.InnerException as DescriptorInvalidException;
            var _ = validationException ?? throw new InvalidOperationException("Unexpected exception type");
            validationException.Should().HaveFailures((DescriptorValidation.CommandNameNotEmpty, $"Root->Command:{DisplayFor.Name(name)}"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("\t")]
        public void DescriptorValidationThrowsIfOptionNameNullOrWhitespace(string? name)
        {
            var descriptor = new CommandDescriptor(SymbolDescriptor.Empty, null);
            descriptor.Options.Add(new OptionDescriptor(SymbolDescriptor.Empty, null)
            { Name = name });

            var ex = Assert.Throws<InvalidOperationException>(() => CommandMaker.MakeRootCommand(descriptor));
            ex.InnerException.Should().BeOfType<DescriptorInvalidException>();
            var validationException = ex.InnerException as DescriptorInvalidException;
            var _ = validationException ?? throw new InvalidOperationException("Unexpected exception type");
            validationException.Should().HaveFailures((DescriptorValidation.OptionNameNotEmpty, $"Root->Option:{DisplayFor.Name(name)}"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("\t")]
        public void DescriptorValidationDoesNotThrowForEmptyRootName(string? name)
        {
            var descriptor = new CommandDescriptor(SymbolDescriptor.Empty, null)
            { Name = name };

            var root = CommandMaker.MakeRootCommand(descriptor);
            root.Should().NotBeNull(); // Just be sure there is no exception
        }

        [Theory]
        [InlineData("Fred")]
        [InlineData("george")]
        public void DescriptorValidationDoesNotThrowForNotEmptySubCommandName(string? name)
        {
            var descriptor = new CommandDescriptor(SymbolDescriptor.Empty, typeof(CommandBasicsTestData));
            descriptor.SubCommands.Add(new CommandDescriptor(SymbolDescriptor.Empty, typeof(CommandBasicsTestData))
            {
                Name = name
            });

            var root = CommandMaker.MakeRootCommand(descriptor);
            root.Should().NotBeNull(); // Just be sure there is no exception
        }

        [Theory]
        [InlineData("Fred")]
        [InlineData("george")]
        public void DescriptorValidationDoesNotThrowForNotEmptyOptionName(string? name)
        {
            var descriptor = new CommandDescriptor(SymbolDescriptor.Empty, null);
            descriptor.Options.Add(new OptionDescriptor(SymbolDescriptor.Empty, null)
            {
                Name = name
            });

            var root = CommandMaker.MakeRootCommand(descriptor);
            root.Should().NotBeNull(); // Just be sure there is no exception
        }

        [Fact]
        public void DescriptorValidationThrowsIfArgumentTypeIsNull()
        {
            var descriptor = new CommandDescriptor(SymbolDescriptor.Empty, null);
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            // DO NOT FIX THE NULLABLE IN THE NEXT LINE
            descriptor.Arguments.Add(new ArgumentDescriptor(null, SymbolDescriptor.Empty, null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            var ex = Assert.Throws<InvalidOperationException>(() => CommandMaker.MakeRootCommand(descriptor));
            ex.InnerException.Should().BeOfType<DescriptorInvalidException>();
            var validationException = ex.InnerException as DescriptorInvalidException;
            var _ = validationException ?? throw new InvalidOperationException("Unexpected exception type");
            validationException.Should().HaveFailures((DescriptorValidation.ArgumentTypeNotNull, $"Root->Argument:{DisplayFor.Name(null)}"));
        }

        [Fact]
        public void DescriptorValidationCanReportMultipleIssues()
        {
            var descriptor = new CommandDescriptor(SymbolDescriptor.Empty, null);
            descriptor.Options.Add(new OptionDescriptor(SymbolDescriptor.Empty, null)
            { Name = null });
            descriptor.SubCommands.Add(new CommandDescriptor(SymbolDescriptor.Empty, null)
            { Name = null });
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            // DO NOT FIX THE NULLABLE IN THE NEXT LINE
            descriptor.Arguments.Add(new ArgumentDescriptor(null, SymbolDescriptor.Empty, null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            var ex = Assert.Throws<InvalidOperationException>(() => CommandMaker.MakeRootCommand(descriptor));
            ex.InnerException.Should().BeOfType<DescriptorInvalidException>();
            var validationException = ex.InnerException as DescriptorInvalidException;
            var _ = validationException ?? throw new InvalidOperationException("Unexpected exception type");
            validationException.Should().HaveFailures(
                (DescriptorValidation.CommandNameNotEmpty, $"Root->Command:{DisplayFor.Name(null)}"),
                (DescriptorValidation.OptionNameNotEmpty, $"Root->Option:{DisplayFor.Name(null)}"),
                (DescriptorValidation.ArgumentTypeNotNull , $"Root->Argument:{DisplayFor.Name(null)}"));
        }

        [Fact]
        public void DescriptorValidationThrowsIfALlowedValuesOfWrongType()
        {
            var descriptor = new CommandDescriptor(SymbolDescriptor.Empty, null);
            // ArgumentType is int, value is string
            var argumentDescriptor = new ArgumentDescriptor(new ArgTypeInfo(typeof(int)), SymbolDescriptor.Empty, null);
            argumentDescriptor.AllowedValues.Add("Fred");
            descriptor.Arguments.Add(argumentDescriptor);


            var ex = Assert.Throws<InvalidOperationException>(() => CommandMaker.MakeRootCommand(descriptor));
            ex.InnerException.Should().BeOfType<DescriptorInvalidException>();
            var validationException = ex.InnerException as DescriptorInvalidException;
            var _ = validationException ?? throw new InvalidOperationException("Unexpected exception type");
            validationException.Should().HaveFailures((DescriptorValidation.AllowedValuesNotOfCorrectType, $"Root->Argument:{DisplayFor.Name(null)}"));
        }
    }
}
