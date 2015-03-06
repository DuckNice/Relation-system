using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NRelationSystem
{
    public class Link
    {

		struct roleStruct{
			string roleName;
			Person person;
		};

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

        public RuleAndStrength actionForLink(List<MAction> notPosActions, float rat, float mor, float imp, float abi, List<float> foci) 
        {
            RuleAndStrength actionToSend;
            try
            {
				actionToSend = roleMask.CalculateActionToUse(notPosActions, rat, mor, imp, abi, levelOfInfluence,foci, roleName);
                
            }
            catch
            {
                actionToSend = new RuleAndStrength();
                actionToSend.chosenRule = new Rule("Empty", new MAction("Empty",0.0f),0.0f,null,"Empty");
                actionToSend.strengthOfAction = 0.0f;
            }

            return actionToSend;
        }
    }
}
