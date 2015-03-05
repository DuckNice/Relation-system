using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRelationSystem
{
    public class MaskContainer
    {
        Dictionary<string, Mask> instMasks = new Dictionary<string, Mask>();

        public MaskContainer()
        {

        }

        public void CreateNewMask(string name, typeMask _maskType, Overlay _maskOverlay) 
        {
            Mask newMask = new Mask(_maskType, _maskOverlay);

            instMasks.Add(name, newMask);
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
                Console.WriteLine("Error: mask '" + maskName + "' not found in list of instantiated masks. Returning null.");
                return null;
            }

            return instMask;
        }


        public void AddRuleToMask(string maskName, string newRuleName, int roleIndex, Rule newRule)
        {
            if(instMasks[maskName].roles.Count > roleIndex)
            {
                instMasks[maskName].AddRule(newRuleName, newRule, roleIndex);
            }
            else
            {
                //Log warning that the role did not exist and no rule was created.
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

        public string GetMaskRole(string maskNames, int index)
        {
            if(index < instMasks[maskNames].roles.Count)
            {
                return instMasks[maskNames].roles[index];
            }
            else
            {
                return null;
            }
        }
    }
}