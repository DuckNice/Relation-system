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
    Thread NPCThread;
    private volatile bool stopNPCLoop = false;

	public void Awake()
    {
		UIFunctions.WriteGameLine("Welcome to Mask\n\n");
		SetupActions ();
		CreateFirstMasks();
		CreateFirstPeople();

		Being Bill = new Being ("Bill", relationSystem);
		Being Therese = new Being ("Therese", relationSystem);
		Being John = new Being ("John", relationSystem);
		
		beings.Add (Bill);
		beings.Add (Therese);
		beings.Add (John);
		Bill.FindFocusToAll (beings);


     /* NPCThread = new Thread(new ThreadStart(NPCThreadFunc));
        NPCThread.Start();

        int i = 1;

        while (!NPCThread.IsAlive)
        {
            switch (i)
            {
                case 1:
                    Debug.Log("NPC Thread starting.");
                    break;
                case 2:
					Debug.Log("NPC Thread starting..");
                    break;
                case 3:
					Debug.Log("NPC Thread starting...");
                    break;
                default:
					Debug.Log("NPC Thread starting....");
                    break;
            }

            i++;

            if (i > 3)
                i = 1;
            Thread.Sleep(200);
        }*/
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
					UIFunctions.WritePlayerLine("Error: 'do' recognized, but second parameter not found in list of people.");
                }
            }
            else
            {
				UIFunctions.WritePlayerLine("Error: 'do' what?");
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


    public void CreateFirstMasks()
    {
        relationSystem.CreateNewMask("Player", new float[]{}, new bool[]{}, TypeMask.selfPerc, new string[]{});

        relationSystem.CreateNewMask("Bungary", new float[] { 0.0f, -0.2f }, new bool[] { }, TypeMask.culture, new string[] { "Bunce", "Buncess", "Bunsant" });

        relationSystem.CreateNewMask("Bill", new float[] { 0.0f, 0.0f }, new bool[] { }, TypeMask.selfPerc, new string[] { "" });

        relationSystem.CreateNewMask("Therese", new float[] { 0.0f, 0.0f }, new bool[] { }, TypeMask.selfPerc, new string[] { "" });

        relationSystem.CreateNewMask("John", new float[] { 0.0f, 0.0f }, new bool[] { }, TypeMask.selfPerc, new string[] { "" });

        relationSystem.CreateNewMask("BillTherese", new float[] { 0.2f, -0.2f }, new bool[] { }, TypeMask.interPers, new string[] { "Married" });

        relationSystem.CreateNewMask("ThereseBill", new float[] { 0.3f, 0.2f }, new bool[] { }, TypeMask.interPers, new string[] { "Married" });

		relationSystem.CreateNewMask("BillJohn", new float[] { -0.2f, -0.2f }, new bool[] { }, TypeMask.interPers, new string[] { "Noble" });

		relationSystem.CreateNewMask("JohnBill", new float[] { -0.2f, -0.2f }, new bool[] { }, TypeMask.interPers, new string[] { "Convicted" });

        relationSystem.CreateNewMask("JohnTherese", new float[] { 0.2f, -0.2f }, new bool[] { }, TypeMask.interPers, new string[] { "Peasant" });

        relationSystem.CreateNewMask("ThereseJohn", new float[] { 0.0f, 0.0f }, new bool[] { }, TypeMask.interPers, new string[] { "Princess" });
    }


    public void CreateFirstPeople()
    {
		#region adding Conditions
		RuleConditioner emptyCondition = (self, other, indPpl) => { 
			//UIFunctions.WriteGameLine("PassedCorrectly ");
			return true;
		};

		RuleConditioner GreetCondition = (self, other, indPpl) =>
        { if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["greet"] && x.GetSubject()==self && x.GetDirect()==other)){
				return false; }
			return true; };

		RuleConditioner threatenCondition = (self, other, indPpl) =>
		{ 	if(self.absTraits.traits[TraitTypes.NiceNasty].value < -0.1f){ return true; }
			if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["threaten"] && x.GetDirect()==self && x.GetDirect()==other))
				{return true; }
			return false; };

		RuleConditioner accuseCondition = (self, other, indPpl) =>
        {
			//UIFunctions.WriteGameLine("accuse? "+ relationSystem.historyBook.Exists(x=>x.GetRule().ruleName));
			if(self.absTraits.traits[TraitTypes.NiceNasty].value < 0.0f && relationSystem.historyBook.Exists(x=>x.GetRule().strength < 0.0f && x.GetSubject()==other)){
			 return true; }
				return false; 
		};

		RuleConditioner pleadCondition = (self, other, indPpl) =>
        {
			if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["accuse"] && x.GetDirect()==self) ) //if accuse me
				{ return true; }
			else
				return false;
		};

		RuleConditioner punchCondition = (self, other, indPpl) =>
        {	
			if(other.culture != null){

				if(other.culture.Exists(x=>x.roleName == "Bunsant") && self.culture.Exists(x=>x.roleName == "Bunce")){
					//UIFunctions.WriteGameLine("He's a bunsant and I'm a Bunce!");
					return true;
				}
			}
			if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["threaten"] && x.GetDirect()==self) ){
				if(self.absTraits.traits[TraitTypes.NiceNasty].value < -0.3f){
					return true; }
			} 
			if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["punch"] && x.GetDirect()==self && x.GetSubject()==other
			) ){
				//if(self.absTraits.traits[TraitTypes.NiceNasty].value < -0.1f){
					return true;
			} 
			return false; };

		RuleConditioner convictCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["punch"] && x.GetDirect()==self && (x.GetSubject()==other && x.GetSubject()!=self)) )
			{	return true; }
			return false; };

		RuleConditioner fleeCondition = (self, other, indPpl) =>
		{	
			if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["convict"] && x.GetDirect()==self) ){
				if(self.absTraits.traits[TraitTypes.NiceNasty].value >= 0.0){

					return true; 
				}
			}
			return false; };

		RuleConditioner fightBackCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["convict"] && x.GetDirect()==self && x.GetSubject()==other) ){
				if(self.absTraits.traits[TraitTypes.NiceNasty].value < 0.0){
					return true; 
				}
			}
			return false; };

		RuleConditioner bribeCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["convict"] && x.GetDirect()==self && x.GetSubject()==other) ){
					return true; 
			}
			return false; };

		RuleConditioner kissCondition = (self, other, indPpl) =>
        {
            if (self.interPersonal.Exists(x => x.roleName == "Married") && other.interPersonal.Exists(x => x.roleName == "Married"))
              { return true; }

            if (self.interPersonal.Exists(x => x.roleName == "Peasant")) { return true; }

            return false;
        };


		#endregion adding Conditions

        #region AddingPlayer
            MaskAdds selfPersMask = new MaskAdds("Self", "Player", 0.0f, new List<Person>());

			relationSystem.CreateNewPerson(selfPersMask, new List<MaskAdds>(), new List<MaskAdds>(), 0f, 0f, 0f, new float[] { 0f, 0f, 0f });
		#endregion AddingPlayer

        #region AddingBill
            selfPersMask = new MaskAdds("Self", "Bill", 0.0f, new List<Person>());

            List<MaskAdds>  culture = new List<MaskAdds>();
            culture.Add(new MaskAdds("Bunce", "Bungary", 0.4f, new List<Person>()));


			relationSystem.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.6f, 0.4f, 0.7f, new float[] { -0.2f, 0.5f, 0.1f });
		#endregion AddingBill

        #region AddingTerese
            selfPersMask = new MaskAdds("Self", "Therese", 0.0f, new List<Person>());

            culture = new List<MaskAdds>();
            culture.Add(new MaskAdds("Buncess", "Bungary", 0.4f, new List<Person>()));


			relationSystem.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.3f, 0.7f, 0.2f, new float[] { 0.6f, -0.5f, 0.6f });
		
		#endregion AddingTerese

        #region AddingJohn
            selfPersMask = new MaskAdds("Self", "John", 0.0f, new List<Person>());

            culture = new List<MaskAdds>();
            culture.Add(new MaskAdds("Bunsant", "Bungary", 0.9f, new List<Person>()));

			relationSystem.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.5f, 0.5f, 0.4f, new float[] { 0.0f, 0.8f, -0.4f });
		#endregion AddingJohn

        #region Rules
        
	    //BILL THERESE RULES
	    //	relationSystem.AddRuleToMask("BillTherese", "Married", "GreetfBill","Greet", 0.2f, new List<Rule>(), GreetCondition);
	    //	relationSystem.AddRuleToMask("ThereseBill", "Married", "GreetfTherese","Greet", 0.2f, new List<Rule>(), GreetCondition);
	    //	relationSystem.AddRuleToMask("John", "Self", "GreetfJohn","Greet", 0.0f, new List<Rule>(), GreetCondition);
	
	    //	relationSystem.AddRuleToMask("BillTherese", "Married", "ComplimentSpouse","Compliment", 0.3f, new List<Rule>(), emptyCondition);
        //  relationSystem.AddRuleToMask("ThereseBill", "Married",  "ComplimentSpouse","Compliment", 0.3f, new List<Rule>(), emptyCondition);
            relationSystem.CreateNewRule("ThreatenSpouse", "Threaten", threatenCondition);
		    relationSystem.AddRuleToMask("BillTherese", "Married", "ThreatenSpouse", -0.4f);

		    relationSystem.AddRuleToMask("ThereseBill", "Married", "ThreatenSpouse", -0.4f);

            relationSystem.CreateNewRule("KissfTherese", "kiss", kissCondition);
            relationSystem.AddRuleToMask("ThereseBill", "Married", "KissfTherese", 0.4f);

            relationSystem.CreateNewRule("KissfBill", "kiss", kissCondition);
            relationSystem.AddRuleToMask("BillTherese", "Married", "KissfBill", 0.4f);    
		
	    //BILL JOHN RULES
            relationSystem.CreateNewRule("accusefBill", "accuse", accuseCondition);
		    relationSystem.AddRuleToMask("BillJohn", "Noble", "accusefBill", 0.4f);

            relationSystem.CreateNewRule("ThreatenfJohn", "Threaten", threatenCondition);
		    relationSystem.AddRuleToMask("JohnBill", "Convicted", "ThreatenfJohn", -0.1f);

            relationSystem.CreateNewRule("accusefJohn", "accuse", accuseCondition);
		    relationSystem.AddRuleToMask("JohnBill", "Convicted", "accusefJohn", -0.2f);

        //THERESE JOHN RULES
            relationSystem.CreateNewRule("KissfJohn", "kiss", kissCondition);
            relationSystem.AddRuleToMask("JohnTherese", "Peasant", "KissfJohn", -0.6f);

            relationSystem.AddRuleToMask("ThereseJohn", "Princess", "accusefJohn", 0.1f);

        // CULTURAL RULES
            relationSystem.CreateNewRule("Lie", "Lie", emptyCondition);
		    relationSystem.AddRuleToMask("Bungary", "Bunce", "Lie", -0.6f);

            relationSystem.CreateNewRule("Plead", "Plead", pleadCondition);
		    relationSystem.AddRuleToMask("Bungary", "Bunsant", "Plead", 0.4f);

            relationSystem.CreateNewRule("punchfBunce", "punch", punchCondition);
		    relationSystem.AddRuleToMask("Bungary", "Bunce", "punchfBunce", 0.0f);

            relationSystem.CreateNewRule("punchfPeasant", "punch", punchCondition);
		    relationSystem.AddRuleToMask("Bungary", "Bunsant", "punchfPeasant", -0.3f);

            relationSystem.CreateNewRule("ConvictfBill", "convict",  convictCondition);
		    relationSystem.AddRuleToMask("Bungary", "Bunce", "ConvictfBill", -0.0f);

            relationSystem.CreateNewRule("flee", "flee", fleeCondition);
		    relationSystem.AddRuleToMask("Bungary", "Bunsant", "flee", 0.2f);

            relationSystem.CreateNewRule("fightback", "fightback", fightBackCondition);
		    relationSystem.AddRuleToMask("Bungary", "Bunsant", "fightback", 0.1f);

            relationSystem.CreateNewRule("bribe", "bribe", bribeCondition);
		    relationSystem.AddRuleToMask("Bungary", "Bunsant", "bribe", 0.3f);


        //SELF RULES
            relationSystem.CreateNewRule("doNothingfSant", "doNothing", emptyCondition);
		    relationSystem.AddRuleToMask("John", "Self", "doNothingfSant", 0.0f);

            relationSystem.CreateNewRule("doNothingfcess", "doNothing", emptyCondition);
		    relationSystem.AddRuleToMask("Therese", "Self", "doNothingfcess", 0.0f);

            relationSystem.CreateNewRule("doNothingfbuncce", "doNothing", emptyCondition);
		    relationSystem.AddRuleToMask("Bill", "Self", "doNothingfbuncce", 0.0f);

		#endregion Rules



		#region LINKS
		relationSystem.AddLinkToPerson("Bill", new string[] { "Therese" }, TypeMask.interPers, "Married", "BillTherese", 0.6f);
		relationSystem.AddLinkToPerson("Therese", new string[] { "Bill" }, TypeMask.interPers, "Married", "ThereseBill", 0.4f);
		relationSystem.AddLinkToPerson("John", new string[] { "Bill" }, TypeMask.interPers, "Convicted", "JohnBill", 0.7f);
		relationSystem.AddLinkToPerson("Bill", new string[] { "John" }, TypeMask.interPers, "Noble", "BillJohn", 0.2f);
		#endregion LINKS 


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
			direct.absTraits.traits[TraitTypes.NiceNasty].value += Calculator.unboundAdd(-0.2f,direct.absTraits.traits[TraitTypes.NiceNasty].value);
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
			direct.absTraits.traits[TraitTypes.ShyBolsterous].value += Calculator.unboundAdd(-0.2f,direct.absTraits.traits[TraitTypes.ShyBolsterous].value);
			direct.absTraits.traits[TraitTypes.NiceNasty].value += Calculator.unboundAdd(-0.4f,direct.absTraits.traits[TraitTypes.NiceNasty].value);
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