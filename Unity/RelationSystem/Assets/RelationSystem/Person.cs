using System;
using System.Collections.Generic;


namespace NRelationSystem
{
    public class Person
    {
        public Link selfPerception;
        public List<Link> interPersonal;
        public List<Link> culture;
        public Overlay absTraits;
		public Dictionary<MoodTypes, float> moods = new Dictionary<MoodTypes, float> ();
		public List<Opinion> opinions = new List<Opinion> ();

		public string name;

        float rationality;
        float morality;
        float impulsivity;
		float ability; 


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

        public Person Copy()
        {
            return (Person)this.MemberwiseClone();
        }

		public float calculateRelation(Person person)
		{
			return 0;
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
				debug.Write("Error: selfPersonMask already exists. Not adding Mask.");
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


        public void RemoveLink(TypeMask type, Link oldLink)
        {
            if (type == TypeMask.selfPerc)
            {
                debug.Write("Error: Cannot remove selfPersonMask.");
            }
            else if (type == TypeMask.interPers)
            {
                interPersonal.Remove(oldLink);
            }
            else
            {
                culture.Remove(oldLink);
            }
        }


        public Rule GetAction(List<MAction> notPosActions, List<PosActionItem> posAction, List<float> foci) 
        {					

            RuleAndStr chosenAction = selfPerception.actionForLink(notPosActions, posAction, this, rationality, morality, impulsivity, ability, foci);

            foreach(Link curLink in interPersonal)
            {
				RuleAndStr curAction = curLink.actionForLink(notPosActions, posAction, this, rationality, morality, impulsivity, ability, foci);

                if(curAction.strOfAct > chosenAction.strOfAct)
                {
                    chosenAction = curAction;
                }
            }

            foreach (Link curLink in culture)
            {
				RuleAndStr curAction = curLink.actionForLink(notPosActions, posAction, this, rationality, morality, impulsivity, ability, foci);

                if (curAction.strOfAct > chosenAction.strOfAct)
                {
                    chosenAction = curAction;
                }
            }

			return chosenAction.chosenRule;
        }


		public float GetRationality(){ return rationality; }
		public void SetRationality(float inp){ rationality = inp; }
		public void AddToRationality(float inp){ rationality += inp; }

		public float GetMorality(){ return morality; }
		public void SetMorality(float inp){ morality = inp; }
		public void AddToMorality(float inp){ morality += inp; }

		public float GetImpulsivity(){ return impulsivity; }
		public void SetImpulsivity(float inp){ impulsivity = inp; }
		public void AddToImpulsivity(float inp){ impulsivity += inp; }

		public float GetAbilityy(){ return ability; }
		public void SetAbility(float inp){ ability = inp; }
		public void AddToAbility(float inp){ ability += inp; }


		public Rule GetRule(string ruleName) 
		{ 
			foreach(Rule r in selfPerception.roleMask.rules.Values){
				if(r.actionToTrigger.name == ruleName){
					return r;
				}
			}

			foreach(Link curLink in interPersonal)
			{
				foreach(Rule r in curLink.roleMask.rules.Values){
					if(r.actionToTrigger.name == ruleName){
						return r;
					}
				}
			}
			
			foreach (Link curLink in culture)
			{
				foreach(Rule r in curLink.roleMask.rules.Values){
					if(r.actionToTrigger.name == ruleName){
						return r;
					}
				}
			}
			debug.Write ("Error in GetRule from Person. Rule not found. Check spelling. Returning Empty.");
			
			return new Rule("Empty", new MAction("Empty", 0.0f,0.0f), null);
		}


		public float GetOpinionValue(TraitTypes traittype, Person pers){
			foreach(Opinion o in opinions ){
				if(o.pers == pers && o.trait == traittype){
					return o.value;
				}
			}
			debug.Write ("Error in GetOpinionValue. Did not find person "+pers.name+" or trait "+traittype+". Check spelling. Returning 0.0");
			return 0.0f;
		}

		public void SetOpinionValue(TraitTypes traittype, Person pers, float valToAdd){
			foreach (Opinion o in opinions) {
				if (o.pers == pers && o.trait == traittype) {
					o.value = valToAdd;
					break;
				}
				else{
					debug.Write ("Error in SetOpinion. Did not find person "+pers.name+" or trait "+traittype+". Check spelling.");
				}
			}
		}

		public void AddToOpinionValue(TraitTypes traittype, Person pers, float valToAdd){
			foreach (Opinion o in opinions) {
				if (o.pers == pers && o.trait == traittype) {
					o.value += Calculator.unboundAdd (valToAdd, o.value);
					break;
				}
				else{
					debug.Write ("Error in AddtoOpinion. Did not find person "+pers.name+" or trait "+traittype+". Check spelling.");
				}
			}
		}


    }
}