using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRelationSystem
{
    public partial class RelationSystem
    {
        public void AddRolesToMask(string maskName, string[] roles = null)
        {
            if (roles != null)
            {
                foreach (string role in roles)
                {
                    if (role != "")
                    {
                        peopleAndMasks.AddRoleToMask(maskName, role);
                    }
                }
            }
        }


        public void AddRuleToMask(string maskName, string roleName, string actionName, float str, List<Rule> possibleRules)
        {
            peopleAndMasks.AddRuleToMask(maskName, roleName, new Rule(roleName, posActions[actionName], str, possibleRules, roleName));
        }


        public void AddAction(MAction action)
        {
            posActions.Add(action.name, action);
        }
    }
}
