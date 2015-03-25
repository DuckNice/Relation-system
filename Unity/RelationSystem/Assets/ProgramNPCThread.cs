using System;
using System.Collections.Generic;
using System.Threading;

//Namespaces
using NRelationSystem;

public partial class Program
{
    List<Being> beings = new List<Being>();
	bool shouldPlay = false;
	public RoomManager roomMan;
	public float timePace;

    void Update()
	{
		if(shouldPlay)
		{
			foreach (Being being in beings) 
			{
				being.NPCAction();
			}
		}
	}


	public void setPlaying(bool _shouldPlay)
	{
		shouldPlay = _shouldPlay;
	}
}
