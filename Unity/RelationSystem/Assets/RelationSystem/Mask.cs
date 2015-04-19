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

            rules.Add(_ruleRoleName, _rule.HalfDeepCopy());
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
        

        public RuleAndStr CalculateActionToUse(List<MAction> notPosActions, List<PosActionItem> possibleActions, Person self, float rat, float mor, float imp, float abi, string role, float genLvlOfInfl, Dictionary<Person, float> roleRef)
        {
            RuleAndStr chosenRule = new RuleAndStr();
			chosenRule.chosenRule = new Rule("Empty", new MAction("Empty", 0.0f, 0.0f), null, null);
            chosenRule.strOfAct = -999999999999999f;

            foreach(Rule rule in rules.Values.ToList())
            {
                if (notPosActions != null && notPosActions.Contains(rule.actionToTrigger))
                    continue;

                List<Person> reactPeople = new List<Person>();

                bool reaction = false;
                if(possibleActions != null && possibleActions.Count > 0)
                {
                    int index = possibleActions.FindIndex(x => x.action == rule.actionToTrigger);
                    reaction = true;

                    if (index >= 0)
                        reactPeople = possibleActions[index].reactToPerson;
                    else
                        continue;
                }

                if (roleRef != null && roleRef.Count > 0)
                    if (reaction)
                        for (int i = reactPeople.Count - 1; i >= 0; i-- )
                            if(!roleRef.ContainsKey(reactPeople[i]))
                                reactPeople.RemoveAt(i);
                    else
                        foreach (Person person in roleRef.Keys)
                            reactPeople.Add(person);
                
				if(rule.role.Equals(role)){
					//debug.Write("Checking condition "+rule.ruleName+"   "+rule.Condition(self,reactPeople));
					if(rule.Condition(self, reactPeople))
					{
						debug.Write("Calculating "+rule.actionToTrigger.name+" to "+rule.selfOther[self].person.name+" in "+maskName);
				
                        float maskCalculation;

                        if (roleRef != null && roleRef.ContainsKey(rule.selfOther[self].person))
                            maskCalculation = Calculator.CalculateRule(rat, mor, imp, abi, rule, rule.rulesThatMightHappen, roleRef[rule.selfOther[self].person]);
                        else
						    maskCalculation = Calculator.CalculateRule(rat, mor, imp, abi, rule, rule.rulesThatMightHappen, genLvlOfInfl);
				
						float newActionStrength = maskCalculation + Calculator.UnboundAdd(rule.selfOther[self].pref, maskCalculation);
						debug.Write(maskCalculation+"  +  "+rule.selfOther[self].pref+"  =  "+newActionStrength);
						if (newActionStrength > chosenRule.strOfAct)
						{
							chosenRule.strOfAct = newActionStrength;
							chosenRule.chosenRule = rule;
						}
					}
				}
			}

			//debug.Write ("RETURNING " + chosenAction.chosenRule.ruleName);
			return chosenRule;
		}
		
		public TypeMask GetMaskType(){ return maskType; }
		public string GetMaskName(){ return maskName; }
    }
}
