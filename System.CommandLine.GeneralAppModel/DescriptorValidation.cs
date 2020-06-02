using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.CommandLine.GeneralAppModel
{
    /// <summary>
    /// The goal of Descrpitor validation is to have it be very rare that a 
    /// System.CommandLine exception is displayed to an AppModel user. This 
    /// should restate those in AppModel terms. Current form is minimal to 
    /// confirm a working system.
    /// </summary>
    /// <remarks>
    /// Use of the ValidationFailureInfo class should make localization
    /// easier and will make testing better.
    /// <br/>
    /// These rules are fixed/not flexible because they are the things that
    /// would break System.CommandLine
    /// </remarks>
    public static class DescriptorValidation
    {
        public const string CommandNameNotNull = "CommandNameNotNull";
        public const string ArgumentTypeNameNotNull = "ArgumentTypeNameNotNull";
        public const string OptionNameNotNull = "OptionNameNotNull";
        public const string DuplicateSymbolName = "DuplicateSymbolName";

        public static (bool success, IEnumerable<ValidationFailureInfo> messages) ValidateRoot(this CommandDescriptor descriptor)
        {
            var messages = new List<ValidationFailureInfo>();
            var path = "Root";
            descriptor.Validate(path, messages, true);
            return (!messages.Any(), messages);
        }

        private static IEnumerable<ValidationFailureInfo> Validate(this CommandDescriptor descriptor, string parentPath, List<ValidationFailureInfo> messages, bool isRoot)
        {
            var path = isRoot
                         ? parentPath
                         : $"{parentPath}->Command:{DisplayFor.Name(descriptor.Name)}";
            messages.AddRange(ValidateNoDuplicateNames(parentPath, descriptor));

            if (!isRoot && string.IsNullOrWhiteSpace(descriptor.Name))
            {
                messages.Add(new ValidationFailureInfo(CommandNameNotNull, path, $"Subcommand name cannot be null. ({path})"));
            }
            foreach (var argument in descriptor.Arguments)
            {
                argument.Validate(path, messages);
            }
            foreach (var option in descriptor.Options)
            {
                option.Validate(path, messages);
            }
            foreach (var subCommand in descriptor.SubCommands)
            {
                subCommand.Validate(path, messages, false);
            }
            return messages;
        }

        private static IEnumerable<ValidationFailureInfo> ValidateNoDuplicateNames(string parentPath, CommandDescriptor descriptor)
        {
            var allNames = descriptor.Options.Select(x => x.Name)
                                   .Union(descriptor.Arguments.Select(x => x.Name))
                                   .Union(descriptor.SubCommands.Select(x => x.Name))
                                   .Distinct();
            var allChildren = descriptor.Options.OfType<SymbolDescriptor>()
                                .Union(descriptor.Arguments.OfType<SymbolDescriptor>())
                                .Union(descriptor.SubCommands.OfType<SymbolDescriptor>());
            if (allNames.Count() != allChildren.Count())
            {
                return new List<ValidationFailureInfo>();
            }
            var messages = new List<ValidationFailureInfo>();
            foreach (var name in allNames)
            {
                var usages = allChildren.Where(x => x.Name == name);
                if (usages.Count() == 1)
                { continue; }
                messages.Add(new ValidationFailureInfo(DuplicateSymbolName, parentPath,
                            $"Option, Arguments, and SubCommands must have unique names. {name} was duplicated. {parentPath}"));
            }
            return messages;
        }

        private static IEnumerable<ValidationFailureInfo> Validate(this ArgumentDescriptor descriptor, string parentPath, List<ValidationFailureInfo> messages)
        {
            var path = $"{parentPath}->Argument:{DisplayFor.Name(descriptor.Name)}";
            if (string.IsNullOrWhiteSpace(descriptor.Name))
            {
                messages.Add(new ValidationFailureInfo(ArgumentTypeNameNotNull, path, $"Argument type for {descriptor.Name} cannot be null. ({path})"));
            }
            return messages;
        }

        private static IEnumerable<ValidationFailureInfo> Validate(this OptionDescriptor descriptor, string parentPath, List<ValidationFailureInfo> messages)
        {
            var path = $"{parentPath}->Option:{DisplayFor.Name(descriptor.Name)}";
            if (string.IsNullOrWhiteSpace(descriptor.Name))
            {
                messages.Add(new ValidationFailureInfo(OptionNameNotNull, path, $"Option name cannot be null. ({path})"));
            }
            foreach (var argument in descriptor.Arguments)
            {
                argument.Validate(path, messages);
            }
            return messages;
        }
    }
}