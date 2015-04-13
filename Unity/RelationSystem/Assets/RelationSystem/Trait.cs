using System;


namespace NRelationSystem
{
    public class Trait
    {
        public TraitTypes name;
        float value;

        public Trait(TraitTypes _name, float _value)
        {
            name = _name;
            value = _value;
        }

		public float GetTraitValue(){ return value; }
		public void SetTraitValue(float inp){ value = inp; }
		public void AddToTraitValue(float inp){ value += inp; }

    }
}