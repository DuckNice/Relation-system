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


        public actionAndStrength CalculateActionToUse(List<MAction> possibleActions, float rat, float mor, float imp, float abi, float maskInfl, List<float> foci)
        {
            actionAndStrength chosenAction = new actionAndStrength();
            chosenAction.chosenAction = new MAction("Empty", 0.0f);
            chosenAction.strengthOfAction = 0.0f;

            foreach (MAction curAction in possibleActions)
            {
                List<Rule> rulesForAction = new List<Rule>();

                foreach(KeyValuePair<string, Rule> rule in rules)
                {
                    if(rule.Value.actionToTrigger.Equals(curAction))
                    {
                        rulesForAction.Add(rule.Value);
                    }
                }

                foreach(Rule rule in rulesForAction)
                {
                    float newActionStrength = Calculator.CalculateRule(rat, mor, imp, abi, rule, curAction.affectedRules, maskInfl,foci);

                    if (newActionStrength > chosenAction.strengthOfAction)
                    {
                        chosenAction.strengthOfAction = newActionStrength;

                        chosenAction.chosenAction = curAction;
                    }
                }
            }

            

            return chosenAction;
        }
    }
}
