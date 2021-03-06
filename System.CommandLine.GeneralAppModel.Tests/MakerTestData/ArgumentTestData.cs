﻿using FluentAssertions;
using FluentAssertions.Execution;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Linq;
using Xunit.Sdk;

namespace System.CommandLine.GeneralAppModel.Tests.Maker
{
    public class ArgumentBasicsTestData : MakerCommandTestData
    {
        public ArgumentBasicsTestData(string name, string description, string[] aliases, bool isHidden, Type argumentType)
            : base(new CommandDescriptor(SymbolDescriptor.Empty, name, raw: typeof(ArgumentBasicsTestData)) { Name = DummyCommandName })
        {
            Descriptor.Arguments.Add(
                new ArgumentDescriptor(new ArgTypeInfo(argumentType), SymbolDescriptor.Empty, name, DummyRaw)
                {
                    Name = name,
                    CommandLineName = name,
                    Description = description,
                    Aliases = aliases,
                    IsHidden = isHidden,
                });
            Name = name;
            CommandLineName = name;
            Description = description;
            Aliases = aliases;
            IsHidden = isHidden;
            ArgumentType = argumentType;
        }

        public string Name { get; }
        public string CommandLineName { get; }
        public string Description { get; }
        public string[] Aliases { get; }
        public bool IsHidden { get; }
        public Type ArgumentType { get; }

        public override void Check(Command command)
        {
            var actual = command.Arguments.FirstOrDefault();

            using var scope = new AssertionScope();
            actual.Should().HaveName(Name)
                       .And.HaveDescription(Description)
                       .And.HaveAliases(Aliases)
                       .And.HaveIsHidden(IsHidden)
                       .And.HaveArgumentType(ArgumentType);
        }
    }

    public class ArgumentArityTestData : MakerCommandTestData
    {
        public ArgumentArityTestData(bool isSet, int? minValue = null, int? maxValue = null)
            : base(new CommandDescriptor(SymbolDescriptor.Empty, DummyCommandName, raw: typeof(ArgumentArityTestData)) { Name = DummyCommandName })
        {
            var argDescriptor = new ArgumentDescriptor(new ArgTypeInfo(typeof(string)), SymbolDescriptor.Empty, DummyArgumentName, DummyRaw)
            {
                Name = DummyArgumentName,
            };

            if (isSet)
            {
                var _ = !minValue.HasValue || !maxValue.HasValue
                            ? throw new InvalidOperationException("MinValue and MaxValue must be set when IsSet is true. For no maxValue, use Int32.Max") : 0;
                argDescriptor.Arity = new ArityDescriptor()
                {
                    MaximumCount = maxValue.Value,
                    MinimumCount = minValue.Value
                };
            }
            Descriptor.Arguments.Add(argDescriptor);
            IsSet = isSet;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public bool IsSet { get; }
        public int? MinValue { get; }
        public int? MaxValue { get; }

        public override void Check(Command command)
        {
            var actual = command.Arguments.FirstOrDefault();

            using var scope = new AssertionScope();
            actual.Should().HaveArity(IsSet, MinValue, MaxValue);
        }
    }

    public class ArgumentDefaultValueTestData : MakerCommandTestData
    {
        public ArgumentDefaultValueTestData(bool isSet, object defaultValue)
            : base(new CommandDescriptor(SymbolDescriptor.Empty, DummyCommandName, raw: typeof(ArgumentDefaultValueTestData)) { Name = DummyCommandName })
        {
            var argDescriptor = new ArgumentDescriptor(new ArgTypeInfo(typeof(string)), SymbolDescriptor.Empty, DummyArgumentName, DummyRaw)
            {
                Name = DummyArgumentName,
            };

            if (isSet)
            {
                argDescriptor.DefaultValue = new DefaultValueDescriptor(defaultValue)
                { };
            }
            Descriptor.Arguments.Add(argDescriptor);
            IsSet = isSet;
            DefaultValue = defaultValue;
        }

        public bool IsSet { get; }
        public object DefaultValue { get; }

        public override void Check(Command command)
        {
            var actual = command.Arguments.FirstOrDefault();

            using var scope = new AssertionScope();
            actual.Should().HaveDefaultValue(IsSet, DefaultValue);
        }
    }

}
