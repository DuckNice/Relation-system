using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

	//Namespaces
using NRelationSystem;

public partial class Program : MonoBehaviour
{
	public UnityEngine.UI.Toggle playToggle;
    [HideInInspector]
    public List<Being> beings = new List<Being>();
    [HideInInspector]
	public bool shouldPlay = false;
    [HideInInspector]
	public RoomManager roomMan;
	public float timePace;
    [HideInInspector]
    public float time = 0.0f;
    [HideInInspector]
    public bool actionStored = false;
    [HideInInspector]
    public MAction playerAction;
    Person playerTarget;
    public MAction currentPlayerAction;
    [HideInInspector]
    public float actionStartTime = 0.0f;

    IEnumerator NPCUpdate()
	{
        while (true)
        {
            if (shouldPlay)
            {
                if (actionStored)
                {
                    PerformAction(playerTarget, playerAction);
                    actionStored = false;
                }

                List<Person> people = relationSystem.CreateActiveListsList();

                foreach (Being being in beings)
                {
                    if (people.Exists(x => x.name == being.name) && (being.name != playerName || debug.PlayerActive))
                    {
                        being.NPCAction(time);
						roomMan.UpdateLvlOfInfl(GetPerson(being.name),0.01f);
                    }
                }

                time += Time.deltaTime;

                if (!UIFunctions.instance.exitButtonActive)
                {
                    if (time > 120)
                    {
                        UIFunctions.ActivateExitButton();
                    }
                }
            }

			if(debug.Toggle){
				UpdateStats();
				UIFunctions.WriteGameStatsInWindow(statsString);
			}


            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIFunctions.ActivateExitButton();
            }


            yield return new WaitForSeconds(timePace);
        }
	}


    void PerformAction(Person target, MAction action)
    {
        if (currentPlayerAction == null || actionStartTime + currentPlayerAction.duration < time)
        {
            currentPlayerAction = action;
            actionStartTime = time;

            Person self = GetPerson(playerName);
            
            action.DoAction(self, target, self.GetRule(action.name));
        }
        else
        {
            UIFunctions.WritePlayerLine("You are currently " + currentPlayerAction.name + "ing. Wait another " + Decimal.Round((decimal)(currentPlayerAction.duration - (time - actionStartTime)),1) + " seconds for your action do finish.");
        }
    }
    
    public void setPlaying()
    {
        shouldPlay = !playToggle.isOn;
    }
	

	void UpdateStats(){
		statsString = "";
		
		foreach (Person p in relationSystem.pplAndMasks.people.Values) {
			statsString += p.name+"\n";
			statsString += "AngFea: "+(p.moods[MoodTypes.angryFear])*100f+"\n";
			statsString += "aroDis: "+(p.moods[MoodTypes.arousDisgus])*100f+"\n";
			statsString += "EnrTir: "+(p.moods[MoodTypes.energTired])*100f+"\n";
			statsString += "HapSad: "+(p.moods[MoodTypes.hapSad])*100f+"\n";
			statsString += "\n";
		}


		statsString += "\nTraits: \n";
		
		foreach (Person p in relationSystem.pplAndMasks.people.Values) {
			statsString += p.name+"\n";
			statsString += "NiceNasty: "+p.CalculateTraitType(TraitTypes.NiceNasty)*10f+"\n";
			statsString += "HonesFals: "+p.CalculateTraitType(TraitTypes.HonestFalse)*10f+"\n";
			statsString += "CharGreed: "+p.CalculateTraitType(TraitTypes.CharitableGreedy)*10f+"\n";
		}


		statsString += "\nMoney: \n";
		
		foreach (Being b in beings) {
			statsString +=  b.name+"  "+ b.GetPosses("money").value+"\n";
		}


	/*	statsString += "\n\n\nMask Links: \n";

		foreach (Person p in relationSystem.pplAndMasks.people.Values) {
			statsString += p.name+"\n";
			//statsString += ""+p.selfPerception._roleMask.GetMaskName()+"\n";

			foreach(Link l in p.culture){
				statsString += l._roleRefs.Count+"\n";
				if(l._roleRefs.Count == 0){ statsString += "\n"; }
				if(l._roleRefs.ContainsKey(p))
					foreach(KeyValuePair<string,float> kvp in l._roleRefs[p]){
						statsString += kvp.Key+" \n"; //
					}

				if(l._roleRefs.ContainsKey(p)){
					foreach(string s in l._roleRefs[p].Keys){
						statsString += s+" \n"; //
					}
				}
			}

			foreach(Link l in p.interPersonal){
				statsString += l._roleRefs.Count+"\n";
				if(l._roleRefs.Count == 0){ statsString += "\n"; }
		
				if(l._roleRefs.ContainsKey(p)){
					foreach(string s in l._roleRefs[p].Keys){
						statsString += s+" \n"; //
					}
				}

			}
				//		foreach(KeyValuePair<string,float> dic in l._roleRefs[p]){
			//		foreach(char s in dic){
				//		statsString += s+" "+dic[s]+"\n";
			//		}
				//}
			//
				//foreach(Person rr in l.GetRoleRefPpl()){
				//	statsString += " "+rr.name+"\n";
				//}
			*/
			statsString += "\n";
		}


		/*foreach(Link l in interPersonal){
				if(l.roleRef_LvlOfInfl.ContainsKey(p)){
					l.roleRef_LvlOfInfl[p] += Calculator.UnboundAdd(val,l.roleRef_LvlOfInfl[p]);
				}
			}
			*/

	//

	public float HowLongAgo(float eventTime){
		if (eventTime == 0) {
			return Mathf.Infinity;
		}
		return time - eventTime;
	}
}