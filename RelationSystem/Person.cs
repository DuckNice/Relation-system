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
        Overlay absTraits;
        public string name;

        float rationality;
        float morality;
        float impulsivity;
		float ability; // JUST A BAD ESTIMATE OF A PERSON'S ABILITY TO DO STUFF


        public Person(string _name, Link _selfPer, List<Link> _interperson, List<Link> _culture, float _ratio, float _moral, float _impulse)
        {
            name = _name;
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


        public Rule GetAction(List<MAction> notPosActions, List<float> foci) 
        {

            RuleAndStrength chosenAction = selfPerception.actionForLink(notPosActions, rationality, morality, impulsivity, ability, foci);


            foreach(Link curLink in interPersonal)
            {
				RuleAndStrength curAction = curLink.actionForLink(notPosActions, rationality, morality, impulsivity, ability, foci);

                if(curAction.strengthOfAction > chosenAction.strengthOfAction)
                {
                    chosenAction = curAction;
                }
            }

            foreach (Link curLink in culture)
            {
				RuleAndStrength curAction = curLink.actionForLink(notPosActions, rationality, morality, impulsivity, ability, foci);

                if (curAction.strengthOfAction > chosenAction.strengthOfAction)
                {
                    chosenAction = curAction;
                }
            }

			return chosenAction.chosenRule;
        }
    }
}