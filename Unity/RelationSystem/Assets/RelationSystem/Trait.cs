using System;


namespace NRelationSystem
{
    public class Trait
    {
        public TraitTypes name;
        float value;
            //True: The trait is relative. False: the trait is absolute.
        bool relative;

        public Trait(TraitTypes _name, float _value, bool _relative)
        {
            name = _name;
            value = _value;
            relative = _relative;
        }

		public float GetTraitValue(){ return value; }
		public void SetTraitValue(float inp){ value = inp; }
		public void AddToTraitValue(float inp){ value += inp; }

    }
}