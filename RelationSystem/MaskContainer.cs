﻿using System;
using System.Collections.Generic;
using System.Linq;


namespace NRelationSystem
{
    public class MaskCont
    {
        protected Dictionary<string, Mask> instMasks = new Dictionary<string, Mask>();

        protected Dictionary<string, Rule> instRules = new Dictionary<string, Rule>();

        public void CreateNewMask(string name, TypeMask _maskType, Overlay _maskOverlay) 
        {
            Mask newMask = new Mask(_maskType, _maskOverlay);

            if(newMask != null && !(instMasks.ContainsKey(name)))
            {
                instMasks.Add(name, newMask);
            }
            else if (newMask != null)
            {
                Console.WriteLine("Error: Mask with name '" + name + "' Already Exists.");
            }
            else
            {
                Console.WriteLine("Error: Mask not created with CreateNewMask.");
            }
        }


        public Mask GetMask(string maskName) 
        {
            Mask instMask;

            try
            {
                instMask = instMasks[maskName];
            }
            catch
            {
                Console.WriteLine("Error: Mask '" + maskName + "' not found in list of instantiated masks. Returning null.");
                return null;
            }

            return instMask;
        }


        public void CreateNewRule(string ruleName, MAction posAction, RuleConditioner ruleCondition)
        {
            ruleName = ruleName.ToLower();

            if (!instRules.Keys.Contains(ruleName))
            {
                instRules.Add(ruleName, new Rule(ruleName.ToLower(), posAction, ruleCondition));
            }
            else
            {
                Console.WriteLine("Warning: Rule with name '" + ruleName + "' Already exists. Not adding rule.");
            }
        }


        public Rule FindRule(string ruleName)
        {
            if (instRules.Keys.Contains(ruleName))
            {
                return instRules[ruleName];
            }

            return null;
        }


        public void AddPossibleRulesToRule(string ruleName, List<Rule> possibleRules)
        {
            instRules[ruleName].rulesThatMightHappen.AddRange(possibleRules);
        }


        public void AddRuleToMask(string maskName, string ruleName, string roleName, float strength, List<Rule> possibleRules = null)
        {
            int roleIndex = GetMaskRoleIndex(maskName, roleName);

            if(instMasks[maskName].roles.Count > roleIndex)
            {
                Rule rule = FindRule(ruleName);

                if(rule != null)
                {
                    rule.role = roleName;
                    rule.strength = strength;

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
                Console.WriteLine("Error: role did not exist, choose other role.");
            }
        }


        public void removeRuleFromMask(string maskName, string ruleName)
        {
            instMasks[maskName].RemoveRule(ruleName);
        }


        public void AddRoleToMask(string maskName, string newRoleName)
        {
            instMasks[maskName].AddRole(newRoleName);
        }


        public string GetMaskRole(string maskName, int index)
        {
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
            return instMasks[maskName].FindRole(roleName);
        }
    }
}