using System;
using System.Collections.Generic;
using System.Linq;


namespace NRelationSystem
{
    public class Mask
    {
		string maskName;
        TypeMask maskType;
        public Dictionary<string, Rule> rules;
        public List<string> roles;
        public Overlay maskOverlay;


        public Mask(TypeMask _maskType, Overlay _maskOverlay, string _name)
        {
			maskName = _name;
            maskType = _maskType;
            roles = new List<string>();
            rules = new Dictionary<string,Rule>();
            maskOverlay = _maskOverlay;
        }


        public void AddRule(string _ruleRoleName, Rule _rule)
        {
            _ruleRoleName = _ruleRoleName.ToLower();

            rules.Add(_ruleRoleName, _rule);
        }


        public bool RemoveRule(string _ruleName)
        {
            _ruleName = _ruleName.ToLower();

            if(rules.Remove(_ruleName))
            {
                return true;
            }

            return false;
        }


        public void AddRole(string name)
        {
            name = name.ToLower();

            if(!roles.Contains(name))
            {
                roles.Add(name);
            }
        }
        

        public int FindRole(string roleName) 
        {
            roleName = roleName.ToLower();

            return roles.FindIndex(x => x == roleName);
        }
        

        public RuleAndStr CalculateActionToUse(List<MAction> notPosActions, List<PosActionItem> possibleActions, Person self, float rat, float mor, float imp, float abi, float maskInfl, List<float> foci, string role, List<Person> roleRef)
        {
            RuleAndStr chosenAction = new RuleAndStr();
			chosenAction.chosenRule = new Rule("Empty", new MAction("Empty", 0.0f, 0.0f), null, null);
            chosenAction.strOfAct = -999999999999999f;

            foreach(Rule rule in rules.Values.ToList())
            {
                List<Person> reactPeople = new List<Person>();
                
                if(possibleActions != null && possibleActions.Count > 0)
                {
                    int index = possibleActions.FindIndex(x => x.action == rule.actionToTrigger);
				
                    if (index >= 0)
                        reactPeople = possibleActions[index].reactToPerson;
                    else
                        continue;
                }

                if(roleRef != null && roleRef.Count > 0)
                {
                    foreach (Person person in roleRef)
                    {
                        reactPeople.Add(person);
                    }
                }

                bool notPosAct = false;

                if(notPosActions != null && notPosActions.Contains(rule.actionToTrigger))
                    notPosAct = true;

				if(rule.role.Equals(role)){
					//debug.Write("Checking condition "+rule.ruleName+"   "+rule.Condition(self,reactPeople));
					if(!notPosAct && rule.Condition(self, reactPeople))
					{
						debug.Write("Calculating "+rule.actionToTrigger.name+" to "+rule.selfOther[self].person.name);
				
						float maskCalculation = Calculator.CalculateRule(rat, mor, imp, abi, rule, rule.rulesThatMightHappen, maskInfl, foci);
				
						float newActionStrength = maskCalculation + Calculator.unboundAdd(rule.selfOther[self].pref,maskCalculation);
						debug.Write(maskCalculation+"  +  "+rule.selfOther[self].pref+"  =  "+newActionStrength);
						if (newActionStrength > chosenAction.strOfAct)
						{
							chosenAction.strOfAct = newActionStrength;
							chosenAction.chosenRule = rule;
						}
					}
				}
			}
			//debug.Write ("RETURNING " + chosenAction.chosenRule.ruleName);
			return chosenAction;
		}
		
		public TypeMask GetMaskType(){ return maskType; }
		public string GetMaskName(){ return maskName; }
    }
}
