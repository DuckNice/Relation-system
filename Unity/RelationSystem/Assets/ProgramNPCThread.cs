using System;
using System.Collections.Generic;
using System.Threading;

//Namespaces
using NRelationSystem;


public class Room
{
	public string name;
	List<Being> occupants = new List<Being>();
	int maxOccupants;

	public Room(string name)
	{
		this.name = name;
		maxOccupants = 999999999;
	}

	public bool AddOccupant(Being newOccupant)
	{
		if(occupants.Contains(newOccupant) || occupants.Count >= maxOccupants)
		{
			return false;
		}

		occupants.Add (newOccupant);

		return true;
	}

	public bool RemoveOccupant(Being occupant)
	{
		if(!occupants.Contains(occupant))
		{
			return false;
		}

		occupants.Remove (occupant);

		return true;
	}
}


public partial class Program
{
    List<Being> beings = new List<Being>();
	List<Room> rooms = new List<Room>(); 
	bool shouldPlay = false;

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
