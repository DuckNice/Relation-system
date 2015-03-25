using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading;
//using System.Windows;

    //Namespaces
using NRelationSystem;


public partial class Program : MonoBehaviour
{       
    volatile RelationSystem relationSystem = new RelationSystem ();
 
        //Threading work.
	public void Awake()
    {
		UIFunctions.WriteGameLine ("Welcome to Mask\n\n");
		CreateFirstRooms ();
		SetupActions ();
		CreateFirstMasks ();
		CreateFirstPeople ();
		CreateFirstBeings ();
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
		action.DoAction(relationSystem.pplAndMasks.GetPerson("Player"), target,new Rule("Empty", new MAction("Empty", 0.0f), null));
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
		relationSystem.AddAction(new MAction("flee", 1.0f, flee, relationSystem));

		ActionInvoker doNothing = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is doing absolutely nothing. What a bore.");
		};
		relationSystem.AddAction(new MAction("doNothing", -1.0f, doNothing, relationSystem));

// ---------- INTERPERSONAL ACTIONS
		ActionInvoker greet = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is greeting "+direct.name);
		};
		relationSystem.AddAction(new MAction("Greet", 0.1f, greet, relationSystem));

		ActionInvoker kiss = (subject, direct, indPpl, misc) =>
        {
			UIFunctions.WriteGameLine(subject.name + " is kissing " + direct.name);
        };
        relationSystem.AddAction(new MAction("kiss", 0.5f, kiss, relationSystem));

		ActionInvoker chooseAnotherAsPartner = (subject, direct, indPpl, misc) =>
		{
			UIFunctions.WriteGameLine(subject.name + " has chosen " + direct.name+" as their partner! How romantic.");
		};
		relationSystem.AddAction(new MAction("chooseAnotherAsPartner", 0.0f, chooseAnotherAsPartner, relationSystem));

		ActionInvoker stayAsPartner = (subject, direct, indPpl, misc) =>
		{
			UIFunctions.WriteGameLine(subject.name + " is staying with " + direct.name+". Nothing separates these two.");
		};
		relationSystem.AddAction(new MAction("stayAsPartner", 0.3f, stayAsPartner, relationSystem));

		ActionInvoker LeavePartner = (subject, direct, indPpl, misc) =>
		{
			UIFunctions.WriteGameLine(subject.name + " is leaving " + direct.name+"!");
		};
		relationSystem.AddAction(new MAction("LeavePartner", -0.3f, LeavePartner, relationSystem));

		ActionInvoker flirt = (subject, direct, indPpl, misc) =>
		{
			UIFunctions.WriteGameLine(subject.name + " is flirting with " + direct.name+".");
		};
		relationSystem.AddAction(new MAction("flirt", 0.1f, flirt, relationSystem));

		ActionInvoker chat = (subject, direct, indPpl, misc) =>
		{
			UIFunctions.WriteGameLine(subject.name + " is chatting with " + direct.name+".");
		};
		relationSystem.AddAction(new MAction("chat", 0.0f, chat, relationSystem));

		ActionInvoker giveGift = (subject, direct, indPpl, misc) =>
		{
			if(misc != null){
				UIFunctions.WriteGameLine(subject.name + " is giving the gift of "+misc+" to " + direct.name+".");
			}
			else{
				UIFunctions.WriteGameLine(subject.name + " is giving a gift to " + direct.name+".");
			}
		};
		relationSystem.AddAction(new MAction("giveGift", 0.0f, giveGift, relationSystem));

		ActionInvoker poison = (subject, direct, indPpl, misc) =>
		{
			UIFunctions.WriteGameLine(subject.name + " is poisoning " + direct.name+"! Oh no!");
		};
		relationSystem.AddAction(new MAction("poison", -0.7f, poison, relationSystem));

		ActionInvoker gossip = (subject, direct, indPpl, misc) =>
		{
			UIFunctions.WriteGameLine(subject.name + " is gossiping with " + direct.name);
		};
		relationSystem.AddAction(new MAction("gossip", -0.1f, gossip, relationSystem));

		ActionInvoker argue = (subject, direct, indPpl, misc) =>
		{
			UIFunctions.WriteGameLine(subject.name + " is arguing with " + direct.name+"!");
		};
		relationSystem.AddAction(new MAction("argue", -0.2f, argue, relationSystem));

		ActionInvoker demandToStopBeingFriendWith = (subject, direct, indPpl, misc) =>
		{
			UIFunctions.WriteGameLine(subject.name + " is demanding that " + direct.name+" stops being friends with ");
			foreach(Person p in indPpl){
				UIFunctions.WriteGameLine(p.name+" ");
			}
		};
		relationSystem.AddAction(new MAction("demandToStopBeingFriendWith", -0.5f, demandToStopBeingFriendWith, relationSystem));

		ActionInvoker makeDistraction = (subject, direct, indPpl, misc) =>
		{
			UIFunctions.WriteGameLine(subject.name + " is making a distraction for " + direct.name+"!");
		};
		relationSystem.AddAction(new MAction("makeDistraction", -0.2f, makeDistraction, relationSystem));

		ActionInvoker reminisce = (subject, direct, indPpl, misc) =>
		{
			UIFunctions.WriteGameLine(subject.name + " is reminiscing about old times with " + direct.name+"!");
		};
		relationSystem.AddAction(new MAction("reminisce", 0.1f, reminisce, relationSystem));

		ActionInvoker planAgainstOthers = (subject, direct, indPpl, misc) =>
		{
			UIFunctions.WriteGameLine(subject.name + " is planning something with " + direct.name+". Just look at them scheme over there!");
		};
		relationSystem.AddAction(new MAction("planAgainstOthers", -0.1f, planAgainstOthers, relationSystem));

		ActionInvoker deny = (subject, direct, indPpl, misc) =>
		{
			UIFunctions.WriteGameLine(subject.name + " is denying " + direct.name+" their wishes.");
		};
		relationSystem.AddAction(new MAction("deny", -0.1f, deny, relationSystem));

		ActionInvoker enthuseAboutGreatnessofPerson = (subject, direct, indPpl, misc) =>
		{
			UIFunctions.WriteGameLine(subject.name + " is saying how great a person " + direct.name+" is!");
		};
		relationSystem.AddAction(new MAction("enthuseAboutGreatnessofPerson", 0.1f, enthuseAboutGreatnessofPerson, relationSystem));

// ---------- CULTURAL ACTIONS

		ActionInvoker convict = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is convicting "+direct.name+" of commiting a crime. To Jail with him!");
			direct.absTraits.traits[TraitTypes.ShyBolsterous].AddToTraitValue(Calculator.unboundAdd(-0.2f,direct.absTraits.traits[TraitTypes.ShyBolsterous].GetTraitValue()));
			direct.absTraits.traits[TraitTypes.NiceNasty].AddToTraitValue(Calculator.unboundAdd(-0.4f,direct.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue()));
		};
		relationSystem.AddAction(new MAction("convict", 0.7f, convict, relationSystem));

		ActionInvoker fight = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is attempting to fight back against "+direct.name);
		};
		relationSystem.AddAction(new MAction("fightBack", 1.0f, fight, relationSystem));

		ActionInvoker bribe = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is attempting to bribe "+direct.name);
		};
		relationSystem.AddAction(new MAction("bribe", 0.2f, bribe, relationSystem));

		ActionInvoker argueInnocence = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is arguing "+direct.name+"'s innocence.");
		};
		relationSystem.AddAction(new MAction("argueInnocence", 0.0f, argueInnocence, relationSystem));

		ActionInvoker argueGuiltiness = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is arguing "+direct.name+"'s guilt!");
		};
		relationSystem.AddAction(new MAction("argueGuiltiness", 0.0f, argueGuiltiness, relationSystem));

		ActionInvoker steal = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is stealing from "+direct.name+". Will they get caught?");
		};
		relationSystem.AddAction(new MAction("steal", -0.5f, steal, relationSystem));

		ActionInvoker practiceStealing = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is practicing the arts of stealth. What are they intending!");
		};
		relationSystem.AddAction(new MAction("practiceStealing", -0.1f, practiceStealing, relationSystem));

		ActionInvoker askForHelpInIllicitActivity = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is asking "+direct.name+" for help in something... dangerous.");
		};
		relationSystem.AddAction(new MAction("askForHelpInIllicitActivity", -0.1f, askForHelpInIllicitActivity, relationSystem));

		ActionInvoker searchForThief = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is searching for the thief!");
		};
		relationSystem.AddAction(new MAction("searchForThief", 0.8f, searchForThief, relationSystem));

// ------ CULTURAL (CULT) ACTIONS

		ActionInvoker exclaimAboutGreatnessOfCult = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is saying how great this cult is to "+direct.name);
		};
		relationSystem.AddAction(new MAction("exclaimAboutGreatnessOfCult", 0.4f, exclaimAboutGreatnessOfCult, relationSystem));

		ActionInvoker enterCult = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is entering the cult.");
		};
		relationSystem.AddAction(new MAction("enterCult", 0.6f, enterCult, relationSystem));

		ActionInvoker exitCult = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is exiting the cult!");
		};
		relationSystem.AddAction(new MAction("exitCult", -0.6f, exitCult, relationSystem));

		ActionInvoker damnCult = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is damning the cult!");
		};
		relationSystem.AddAction(new MAction("damnCult", -0.3f, damnCult, relationSystem));

		ActionInvoker excommunicateFromCult = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is excommunicating "+direct.name+" from the cult");
		};
		relationSystem.AddAction(new MAction("excommunicateFromCult", -0.1f, excommunicateFromCult, relationSystem));

// -------- CULTURAL (MERCHANT) ACTIONS

		ActionInvoker buyCompany = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is making a deal to buy "+direct.name+"'s company");
		};
		relationSystem.AddAction(new MAction("buyCompany", 0.3f, buyCompany, relationSystem));

		ActionInvoker sellCompany = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is making a deal to sell a company");
		};
		relationSystem.AddAction(new MAction("sellCompany", 0.1f, sellCompany, relationSystem));

		ActionInvoker sabotage = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is sabotaging "+direct.name);
		};
		relationSystem.AddAction(new MAction("sabotage", -0.5f, sabotage, relationSystem));

		ActionInvoker advertise = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is advertising for their wares!");
		};
		relationSystem.AddAction(new MAction("advertise", 0.3f, advertise, relationSystem));

		ActionInvoker convinceToLeaveGuild = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is convincing "+direct.name+" to leave the merchant's guild!");
		};
		relationSystem.AddAction(new MAction("convinceToLeaveGuild", -0.1f, convinceToLeaveGuild, relationSystem));

		ActionInvoker DemandtoLeaveGuild = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is demanding "+direct.name+" to leave the merchant's guild!");
		};
		relationSystem.AddAction(new MAction("DemandtoLeaveGuild", -0.3f, DemandtoLeaveGuild, relationSystem));

		ActionInvoker askForHelp = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is asking "+direct.name+" for help");
		};
		relationSystem.AddAction(new MAction("askForHelp", 0.3f, askForHelp, relationSystem));

		ActionInvoker buyGoods = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is buying goods from "+direct.name);
		};
		relationSystem.AddAction(new MAction("buyGoods", 0.3f, buyGoods , relationSystem));

		ActionInvoker sellGoods = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is selling goods to "+direct.name);
		};
		relationSystem.AddAction(new MAction("sellGoods", 0.3f, sellGoods , relationSystem));

	}
}