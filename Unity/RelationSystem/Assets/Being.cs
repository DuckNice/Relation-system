using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

    //Namespaces
using NRelationSystem;


public class Being
{
	public string name;
	public RelationSystem maskSystem;
	public List<Possession> possessions = new List<Possession>();
	List<MAction> notPossibleActions;
    public float reactMemory = 10f;

	public Rule currentRule;
	public float actionStartTime;


	public Being (string _name, RelationSystem relsys)
	{
		name = _name;
		maskSystem = relsys;

		possessions.Add (new Axe(1.0f, "Lead", 10.0f, 0.5f));
	}

    
	public void NPCAction(float time)
	{
		if (name.ToLower() != "player" || debug.inst.playerActive)
        {
            Person self = maskSystem.pplAndMasks.GetPerson(name);
			//UIFunctions.WritePlayerLine(""+debug.inst.playerActive);

            if (currentRule != null && actionStartTime + currentRule.actionToTrigger.duration > time)
            {
                currentRule.SustainAction(self, currentRule.selfOther[self].person, currentRule, misc: possessions.ToArray());
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

                    Person subject = item.GetSubject();
                    if (subject.name != name)
                    {
                        foreach (Rule rule in item.GetRule().rulesThatMightHappen)
                        {
                            int index = possibleActions.FindIndex(x => x.action == rule.actionToTrigger);
						
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

                debug.Write("---------------------------------------------------------------------------------------------------------------------------------------------------------- " + self.name + "'s TURN.");

                Rule _rule = self.GetAction(notPossibleActions, possibleActions);
				//debug.Write("ACTION FROM "+name+" "+possibleActions.Count);
				if(_rule == null || _rule.ruleName.ToLower() == "empty"){
					debug.Write("COULD NOT DO REACTION "+name+" ");
					_rule = self.GetAction(notPossibleActions, null);
				}else{
                    if (possibleActions.Count > 0)
                    {
					    debug.Write("DOING REACTION");
                    }
                    else
                    {
                        debug.Write("DOING NORMAL ACTION");
                    }
				}

				debug.Write("DOING ACTION '" + _rule.actionToTrigger.name + "' FROM " + name + ". Rule: "+_rule.ruleName+". Role: "+_rule.role);

                if (_rule.actionToTrigger.name.ToLower() != "empty")
                {
                    currentRule = _rule;
                    actionStartTime = Time.time;

                    _rule.DoAction(self, _rule.selfOther[self].person, _rule, misc: possessions.ToArray());
                }
                else
                {
                    currentRule = null;
                }
            }
        }
    }
}