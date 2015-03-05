using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NRelationSystem
{
    public class Trait
    {
        public traitTypes name;
        float value;
            //True: The trait is relative. False: the trait is absolute.
        bool relative;

        public Trait(traitTypes _name, float _value, bool _relative)
        {
            name = _name;
            value = _value;
            relative = _relative;
        }
    }
}