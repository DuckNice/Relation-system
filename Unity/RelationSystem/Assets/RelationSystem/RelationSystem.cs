using System;
using System.Collections.Generic;
using System.Linq;


namespace NRelationSystem
{
    public partial class RelationSystem
    {
        public PersCont pplAndMasks = new PersCont();
        public Dictionary<string, MAction> posActions = new Dictionary<string, MAction>();
        public Dictionary<string, List<Person>> updateLists = new Dictionary<string, List<Person>>();
        public Dictionary<string, List<Person>> activeLists = new Dictionary<string, List<Person>>();
        public List<HistoryItem> historyBook = new List<HistoryItem>();
        public static Program program;
        public static string playerName = "you";

        public List<Person> GetAllPeople()
        {
            return pplAndMasks.people.Values.ToList();
        }

        public bool HistoryBookContains(MAction action) 
        {
            return historyBook.Exists(x => x.GetAction() == action);
        }


        public List<Person> CreateActiveListsList()
        {
            List<Person> list = new List<Person>();

            foreach(List<Person> people in activeLists.Values)
            {
                foreach (Person person in people)
                {
                    if (!list.Contains(person))
                    {
                        list.Add(person);
                    }
                }
            }

            return list;
        }


        public List<string> CreateActiveListsListNames()
        {
            List<string> list = new List<string>();

            foreach (List<Person> people in activeLists.Values)
            {
                foreach (Person person in people)
                {
                    if (!list.Contains(person.name))
                    {
                        list.Add(person.name);
                    }
                }
            }

            return list;
        }


        public void CreateNewMask(string nameOfMask, float[] _traits = null, TypeMask maskType = TypeMask.interPers, string[] roles = null) 
        {
            nameOfMask = nameOfMask.ToLower();

            List<Trait> traits = new List<Trait>();
            
            for(int i = 0; i < Enum.GetNames(typeof(TraitTypes)).Length; i++)
            {
                float insertTrait = 0.0f;

                if(i < _traits.Length && _traits[i] >= -1.0f && _traits[i] <= 1.0f)
                    insertTrait = _traits[i];

                traits.Add(new Trait((TraitTypes)i, insertTrait));
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


        public void CreateNewRule(string ruleName, string actName, RuleConditioner ruleCondition = null, RulePreference rulePreference = null, VisibilityCalculator visCalc = null)
        {
           ruleName = ruleName.ToLower();
           actName = actName.ToLower();

           if(pplAndMasks.FindRule(ruleName) == null){
               pplAndMasks.CreateNewRule(ruleName, posActions[actName.ToLower()], ruleCondition, rulePreference, visCalc);
           }
           else
           {
               debug.Write("Warning: Rule with name '" + ruleName + "' Already exists. Not adding rule.");
           }
        }


		public void CreateNewPerson(MaskAdds selfMask, List<MaskAdds> _cults, List<MaskAdds> _intPpl, float rational, float moral, float impulse, float[] _traits = null, float[] _moods = null)
		{
			List<Trait> traits = new List<Trait>();
			
			for(int i = 0; i < Enum.GetNames(typeof(TraitTypes)).Length; i++)
			{
				float insertTrait = 0.0f;
				
				if(i < _traits.Length && _traits[i] >= -1.0f && _traits[i] <= 1.0f)
					insertTrait = _traits[i];
				
				traits.Add(new Trait((TraitTypes)i, insertTrait));
			}


			Dictionary<MoodTypes, float> moods = new Dictionary<MoodTypes, float>();
			
			for(int i = 0; i < Enum.GetNames(typeof(MoodTypes)).Length; i++)
			{
				float insertMood = 0.0f;

				if(_moods != null && i < _moods.Length && _moods[i] >= -1.0f && _moods[i] <= 1.0f)
					insertMood = _moods[i];
				
				moods.Add((MoodTypes)i, insertMood);
			}


            Link selfPersMask = new Link(selfMask.role, pplAndMasks.GetMask(selfMask.mask), selfMask.lvlOfInfl);

            List<Link> cults = new List<Link>();

            foreach(MaskAdds cult in _cults)
            {
				cults.Add(new Link(cult.role, pplAndMasks.GetMask(cult.mask), cult.lvlOfInfl));
            }

            List<Link> intPpl = new List<Link>();

            foreach(MaskAdds intPers in _intPpl)
            {
                intPpl.Add(new Link(intPers.role, pplAndMasks.GetMask(intPers.mask), intPers.lvlOfInfl));
            }

            Person person = new Person(selfMask.mask, selfPersMask, intPpl, cults, rational, moral, impulse, this);
			Overlay persOverlay = new Overlay (traits);
			person.absTraits = persOverlay;
			person.moods = moods;

            pplAndMasks.CreateNewPerson(selfMask.mask, person);
        }


        public void DidAction(MAction action, Person subject, Person direct, Rule _rule)
        {
			historyBook.Add(new HistoryItem(action, subject, direct, program.time, _rule));
		}


		public string CapitalizeName(string s){
			if (String.IsNullOrEmpty(s))
				throw new ArgumentException("ARGH!");
			return s.First().ToString().ToUpper() + s.Substring(1);
		}


        public Person GetPerson(string name)
        {
            return pplAndMasks.GetPerson(name);
        }
    }
}