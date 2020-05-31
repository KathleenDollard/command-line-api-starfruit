using FluentAssertions;
using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
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
            validationException.Should().HaveFailures((DescriptorValidation.CommandNameNotNull, $"Root->Command:{DisplayFor.Name(name)}"));
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
            validationException.Should().HaveFailures((DescriptorValidation.OptionNameNotNull, $"Root->Option:{DisplayFor.Name(name)}"));
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
            var descriptor = new CommandDescriptor(SymbolDescriptor.Empty, null);
            descriptor.SubCommands.Add(new CommandDescriptor(SymbolDescriptor.Empty, null)
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
            validationException.Should().HaveFailures((DescriptorValidation.ArgumentTypeNameNotNull, $"Root->Argument:{DisplayFor.Name(null)}"));
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
                (DescriptorValidation.CommandNameNotNull, $"Root->Command:{DisplayFor.Name(null)}"),
                (DescriptorValidation.OptionNameNotNull , $"Root->Option:{DisplayFor.Name(null)}"),
                (DescriptorValidation.ArgumentTypeNameNotNull , $"Root->Argument:{DisplayFor.Name(null)}"));
        }
    }
}
