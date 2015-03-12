using System;
using System.Collections.Generic;


namespace NRelationSystem
{
    public class Link
    {
        string roleName;
        List<Person> roleRef;
        Mask roleMask;
        float lvlOfInfl;


        public Link(string _roleName, List<Person> _roleRef, Mask _roleMask, float _lvlOfInfl) 
        {
            roleName = _roleName;
            roleRef = _roleRef;
            roleMask = _roleMask;
            lvlOfInfl = _lvlOfInfl;
        }


        public void AddRoleRef(Person _roleRef)
        {
            roleRef.Add(_roleRef);
        }


        public RuleAndStr actionForLink(List<MAction> notPosActions, Person self, float rat, float mor, float imp, float abi, List<float> foci) 
        {
            RuleAndStr actionToSend;
            try
            {
				actionToSend = roleMask.CalculateActionToUse(notPosActions, self, rat, mor, imp, abi, lvlOfInfl,foci, roleName);
            }
            catch
            {
                actionToSend = new RuleAndStr();
                actionToSend.chosenRule = new Rule("Empty", new MAction("Empty", 0.0f), 0.0f, null, "Empty", delegate { return false; });
                actionToSend.strOfAct = 0.0f;
            }

            return actionToSend;
        }
    }
}
