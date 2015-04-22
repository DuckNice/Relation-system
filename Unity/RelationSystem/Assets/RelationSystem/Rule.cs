using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


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
        public Dictionary<Person, PersonAndPreference> selfOther = new Dictionary<Person, PersonAndPreference>();
        private RuleConditioner ruleCondition;
        private RulePreference rulePreference;
        public VisibilityCalculator visCalc;
        
        public Rule HalfDeepCopy()
        {
            Rule other = (Rule)this.MemberwiseClone();
            other.ruleName = String.Copy(ruleName);
            other.role = String.Copy(role);
            other.selfOther = new Dictionary<Person, PersonAndPreference>();

            return other;
        }
        

        public Rule(string _ruleName, MAction act, RuleConditioner _ruleCondition, RulePreference _rulePreference, VisibilityCalculator _visCalc = null)
        {
            ruleName = _ruleName;
            actionToTrigger = act;
            ruleCondition = _ruleCondition;
            rulePreference = _rulePreference;
            if (_visCalc == null)
                visCalc = new VisibilityCalculator(x => { return 1.0f; });
            else
                visCalc = _visCalc;
        }


        public bool Condition(Person self, List<Person> reacters = null, bool reaction = false)
        {
            if (selfOther.ContainsKey(self)) 
                selfOther.Remove(self);
            
            List<Person> people = MAction.relationSystem.createActiveListsList();

            if(people.Contains(self))
                people.Remove(self);

            if (reacters != null && reacters.Count > 0)
            {
                for (int i = 0; i < people.Count; i++)
                {
                    if (!reacters.Contains(people[i]))
                    {
                        people.RemoveAt(i);
                        i--;
                    }
                }
            }
            else if(reaction)
            {
                return false;
            }

            float strength = -10.0f;
            Person personToAdd = self;

            foreach(Person other in people)
            {
                try
                {
                    if (ruleCondition == null || ruleCondition(self, other))
                    {
                        if(rulePreference != null)
                        {
                            float _strength = rulePreference(self, other);
                        
                            if(_strength > strength)
                            {
                                strength = _strength;

                                personToAdd = other;
                            }
                        }
                        else
                        {
                            selfOther.Add(self, new PersonAndPreference(other, 0.0f));
                            return true;
                        }
                    }
                }
                catch(Exception e)
                {
                    debug.Write("Warning: ruleCondition for " + other.name + " in " + ruleName + " returned and error with code: '" + e + "'. Skipping condition.");
                }
            }

            if (personToAdd != self)
            {
                selfOther.Add(self, new PersonAndPreference(personToAdd, strength));
                return true;
            }
            return false;
        }


		public void DoAction(Person subject, Person dirObject, Rule _rule, Person[] indPpl = null, object[] misc = null){
			actionToTrigger.DoAction (subject, dirObject,_rule, indPpl, misc);
		}

		public void SustainAction(Person subject, Person dirObject, Rule _rule, Person[] indPpl = null, object[] misc = null){
			actionToTrigger.DoSustainAction (subject, dirObject,_rule, indPpl, misc);
		}

        public float GetRuleStrength() { return strength; }
		public void SetRuleStrength(float inp){ strength = inp; }
		public void AddToRuleStrength(float inp){ strength += Calculator.UnboundAdd (inp, strength); }
    }
}