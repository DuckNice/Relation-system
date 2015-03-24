using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading;
//using System.Windows;

    //Namespaces
using NRelationSystem;


public partial class Program:MonoBehaviour
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




// ---------- INTERPERSONAL ACTIONS
		ActionInvoker greet = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is greeting "+direct.name);
		};
		relationSystem.AddAction(new MAction("Greet", 0.1f, greet, relationSystem));

		ActionInvoker compliment = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is complimenting "+direct.name);
		};
		relationSystem.AddAction(new MAction("Compliment", 0.0f, compliment, relationSystem));

		ActionInvoker threaten = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is threatening "+direct.name);
		};
		relationSystem.AddAction(new MAction("Threaten", 0.0f, threaten, relationSystem));

		ActionInvoker accuse = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is accusing "+direct.name+" of heinous crimes!");
		};
		relationSystem.AddAction(new MAction("accuse", 0.0f, accuse, relationSystem));

		ActionInvoker kiss = (subject, direct, indPpl, misc) =>
        {
			UIFunctions.WriteGameLine(subject.name + " is kissing " + direct.name);
        };
        relationSystem.AddAction(new MAction("kiss", 0.5f, kiss, relationSystem));



// ---------- CULTURAL ACTIONS

		ActionInvoker doNothing = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is doing absolutely nothing. What a bore.");
		};
		relationSystem.AddAction(new MAction("doNothing", -1.0f, doNothing, relationSystem));

		ActionInvoker order = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is ordering "+direct.name+" to go away.");
		};
		relationSystem.AddAction(new MAction("Order", 0.1f, order, relationSystem));

		ActionInvoker lie = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is lying to "+direct.name+" about his own might");
		};
		relationSystem.AddAction(new MAction("Lie", 0.2f, lie, relationSystem));

		ActionInvoker plead_innocence = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is pleading innocence to "+direct.name);
			direct.absTraits.traits[TraitTypes.NiceNasty].AddToTraitValue(Calculator.unboundAdd(-0.2f,direct.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue()));
		};
		relationSystem.AddAction(new MAction("Plead", 0.5f, plead_innocence, relationSystem));

		ActionInvoker punch = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is PUNCHING "+direct.name+"!! OUCH");
		};
		relationSystem.AddAction(new MAction("punch", 0.4f, punch, relationSystem));

		ActionInvoker convict = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is convicting "+direct.name+" of commiting a crime. To Jail with him!");
			direct.absTraits.traits[TraitTypes.ShyBolsterous].AddToTraitValue(Calculator.unboundAdd(-0.2f,direct.absTraits.traits[TraitTypes.ShyBolsterous].GetTraitValue()));
			direct.absTraits.traits[TraitTypes.NiceNasty].AddToTraitValue(Calculator.unboundAdd(-0.4f,direct.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue()));
		};
		relationSystem.AddAction(new MAction("convict", 0.7f, convict, relationSystem));

		ActionInvoker flee = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is attempting to flee the scene!");
		};
		relationSystem.AddAction(new MAction("flee", 1.0f, flee, relationSystem));

		ActionInvoker fightBack = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is attempting to fight back against "+direct.name);
		};
		relationSystem.AddAction(new MAction("fightBack", 1.0f, fightBack, relationSystem));

		ActionInvoker bribe = (subject, direct, indPpl, misc) => 
		{
			UIFunctions.WriteGameLine(subject.name + " is attempting to bribe "+direct.name);
		};
		relationSystem.AddAction(new MAction("bribe", 0.2f, bribe, relationSystem));
	}
}