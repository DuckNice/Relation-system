using System;
using System.Collections.Generic;


namespace NRelationSystem
{
    public partial class RelationSystem
    {
        public void AddListToActives(string name)
        {
            if(!activeLists.ContainsKey(name) && updateLists.ContainsKey(name))
            {
                activeLists.Add(name, updateLists[name]);
            }
        }


        public void RemoveListFromActives(string name)
        {
            if(activeLists.ContainsKey(name))
            {
                activeLists.Remove(name);
            }
        }


        public void AddUpdateList(string name)
        {
            if (!updateLists.ContainsKey(name))
            {
                updateLists.Add(name, new List<Person>());
            }
        }


        public void RemoveUpdateList(string name)
        {
            if (updateLists.ContainsKey(name))
            {
                updateLists.Remove(name);
            }
        }


        public void AddPersonToUpdateList(string name, Person person)
        {
            if(updateLists.ContainsKey(name) && person != null)
            {
                if(!updateLists[name].Contains(person))
                {
                    updateLists[name].Add(person);
                }
            }
        }


        public bool RemovePersonFromUpdateList(string name, Person person)
        {
            if (updateLists.ContainsKey(name) && person != null)
            {
                if (updateLists[name].Contains(person))
                {
                    updateLists[name].Remove(person);

                    return true;
                }
            }

            return false;
        }


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

        public void AddLinkToPerson(string persName, TypeMask maskType, string role, string mask, float genStr, string linkRel = "", float str = 0)
        {
            persName = persName.ToLower();
            role = role.ToLower();
			mask = mask.ToLower ();

            Person personRelated = pplAndMasks.GetPerson(linkRel.ToLower());
          
            pplAndMasks.GetPerson(persName).AddLink(maskType, new Link(role, pplAndMasks.GetMask(mask), genStr, personRelated, str));
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
