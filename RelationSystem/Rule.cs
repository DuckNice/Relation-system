﻿using System;
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
        public float strength = 0.0f;
        public string role = "none";
        public Dictionary<Person, Person> selfOther = new Dictionary<Person,Person>();
        private RuleConditioner ruleCondition;


        public Rule(string _ruleName, MAction act, RuleConditioner _ruleCondition)
        {
            ruleName = _ruleName;
            actionToTrigger = act;
            ruleCondition = _ruleCondition;
        }


        public bool Condition(Person self)
        {
            if (selfOther.ContainsKey(self)) 
                selfOther.Remove(self);

            List<Person> people = actionToTrigger.relationSystem.pplAndMasks.people.Values.ToList();

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
                    Console.WriteLine("Warning: ruleCondition for " + other.name + " in " + ruleName + " returned and error. Skipping condition.");
                }
            }

            return false;
        }


		public void DoAction(Person subject, Person dirObject, Rule _rule){
			actionToTrigger.DoAction (subject, dirObject,_rule);
		}

    }
}