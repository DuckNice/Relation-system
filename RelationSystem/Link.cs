using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NRelationSystem
{
    public class Link
    {
        string roleName;
        List<Person> roleRef;
        Mask roleMask;
        float levelOfInfluence;

        public Link(string _roleName, List<Person> _roleRef, Mask _roleMask, float _levelOfInfluence) 
        {
            roleName = _roleName;
            roleRef = _roleRef;
            roleMask = _roleMask;
            levelOfInfluence = _levelOfInfluence;
        }


        public void AddRoleRef(Person _roleRef)
        {
            roleRef.Add(_roleRef);
        }

        public actionAndStrength actionForLink(List<MAction> possibleActions, float rat, float mor, float imp) 
        {
            actionAndStrength actionToSend;
            try
            {
                actionToSend = roleMask.CalculateActionToUse(possibleActions, rat, mor, imp, levelOfInfluence);
            }
            catch
            {
                return new actionAndStrength();
            }

            return actionToSend;
        }
    }
}
