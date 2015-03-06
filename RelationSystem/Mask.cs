using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NRelationSystem
{
  
    public class Mask
    {
        typeMask maskType;
        Dictionary<string, Rule> rules;
        public List<string> roles;
        Overlay maskOverlay;

        public Mask(typeMask _maskType, Overlay _maskOverlay)
        {
            maskType = _maskType;
            roles = new List<string>();
            rules = new Dictionary<string,Rule>();
            maskOverlay = _maskOverlay;
        }


        public void AddRule(string _ruleName, Rule _rule, int roleIndex)
        {
            rules.Add(_ruleName, _rule);
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



        public RuleAndStrength CalculateActionToUse(List<MAction> notPosActions, float rat, float mor, float imp, float abi, float maskInfl, List<float> foci, string role)
        {
            RuleAndStrength chosenAction = new RuleAndStrength();
			chosenAction.chosenRule = new Rule("Empty", new MAction("Empty",0.0f),0.0f,null,"Empty");
            chosenAction.strengthOfAction = 0.0f;

            foreach(Rule rule in rules.Values.ToList())
            {
                if(!notPosActions.Contains(rule.actionToTrigger) && rule.role == role)
                {
                    float newActionStrength = Calculator.CalculateRule(rat, mor, imp, abi, rule, rule.actionToTrigger.affectedRules, maskInfl, foci);

                    if (newActionStrength > chosenAction.strengthOfAction)
                    {
                        chosenAction.strengthOfAction = newActionStrength;

                        chosenAction.chosenRule = rule;
                    }
                }
            }


            return chosenAction;
        }
    }
}
