using System;
using System.Collections.Generic;


namespace NRelationSystem
{
    public class Rule
    {
        public string ruleName;
        //	public Dictionary<string, MAction> actionsByRoles; 
        public MAction actionToTrigger;
        public List<Rule> rulesThatMightHappen;
        public float strength;
        public string role;
        public Person self;
        public Person other;
        private RuleConditioner ruleCondition;


        public Rule(string _ruleName, MAction act, float _str, List<Rule> _rulesThatMightHappen, string _role, Person _self, Person _other, RuleConditioner _ruleCondition)
        {
            ruleName = _ruleName;
            actionToTrigger = act;
            strength = _str;
            rulesThatMightHappen = _rulesThatMightHappen;
            role = _role;
            self = _self;
            other = _other;
            ruleCondition = _ruleCondition;
        }


        public bool Condition(Person pers)
        {
            return ruleCondition(pers);
        }


		public void DoAction(Person subject, Person dirObject){
			actionToTrigger.DoAction (subject, dirObject);
		}

    }
}