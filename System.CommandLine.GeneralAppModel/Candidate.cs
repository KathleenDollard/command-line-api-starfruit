using System.Collections.Generic;

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
