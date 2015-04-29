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
        

        public RuleAndStr CalculateActionToUse(List<MAction> notPosActions, List<PosActionItem> possibleActions, Person self, float rat, float mor, float imp, float abi, Person empty, Dictionary<Person, Dictionary<string, float>> roleRef)
        {
            RuleAndStr chosenRule = new RuleAndStr();
			chosenRule.chosenRule = new Rule("Empty", new MAction("Empty", 0.0f, 0.0f), null, null);
            chosenRule.strOfAct = -9999f;

            foreach(Rule rule in rules.Values.ToList())
            {
                if (notPosActions != null && notPosActions.Contains(rule.actionToTrigger))
                    continue;

                List<Person> posPeople = new List<Person>();

                bool reaction = false;
                
                if(possibleActions != null && possibleActions.Count > 0){
                    int index = possibleActions.FindIndex(x => x.action == rule.actionToTrigger);
                    reaction = true;

                    if (index >= 0)
                        foreach (Person person in possibleActions[index].reactToPeople)
                            posPeople.Add(person);
                    else
                        continue;
                }

                if (roleRef != null && roleRef.Count > 0)
                {
                    if (reaction){
                        for (int i = posPeople.Count - 1; i >= 0; i--){
                            if (!roleRef[empty].ContainsKey(rule.role) && (!roleRef.ContainsKey(posPeople[i]) || !roleRef[posPeople[i]].ContainsKey(rule.role))){
                                posPeople.RemoveAt(i);
                            }
                        }
                    }
                    else
                    {
                        if (!roleRef[empty].ContainsKey(rule.role)){
                            reaction = true;

                            foreach (Person person in roleRef.Keys){
                                if (roleRef[person].ContainsKey(rule.role) && person != empty){
                                    posPeople.Add(person);
                                }
                            }
                        }
                    }
                }
                else
                {
                    debug.Write("Warning: No roleRefs were passed to mask '" + maskName + "'.");
                    
                    break;
                }

				//debug.Write("Checking condition "+rule.ruleName+"  "+self.name);
				
                if(rule.Condition(self, posPeople, reaction))
				{
                    float maskCalculation = -99999999999f;

					if (    roleRef != null && 
                            roleRef.ContainsKey(rule.selfOther[self].person) && 
                            roleRef[rule.selfOther[self].person].ContainsKey(rule.role)){
						maskCalculation = Calculator.CalculateRule(rat, mor, imp, abi, rule, rule.rulesThatMightHappen, 
                                                                    roleRef[rule.selfOther[self].person][rule.role]);
					} 
					else if (roleRef != null && 
                            roleRef.ContainsKey(empty) && 
                            roleRef[empty].ContainsKey(rule.role)){
						maskCalculation = Calculator.CalculateRule(rat, mor, imp, abi, rule, rule.rulesThatMightHappen, 
                                                                    roleRef[empty][rule.role]);
					}
					else{
						debug.Write("Did not calculate "+rule.ruleName+". Maybe rolerefs did not contain person to check for: "+rule.selfOther[self].person.name);
						continue;
					}
                        

					debug.Write("Calculating "+rule.actionToTrigger.name+" to "+rule.selfOther[self].person.name+" in "+maskName);
					
                    float newActionStrength = maskCalculation + rule.selfOther[self].pref;
					
                    debug.Write(maskCalculation+"  (+)  "+rule.selfOther[self].pref+"  =  "+newActionStrength);
					
                    if (newActionStrength > chosenRule.strOfAct)
					{
						chosenRule.strOfAct = newActionStrength;
						chosenRule.chosenRule = rule;
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
