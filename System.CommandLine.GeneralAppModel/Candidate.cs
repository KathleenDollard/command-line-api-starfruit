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

        public Candidate(object item, string name)
            : this (item)
        {
            Name = name;
        }

        public object Item { get; }

        public string Name { get; set; }

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
