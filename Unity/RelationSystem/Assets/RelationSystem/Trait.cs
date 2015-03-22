using System;


namespace NRelationSystem
{
    public class Trait
    {
        public TraitTypes name;
        public float value;
            //True: The trait is relative. False: the trait is absolute.
        bool relative;

        public Trait(TraitTypes _name, float _value, bool _relative)
        {
            name = _name;
            value = _value;
            relative = _relative;
        }
    }
}