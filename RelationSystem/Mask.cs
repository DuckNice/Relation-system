using System;
using System.Collections.Generic;
using System.Linq;


namespace NRelationSystem
{
    public class Mask
    {
        TypeMask maskType;
        Dictionary<string, Rule> rules;
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
        

        public RuleAndStr CalculateActionToUse(List<MAction> notPosActions, float rat, float mor, float imp, float abi, float maskInfl, List<float> foci, string role)
        {
            RuleAndStr chosenAction = new RuleAndStr();
			chosenAction.chosenRule = new Rule("Empty", new MAction("Empty",0.0f),0.0f,null, "Empty", null, null);
            chosenAction.strOfAct = 0.0f;

            foreach(Rule rule in rules.Values.ToList())
            {
                if(!notPosActions.Contains(rule.actionToTrigger) && rule.role.Equals(role))
                {
					Console.WriteLine("calculating "+rule.actionToTrigger.name);
                    float newActionStrength = Calculator.CalculateRule(rat, mor, imp, abi, rule, rule.actionToTrigger.affectedRules, maskInfl, foci);

                    if (newActionStrength > chosenAction.strOfAct)
                    {
                        chosenAction.strOfAct = newActionStrength;

                        chosenAction.chosenRule = rule;
                    }
                }
            }
            
            return chosenAction;
        }
    }
}
