using System;
using System.Collections.Generic;


namespace NRelationSystem
{
    public class Link
    {

		

		//use this struct. pass them into rule (or make rule get it somehow)
		//Rule needs to get access to all these, which it can get through the list of roles in the Mask (RIGHT?)
		//then, in rule, the rules happen on roles, meaning that fx a greet action is defined as role x greets role y
		//this is then purely defined in the masks. so if an action only involves one person it's just role x does whatever
		//interpersonal actions are then solely in the interpersonal masks, since these have access to the links.

		//when the action actually happens, because of this struct, we can pass that out to the Persons.

		//this means that we, when we create the link, have to pass the person into the struct.
        
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

        public RuleAndStr actionForLink(List<MAction> notPosActions, float rat, float mor, float imp, float abi, List<float> foci) 
        {
            RuleAndStr actionToSend;
            try
            {
				actionToSend = roleMask.CalculateActionToUse(notPosActions, rat, mor, imp, abi, lvlOfInfl,foci, roleName);
                
            }
            catch
            {
                actionToSend = new RuleAndStr();
                actionToSend.chosenRule = new Rule("Empty", new MAction("Empty",0.0f),0.0f,null, "Empty", null, null);
                actionToSend.strOfAct = 0.0f;
            }

            return actionToSend;
        }
    }
}
