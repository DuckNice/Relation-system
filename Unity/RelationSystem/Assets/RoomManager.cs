using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using NRelationSystem;

public class RoomManager {

    RelationSystem relSys;

    public RoomManager(RelationSystem _relSys)
    {
        relSys = _relSys;
    }

    public void EnterRoom(string roomName, Person person)
    {
        for (int i = 0; i < relSys.updateLists.Count; i++ )
        {
            string key = relSys.updateLists.Keys.ToArray()[i];

            if(relSys.RemovePersonFromUpdateList(key, person))
            {
                i--;
            }
        }

        relSys.AddPersonToUpdateList(roomName, person);
    }

    public string GetRoomIAmIn(Person person)
    {
        foreach(string key in relSys.updateLists.Keys)
        {
            if(relSys.updateLists[key].Contains(person))
                return key;
        }

        return "";
    }

	public bool IsPersonInSameRoomAsMe(Person self, Person other){
		foreach (string key in relSys.updateLists.Keys) {
			if (relSys.updateLists [key].Contains (self) && relSys.updateLists [key].Contains (other)) {
				return true;
			} 
		}
		return false;
	}

}