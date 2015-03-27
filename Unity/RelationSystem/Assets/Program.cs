﻿using UnityEngine;
using System;
using System.Collections.Generic;
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
        else if(sepInput[0] == "do")
        {
            if(sepInput.Length > 1)
            {
                Person target = relationSystem.pplAndMasks.GetPerson(sepInput[1]);

                if(target != null)
                {
                    if(sepInput.Length > 2)
                    {
                        if (relationSystem.posActions.ContainsKey(sepInput[2]))
                        {
                            MAction actionToDo = relationSystem.posActions[sepInput[2]];

                            PerformAction(target, actionToDo);
                        }
                    }
                }
                else
                {
					UIFunctions.WritePlayerLine("'Do' recognized, but second parameter not found in list of people.");
                }
            }
            else
            {
				UIFunctions.WritePlayerLine("'Do' what?");
            }
        }
        else if(sepInput[0] == "help")
        {
            if(sepInput.Length > 1)
            {
                switch(sepInput[1])
                {
                    case "do":
						UIFunctions.WritePlayerLine("Nothing here yet");
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
				UIFunctions.WritePlayerLine((string)"'display <person>': Get information about character.");
				UIFunctions.WritePlayerLine("'do <person> <action>': Perform the mentioned <action> interacting with the stated <person>");
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
        NPCActions();
    }


    void NPCActions() 
    {
        foreach(Being being in beings)
        {
			being.NPCAction();
        }
    }


	void SetupActions()
	{
// ---------- SELF ACTIONS

		ActionInvoker flee = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is attempting to flee the scene!");
		};
		relationSystem.AddAction(new MAction("flee", 1.0f, relationSystem, flee));

		ActionInvoker doNothing = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is doing absolutely nothing. What a bore.");
		};
        relationSystem.AddAction(new MAction("doNothing", -1.0f, relationSystem, doNothing));

// ---------- INTERPERSONAL ACTIONS
		ActionInvoker greet = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is greeting "+direct.name);


		};
        relationSystem.AddAction(new MAction("Greet", 0.1f, relationSystem, greet));

		ActionInvoker kiss = (subject, direct, indPpl, misc) =>
        {
			UIFunctions.WriteGameLine(subject.name + " is kissing " + direct.name);
			direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.5f,direct.moods[MoodTypes.hapSad]);
			direct.moods[MoodTypes.arousDisgus] += Calculator.unboundAdd(0.5f,direct.moods[MoodTypes.arousDisgus]);
			subject.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.5f,subject.moods[MoodTypes.hapSad]);
			subject.moods[MoodTypes.arousDisgus] += Calculator.unboundAdd(0.5f,subject.moods[MoodTypes.arousDisgus]);
        };
        relationSystem.AddAction(new MAction("kiss", 0.5f, relationSystem, kiss));

		ActionInvoker chooseAnotherAsPartner = (subject, direct, indPpl, misc) =>
		{
			UIFunctions.WriteGameLine(subject.name + " has chosen " + direct.name+" as their partner! How romantic.");

			direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.6f,direct.moods[MoodTypes.hapSad]);
			direct.moods[MoodTypes.arousDisgus] += Calculator.unboundAdd(0.6f,direct.moods[MoodTypes.arousDisgus]);
			subject.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.6f,subject.moods[MoodTypes.hapSad]);
			subject.moods[MoodTypes.arousDisgus] += Calculator.unboundAdd(0.6f,subject.moods[MoodTypes.arousDisgus]);

			foreach(Person p in indPpl){
				p.moods[MoodTypes.hapSad] += Calculator.unboundAdd(-0.4f,p.moods[MoodTypes.hapSad]);
				p.moods[MoodTypes.arousDisgus] += Calculator.unboundAdd(-0.4f,p.moods[MoodTypes.arousDisgus]);
			}

		};
        relationSystem.AddAction(new MAction("chooseAnotherAsPartner", 0.4f, relationSystem, chooseAnotherAsPartner));

		ActionInvoker stayAsPartner = (subject, direct, indPpl, misc) =>
		{
			UIFunctions.WriteGameLine(subject.name + " is staying with " + direct.name+". Nothing separates these two.");
			direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.3f,direct.moods[MoodTypes.hapSad]);
			subject.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.1f,subject.moods[MoodTypes.hapSad]);
		};
        relationSystem.AddAction(new MAction("stayAsPartner", 0.3f, relationSystem, stayAsPartner));

		ActionInvoker LeavePartner = (subject, direct, indPpl, misc) =>
		{
			UIFunctions.WriteGameLine(subject.name + " is leaving " + direct.name+"!");

			direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(-0.7f,direct.moods[MoodTypes.hapSad]);
			subject.moods[MoodTypes.hapSad] += Calculator.unboundAdd(-0.2f,subject.moods[MoodTypes.hapSad]);

		};
        relationSystem.AddAction(new MAction("LeavePartner", -0.3f, relationSystem, LeavePartner));

		ActionInvoker flirt = (subject, direct, indPpl, misc) =>
		{
			UIFunctions.WriteGameLine(subject.name + " is flirting with " + direct.name+".");
			direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.2f,direct.moods[MoodTypes.hapSad]);
			direct.moods[MoodTypes.arousDisgus] += Calculator.unboundAdd(0.2f,direct.moods[MoodTypes.arousDisgus]);
			subject.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.2f,subject.moods[MoodTypes.hapSad]);
			subject.moods[MoodTypes.arousDisgus] += Calculator.unboundAdd(0.2f,subject.moods[MoodTypes.arousDisgus]);
		};
        relationSystem.AddAction(new MAction("flirt", 0.1f, relationSystem, flirt));

		ActionInvoker chat = (subject, direct, indPpl, misc) =>
		{
			UIFunctions.WriteGameLine(subject.name + " is chatting with " + direct.name+".");
			direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.2f,direct.moods[MoodTypes.hapSad]);
			subject.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.2f,subject.moods[MoodTypes.hapSad]);
		};
        relationSystem.AddAction(new MAction("chat", 0.0f, relationSystem, chat));

		ActionInvoker giveGift = (subject, direct, indPpl, misc) =>
		{
			if(misc != null){
				UIFunctions.WriteGameLine(subject.name + " is giving the gift of "+misc+" to " + direct.name+".");
			}
			else{
				UIFunctions.WriteGameLine(subject.name + " is giving a gift to " + direct.name+".");
			}
			direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.5f,direct.moods[MoodTypes.hapSad]);
		};
        relationSystem.AddAction(new MAction("giveGift", 0.2f, relationSystem, giveGift));

		ActionInvoker poison = (subject, direct, indPpl, misc) =>
		{
			UIFunctions.WriteGameLine(subject.name + " is poisoning " + direct.name+"! Oh no!");
			direct.moods[MoodTypes.angryFear] += Calculator.unboundAdd(-0.8f,direct.moods[MoodTypes.angryFear]);
			direct.moods[MoodTypes.energTired] += Calculator.unboundAdd(-0.5f,direct.moods[MoodTypes.energTired]);
		};
        relationSystem.AddAction(new MAction("poison", -0.4f, relationSystem, poison));

		ActionInvoker gossip = (subject, direct, indPpl, misc) =>
		{
			UIFunctions.WriteGameLine(subject.name + " is gossiping with " + direct.name);
		};
        relationSystem.AddAction(new MAction("gossip", 0.1f, relationSystem, gossip));

		ActionInvoker argue = (subject, direct, indPpl, misc) =>
		{
			UIFunctions.WriteGameLine(subject.name + " is arguing with " + direct.name+"!");
			direct.moods[MoodTypes.angryFear] += Calculator.unboundAdd(0.3f,direct.moods[MoodTypes.angryFear]);
			subject.moods[MoodTypes.angryFear] += Calculator.unboundAdd(0.3f,subject.moods[MoodTypes.angryFear]);
			direct.moods[MoodTypes.energTired] += Calculator.unboundAdd(0.3f,direct.moods[MoodTypes.energTired]);
			subject.moods[MoodTypes.energTired] += Calculator.unboundAdd(0.3f,subject.moods[MoodTypes.energTired]);
		};
        relationSystem.AddAction(new MAction("argue", -0.2f, relationSystem, argue));

		ActionInvoker demandToStopBeingFriendWith = (subject, direct, indPpl, misc) =>
		{
			UIFunctions.WriteGameLine(subject.name + " is demanding that " + direct.name+" stops being friends with ");
			//I HAVE NO WAY OF TESTING IF THIS WORKS
			foreach(Person p in indPpl){
				UIFunctions.WriteGameLine(p.name+" ");
				foreach(Link l in p.interPersonal){
					if(l.roleRef.Contains(direct)){ direct.interPersonal.Remove(l); }
				}
			}
		};
        relationSystem.AddAction(new MAction("demandToStopBeingFriendWith", -0.5f, relationSystem, demandToStopBeingFriendWith));

		ActionInvoker makeDistraction = (subject, direct, indPpl, misc) =>
		{
			UIFunctions.WriteGameLine(subject.name + " is making a distraction for " + direct.name+"!");
			int rand = UnityEngine.Random.Range(0,2);
			if(rand == 0){
				UIFunctions.WriteGameLine("It was a success! They are very distracted");
				direct.moods[MoodTypes.energTired] += Calculator.unboundAdd(-0.5f,direct.moods[MoodTypes.energTired]);
				subject.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.3f,subject.moods[MoodTypes.hapSad]);
			}
			else{
				UIFunctions.WriteGameLine("It failed. They are now more wary than before.");
				direct.moods[MoodTypes.energTired] += Calculator.unboundAdd(0.3f,direct.moods[MoodTypes.energTired]);
				direct.moods[MoodTypes.angryFear] += Calculator.unboundAdd(-0.3f,direct.moods[MoodTypes.angryFear]);
				subject.moods[MoodTypes.hapSad] += Calculator.unboundAdd(-0.4f,subject.moods[MoodTypes.hapSad]);
			}

		};
        relationSystem.AddAction(new MAction("makeDistraction", 0.2f, relationSystem, makeDistraction));

		ActionInvoker reminisce = (subject, direct, indPpl, misc) =>
		{
			UIFunctions.WriteGameLine(subject.name + " is reminiscing about old times with " + direct.name+"!");
			direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.2f,direct.moods[MoodTypes.hapSad]);
			subject.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.2f,subject.moods[MoodTypes.hapSad]);
		};
        relationSystem.AddAction(new MAction("reminisce", 0.1f, relationSystem, reminisce));

		ActionInvoker deny = (subject, direct, indPpl, misc) =>
		{
			UIFunctions.WriteGameLine(subject.name + " is denying " + direct.name+" their wishes.");
			direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(-0.4f,direct.moods[MoodTypes.hapSad]);
		};
        relationSystem.AddAction(new MAction("deny", 0.4f, relationSystem, deny));

		ActionInvoker enthuseAboutGreatnessofPerson = (subject, direct, indPpl, misc) =>
		{
			UIFunctions.WriteGameLine(subject.name + " is saying how great a person " + direct.name+" is!");
			direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.4f,direct.moods[MoodTypes.hapSad]);


			subject.GetRule("enthuseaboutgreatnessofperson").SetRuleStrength(-1.0f);


		};
        relationSystem.AddAction(new MAction("enthuseAboutGreatnessofPerson", 0.4f, relationSystem, enthuseAboutGreatnessofPerson));

// ---------- CULTURAL ACTIONS

		ActionInvoker convict = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is convicting "+direct.name+" of commiting a crime. To Jail with him!");
			direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(-0.8f,direct.moods[MoodTypes.hapSad]);
			direct.moods[MoodTypes.angryFear] += Calculator.unboundAdd(0.6f,direct.moods[MoodTypes.angryFear]);
		};
        relationSystem.AddAction(new MAction("convict", 0.7f, relationSystem, convict));

		ActionInvoker fight = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is attempting to fight back against "+direct.name);
			direct.moods[MoodTypes.angryFear] += Calculator.unboundAdd(0.6f,direct.moods[MoodTypes.angryFear]);
			direct.moods[MoodTypes.energTired] += Calculator.unboundAdd(0.6f,direct.moods[MoodTypes.energTired]);
		};
        relationSystem.AddAction(new MAction("fight", 1.0f, relationSystem, fight));

		ActionInvoker bribe = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is attempting to bribe "+direct.name);
			direct.moods[MoodTypes.angryFear] += Calculator.unboundAdd(0.6f,direct.moods[MoodTypes.angryFear]);
			//MONEY!
		};
        relationSystem.AddAction(new MAction("bribe", 0.6f, relationSystem, bribe));

		ActionInvoker argueInnocence = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is arguing "+direct.name+"'s innocence.");
			direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.3f,direct.moods[MoodTypes.hapSad]);
		};
        relationSystem.AddAction(new MAction("argueInnocence", 0.0f, relationSystem, argueInnocence));

		ActionInvoker argueGuiltiness = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is arguing "+direct.name+"'s guilt!");
			direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(-0.3f,direct.moods[MoodTypes.hapSad]);
		};
        relationSystem.AddAction(new MAction("argueGuiltiness", 0.0f, relationSystem, argueGuiltiness));

		ActionInvoker steal = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is stealing from "+direct.name+". Will they get caught?");
			subject.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.6f,subject.moods[MoodTypes.hapSad]);
			subject.moods[MoodTypes.angryFear] += Calculator.unboundAdd(-0.3f,subject.moods[MoodTypes.angryFear]);
			//money
		};
        relationSystem.AddAction(new MAction("steal", 0.9f, relationSystem, steal));

		ActionInvoker practiceStealing = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is practicing the arts of stealth. What are they intending!");
			subject.AddToAbility(Calculator.unboundAdd(0.2f,subject.GetAbilityy()));
		};
        relationSystem.AddAction(new MAction("practiceStealing", 0.2f, relationSystem, practiceStealing));

		ActionInvoker askForHelpInIllicitActivity = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is asking "+direct.name+" for help in something... dangerous.");
		};
        relationSystem.AddAction(new MAction("askForHelpInIllicitActivity", 0.4f, relationSystem, askForHelpInIllicitActivity));

		ActionInvoker searchForThief = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is searching for the thief!");
		};
        relationSystem.AddAction(new MAction("searchForThief", 0.6f, relationSystem, searchForThief));

// ------ CULTURAL (CULT) ACTIONS

		ActionInvoker praiseCult = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is saying how great this cult is to "+direct.name);
		};
        relationSystem.AddAction(new MAction("praiseCult", 0.1f, relationSystem, praiseCult));

		ActionInvoker enterCult = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is entering the cult.");
		};
        relationSystem.AddAction(new MAction("enterCult", 0.0f, relationSystem, enterCult));

		ActionInvoker exitCult = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is exiting the cult!");
		};
        relationSystem.AddAction(new MAction("exitCult", 0.0f, relationSystem, exitCult));

		ActionInvoker damnCult = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is damning the cult!");
		};
        relationSystem.AddAction(new MAction("damnCult", -0.2f, relationSystem, damnCult));

		ActionInvoker excommunicateFromCult = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is excommunicating "+direct.name+" from the cult");
		};
        relationSystem.AddAction(new MAction("excommunicateFromCult", 0.0f, relationSystem, excommunicateFromCult));

// -------- CULTURAL (MERCHANT) ACTIONS

		ActionInvoker buyCompany = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is making a deal to buy "+direct.name+"'s company");
		};
        relationSystem.AddAction(new MAction("buyCompany", 0.8f, relationSystem, buyCompany));

		ActionInvoker sellCompany = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is making a deal to sell a company");
		};
        relationSystem.AddAction(new MAction("sellCompany", 0.4f, relationSystem, sellCompany));

		ActionInvoker sabotage = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is sabotaging "+direct.name);
			direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(-0.6f,direct.moods[MoodTypes.hapSad]);
			direct.moods[MoodTypes.angryFear] += Calculator.unboundAdd(0.4f,direct.moods[MoodTypes.angryFear]);
		};
        relationSystem.AddAction(new MAction("sabotage", 0.5f, relationSystem, sabotage));

		ActionInvoker advertise = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is advertising for their wares!");
		};
        relationSystem.AddAction(new MAction("advertise", 0.3f, relationSystem, advertise));

		ActionInvoker convinceToLeaveGuild = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is convincing "+direct.name+" to leave the merchant's guild!");
			direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(-0.2f,direct.moods[MoodTypes.hapSad]);
		};
        relationSystem.AddAction(new MAction("convinceToLeaveGuild", 0.4f, relationSystem, convinceToLeaveGuild));

		ActionInvoker DemandtoLeaveGuild = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is demanding "+direct.name+" to leave the merchant's guild!");
			direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(-0.6f,direct.moods[MoodTypes.hapSad]);
		};
        relationSystem.AddAction(new MAction("DemandtoLeaveGuild", 0.4f, relationSystem, DemandtoLeaveGuild));

		ActionInvoker askForHelp = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is asking "+direct.name+" for help");
		};
        relationSystem.AddAction(new MAction("askForHelp", 0.5f, relationSystem, askForHelp));

		ActionInvoker buyGoods = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is buying goods from "+direct.name);
		};
        relationSystem.AddAction(new MAction("buyGoods", 0.4f, relationSystem, buyGoods));

		ActionInvoker sellGoods = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is selling goods to "+direct.name);
		};
        relationSystem.AddAction(new MAction("sellGoods", 0.7f, relationSystem, sellGoods));

	}
}