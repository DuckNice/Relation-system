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
	List<MAction> notPossibleActions;
    public float reactMemory = 10f;

	public Rule currentRule;
	public float actionStartTime;


	public Being (string _name, RelationSystem relsys)
	{
		name = _name;
		focus = new Dictionary<Being,float> ();
		maskSystem = relsys;

		possessions.Add (new Axe(1.0f, "Lead", 10.0f, 0.5f));
	}


	//Use in beginning for all Beings, to set initial focus for all beings in the world.
	public void FindFocusToAll(List<Being> beingsInWorld){
		foreach (Being b in beingsInWorld) {
			focus.Add(b,1);
		}
	}


	public void SetFocusToOther(Being otherPerson, float f){
		focus [otherPerson] = f;
	}


	public void NPCAction(float time)
	{
        if (name.ToLower() != "player")
        {
            Person self = maskSystem.pplAndMasks.GetPerson(name);


            if (currentRule != null && actionStartTime + currentRule.actionToTrigger.duration > time)
            {
                currentRule.SustainAction(self, currentRule.selfOther[self], currentRule, misc: possessions.ToArray());
            }
            else
            {
                List<PosActionItem> possibleActions = new List<PosActionItem>();
                //debug.Write("BEFORE LOOP "+maskSystem.historyBook.Count);


                for (int i = maskSystem.historyBook.Count - 1; i >= 0; i--)
                {
                    HistoryItem item = maskSystem.historyBook[i];
                    
                    if (item.GetTime() < time - reactMemory)
                    {
						break;
                    }

                    if (item.HasReacted(self) || item.GetDirect() != self)
                    {
						continue;
                    }


                    foreach (Rule rule in item.GetRule().rulesThatMightHappen)
                    {
                        int index = possibleActions.FindIndex(x => x.action == rule.actionToTrigger);

                        Person subject = item.GetSubject();

						if (subject.name != name)
                        {
                            if (index < 0)
                            {
                                possibleActions.Add(new PosActionItem(rule.actionToTrigger, subject));
                            }
                            else if (!possibleActions[index].reactToPerson.Contains(subject))
                            {
                                possibleActions[index].reactToPerson.Add(subject);
                            }
                        }
                    }
                }
                debug.Write("---------- " + self.name + "'s TURN.");

                Rule _rule = self.GetAction(notPossibleActions, possibleActions, focus.Values.ToList());
				//debug.Write("ACTION FROM "+name+" "+possibleActions.Count);
				if(_rule == null || _rule.ruleName == "Empty"){
					debug.Write("COULD NOT DO REACTION "+name+" ");
					_rule = self.GetAction(notPossibleActions, null, focus.Values.ToList());
				}else{
					debug.Write("DOING REACTION");
				}

				debug.Write("DOING ACTION '" + _rule.actionToTrigger.name + "' FROM " + name + ". Rule: "+_rule.ruleName+". Role: "+_rule.role+". To: "+_rule.selfOther[self].name);

                if (_rule.actionToTrigger.name.ToLower() != "empty")
                {
                    currentRule = _rule;
                    actionStartTime = Time.time;

                    _rule.DoAction(self, _rule.selfOther[self], _rule, misc: possessions.ToArray());
                }
                else
                {
                    currentRule = null;
                }
            }
        }
    }
}