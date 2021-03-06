﻿using System;
using System.Collections.Generic;


namespace NRelationSystem
{
    public class Person
    {
        public Link selfPerception;
        public List<Link> interPersonal;
        public List<Link> culture;
        public Overlay absTraits;
        public Dictionary<MoodTypes, float> moods = new Dictionary<MoodTypes, float>();
        public string name;

        float rationality;
        float morality;
        float impulsivity;
		float ability; // JUST A BAD ESTIMATE OF A PERSON'S ABILITY TO DO STUFF


        public Person(string _name, Link _selfPer, List<Link> _interpers, List<Link> _culture, float _ratio, float _moral, float _impulse)
        {
            name = _name;
            selfPerception = _selfPer;
            interPersonal = _interpers;
            culture = _culture;
            rationality = _ratio;
            morality = _moral;
            impulsivity = _impulse;
        }


        public List<Link> GetLinks(TypeMask type)
        {
            if(type == TypeMask.culture)
            {
                return culture;
            }
            else if(type == TypeMask.interPers)
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


        public void AddLink(TypeMask type, Link newLink) 
        {
            if(type == TypeMask.selfPerc && selfPerception != null)
            {
                Console.WriteLine("Error: selfPersonMask already exists. Not adding Mask.");
            }
            else if(type == TypeMask.interPers)
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
			Console.WriteLine ("");
			Console.WriteLine (name + " ACTION TURN   " );

            RuleAndStr chosenAction = selfPerception.actionForLink(notPosActions, this, rationality, morality, impulsivity, ability, foci);


            foreach(Link curLink in interPersonal)
            {
				RuleAndStr curAction = curLink.actionForLink(notPosActions, this, rationality, morality, impulsivity, ability, foci);

                if(curAction.strOfAct > chosenAction.strOfAct)
                {
                    chosenAction = curAction;
                }
            }

            foreach (Link curLink in culture)
            {
				RuleAndStr curAction = curLink.actionForLink(notPosActions, this, rationality, morality, impulsivity, ability, foci);

                if (curAction.strOfAct > chosenAction.strOfAct)
                {
                    chosenAction = curAction;
                }
            }
			return chosenAction.chosenRule;
        }
    }
}