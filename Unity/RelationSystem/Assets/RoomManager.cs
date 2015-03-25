using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RoomManager {
	
	List<Room> rooms = new List<Room>(); 


	public void NewRoom(string newRoom)
	{
		if(rooms.Exists(x => x.name == newRoom))
		{
			debug.Write ("Warning: Room with name '" + newRoom + "' already exists. Not adding room.");

			return;
		}

		rooms.Add (new Room(newRoom));
	}
	
	
	public bool EnterRoom(string enterRoom, Being subject)
	{
		int index = rooms.FindIndex (x => x.name == enterRoom);

		if(index > -1)
		{
			if(!rooms[index].OccupantExists(subject))
			{
				rooms[index].AddOccupant(subject);
			}
			else
			{
				debug.Write("Warning: Occupant '" + subject.name + "' already exists in room '" + enterRoom + "' not adding subject.");
			}
		}
		else
		{
			debug.Write ("Error: Couldn't find room with name '" + enterRoom + "'.");

			return false;
		}

		foreach(Room room in rooms)
		{
			room.RemoveOccupant(subject);
		}

		return true;
	}
}


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


	public bool OccupantExists(Being occupant)
	{
		if(occupants.Contains(occupant))
		{
			return true;
		}

		return false;
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