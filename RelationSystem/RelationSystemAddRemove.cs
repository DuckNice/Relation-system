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
                        pplAndMasks.AddRoleToMask(maskName, role);
                    }
                }
            }
        }


        public void AddRuleToMask(string maskName, string roleName, Person self, Person other, string actionName, float str, List<Rule> possibleRules)
        {
            pplAndMasks.AddRuleToMask(maskName, roleName, new Rule(roleName, posActions[actionName], str, possibleRules, roleName, self, other));
        }


        public void AddLinkToPerson(string personName, string[] linkRelations, typeMask maskType, string role, string mask, float str)
        {
            List<Person> peopleRelated = new List<Person>();

            foreach (string linkRelation in linkRelations)
            {
                peopleRelated.Add(pplAndMasks.GetPerson(linkRelation));
            }

            pplAndMasks.GetPerson(personName).AddLink(maskType, new Link(role, peopleRelated, pplAndMasks.GetMask(mask), str));
        }


        public void AddAction(MAction action)
        {
            posActions.Add(action.name, action);
        }
    }
}
