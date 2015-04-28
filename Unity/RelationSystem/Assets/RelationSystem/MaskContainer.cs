using System;
using System.Collections.Generic;
using System.Linq;


namespace NRelationSystem
{
    public class MaskCont
    {
        public Dictionary<string, Mask> instMasks = new Dictionary<string, Mask>();

        public Dictionary<string, Rule> instRules = new Dictionary<string, Rule>();

        public void CreateNewMask(string name, TypeMask _maskType, Overlay _maskOverlay) 
        {
            name = name.ToLower();

            Mask newMask = new Mask(_maskType, _maskOverlay, name);

            if(newMask != null && !(instMasks.ContainsKey(name)))
            {
                instMasks.Add(name, newMask);
            }
            else if (newMask != null)
            {
                debug.Write("Error: Mask with name '" + name + "' Already Exists.");
            }
            else
            {
                debug.Write("Error: Mask not created with CreateNewMask.");
            }
        }


        public Mask GetMask(string maskName) 
        {
            maskName.ToLower();

            Mask instMask;

            try
            {
                instMask = instMasks[maskName];
            }
            catch
            {
                debug.Write("Error: Mask '" + maskName + "' not found in list of instantiated masks. Returning null.");

                return null;
            }

            return instMask;
        }


        public void CreateNewRule(string ruleName, MAction posAction, RuleConditioner ruleCondition = null, RulePreference rulePreference = null, VisibilityCalculator visCalc = null)
        {
            ruleName = ruleName.ToLower();

            if (!instRules.Keys.Contains(ruleName))
            {
                instRules.Add(ruleName, new Rule(ruleName.ToLower(), posAction, ruleCondition, rulePreference, visCalc));
            }
            else
            {
                debug.Write("Warning: Rule with name '" + ruleName + "' Already exists. Not adding rule.");
            }
        }


        public Rule FindRule(string ruleName)
        {
            ruleName = ruleName.ToLower();

            if (instRules.Keys.Contains(ruleName))
            {
                return instRules[ruleName];
            }

            return null;
        }


        public void AddPossibleRulesToRule(string ruleName, List<Rule> possibleRules)
        {
            ruleName = ruleName.ToLower();

            instRules[ruleName].rulesThatMightHappen.AddRange(possibleRules);
        }


        public void AddRuleToMask(string maskName, string ruleName, string roleName, float strength, List<Rule> possibleRules = null)
        {
            maskName = maskName.ToLower();
            ruleName = ruleName.ToLower();
            roleName = roleName.ToLower();

            int roleIndex = GetMaskRoleIndex(maskName, roleName);

            if(instMasks[maskName].roles.Count > roleIndex)
            {
                Rule rule = FindRule(ruleName);

                if(rule != null)
                {
                    rule.role = roleName;
                    rule.SetRuleStrength(strength);

                    if(possibleRules != null)
                    {
                        foreach(Rule possibleRule in possibleRules)
                        {
                            rule.rulesThatMightHappen.Add(possibleRule);
                        }
                    }

                    instMasks[maskName].AddRule(ruleName, rule);
                }
            }
            else
            {
                debug.Write("Error: role did not exist, choose other role.");
            }
        }


        public void removeRuleFromMask(string maskName, string ruleName)
        {
            maskName = maskName.ToLower();
            ruleName = ruleName.ToLower();

            instMasks[maskName].RemoveRule(ruleName);
        }


        public void AddRoleToMask(string maskName, string newRoleName)
        {
            maskName = maskName.ToLower();
            newRoleName = newRoleName.ToLower();

            instMasks[maskName].AddRole(newRoleName);
        }


        public string GetMaskRole(string maskName, int index)
        {
            maskName = maskName.ToLower();

            if(index < instMasks[maskName].roles.Count)
            {
                return instMasks[maskName].roles[index];
            }
            else
            {
                return null;
            }
        }


        public int GetMaskRoleIndex(string maskName, string roleName)
        {
            maskName = maskName.ToLower();
            roleName = roleName.ToLower();

            return instMasks[maskName].FindRole(roleName);
        }

		public Rule GetRule(string _ruleName){
			_ruleName = _ruleName.ToLower ();
			foreach (Rule r in instRules.Values) {
				if(r.ruleName == _ruleName){
					return r;
				}
			}
			debug.Write ("Rule " + _ruleName + " doesn't exist. Spelling error?");
			return null;
		}

    }
}