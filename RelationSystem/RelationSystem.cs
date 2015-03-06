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


        public void CreateFirstPeople()
        {
            #region AddingBill
                MaskAdds selfPersMask = new MaskAdds("Self", "Bill", 0.4f, new List<Person>());

                List<MaskAdds> culture = new List<MaskAdds>();
                culture.Add(new MaskAdds("Bunce", "Bungary", 0.4f, new List<Person>()));

                CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.2f, 0.2f, 0.2f);
            #endregion AddingBill

            #region AddingTerese
                selfPersMask = new MaskAdds("Self", "Therese", 0.4f, new List<Person>());

                culture = new List<MaskAdds>();
                culture.Add(new MaskAdds("Buncess", "Bungary", 0.4f, new List<Person>()));

                CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.2f, 0.2f, 0.2f);
            #endregion AddingTerese

            #region AddingJohn
                selfPersMask = new MaskAdds("Self", "John", 0.4f, new List<Person>());

                culture = new List<MaskAdds>();
                culture.Add(new MaskAdds("Bunsant", "Bungary", 0.4f, new List<Person>()));

                CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.2f, 0.2f, 0.2f);
            #endregion AddingJohn


            #region InsertInterPeople
                List<Person> peopleRelated = new List<Person>();
                peopleRelated.Add(pplAndMasks.GetPerson("Therese"));
                pplAndMasks.GetPerson("Bill").AddLink(typeMask.interPers, new Link("Married", peopleRelated, pplAndMasks.GetMask("BillTherese"), 0.4f));

                peopleRelated = new List<Person>();
                peopleRelated.Add(pplAndMasks.GetPerson("Bill"));
                pplAndMasks.GetPerson("Therese").AddLink(typeMask.interPers, new Link("Married", new List<Person>(), pplAndMasks.GetMask("ThereseBill"), 0.4f));
            #endregion InsertInterPeople
        }

        public void AddLinkToPerson()
        {

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