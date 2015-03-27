using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

    //Namespaces
using NRelationSystem;


public class Being
{
	public string name;
	public Dictionary<Being, float> focus;
	public RelationSystem maskSystem;
	public List<Possession> possessions = new List<Possession>();
	Dictionary<string, MAction> notPossibleActions;

	public Rule currentRule;
	public float actionStartTime;


	public Being (string _name, RelationSystem relsys)
	{
		name = _name;
		focus = new Dictionary<Being,float > ();
		notPossibleActions = new Dictionary<string, MAction> ();
		maskSystem = relsys;

		possessions.Add (new Money ());
		possessions.Add (new Axe(1.0f, "Lead", 10.0f, 0.5f));
	}


	//Use in beginning for all Beings, to set initial focus for all beings in the world.
	public void FindFocusToAll(List<Being> beingsInWorld){
		foreach (Being b in beingsInWorld) {
			focus.Add(b,0);
		}
	}


	public void SetFocusToOther(Being otherPerson, float f){
		focus [otherPerson] = f;
	}


	public void NPCAction()
	{
		Person self = maskSystem.pplAndMasks.GetPerson (name);

		if (currentRule == null || actionStartTime + currentRule.actionToTrigger.Duration > Time.time) 
		{
            currentRule.SustainAction(self, currentRule.selfOther[self], currentRule, misc: possessions.ToArray());
		}
		else 
		{
			Rule rule = self.GetAction (notPossibleActions.Values.ToList (), focus.Values.ToList ());

			if (debug.Toggle) 
			{
				debug.Write ("Doing action '" + rule.actionToTrigger.name + "' from " + name);
			}


			if (rule.actionToTrigger.name.ToLower () != "empty") 
			{
                currentRule = rule;
                actionStartTime = Time.time;

				rule.DoAction (self, rule.selfOther [self], rule, misc: possessions.ToArray ());
			}
            else
            {
                currentRule = null;
            }
		}
    }
}