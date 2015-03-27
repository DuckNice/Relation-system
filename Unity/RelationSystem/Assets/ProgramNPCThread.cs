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


    IEnumerator NPCUpdate()
	{
        while (true)
        {
            if (shouldPlay)
            {
                foreach (Being being in beings)
                {
                    being.NPCAction();
                }
            }

			if(debug.Toggle){
				UpdateStats();
				UIFunctions.WriteGameStatsInWindow(statsString);
			}


            yield return new WaitForSeconds(timePace);
        }
	}


	public void setPlaying()
	{
		shouldPlay = playToggle.isOn;
	}
}