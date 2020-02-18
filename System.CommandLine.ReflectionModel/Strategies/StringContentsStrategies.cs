using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine.ReflectionModel.Strategies
{

    public class StringContentsStrategy : StrategyBase
    {
        // TODO: Extend strategy replacement for Func to other strategy types
        // TODO: Should all strategies have a symbol type filter
        public StringContentsStrategy(StringPosition position,
                                      string compareTo,
                                      SymbolType symbolType = SymbolType.All)
        : base(symbolType)
        {
            Position = position;
            CompareTo = compareTo;
        }

        public StringPosition Position { get; }
        public string CompareTo { get; }

        public bool IsFound(string s)
        {
            if (s is null)
            {
                return false;
            }
            return Position switch
            {
                StringPosition.Prefix => s.StartsWith(CompareTo),
                StringPosition.Suffix => s.EndsWith(CompareTo),
                StringPosition.Contains => s.Contains(CompareTo),
                _ => throw new ArgumentException("Unexpected position")
            };
        }

        public enum StringPosition
        {
            Prefix = 1,
            Suffix,
            Contains
        }
        public override string Description
            => $"String Contents: {CompareTo} as {Position}";
    }

    public class StringContentStrategies
    {
        private readonly List<StringContentsStrategy> stringStrategies = new List<StringContentsStrategy>();

        protected IEnumerable<StringContentsStrategy> StringContentsStrategy
            => stringStrategies;
        public void AddInternal(StringContentsStrategy strategy)
            => stringStrategies.Add((strategy));

        public void Add(StringContentsStrategy.StringPosition position, string compareTo)
             => AddInternal(new StringContentsStrategy(position, compareTo));

        public bool AreAnyFound(string stringToMatch, SymbolType symbolType)
            => stringStrategies
                        .Where(s => (s.SymbolType & symbolType) != 0)
                        .Any(s => s.IsFound(stringToMatch));

    }
}
