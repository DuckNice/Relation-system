using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

	//Namespaces
using NRelationSystem;

public partial class Program : MonoBehaviour
{
	public UnityEngine.UI.Toggle playToggle;
    List<Being> beings = new List<Being>();
	public bool shouldPlay = false;
	public RoomManager roomMan;
	public float timePace;
    public float time = 0.0f;
    public bool actionStored = false;
    public MAction playerAction;
    Person playerTarget;
    public MAction currentPlayerAction;
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
                    if (people.Exists(x => x.name == being.name) && (being.name != "player" || debug.inst.playerActive))
                    {
                        being.NPCAction(time);
						roomMan.UpdateLvlOfInfl(GetPerson(being.name),0.01f);
                    }
                }
            }

			if(debug.Toggle){
				UpdateStats();
				UIFunctions.WriteGameStatsInWindow(statsString);
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
            action.DoAction(GetPerson("Player"), target, new Rule("Empty", new MAction("Empty", 0.0f, 0.0f), null, null));
        }
        else
        {
            UIFunctions.WritePlayerLine("Warning: You are currently " + currentPlayerAction.name + "ing. Please wait another " + Decimal.Round((decimal)(currentPlayerAction.duration - (time - actionStartTime)),1) + " seconds for your action do finish.");
        }
    }


    public void Update()
    {
        if (shouldPlay)
        {
            time += Time.deltaTime;

			if(!UIFunctions.instance.exitButtonActive){
				if(time > 10){
					UIFunctions.ActivateExitButton();
				}
			}
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


		//statsString += "\n\n\nMask Links: \n";

	/*	foreach (Person p in relationSystem.pplAndMasks.people.Values) {
			statsString += p.name+"\n";
			statsString += ""+p.selfPerception.roleName+" "+p.selfPerception.GetlvlOfInfl()+"\n";

			foreach(Link l in p.culture){
				statsString += l.roleName;
				if(l._roleRef.Count == 0){ statsString += " "+l.GetlvlOfInfl()+"\n"; }
				foreach(Person rr in l.GetRoleRefPpl()){
					statsString += " "+l.GetlvlOfInfl(rr)+" "+rr.name+"\n";
				}
			}
			foreach(Link l in p.interPersonal){
				statsString += l.roleName;
				if(l._roleRef.Count == 0){ statsString += "\n"; }
				foreach(Person rr in l.GetRoleRefPpl()){
					statsString += " "+l.GetlvlOfInfl(rr)+" "+rr.name+"\n";
				}
			}
			statsString += "\n";
		}
		statsString += "\nSpaces\n";
*/

		/*foreach(Link l in interPersonal){
				if(l.roleRef_LvlOfInfl.ContainsKey(p)){
					l.roleRef_LvlOfInfl[p] += Calculator.UnboundAdd(val,l.roleRef_LvlOfInfl[p]);
				}
			}
			*/

	}

	public float HowLongAgo(float eventTime){
		if (eventTime == 0) {
			return Mathf.Infinity;
		}
		return time - eventTime;
	}
}