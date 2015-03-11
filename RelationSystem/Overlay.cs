using System;
using System.Collections.Generic;


namespace NRelationSystem
{
    public class Overlay
    {
        public Dictionary<TraitTypes, Trait> traits = new Dictionary<TraitTypes, Trait>();

        public Overlay(List<Trait> _traits)
        {
            foreach(Trait trait in _traits)
            {
                traits.Add(trait.name, trait);
            }
        }
    }
}
