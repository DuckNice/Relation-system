using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using NRelationSystem;

public class RoomManager {

    RelationSystem relSys;

    public void EnterRoom(string roomName, Person person)
    {
        foreach (string key in relSys.updateLists.Keys)
        {
            relSys.RemovePersonFromUpdateList(key, person);
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
}