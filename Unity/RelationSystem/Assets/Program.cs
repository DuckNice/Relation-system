using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine.UI;

    //Namespaces
using NRelationSystem;


public partial class Program : MonoBehaviour
{       
    public static RelationSystem relationSystem = new RelationSystem ();
    public static CentralStoryManager storyManager;
    public GameObject dataFetchingPanel;
	public GameObject errorPanel;
    public GameObject dynamicActionsUI;

        //Threading work.
	public void Start()
    {
        SystemVersionManager.program = this;

		UIFunctions.WriteGameLine ("Welcome. Press play toggle to start\n\n");
        RelationSystem.program = this;
        MAction.relationSystem = relationSystem;

        SetupGame();

        dataFetchingPanel.SetActive(true);
        //Thread thread = new Thread(SystemVersionManager.PlayingVersion);
        //thread.IsBackground = true;
        //thread.Start();
        shouldStart = true;
	}


    public void SetupGame()
    {
        CreateFirstRooms();
        SetupActions();
        CreateFirstMasks();
        CreateFirstPeople();
        CreateFirstBeings();

        storyManager = gameObject.AddComponent<CentralStoryManager>();
        storyManager.beings = beings;
    }


    public volatile bool shouldStart = true;
    public volatile bool playerActive = false;
	public volatile bool couldntStart = false;
    bool isStarted = false;


    void Update()
    {
		if(couldntStart){
			dataFetchingPanel.SetActive(false);
			errorPanel.SetActive(true);
			Time.timeScale = 0;
		}

        if(!isStarted && shouldStart)
        {
            debug.inst.playerActive = playerActive;
            dataFetchingPanel.SetActive(false);
			debug.SetPlayerActiveness();
            StartCoroutine("NPCUpdate");
            isStarted = true;
            print("Started");
        }
    }


    public void playerInput(string input)
    {
        input = input.ToLower();

        string[] seps = {" "};

        string[] sepInput = input.Split(seps, StringSplitOptions.RemoveEmptyEntries);

        if (isStarted && sepInput != null && sepInput.Length > 0)
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
								if(roomMan.IsPersonInSameRoomAsMe(GetPerson(playerName),GetPerson(sepInput[1]))){
									playerAction = actionToDo;
									playerTarget = target;
									actionStored = true;
                                    dynamicActionsUI.SetActive(false);
									UIFunctions.WritePlayerLine("You did action: "+sepInput[0],false);
								}
								else{
									UIFunctions.WritePlayerLine(""+CapitalizeName(sepInput[1])+" is in another room. You can't do actions to them here.");

                                    WriteToRecentActionsPanel("" + CapitalizeName(sepInput[1]) + " is in another room. You can't do actions to them here.");
                                }
                            }
                            else
                            {
                                UIFunctions.WritePlayerLine("Error: didn't recognize '" + sepInput[1] + "' as valid target. Not doing action.");
                                WriteToRecentActionsPanel("Error: didn't recognize '" + sepInput[1] + "' as valid target. Not doing action.");

                            }
                        }
                        else if (!actionToDo.NeedsDirect)
                        {
                            playerAction = actionToDo;
                            actionStored = true;
                            dynamicActionsUI.SetActive(false);
							UIFunctions.WritePlayerLine("You did action: "+sepInput[0],false);
                        }
                        else
                        {
                            UIFunctions.WritePlayerLine("Action needs a target person.");
                            WriteToRecentActionsPanel("Action needs a target person.");
                        }
                    }
                    else
                    {
                        UIFunctions.WritePlayerLine("Already doing action: " + playerAction.name + " to " + playerTarget.name + ". Write 'cancel' to cancel current action selection.");
                        WriteToRecentActionsPanel("Already doing action: " + playerAction.name + " to " + playerTarget.name + ". Write 'cancel' to cancel current action selection.");
                    }
                }
				else{
					UIFunctions.WritePlayerLine("You are in Jail! You can't do anything! Now think about what you've done.");
                    WriteToRecentActionsPanel("You are in Jail! You can't do anything! Now think about what you've done.");
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


    private void WriteToRecentActionsPanel(string input)
    {
        UIFunctions.instance.RecentActionsText.text = ("<color=#ff0000>" + input + "</color>\n");

        if (UIFunctions.instance.RecentActionsText.text.Length > 150)
        {
            int lastNewline = UIFunctions.instance.RecentActionsText.text.IndexOf("\n");
            UIFunctions.instance.RecentActionsText.text = UIFunctions.instance.RecentActionsText.text.Remove(0, lastNewline + 1);
        }
    }
}