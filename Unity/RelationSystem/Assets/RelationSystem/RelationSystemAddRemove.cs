using System;
using System.Collections.Generic;


namespace NRelationSystem
{
    public partial class RelationSystem
    {
        public void AddRolesToMask(string maskName, string[] roles = null)
        {
            maskName = maskName.ToLower();

            if (roles != null)
            {
                foreach (string role in roles)
                {
                    if (role != "")
                    {
                        pplAndMasks.AddRoleToMask(maskName, role.ToLower());
                    }
                }
            }
        }


        public void AddRuleToMask(string maskName, string roleName, string ruleName, float str, List<Rule> possibleRules = null)
        {
            maskName = maskName.ToLower();
            roleName = roleName.ToLower();
            ruleName = ruleName.ToLower();

			pplAndMasks.AddRuleToMask(maskName, ruleName, roleName, str, possibleRules);
        }

        public void AddPossibleRulesToRule(string ruleName, List<Rule> possibleRules)
        {
            ruleName = ruleName.ToLower();

            pplAndMasks.AddPossibleRulesToRule(ruleName, possibleRules);
        }

        public void AddLinkToPerson(string persName, string[] linkRel, TypeMask maskType, string role, string mask, float str)
        {
            persName = persName.ToLower();
            role = role.ToLower();

            List<Person> peopleRelated = new List<Person>();

            foreach (string linkRelation in linkRel)
            {
                peopleRelated.Add(pplAndMasks.GetPerson(linkRelation.ToLower()));
            }

            pplAndMasks.GetPerson(persName).AddLink(maskType, new Link(role, peopleRelated, pplAndMasks.GetMask(mask), str));
        }


        public void AddAction(MAction action)
        {
            if(!posActions.ContainsKey(action.name))
            {
                posActions.Add(action.name, action);
            }
            else
            {
                debug.Write("Warning: Action with name: '" + action.name + "' already exists. Please note that action names are not case sensitive.");
            }
            
        }
    }
}
