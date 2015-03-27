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
        Overlay maskOverlay;


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
        

        public RuleAndStr CalculateActionToUse(List<MAction> notPosActions, Person self, float rat, float mor, float imp, float abi, float maskInfl, List<float> foci, string role)
        {
            RuleAndStr chosenAction = new RuleAndStr();

			chosenAction.chosenRule = new Rule("Empty", new MAction("Empty", 0.0f), null);
            chosenAction.strOfAct = 0.0f;

            foreach(Rule rule in rules.Values.ToList())
            {
				debug.Write("Checking "+rule.actionToTrigger.name);

                if(!notPosActions.Contains(rule.actionToTrigger) && rule.role.Equals(role) && rule.Condition(self))
                {
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

		public TypeMask GetMaskType(){ return maskType; }
		public string GetMaskName(){ return maskName; }

    }
}
