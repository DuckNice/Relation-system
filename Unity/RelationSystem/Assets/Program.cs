using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

    //Namespaces
using NRelationSystem;


public partial class Program : MonoBehaviour
{       
    RelationSystem relationSystem = new RelationSystem ();
 
        //Threading work.
	public void Start()
    {
		UIFunctions.WriteGameLine ("Welcome to Mask\n\n");
        relationSystem.program = this;
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

        if(sepInput[0] == "display")
        {
            relationSystem.PrintPersonStatus();
        }
        else if (relationSystem.posActions.ContainsKey(sepInput[0]))
        {
            MAction actionToDo = relationSystem.posActions[sepInput[0]];
                
            if(sepInput.Length > 1)
            {
                Person target = relationSystem.pplAndMasks.GetPerson(sepInput[1]);

                if (target != null)
                {
                    PerformAction(target, actionToDo);
                }
            }
        }
        else if(sepInput[0] == "help")
        {
            if(sepInput.Length > 1)
            {
                switch(sepInput[1])
                {
                    case "people":
						UIFunctions.WritePlayerLine("List of actions:\n");

                        foreach(string personName in relationSystem.pplAndMasks.people.Keys)
							UIFunctions.WritePlayerLine("  " + personName + ".");

						break;
                    case "actions":
						UIFunctions.WritePlayerLine("List of actions:\n");

                        foreach(string actionNames in relationSystem.posActions.Keys)
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
        else if(sepInput[0] == "history")
        {
            UIFunctions.WritePlayerLine("");
        }
        else
        {
			UIFunctions.WritePlayerLine("Error: No command '" + sepInput[0] + "' recognized.\nWrite 'help' for list of commands.");
        }
    }


    

    void PerformAction(Person target, MAction action)
    {
		action.DoAction(relationSystem.pplAndMasks.GetPerson("Player"), target,new Rule("Empty", new MAction("Empty", 0.0f, 0.0f), null));
		    //passing empty rule because I don't know how to pass rules from player.
    }


	
}