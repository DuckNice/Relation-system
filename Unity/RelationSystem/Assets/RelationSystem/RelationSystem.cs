using System;
using System.Collections.Generic;


namespace NRelationSystem
{
    public partial class RelationSystem
    {
        public PersCont pplAndMasks = new PersCont();
        public Dictionary<string, MAction> posActions = new Dictionary<string, MAction>();


        public List<HistoryItem> historyBook = new List<HistoryItem>();


        public void CreateNewMask(string nameOfMask, float[] _traits = null, bool[] relatives = null, TypeMask maskType = TypeMask.interPers, string[] roles = null) 
        {
            nameOfMask = nameOfMask.ToLower();

            List<Trait> traits = new List<Trait>();
            
            for(int i = 0; i < Enum.GetNames(typeof(TraitTypes)).Length; i++)
            {
                float insertTrait = 0.0f;
                bool insertRelative = true;

                if(i < _traits.Length && _traits[i] >= -1.0f && _traits[i] <= 1.0f)
                    insertTrait = _traits[i];
                if(relatives.Length > i)
                    insertRelative = relatives[i];

                traits.Add(new Trait((TraitTypes)i, insertTrait, insertRelative));
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


        public void CreateNewRule(string ruleName, string actName, RuleConditioner ruleCondition)
        {
           ruleName = ruleName.ToLower();
           actName = actName.ToLower();

           if(pplAndMasks.FindRule(ruleName) == null){
               pplAndMasks.CreateNewRule(ruleName, posActions[actName.ToLower()], ruleCondition);
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
				bool insertRelative = true;
				
				if(i < _traits.Length && _traits[i] >= -1.0f && _traits[i] <= 1.0f)
					insertTrait = _traits[i];
				
				traits.Add(new Trait((TraitTypes)i, insertTrait, insertRelative));
			}


			Dictionary<MoodTypes, float> moods = new Dictionary<MoodTypes, float>();
			
			for(int i = 0; i < Enum.GetNames(typeof(MoodTypes)).Length; i++)
			{
				float insertMood = 0.0f;

				if(_moods != null && i < _moods.Length && _moods[i] >= -1.0f && _moods[i] <= 1.0f)
					insertMood = _moods[i];
				
				moods.Add((MoodTypes)i, insertMood);
			}
			
			
			Link selfPersMask = new Link(selfMask.role, selfMask.linkPpl, pplAndMasks.GetMask(selfMask.mask), selfMask.lvlOfInfl);

            List<Link> cults = new List<Link>();

            foreach(MaskAdds cult in _cults)
            {
                cults.Add(new Link(cult.role, cult.linkPpl, pplAndMasks.GetMask(cult.mask), cult.lvlOfInfl));
            }

            List<Link> intPpl = new List<Link>();

            foreach(MaskAdds intPers in _intPpl)
            {
                intPpl.Add(new Link(intPers.role, intPers.linkPpl, pplAndMasks.GetMask(intPers.mask), intPers.lvlOfInfl));
            }

            Person person = new Person(selfMask.mask, selfPersMask, intPpl, cults, rational, moral, impulse);
			Overlay persOverlay = new Overlay (traits);
			person.absTraits = persOverlay;
			person.moods = moods;

            pplAndMasks.CreateNewPerson(selfMask.mask, person);
        }


        public void DidAction(MAction action, Person subject, Person direct, Rule _rule)
        {
			historyBook.Add(new HistoryItem(action, subject, direct, 0.0f, _rule));
		}
		
		
        public void PrintPersonStatus()
        {
            Person personToPrint = pplAndMasks.GetPerson("Bill");

            foreach (Link thisLink in personToPrint.GetLinks(TypeMask.selfPerc))
            {
                UIFunctions.WritePlayerLine("Nothing here yet.");
            }
        }
    }
}