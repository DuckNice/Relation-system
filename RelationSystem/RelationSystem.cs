using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NRelationSystem
{
    public partial class RelationSystem
    {
        public PersCont pplAndMasks = new PersCont();
        public Dictionary<string, MAction> posActions = new Dictionary<string, MAction>();


        public void CreateNewMask(string nameOfMask, float[] _traits = null, bool[] relatives = null, typeMask maskType = typeMask.interPers, string[] roles = null) 
        { 
            List<Trait> traits = new List<Trait>();
            
            for(int i = 0; i < Enum.GetNames(typeof(traitTypes)).Length; i++)
            {
                float insertTrait = 0.0f;
                bool insertRelative = true;

                if(i < _traits.Length && _traits[i] >= -1.0f && _traits[i] <= 1.0f)
                    insertTrait = _traits[i];
                if(relatives.Length > i)
                    insertRelative = relatives[i];

                traits.Add(new Trait((traitTypes)i, insertTrait, insertRelative));
            }

            pplAndMasks.CreateNewMask(nameOfMask, maskType, new Overlay(traits));

            if(roles != null)
            {
                foreach(string role in roles)
                {
                    if(role != "")
                    {
                        pplAndMasks.AddRoleToMask(nameOfMask, role);
                    }
                }
            }
        }


        public void CreateNewPerson(MaskAdds selfMask, List<MaskAdds> _cults, List<MaskAdds> _intPpl, float rational, float moral, float impulse)
        {
            Link selfPersMask = new Link(selfMask.role, selfMask.linkPpl, pplAndMasks.GetMask(selfMask.mask), selfMask.levelOfInfluence);

            List<Link> cults = new List<Link>();

            foreach(MaskAdds cult in _cults)
            {
                cults.Add(new Link(cult.role, cult.linkPpl, pplAndMasks.GetMask(cult.mask), cult.levelOfInfluence));
            }

            List<Link> intPpl = new List<Link>();

            foreach(MaskAdds intPers in _intPpl)
            {
                intPpl.Add(new Link(intPers.role, intPers.linkPpl, pplAndMasks.GetMask(intPers.mask), intPers.levelOfInfluence));
            }

            Person person = new Person(selfMask.mask, selfPersMask, intPpl, cults, rational, moral, impulse);

            pplAndMasks.CreateNewPerson(selfMask.mask, person);
        }


        public void PrintPersonStatus()
        {
            Person personToPrint = pplAndMasks.GetPerson("Bill");

            foreach (Link thisLink in personToPrint.GetLinks(typeMask.selfPerc))
            {
                Console.WriteLine("Bill -> Link With " + "");
            }
        }
    }
}