using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NRelationSystem
{
    public class Person
    {
        Link selfPerception;
        List<Link> interPersonal;
        List<Link> culture;

        float rationality;
        float morality;
        float impulsivity;
		float ability; // JUST A BAD ESTIMATE OF A PERSON'S ABILITY TO DO STUFF


        public Person(Link _selfPer, List<Link> _interperson, List<Link> _culture, float _ratio, float _moral, float _impulse)
        {
            selfPerception = _selfPer;
            interPersonal = _interperson;
            culture = _culture;
            rationality = _ratio;
            morality = _moral;
            impulsivity = _impulse;
        }


        public List<Link> GetLinks(typeMask type)
        {
            if(type == typeMask.culture)
            {
                return culture;
            }
            else if(type == typeMask.interPersonal)
            {
                return interPersonal;
            }
            else
            {
                List<Link> go = new List<Link>();
                go.Add(selfPerception);
                return go;
            }
        }


        public void AddLink(typeMask type, Link newLink) 
        {
            if(type == typeMask.selfPerception && selfPerception != null)
            {
                Console.WriteLine("Error: selfPersonMask already exists. Not adding Mask.");
            }
            else if(type == typeMask.interPersonal)
            {
                interPersonal.Add(newLink);
            }
            else
            {
                culture.Add(newLink);
            }
        }


        public MAction GetAction(List<MAction> possibleActions, List<float> foci) 
        {

            actionAndStrength chosenAction = selfPerception.actionForLink(possibleActions, rationality, morality, impulsivity, ability, foci);


            foreach(Link curLink in interPersonal)
            {
				actionAndStrength curAction = curLink.actionForLink(possibleActions, rationality, morality, impulsivity, ability, foci);

                if(curAction.strengthOfAction > chosenAction.strengthOfAction)
                {
                    chosenAction = curAction;
                }
            }

            foreach (Link curLink in culture)
            {
				actionAndStrength curAction = curLink.actionForLink(possibleActions, rationality, morality, impulsivity, ability, foci);

                if (curAction.strengthOfAction > chosenAction.strengthOfAction)
                {
                    chosenAction = curAction;
                }
            }

            return chosenAction.chosenAction;
        }
    }
}