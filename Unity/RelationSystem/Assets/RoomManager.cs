using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using NRelationSystem;

public class RoomManager {

    RelationSystem relSys;
	string roomWindowString;

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
		UpdateRoomWindow ();
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


    public void UpdateLvlOfInfl(Person self, float changeValue)
    {
        List<Person> personRoom = relSys.updateLists.First(x => x.Value.Contains(self)).Value;

        List<Mask> cultureMasks = new List<Mask>();

        foreach(Person person in personRoom)
        {
            foreach(Link link in person.culture)
            {
                if(!cultureMasks.Contains(link._roleMask))
                {
                    cultureMasks.Add(link._roleMask);
                }
            }
        }

        foreach(Link link in new List<Link>(self.culture))
        {
            foreach(Person person in new List<Person>(link.GetRoleRefPpl())){
                foreach (string role in new List<string>(link._roleRefs[person].Keys))
                {
                    if (cultureMasks.Exists(x => x == link._roleMask))
                        link.AddToLvlOfInfl(changeValue, role, person);
                    else
                        link.AddToLvlOfInfl(-changeValue, role, person);
                }
            }
        }

        foreach (Link link in new List<Link>(self.interPersonal))
        {
            foreach (Person person in new List<Person>(link.GetRoleRefPpl()))
            {
                foreach (string role in new List<string>(link._roleRefs[person].Keys))
                {
                    if (personRoom.Contains(person))
                        link.AddToLvlOfInfl(changeValue, role, person);
                    else
                        link.AddToLvlOfInfl(-changeValue, role, person);
                }
            }
        }
    }

	public void UpdateRoomWindow(){
		roomWindowString = "\n\n<b>Rooms:</b> \n\n";
		int i = 0;
		foreach (string s in relSys.updateLists.Keys) {
			if(i==0){
				roomWindowString += "<color=#00ff00ff>"+s+":</color> \n";
			}
			else if(i==1){
				roomWindowString += "<color=#008080ff>"+s+":</color> \n";
			}
			else if(i==2){
				roomWindowString += "<color=#00ffffff>"+s+":</color> \n";
			}
			else if(i==3){
				roomWindowString += "<color=#ff0000ff>"+s+":</color> \n";
			}
			for (int g = 0; g < relSys.updateLists[s].Count; g++)
			{
				if(relSys.updateLists[s].Count-1==g){
					roomWindowString += "<i>"+relSys.CapitalizeName(relSys.updateLists[s][g].name)+".</i> ";
				}
				else{
					roomWindowString += "<i>"+relSys.CapitalizeName(relSys.updateLists[s][g].name)+",</i> ";
				}
			}
			roomWindowString += "\n\n";
			i++;
		}
		UIFunctions.WriteRoomsInWindow (roomWindowString);
	}


}