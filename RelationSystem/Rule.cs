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

        public Rule(string _ruleName, MAction act, float _str, List<Rule> _rulesThatMightHappen, string _role)
        {
            ruleName = _ruleName;
            actionToTrigger = act;
            strength = _str;
            rulesThatMightHappen = _rulesThatMightHappen;
            role = _role;
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

		public void DoAction(object subject, string verb, object dirObject, object indiObject){
			if (indiObject == null) {
				if (dirObject == null) {
					actionToTrigger.DoAction (subject, " ");
				} else {
					actionToTrigger.DoAction (subject, " ", dirObject);
				}
			} else {
				actionToTrigger.DoAction(subject, " ", dirObject, indiObject);
			}
		}

    }
}