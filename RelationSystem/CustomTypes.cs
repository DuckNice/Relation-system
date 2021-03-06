﻿using System;
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
        angryfear,
        energTired
    }
    

    public struct RuleAndStr
    {
        public Rule chosenRule;
        public float strOfAct;
    };


    public enum TraitTypes
    {
        NiceNasty,
        ShyBolsterous,
        HonestFalse
    }


    public struct MaskAdds
    {
        public string role;
        public string mask;
        public float lvlOfInfl;
        public List<Person> linkPpl;

        public MaskAdds(string _role, string _mask, float _lvlOIf, List<Person> _linkPeople)
        {
            role = _role;
            mask = _mask;
            lvlOfInfl = _lvlOIf;
            linkPpl = _linkPeople;
        }
    };


    public struct HistoryItem
    {
        private MAction action;
        private Person subject;
        private Person direct;
        private float time;
		private Rule rule;

        public HistoryItem(MAction _action, Person _subject, Person _direct, float _time, Rule _rule)
        {
            action = _action;
            subject = _subject;
            direct = _direct;
            time = _time;
			rule = _rule;
        }

        public MAction GetAction() { return action; }
        public Person GetSubject() { return subject; }
        public Person GetDirect() { return direct; }
        public float GetTime() { return time; }
		public Rule GetRule() { return rule; }
    };


    public delegate void ActionInvoker(Person subject, Person direct);

	public delegate bool RuleConditioner(Person self, Person other);
}
