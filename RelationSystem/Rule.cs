using System;
using System.Collections.Generic;
using System.Linq;


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
        public Dictionary<Person, Person> selfOther = new Dictionary<Person,Person>();
        private RuleConditioner ruleCondition;


        public Rule(string _ruleName, MAction act, float _str, List<Rule> _rulesThatMightHappen, string _role, RuleConditioner _ruleCondition)
        {
            ruleName = _ruleName;
            actionToTrigger = act;
            strength = _str;
            rulesThatMightHappen = _rulesThatMightHappen;
            role = _role;
            ruleCondition = _ruleCondition;
        }


        public bool Condition(Person self)
        {
            if (selfOther.ContainsKey(self)) 
                selfOther.Remove(self);

            List<Person> people = actionToTrigger.relationSystem.pplAndMasks.people.Values.ToList();

            foreach(Person other in people)
            {
                if (ruleCondition(self, other))
                {
                    selfOther.Add(self, other);

                    return true;
                }
            }

            return false;
        }


		public void DoAction(Person subject, Person dirObject, Rule _rule){
			actionToTrigger.DoAction (subject, dirObject,_rule);
		}

    }
}