using System;
using System.Collections.Generic;

namespace NRelationSystem
{
    public class RelationSystem
    {
        public PersonContainer peopleAndMasks = new PersonContainer();

        public Dictionary<string, MAction> posActions = new Dictionary<string, MAction>();


        public RelationSystem()
        {
            SetupActions();
            CreateFirstMasks();
            CreateFirstPeople();
        }


        void CreateFirstMasks()
        {
            List<Trait> traits = new List<Trait>();
            traits.Add(new Trait(traitTypes.NiceNasty, 0.2f, true));
            traits.Add(new Trait(traitTypes.ShyBolsterous, -0.3f, true));

            peopleAndMasks.CreateNewMask("Bungary", typeMask.culture, new Overlay(traits));

            peopleAndMasks.AddRoleToMask("Bungary", "Bunce");
            peopleAndMasks.AddRoleToMask("Bungary", "Buncess");

            traits = new List<Trait>();
            traits.Add(new Trait(traitTypes.NiceNasty, 0.2f, false));
            traits.Add(new Trait(traitTypes.ShyBolsterous, -0.3f, false));

            peopleAndMasks.CreateNewMask("Bill", typeMask.selfPerception, new Overlay(traits));


            traits = new List<Trait>();
            traits.Add(new Trait(traitTypes.NiceNasty, 0.2f, false));
            traits.Add(new Trait(traitTypes.ShyBolsterous, -0.3f, false));

            peopleAndMasks.CreateNewMask("Terese", typeMask.selfPerception, new Overlay(traits));



            traits = new List<Trait>();
            traits.Add(new Trait(traitTypes.NiceNasty, 0.2f, true));
            traits.Add(new Trait(traitTypes.ShyBolsterous, 0.2f, true));

            peopleAndMasks.CreateNewMask("BillTerese", typeMask.selfPerception, new Overlay(traits));
            peopleAndMasks.AddRuleToMask("BillTerese", "Married", 0, new Rule("Married", posActions["Greet"], 0.7f, new List<Rule>(), "Bunce"));


            traits = new List<Trait>();
            traits.Add(new Trait(traitTypes.NiceNasty, 0.2f, true));
            traits.Add(new Trait(traitTypes.ShyBolsterous, 0.2f, true));

            peopleAndMasks.CreateNewMask("TereseBill", typeMask.selfPerception, new Overlay(traits));
            peopleAndMasks.AddRuleToMask("TereseBill", "Married", 0, new Rule("Married", posActions["Greet"], 0.7f, new List<Rule>(), "Buncess"));

        }


        void CreateFirstPeople()
        {
            #region AddingBill
                Person person;

                Link selfPersMask = new Link("Self", new List<Person>(), peopleAndMasks.GetMask("Bill"), 0.4f);

                List<Link> interPerson = new List<Link>();
                
                List<Link> culture = new List<Link>();
                culture.Add(new Link("Bunce", new List<Person>(), peopleAndMasks.GetMask("Bungary"), 0.4f));

                person = new Person(selfPersMask, interPerson, culture, 0.2f, 0.2f, 0.2f);

                peopleAndMasks.CreateNewPerson("Bill", person);
            #endregion AddingBill


            #region AddingTerese
                selfPersMask = new Link("Self", new List<Person>(), peopleAndMasks.GetMask("Terese"), 0.4f);

                interPerson = new List<Link>();

                culture = new List<Link>();
                culture.Add(new Link("Buncess", new List<Person>(), peopleAndMasks.GetMask("Bungary"), 0.4f));

                person = new Person(selfPersMask, interPerson, culture, 0.2f, 0.2f, 0.2f);

                peopleAndMasks.CreateNewPerson("Terese", person);
            #endregion AddingTerese


            #region InsertInterPeople
                List<Person> peopleRelated = new List<Person>();
                peopleRelated.Add(peopleAndMasks.GetPerson("Terese"));
                peopleAndMasks.GetPerson("Bill").AddLink(typeMask.interPersonal, new Link("BillTerese", peopleRelated, peopleAndMasks.GetMask("BillTerese"), 0.4f));

                peopleRelated = new List<Person>();
                peopleRelated.Add(peopleAndMasks.GetPerson("Bill"));
                peopleAndMasks.GetPerson("Terese").AddLink(typeMask.interPersonal, new Link("TereseBill", new List<Person>(), peopleAndMasks.GetMask("TereseBill"), 0.4f));
            #endregion InsertInterPeople
        }


        void SetupActions()
        {
            AddAction(new MAction("Greet", 0.5f));
        }


        public void AddAction(MAction action) 
        {
            posActions.Add(action.name, action);
        }


        public void PrintPersonStatus()
        {
            Person personToPrint = peopleAndMasks.GetPerson("Bill");

            foreach (Link thisLink in personToPrint.GetLinks(typeMask.selfPerception))
            {
                Console.WriteLine("Bill -> Link With " + "");
            }
        }
    }
}