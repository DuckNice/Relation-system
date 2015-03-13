using System;
using System.Collections.Generic;
using System.Linq;


namespace NRelationSystem
{
    public class Mask
    {
        TypeMask maskType;
        public Dictionary<string, Rule> rules;
        public List<string> roles;
        Overlay maskOverlay;


        public Mask(TypeMask _maskType, Overlay _maskOverlay)
        {
            maskType = _maskType;
            roles = new List<string>();
            rules = new Dictionary<string,Rule>();
            maskOverlay = _maskOverlay;
        }


        public void AddRule(string _ruleRoleName, Rule _rule)
        {
            rules.Add(_ruleRoleName, _rule);
        }


        public bool RemoveRule(string _ruleName)
        {
            if(rules.Remove(_ruleName))
            {
                return true;
            }

            return false;
        }


        public void AddRole(string name)
        {
            if(!roles.Contains(name))
            {
                roles.Add(name);
            }
        }
        

        public int FindRole(string roleName) 
        {
            return roles.FindIndex(x => x == roleName);
        }
        

        public RuleAndStr CalculateActionToUse(List<MAction> notPosActions, Person self, float rat, float mor, float imp, float abi, float maskInfl, List<float> foci, string role)
        {
            RuleAndStr chosenAction = new RuleAndStr();
            RuleConditioner empty = delegate { return false; };

			chosenAction.chosenRule = new Rule("Empty", new MAction("Empty", 0.0f), 0.0f, null, "Empty", empty);
            chosenAction.strOfAct = 0.0f;

            foreach(Rule rule in rules.Values.ToList())
            {
				Console.Write("check "+rule.actionToTrigger.name+"   ");
                if(!notPosActions.Contains(rule.actionToTrigger) && rule.role.Equals(role) && rule.Condition(self))
                {
                    float newActionStrength = Calculator.CalculateRule(rat, mor, imp, abi, rule, rule.actionToTrigger.affectedRules, maskInfl, foci);

                    if (newActionStrength > chosenAction.strOfAct)
                    {
                        chosenAction.strOfAct = newActionStrength;
                        chosenAction.chosenRule = rule;
                    }
                }
				Console.WriteLine(" ");
            }
            
			//Console.WriteLine ("SENDING OUT FROM MASK: " + chosenAction.chosenRule.ruleName);
            return chosenAction;
        }
    }
}
