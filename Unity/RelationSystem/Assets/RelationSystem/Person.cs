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
		public Dictionary<MoodTypes, float> moods = new Dictionary<MoodTypes, float> ();
		public List<Opinion> opinions = new List<Opinion> ();
        public RelationSystem relationSystem;

		public string name;

        float rationality;
        float morality;
        float impulsivity;
		float ability;


        public Person(string name)
        {
            this.name = name;
        }


        public Person(string _name, Link _selfPer, List<Link> _interpers, List<Link> _culture, float _ratio, float _moral, float _impulse, RelationSystem _relationSystem)
        {
            name = _name;
            selfPerception = _selfPer;
            interPersonal = _interpers;
            culture = _culture;
            rationality = _ratio;
            morality = _moral;
            impulsivity = _impulse;
            relationSystem = _relationSystem;
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


        public Rule GetAction(List<MAction> notPosActions, List<PosActionItem> posAction) 
        {					

            RuleAndStr chosenAction = selfPerception.actionForLink(notPosActions, posAction, this, rationality, morality, impulsivity, ability);

            foreach(Link curLink in interPersonal)
            {
				RuleAndStr curAction = curLink.actionForLink(notPosActions, posAction, this, rationality, morality, impulsivity, ability);

                if(curAction.strOfAct > chosenAction.strOfAct)
                {
                    chosenAction = curAction;
                }
            }

            foreach (Link curLink in culture)
            {
				RuleAndStr curAction = curLink.actionForLink(notPosActions, posAction, this, rationality, morality, impulsivity, ability);

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

		public float GetAbility(){ return ability; }
		public void SetAbility(float inp){ ability = inp; }
		public void AddToAbility(float inp){ ability += inp; }


		public Rule GetRule(string ruleName) 
		{ 
			foreach(Rule r in selfPerception._roleMask.rules.Values){
				if(r.actionToTrigger.name == ruleName){
					return r;
				}
			}

			foreach(Link curLink in interPersonal)
			{
				foreach(Rule r in curLink._roleMask.rules.Values){
					if(r.actionToTrigger.name == ruleName){
						return r;
					}
				}
			}
			
			foreach (Link curLink in culture)
			{
				foreach(Rule r in curLink._roleMask.rules.Values){
					if(r.actionToTrigger.name == ruleName){
						return r;
					}
				}
			}
			debug.Write ("Error in GetRule from Person. Rule not found. Check spelling. Returning Empty.");
			
			return new Rule("Empty", new MAction("Empty", 0.0f,0.0f), null, null);
		}


        public float CalculateTraitType(TraitTypes traitType)
        {
            float baseVal = absTraits.traits[traitType].GetTraitValue();

            List<Person> activePeople = relationSystem.createActiveListsList();

            foreach(Link link in interPersonal)
            {
                foreach (Person person in link.GetRoleRefPpl())
                {
                    if (activePeople.Contains(person))
                    {
                        float go = link._roleMask.maskOverlay.traits[traitType].GetTraitValue() * GetLvlOfInflToPerson(person);
                        baseVal += Calculator.UnboundAdd(go, baseVal);
                        break;
                    }
                }
            }

            foreach(Link link in culture)
            {
				float go = link._roleMask.maskOverlay.traits[traitType].GetTraitValue() * GetLvlOfInflToPerson();
                baseVal += Calculator.UnboundAdd(go, baseVal);
            }

            return baseVal;
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
					return;
				}
			}
		}

		public void AddToOpinionValue(TraitTypes traittype, Person pers, float valToAdd){
			foreach (Opinion o in opinions) {
				if (o.pers == pers && o.trait == traittype) {
					o.value += Calculator.UnboundAdd (valToAdd, o.value);
					return;
				}
			}
		}

		public void AddToInterPersonalLvlOfInfl(Person p,float val){
			foreach(Link l in interPersonal){
				if(l._roleRef.ContainsKey(p)){
					foreach(string s in l._roleRef[p].Keys){
						l._roleRef[p][s] += Calculator.UnboundAdd(val,l._roleRef[p][s]);
					}
				}
			}
		}


		public bool CheckRoleName(string s, Person p = null){

			if(p == null){
				foreach (Link l in interPersonal) {
				if (p == null) {
					p = l.empty;
				}
					foreach(Person i in l._roleRef.Keys){
						if(l._roleRef[i].ContainsKey(s)){
							return true;
						}
					}
				}
				foreach (Link l in culture) {
				if (p == null) {
					p = l.empty;
				}
					foreach(Person i in l._roleRef.Keys){
						if(l._roleRef[i].ContainsKey(s)){
							return true;
						}
					}
				}
			}
			foreach (Link l in interPersonal) {
				if(l._roleRef[p].ContainsKey(s)){
					return true;
				}
			}
			foreach (Link l in culture) {
				if(l._roleRef[p].ContainsKey(s)){
					return true;
				}
			}
			return false;
		}

		public float GetLvlOfInflToPerson(Person p = null){
			foreach (Link l in interPersonal) {
				if (p == null) {
					p = l.empty;
				}
				if(l._roleRef.ContainsKey(p)){
					foreach(string s in l._roleRef[p].Keys){
						return l._roleRef[p][s];
					}
				}
			}
			debug.Write ("ERROR. Did not find person to getlvlofInfl from. In Person.cs");
			return 0.0f;
		}




    }
}