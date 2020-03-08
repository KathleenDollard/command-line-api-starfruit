using System.CommandLine.ReflectionModel.ModelStrategies;

namespace System.CommandLine.ReflectionModel.Strategies
{
    public class StrategiesSet
    {
        public ArgumentStrategies ArgumentStrategies { get; } = new ArgumentStrategies();
        public CommandStrategies CommandStrategies { get; } = new CommandStrategies();
        public SubCommandStrategies SubCommandStrategies { get; } = new SubCommandStrategies();
        public ArityStrategies ArityStrategies { get; } = new ArityStrategies();
        public DescriptionStrategies DescriptionStrategies { get; } = new DescriptionStrategies();
        public NameStrategies NameStrategies { get; } = new NameStrategies();
        public IsRequiredStrategies RequiredStrategies { get; } = new IsRequiredStrategies();

        public string StrategiesSetDescription
        {
            get
            {
                var newLine = "\r\n       ";
                return $@"
AppModel:
   IsArgumentStrategies: 
       {string.Join(newLine, ArgumentStrategies.StrategyDescriptions)}
   IsCommandStrategies:
       {string.Join(newLine, CommandStrategies.StrategyDescriptions)}
   SubCommandStrategies:
       {string.Join(newLine, SubCommandStrategies.StrategyDescriptions)}
   ArityStrategies:
       {string.Join(newLine, ArityStrategies.StrategyDescriptions)}
   DescriptionStrategies:
       {string.Join(newLine, DescriptionStrategies.StrategyDescriptions)}
   NameStrategies:
       {string.Join(newLine, NameStrategies.StrategyDescriptions)}
   IsRequiredStrategies:
       {string.Join(newLine, RequiredStrategies.StrategyDescriptions)}";
            }
        }
    }
}
