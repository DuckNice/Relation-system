using System;
using System.Collections.Generic;


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

        public void CreateNewRule()
        {

        }

        public void AddRuleToMask(string maskName, string newRuleName, Rule newRule)
        {
            int roleIndex = GetMaskRoleIndex(maskName, newRuleName);

            if(instMasks[maskName].roles.Count > roleIndex)
            {
                instMasks[maskName].AddRule(newRuleName, newRule);
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