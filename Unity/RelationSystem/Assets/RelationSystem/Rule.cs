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
        public List<Rule> rulesThatMightHappen = new List<Rule>();
        float strength = 0.0f;
        public string role = "none";
        public Dictionary<Person, Person> selfOther = new Dictionary<Person,Person>();
        private RuleConditioner ruleCondition;


        public Rule(string _ruleName, MAction act, RuleConditioner _ruleCondition)
        {
            ruleName = _ruleName;
            actionToTrigger = act;
            ruleCondition = _ruleCondition;
        }


        public bool Condition(Person self, List<Person> reacters = null)
        {
            if (selfOther.ContainsKey(self)) 
                selfOther.Remove(self);
            
            List<Person> people = actionToTrigger.relationSystem.pplAndMasks.people.Values.ToList();

            if(reacters != null && reacters.Count > 0)
            {
                foreach(Person pers in people)
                {
                    if(!reacters.Contains(pers))
                    {
                        people.Remove(pers);
                    }
                }
            }

            foreach(Person other in people)
            {
                try
                {
                    if (ruleCondition(self, other))
                    {
                        selfOther.Add(self, other);

                        return true;
                    }
                }
                catch
                {
                    debug.Write("Warning: ruleCondition for " + other.name + " in " + ruleName + " returned and error. Skipping condition.");
                }
            }

            return false;
        }


		public void DoAction(Person subject, Person dirObject, Rule _rule, Person[] indPpl = null, object[] misc = null){
			actionToTrigger.DoAction (subject, dirObject,_rule, indPpl, misc);
		}

		public void SustainAction(Person subject, Person dirObject, Rule _rule, Person[] indPpl = null, object[] misc = null){
			actionToTrigger.DoSustainAction (subject, dirObject,_rule, indPpl, misc);
		}

		public float GetRuleStrength(){ return strength; }
		public void SetRuleStrength(float inp){ strength = inp; }
		public void AddToRuleStrength(float inp){ strength += inp; }
    }
}