using System.Collections.Generic;
using System.Linq;

namespace System.CommandLine.GeneralAppModel
{
    public class Candidate
    {
        private readonly List<object> traits = new List<object>();

        public Candidate(object item)
        {
            Item = item;
        }

        public object Item { get; }

        public string Identity
        {
            get
            {
                var wrapper = traits
                        .OfType<IdentityWrapper>()
                        .FirstOrDefault();
                return wrapper switch
                {
                    null => string.Empty,
                    IdentityWrapper<string> w => w.Value,
                    _ => (wrapper.ValueAsObject ?? string.Empty).ToString()
                };
            }
        }

        public IEnumerable<object> Traits => traits;

        public void AddTrait(object trait)
        {
            traits.Add(trait);
        }

        public void AddTraitRange(IEnumerable<object> traits)
        {
            foreach (var trait in traits)
            {
                AddTrait(trait);
            }
        }

    }
}
