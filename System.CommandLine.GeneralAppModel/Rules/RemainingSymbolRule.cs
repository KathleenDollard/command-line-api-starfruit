using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel.Descriptors;
using System.Linq;
using System.Text;

namespace System.CommandLine.GeneralAppModel.Rules
{
    public class RemainingSymbolRule : RuleBase<string>
    {
        public RemainingSymbolRule(SymbolType symbolType)
            : base(symbolType)
        {
        }

        public override string RuleDescription { get; }

        public override bool HasMatch(SymbolDescriptorBase symbolDescriptor, IEnumerable<object> items)
        {
            var commandDescriptor = symbolDescriptor as CommandDescriptor;
            var used = new List<object>();
            if (SymbolType != SymbolType.Option && AnyUsed(items, commandDescriptor))
            {
                return false;
            }
            if (SymbolType != SymbolType.Option && AnyUsed(items, commandDescriptor))
            {
                return false;
            }
            if (SymbolType != SymbolType.Option && AnyUsed(items, commandDescriptor))
            {
                return false;
            }
            return true;
            static bool AnyUsed(IEnumerable<object> items, CommandDescriptor commandDescriptor)
            {
                return items.Any(item => commandDescriptor.Options
                                           .Any(x => x.Raw.Equals(x)));
            }

        }

        protected override IEnumerable<object> GetMatchingItems(SymbolDescriptorBase symbolDescriptor, IEnumerable<object> items)
        {
            var commandDescriptor = symbolDescriptor as CommandDescriptor;
            var used = new List<object>();
            if (SymbolType != SymbolType.Option)
            {
                used.AddRange(FindUsed(items, commandDescriptor));
            }
            if (SymbolType != SymbolType.Argument)
            {
                used.AddRange(FindUsed(items, commandDescriptor));
            }
            if (SymbolType != SymbolType.Command)
            {
                used.AddRange(FindUsed(items, commandDescriptor));
            }

            return items.Except(used);

            static IEnumerable<object> FindUsed(IEnumerable<object> items, CommandDescriptor commandDescriptor)
            {
                return items.Where(item => commandDescriptor.Options
                                           .Any(x => x.Raw.Equals(x)));
            }
        }
    }
}
