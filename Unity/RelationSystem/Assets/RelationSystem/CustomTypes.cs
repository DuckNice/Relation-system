using System;
using System.Collections.Generic;


namespace NRelationSystem
{
    public enum TypeMask
    {
        selfPerc,
        interPers,
        culture
    }
    

	public enum MoodTypes
	{
		hapSad,
		arousDisgus,
		angryFear,
		energTired
	}


    public struct RuleAndStr
    {
        public Rule chosenRule;
        public float strOfAct;
    };

    public struct PersonAndPreference
    {
        public Person person;
        public float pref;

        public PersonAndPreference(Person _person, float _pref)
        {
            person = _person;
            pref = _pref;
        }
    }


    public enum TraitTypes
    {
        NiceNasty,
        ShyBolsterous,
        HonestFalse
    }

	public class Opinion
	{
		public TraitTypes trait;
		public Person pers;
		public float value;

		public Opinion(TraitTypes _trait,Person _person, float _val){
			trait = _trait;
			pers = _person;
			value = _val;
		}
	};


    public struct MaskAdds
    {
        private string Role;
        public string role { get { return Role; } set { Role = value.ToLower(); } }
        private string Mask;
        public string mask { get { return Mask; } set { Mask = value.ToLower(); } }
        public float genlvlOfInfl;
        public float lvlOfInfl;
        public Person linkPers;

        public MaskAdds(string _role, string _mask, float _genLvlOIf, float _lvlOIf = 0, Person _linkPers = null)
        {
            Role = _role.ToLower();
            Mask = _mask.ToLower();
            lvlOfInfl = _lvlOIf;
            genlvlOfInfl = _genLvlOIf;
            linkPers = _linkPers;
        }
    };


    public class PosActionItem
    {
        public MAction action;
        public List<Person> reactToPerson;

        public PosActionItem(MAction _act, Person _rTp)
        {
            action = _act;
            reactToPerson = new List<Person>();
            reactToPerson.Add(_rTp);
        }

        public PosActionItem(MAction _act, List<Person> _rTp)
        {
            action = _act;
            reactToPerson = _rTp;
        }
    }


    public struct HistoryItem
    {
        private MAction action;
        private Person subject;
        private Person direct;
        private float time;
		private Rule rule;
        private List<Person> peopleReacted;

        public HistoryItem(MAction _action, Person _subject, Person _direct, float _time, Rule _rule)
        {
            action = _action;
            subject = _subject;
            direct = _direct;
            time = _time;
			rule = _rule;
            peopleReacted = new List<Person>();
        }

        public MAction GetAction() { return action; }
        public Person GetSubject() { return subject; }
        public Person GetDirect() { return direct; }
        public float GetTime() { return time; }
		public Rule GetRule() { return rule; }
        public bool HasReacted( Person person ) { if(peopleReacted.Contains( person )){ return true; } return false; }
        public void SetReacted( Person person ) { peopleReacted.Add( person ); }
    };


    public delegate void ActionInvoker(Person subject, Person direct, Person[] indiPpl = null, object[] misc = null);

	public delegate bool RuleConditioner(Person self, Person other, Person[] indiPpl = null);

    public delegate float RulePreference(Person self, Person other);

    public delegate float VisibilityCalculator(object[] misc = null);
}