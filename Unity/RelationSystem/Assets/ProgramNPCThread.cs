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
}