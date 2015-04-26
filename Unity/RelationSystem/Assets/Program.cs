using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

    //Namespaces
using NRelationSystem;


public partial class Program : MonoBehaviour
{       
    public RelationSystem relationSystem = new RelationSystem ();
    public static string playerName { get { return RelationSystem.playerName; } }
 
        //Threading work.
	public void Start()
    {
		UIFunctions.WriteGameLine ("Welcome. Press play toggle to start\n\n");
        RelationSystem.program = this;
        MAction.relationSystem = relationSystem;
		CreateFirstRooms ();
		SetupActions ();
		CreateFirstMasks ();
		CreateFirstPeople ();
		CreateFirstBeings ();

        StartCoroutine("NPCUpdate");
	}


    public void playerInput(string input)
    {
        input = input.ToLower();

        string[] seps = {" "};

        string[] sepInput = input.Split(seps, StringSplitOptions.RemoveEmptyEntries);

        if (sepInput != null && sepInput.Length > 0)
        {
            if (relationSystem.posActions.ContainsKey(sepInput[0]))
            {
                if(roomMan.GetRoomIAmIn(GetPerson(playerName)) != "Jail")
                {
                    if (!actionStored)
                    {
                        MAction actionToDo = relationSystem.posActions[sepInput[0]];

                        if (sepInput.Length > 1)
                        {
                            Person target = relationSystem.pplAndMasks.GetPerson(sepInput[1]);

                            if (target != null)
                            {
                                playerAction = actionToDo;
                                playerTarget = target;
                                actionStored = true;
                            }
                            else
                            {
                                UIFunctions.WritePlayerLine("Error: didn't recognize '" + sepInput[1] + "' as valid target. Not doing action.");
                            }
                        }
                        else if (!actionToDo.NeedsDirect)
                        {
                            playerAction = actionToDo;
                            actionStored = true;
                        }
                        else
                        {
                            UIFunctions.WritePlayerLine("Action needs a target person. Not doing action.");
                        }
                    }
                    else
                    {
                        UIFunctions.WritePlayerLine("Already doing action: " + playerAction.name + " for target: " + playerTarget.name + ". Write 'cancel' to cancel current action selection.");
                    }
                }
				else{
					UIFunctions.WritePlayerLine("You are in Jail! You can't do anything! Now think about what you've done.");
				}
            }
            else if (sepInput[0] == "cancel")
            {
                actionStored = false;
                UIFunctions.WritePlayerLine("Removed current action selection. Ready to assign new action to player.");
            }
            else if (sepInput[0] == "help")
            {
                if (sepInput.Length > 1)
                {
                    switch (sepInput[1])
                    {
                        case "people":
                            UIFunctions.WritePlayerLine("List of actions:\n");

                            foreach (string personName in relationSystem.pplAndMasks.people.Keys)
                                UIFunctions.WritePlayerLine("  " + personName + ".");

                            break;
                        case "actions":
                            UIFunctions.WritePlayerLine("List of actions:\n");

                            foreach (string actionNames in relationSystem.posActions.Keys)
                                UIFunctions.WritePlayerLine("  " + actionNames + ".");

                            break;
                    }
                }
                else
                {
                    UIFunctions.WritePlayerLine("'display <person>': Get information about character.");
                    UIFunctions.WritePlayerLine("'<action> <person>': Perform the mentioned <action> interacting with the stated <person>");
                    UIFunctions.WritePlayerLine("'history': Show the history log.");
                }
            }
            else if (sepInput[0] == "history")
            {
                UIFunctions.WritePlayerLine("");
            }
            else
            {
                UIFunctions.WritePlayerLine("Error: No command '" + sepInput[0] + "' recognized.\nWrite 'help' for list of commands.");
            }
        }
    }
}