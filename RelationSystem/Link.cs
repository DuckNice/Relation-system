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

        public actionAndStrength actionForLink(List<MAction> notPosActions, float rat, float mor, float imp, float abi, List<float> foci) 
        {
            actionAndStrength actionToSend;
            try
            {
				actionToSend = roleMask.CalculateActionToUse(notPosActions, rat, mor, imp, abi, levelOfInfluence,foci, roleName);
                
            }
            catch
            {
                actionToSend = new actionAndStrength();
                actionToSend.chosenAction = new MAction("Empty", 0.0f);
                actionToSend.strengthOfAction = 0.0f;
            }

            return actionToSend;
        }
    }
}
