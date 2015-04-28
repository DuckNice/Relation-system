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


		public float CalculateRelation(Person person)
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


        //TODO: make this work.
        public void AddRoleRefToLink(TypeMask type, Mask maskRef, string role, Person _ref, float lvlOfInfl)
        {
            if (type == TypeMask.selfPerc)
            {
                debug.Write("Error: selfPersonMask does not contain roleRefs. Not adding RoleRef.");
            }
            else if (type == TypeMask.interPers)
            {
                int index = interPersonal.FindIndex(x => x._roleMask == maskRef);

                if (index < 0)
                    debug.Write("Warning: link doesn't exist. Not Adding roleref.");
                else
                    interPersonal[index].AddRoleRef(role, lvlOfInfl, _ref);
            }
            else
            {
                int index = culture.FindIndex(x => x._roleMask == maskRef);

                if (index < 0)
                    debug.Write("Warning: link doesn't exist. Not Adding roleref.");
                else
                    culture[index].AddRoleRef(role, lvlOfInfl, _ref);
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
                int index = interPersonal.FindIndex(x => x._roleMask == newLink._roleMask);

                if (index < 0)
                    interPersonal.Add(newLink);
                else
                    interPersonal[index].AddRoleRef(newLink._roleRefs);
            }
            else
            {
                int index = culture.FindIndex(x => x._roleMask == newLink._roleMask);

                if (index < 0)
                    culture.Add(newLink);
                else
                    culture[index].AddRoleRef(newLink._roleRefs);
            }
        }


        //TODO: make this function take in mask and remove people and/or roles associated.
        public void RemoveRoleRef(TypeMask type, Mask maskRef, Person _ref = null, string role = "")
        {
            if (type == TypeMask.selfPerc)
            {
                debug.Write("Error: selfPersonMask does not contain roleRefs. Not removing RoleRef.");
            }
            else if (type == TypeMask.interPers)
            {
                int index = interPersonal.FindIndex(x => x._roleMask == maskRef);

                if (index < 0)
                    interPersonal[index].RemoveRoleRef(role, _ref);
                else
                    debug.Write("Warning: link doesn't exist. Not Adding roleref.");
            }
            else
            {
                int index = culture.FindIndex(x => x._roleMask == maskRef);

                if (index < 0)
                    debug.Write("Warning: link doesn't exist. Not Adding roleref.");
                else
                    culture[index].RemoveRoleRef(role, _ref);
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


        public Rule GetRule(string actionName) 
		{ 
			foreach(Rule r in selfPerception._roleMask.rules.Values){
				if(r.actionToTrigger.name.ToLower() == actionName){
					return r;
				}
			}

			foreach(Link curLink in interPersonal)
			{
				foreach(Rule r in curLink._roleMask.rules.Values){
					if(r.actionToTrigger.name.ToLower() == actionName){
						return r;
					}
				}
			}
			
			foreach (Link curLink in culture)
			{
				foreach(Rule r in curLink._roleMask.rules.Values){
					if(r.actionToTrigger.name.ToLower() == actionName){
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

            List<Person> activePeople = relationSystem.CreateActiveListsList();

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

		public void SetOpinionValue(TraitTypes traitType, Person pers, float valToAdd){
			if (opinions.Exists(x => x.trait == traitType && x.pers == pers)){
                foreach (Opinion o in opinions){
                    if (o.pers == pers && o.trait == traitType){
                        o.value = valToAdd;
                        return;
                    }
                }
            }
            else{
                opinions.Add(new Opinion(traitType, pers, valToAdd));
            }
		}

		public void AddToOpinionValue(TraitTypes traitType, Person pers, float valToAdd){
            if (opinions.Exists(x => x.trait == traitType)){
                foreach (Opinion o in opinions){
                    if (o.pers == pers && o.trait == traitType){
                        o.value += Calculator.UnboundAdd(valToAdd, o.value);
                        return;
                    }
                }
            }
            else{
                opinions.Add(new Opinion(traitType, pers, valToAdd));
            }
		}

	/*	public void AddToInterPersonalLvlOfInfl(Person p,float val){
			foreach(Link l in interPersonal){
				if(l._roleRefs.ContainsKey(p)){
					foreach(string s in l._roleRefs[p].Keys){
						l._roleRefs[p][s] += Calculator.UnboundAdd(val,l._roleRefs[p][s]);
					}
				}
			}
		}

*/
		public bool CheckRoleName(string s, Person p = null){

			if(p == null){
				foreach (Link l in interPersonal) {
					if (p == null) {
						p = l.empty;
					}
					if(l._roleRefs.ContainsKey(p)){
						foreach(Person i in l._roleRefs.Keys){
							if(l._roleRefs[i].ContainsKey(s)){
								return true;
							}
						}
					}
				}
					foreach (Link l in culture) {
						if (p == null) {
							p = l.empty;
						}
						if(l._roleRefs.ContainsKey(p)){
							foreach(Person i in l._roleRefs.Keys){
								if(l._roleRefs[i].ContainsKey(s)){
									return true;
								}
							}
						}
					}
				}

				foreach (Link l in interPersonal) {
					if (p == null) {
						p = l.empty;
					}
					if(l._roleRefs.ContainsKey(p)){
						if(l._roleRefs[p].ContainsKey(s)){
							return true;
						}
					}
				}
				foreach (Link l in culture) {
					if (p == null) {
						p = l.empty;
					}
					if(l._roleRefs.ContainsKey(p)){
						if(l._roleRefs[p].ContainsKey(s)){
							return true;
						}
					}
				}
				return false;
		}

		public float GetLvlOfInflToPerson(Person p = null){
			foreach (Link l in interPersonal) {
				if (p == null) {
					p = l.empty;
				}
				if(l._roleRefs.ContainsKey(p)){
					foreach(string s in l._roleRefs[p].Keys){
						return l._roleRefs[p][s];
					}
				}
			}
			debug.Write ("ERROR. Did not find person to getlvlofInfl from. In Person.cs");
			return 0.0f;
		}




    }
}