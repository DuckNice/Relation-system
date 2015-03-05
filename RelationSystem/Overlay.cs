using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NRelationSystem
{
    public class Overlay
    {
        Dictionary<traitTypes, Trait> traits = new Dictionary<traitTypes, Trait>();

        public Overlay(List<Trait> _traits)
        {
            foreach(Trait trait in _traits)
            {
                traits.Add(trait.name, trait);
            }
        }
    }
}
