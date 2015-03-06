using System;
using System.Collections.Generic;
using System.Linq.Expressions;

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
            CreateNewMask("Bungary", new float[]{0.2f, -0.3f}, new bool[]{false, false}, typeMask.culture, new string[]{"Bunce", "Buncess", "Bunsant"});

            List<Trait> traits;
            

            traits = new List<Trait>();
            traits.Add(new Trait(traitTypes.NiceNasty, 0.2f, false));
            traits.Add(new Trait(traitTypes.ShyBolsterous, -0.3f, false));

            peopleAndMasks.CreateNewMask("Bill", typeMask.selfPerception, new Overlay(traits));


            traits = new List<Trait>();
            traits.Add(new Trait(traitTypes.NiceNasty, 0.2f, false));
            traits.Add(new Trait(traitTypes.ShyBolsterous, -0.3f, false));

            peopleAndMasks.CreateNewMask("Therese", typeMask.selfPerception, new Overlay(traits));

            peopleAndMasks.CreateNewMask("John", typeMask.selfPerception, new Overlay(traits));


            traits = new List<Trait>();
            traits.Add(new Trait(traitTypes.NiceNasty, 0.2f, true));
            traits.Add(new Trait(traitTypes.ShyBolsterous, 0.2f, true));

            peopleAndMasks.CreateNewMask("BillTherese", typeMask.interPersonal, new Overlay(traits));
            peopleAndMasks.AddRoleToMask("BillTherese", "Married");
            peopleAndMasks.AddRuleToMask("BillTherese", "Married", peopleAndMasks.GetMaskRoleIndex("BillTherese", "Married"), new Rule("Married", posActions["Greet"], 0.7f, new List<Rule>(), "Married"));


            traits = new List<Trait>();
            traits.Add(new Trait(traitTypes.NiceNasty, 0.2f, true));
            traits.Add(new Trait(traitTypes.ShyBolsterous, 0.2f, true));

            peopleAndMasks.CreateNewMask("ThereseBill", typeMask.interPersonal, new Overlay(traits));
            peopleAndMasks.AddRoleToMask("ThereseBill", "Married");
            peopleAndMasks.AddRuleToMask("ThereseBill", "Married", peopleAndMasks.GetMaskRoleIndex("ThereseBill", "Married"), new Rule("Married", posActions["Greet"], 0.7f, new List<Rule>(), "Married"));
        }


        public void CreateNewMask(string nameOfMask, float[] _traits = null, bool[] relatives = null, typeMask maskType = typeMask.interPersonal, string[] roles = null) 
        { 
            List<Trait> traits = new List<Trait>();
            
            for(int i = 0; i < Enum.GetNames(typeof(traitTypes)).Length; i++)
            {
                float insertTrait = 0.0f;
                bool insertRelative = true;

                if(i < _traits.Length && _traits[i] >= -1.0f && _traits[i] <= 1.0f)
                    insertTrait = _traits[i];
                if(relatives.Length < i)
                    insertRelative = relatives[i];

                traits.Add(new Trait((traitTypes)i, insertTrait, insertRelative));
            }

            peopleAndMasks.CreateNewMask(nameOfMask, maskType, new Overlay(traits));

            if(roles != null)
            {
                foreach(string role in roles)
                {
                    if(role != "")
                    {
                        peopleAndMasks.AddRoleToMask(nameOfMask, role);
                    }
                }
            }
        }

        public void AddTraitToMask()
        {

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
                selfPersMask = new Link("Self", new List<Person>(), peopleAndMasks.GetMask("Therese"), 0.4f);

                interPerson = new List<Link>();

                culture = new List<Link>();
                culture.Add(new Link("Buncess", new List<Person>(), peopleAndMasks.GetMask("Bungary"), 0.4f));

                person = new Person(selfPersMask, interPerson, culture, 0.2f, 0.2f, 0.2f);

                peopleAndMasks.CreateNewPerson("Therese", person);
            #endregion AddingTerese


            #region AddingJohn
                selfPersMask = new Link("Self", new List<Person>(), peopleAndMasks.GetMask("John"), 0.4f);

                interPerson = new List<Link>();

                culture = new List<Link>();
                culture.Add(new Link("Bunsant", new List<Person>(), peopleAndMasks.GetMask("Bungary"), 0.4f));
            
                person = new Person(selfPersMask, interPerson, culture, 0.2f, 0.2f, 0.2f);

                peopleAndMasks.CreateNewPerson("John", person);
            #endregion AddingJohn


            #region InsertInterPeople
                List<Person> peopleRelated = new List<Person>();
                peopleRelated.Add(peopleAndMasks.GetPerson("Therese"));
                peopleAndMasks.GetPerson("Bill").AddLink(typeMask.interPersonal, new Link("Married", peopleRelated, peopleAndMasks.GetMask("BillTherese"), 0.4f));

                peopleRelated = new List<Person>();
                peopleRelated.Add(peopleAndMasks.GetPerson("Bill"));
                peopleAndMasks.GetPerson("Therese").AddLink(typeMask.interPersonal, new Link("Married", new List<Person>(), peopleAndMasks.GetMask("ThereseBill"), 0.4f));
            #endregion InsertInterPeople
        }


        void SetupActions()
        {
            ActionInvoker myET = (subject, verb, direct, indirect) => 
            {
                Console.WriteLine("This Is Lambda");
            };

            AddAction(new MAction("Greet", 0.5f, myET));
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