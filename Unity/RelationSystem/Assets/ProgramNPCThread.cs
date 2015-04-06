﻿using System;
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
	bool shouldPlay = false;
	public RoomManager roomMan;
	public float timePace;
    public float time = 0.0f;


    IEnumerator NPCUpdate()
	{
        while (true)
        {
            if (shouldPlay)
            {
                foreach (Being being in beings)
                {
					if(being.name != "player"){
						being.NPCAction(time);
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


    public void Update()
    {
        if(shouldPlay)
        {
            time += Time.deltaTime;
        }
    }


	public void setPlaying()
	{
		shouldPlay = playToggle.isOn;
	}



	
	
	void UpdateStats(){
		statsString = "";
		
		foreach (Person p in relationSystem.pplAndMasks.people.Values) {
			statsString += p.name+"\n";
			statsString += "AngFea: "+p.moods[MoodTypes.angryFear]+"\n";
			statsString += "aroDis: "+p.moods[MoodTypes.arousDisgus]+"\n";
			statsString += "EnrTir: "+p.moods[MoodTypes.energTired]+"\n";
			statsString += "HapSad: "+p.moods[MoodTypes.hapSad]+"\n";
			statsString += "\n";
		}
		statsString += "Money: \n";
		
		
		foreach (Being b in beings) {
			statsString +=  b.name+"  "+ b.possessions.Find(x=>x.Name == "money").value+"\n";
		}

		statsString += "\n\n\nMask Links: \n";


		foreach (Person p in relationSystem.pplAndMasks.people.Values) {
			statsString += p.name+"\n";
			statsString += ""+p.selfPerception.roleName+" "+p.selfPerception.GetlvlOfInfl()+"\n";

			foreach(Link l in p.culture){
				statsString += l.roleName+" "+l.GetlvlOfInfl();
				if(l.roleRef.Count == 0){ statsString += "\n"; }
				foreach(Person rr in l.roleRef){
					statsString += " "+rr.name+"\n";
				}
			}
			foreach(Link l in p.interPersonal){
				statsString += l.roleName+" "+l.GetlvlOfInfl();
				if(l.roleRef.Count == 0){ statsString += "\n"; }
				foreach(Person rr in l.roleRef){
					statsString += " "+rr.name+"\n";
				}
			}
			statsString += "\n";
		}


	}




}