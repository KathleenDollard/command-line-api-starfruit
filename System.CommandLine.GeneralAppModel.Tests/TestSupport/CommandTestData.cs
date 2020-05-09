using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;

namespace System.CommandLine.GeneralAppModel.Tests
{
    public class CommandTestData
    {
        public IEnumerable<string> Aliases { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsHidden { get; set; }
        public bool TreatUnmatchedTokensAsErrors { get; set; } = true;

        public object CreateModel()
        {
            throw new NotImplementedException();
        }
    }

    public static class CommandDataExtensions
    {
        public static Command CreateCommand(this CommandTestData data)
        {
            var command = new Command(data.Name)
            {
                Description = data.Description ,
                IsHidden = true
            };
            CommonTestSupport.AddAliases(command, data.Aliases);
            return command;
        }

        public static CommandDescriptor CreateDescriptor(this CommandTestData data)
        {

            var command = new CommandDescriptor
            {
                Name = data.Name,
                Description = data.Description,
                IsHidden = data.IsHidden,
                Aliases = data.Aliases,
                TreatUnmatchedTokensAsErrors = data.TreatUnmatchedTokensAsErrors
            };
            return command;
        }
    }
}
