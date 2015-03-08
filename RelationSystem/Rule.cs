using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ConsoleApplication2;

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

        public Rule(string _ruleName, MAction act, float _str, List<Rule> _rulesThatMightHappen, string _role, Person _self, Person _other)
        {
            ruleName = _ruleName;
            actionToTrigger = act;
            strength = _str;
            rulesThatMightHappen = _rulesThatMightHappen;
            role = _role;
            self = _self;
            other = _other;
        }

        public bool RoleTest(string roleToTest)
        { //Tests if the rule belongs to the role of the character. input person role as argument.
            if (roleToTest == role)
            {
                Console.WriteLine("RightRole");

                return true;
            }
            else
            {
                Console.WriteLine("WrongRole");
                return false;
            }
        }

        public bool Condition()
        { // Tests if rule conditions are fulfilled. Make delegates.
            //IFCONDITION IS FULFILLED 
            Console.WriteLine("ConditionTrue");
            return true;
        }

		public void DoAction(Person subject, Person dirObject){
			actionToTrigger.DoAction (subject, dirObject);
		}

    }
}