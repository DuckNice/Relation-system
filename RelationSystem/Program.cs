using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


    //Namespaces
using NRelationSystem;


namespace RelationSystemProgram
{
    partial class Program:UserControl
    {   
    
    
        public void Canvas_Loaded(object sender, EventArgs e)    
        {    
    
            // The current date and time.    
            System.DateTime date = DateTime.Now;    
    
            // Find the appropriate angle (in degrees) for the hour hand    
            // based on the current time.    
            double hourangle = (((float)date.Hour) / 12) * 360 + date.Minute / 2;    
    
            // The transform is already rotated 116.5 degrees to make the hour hand be    
            // in the 12 o'clock position. You must build this already existing angle    
            // into the hourangle.    
            hourangle += 180;    
    
            // The same as for the hour angle.    
            double minangle = (((float)date.Minute) / 60) * 360;    
            minangle += 180;    
    
            // The same for the hour angle.    
            double secangle = (((float)date.Second) / 60) * 360;    
            secangle += 180;    
    
            // Set the beginning of the animation (From property) to the angle     
            // corresponging to the current time.    
    
            // Set the end of the animation (To property)to the angle     
            // corresponding to the current time PLUS 360 degrees. Thus, the    
            // animation will end after the clock hand moves around the clock     
            // once. Note: The RepeatBehavior property of the animation is set    
            // to "Forever" so the animation will begin again as soon as it completes.    
    
            // Same as with the hour animation.    
    
            // Same as with the hour animation.    
    
            // Start the storyboard.    
    
        }    
    
      



		volatile RelationSystem relationSystem = new RelationSystem ();
        
            //Threading work.
        Thread NPCThread;
        private volatile bool stopNPCLoop = false;


		public Program()
        {
            InitializeComponent();  

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
			//Bill.SetFocusToOther (Therese,1);


            NPCThread = new Thread(new ThreadStart(NPCThreadFunc));
            NPCThread.Start();

            int i = 1;

            while (!NPCThread.IsAlive)
            {
                Console.Clear();
                switch (i)
                {
                    case 1:
                        Console.WriteLine("Thread starting.");
                        break;
                    case 2:
                        Console.WriteLine("Thread starting..");
                        break;
                    case 3:
                        Console.WriteLine("Thread starting...");
                        break;
                    default:
                        Console.WriteLine("Thread starting....");
                        break;
                }

                i++;

                if (i > 3)
                    i = 1;
                Thread.Sleep(200);
            }
		}


        
        public void Close() 
        {
            stopNPCLoop = true;

            while (NPCThread.ThreadState != ThreadState.Stopped);
        }


        static void Main(string[] args)
        {
            Thread programThread = new Thread(programThreadFunc);
            programThread.SetApartmentState(ApartmentState.STA);

            programThread.Start();
        }

        static void programThreadFunc()
        {
            Program main = new Program();

            main.Update();

            main.Close();
        }


        void Update()
        {
            Console.WriteLine("Welcome to Mask\n\n");

            while (true)
            {
                Console.WriteLine("\nPlease Insert Command.");

                string input = Console.ReadLine();
                input = input.ToLower();


                string[] seps = {" "};

                string[] sepInput = input.Split(seps, StringSplitOptions.RemoveEmptyEntries);

                if(sepInput[0] == "display")
                {
                    relationSystem.PrintPersonStatus();
                }
                else if(sepInput[0] == "close" || input == "exit")
                {
                    break;
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
                            Console.WriteLine("Error: 'do' recognized, but second parameter not found in list of people.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: 'do' what?");
                    }
                }
                else if(sepInput[0] == "help")
                {
                    if(sepInput.Length > 1)
                    {
                        switch(sepInput[1])
                        {
                            case "do":
                                Console.WriteLine("Nothing here yet");
                                break;
                            case "actions":
                                Console.WriteLine("List of actions:\n");

                                foreach(string actionNames in relationSystem.posActions.Keys)
                                    Console.WriteLine("  " + actionNames + ".");

                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("'display <person>': Get information about character.");
                        Console.WriteLine("'do <person> <action>': Perform the mentioned <action> interacting with the stated <person>");
                        Console.WriteLine("'history': Show the history log.");
                        Console.WriteLine("'exit' or 'close': Properly close the application and all related threads.");
                    }
                }
                else if(sepInput[0] == "history")
                {
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine("Error: No command '" + sepInput[0] + "' recognized.\nWrite 'help' for list of commands.");
                }

                Thread.Sleep(1);
            }

            Console.WriteLine("Closing program");

            Thread.Sleep(1000);
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
        }


        public void CreateFirstPeople()
        {
			#region adding Conditions
			RuleConditioner emptyCondition = (self, other) => { 
				//Console.WriteLine("PassedCorrectly ");
				return true;
			};

            RuleConditioner GreetCondition = (self, other) =>
            { if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["greet"] && x.GetSubject()==self && x.GetDirect()==other)){
					return false; }
				return true; };

			RuleConditioner threatenCondition = (self, other) =>
			{ if(self.absTraits.traits[TraitTypes.NiceNasty].value > -0.2f){ return false; }
				return true; };

            RuleConditioner accuseCondition = (self, other) =>
            {
				//Console.WriteLine("accuse? "+ relationSystem.historyBook.Exists(x=>x.GetRule().ruleName));
				if(self.absTraits.traits[TraitTypes.NiceNasty].value < 0.0f && relationSystem.historyBook.Exists(x=>x.GetRule().strength < 0.0f)){
				 return true; }
					return false; 
			};

            RuleConditioner pleadCondition = (self, other) =>
            {
				if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["accuse"] && x.GetDirect()==self) ) //if blame me
					{ return true; }
				else
					return false;
			};

            RuleConditioner punchCondition = (self, other) =>
            {	
				//Console.Write("checking punch condition "+other.name+" ");
				if(other.culture != null){
					if(other.culture[0].roleRef[0].name == "Bunce" && self.culture[0].roleRef[0].name == "Bunsant"){
						Console.Write("He's a bunce and I'm a bunsant!");
						return true;
					}
				}
				if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["threaten"] && x.GetDirect()==self) ){  //if plead to me

					if(self.absTraits.traits[TraitTypes.NiceNasty].value < -0.2f){
						return true; }
				} return false; };

			RuleConditioner convictCondition = (self, other) =>
			{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["punch"] && x.GetSubject()==self) ){
					return true; }
				return false; };

			RuleConditioner fleeCondition = (self, other) =>
			{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["convict"] && x.GetSubject()==self) ){
					if(self.absTraits.traits[TraitTypes.NiceNasty].value > 0.0){
						return true; 
					}
				}
				return false; };

			RuleConditioner fightBackCondition = (self, other) =>
			{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["convict"] && x.GetSubject()==self) ){
					if(self.absTraits.traits[TraitTypes.NiceNasty].value < 0.0){
						return true; 
					}
				}
				return false; };


			#endregion adding Conditions

            #region AddingPlayer
                MaskAdds selfPersMask = new MaskAdds("Self", "Player", 0.4f, new List<Person>());

				relationSystem.CreateNewPerson(selfPersMask, new List<MaskAdds>(), new List<MaskAdds>(), 0f, 0f, 0f, new float[] { 0f, 0f, 0f });
			#endregion AddingPlayer

            #region AddingBill
                selfPersMask = new MaskAdds("Self", "Bill", 0.4f, new List<Person>());

                List<MaskAdds>  culture = new List<MaskAdds>();
                culture.Add(new MaskAdds("Bunce", "Bungary", 0.4f, new List<Person>()));


				relationSystem.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.2f, 0.2f, 0.2f, new float[] { -0.2f, 0.5f, 0.1f });
			#endregion AddingBill

            #region AddingTerese
                selfPersMask = new MaskAdds("Self", "Therese", 0.4f, new List<Person>());

                culture = new List<MaskAdds>();
                culture.Add(new MaskAdds("Buncess", "Bungary", 0.4f, new List<Person>()));


				relationSystem.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.2f, 0.2f, 0.2f, new float[] { 0.6f, -0.5f, 0.6f });
			
			#endregion AddingTerese

            #region AddingJohn
                selfPersMask = new MaskAdds("Self", "John", 0.4f, new List<Person>());

                culture = new List<MaskAdds>();
                culture.Add(new MaskAdds("Bunsant", "Bungary", 0.9f, new List<Person>()));

				relationSystem.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.2f, 0.2f, 0.2f, new float[] { 0.3f, 0.8f, -0.4f });
			#endregion AddingJohn

            #region InterPeople

            
		//BILL THERESE RULES
		//	relationSystem.AddRuleToMask("BillTherese", "Married", "GreetfBill","Greet", 0.2f, new List<Rule>(), GreetCondition);
		//	relationSystem.AddRuleToMask("ThereseBill", "Married", "GreetfTherese","Greet", 0.2f, new List<Rule>(), GreetCondition);
		//	relationSystem.AddRuleToMask("John", "Self", "GreetfJohn","Greet", 0.0f, new List<Rule>(), GreetCondition);

		//	relationSystem.AddRuleToMask("BillTherese", "Married", "ComplimentSpouse","Compliment", 0.3f, new List<Rule>(), emptyCondition);
        //  relationSystem.AddRuleToMask("ThereseBill", "Married",  "ComplimentSpouse","Compliment", 0.3f, new List<Rule>(), emptyCondition);

			relationSystem.AddRuleToMask("BillTherese", "Married", "ThreatenSpouse", "Threaten", -0.4f, new List<Rule>(), threatenCondition);
			relationSystem.AddRuleToMask("ThereseBill", "Married", "ThreatenSpouse", "Threaten", -0.4f, new List<Rule>(), threatenCondition);    
			
		//BILL JOHN RULES
			relationSystem.AddRuleToMask("BillJohn", "Noble", "accusefBill", "accuse", 0.4f, new List<Rule>(), accuseCondition);    
			relationSystem.AddRuleToMask("JohnBill", "Convicted", "ThreatenBill", "Threaten", -0.4f, new List<Rule>(), threatenCondition);    
			relationSystem.AddRuleToMask("JohnBill", "Convicted", "accusefJohn", "accuse", 0.4f, new List<Rule>(), accuseCondition);


			relationSystem.AddLinkToPerson("Bill", new string[] { "Therese" }, TypeMask.interPers, "Married", "BillTherese", 0.6f);
            relationSystem.AddLinkToPerson("Therese", new string[] { "Bill" }, TypeMask.interPers, "Married", "ThereseBill", 0.4f);
			relationSystem.AddLinkToPerson("John", new string[] { "Bill" }, TypeMask.interPers, "Convicted", "JohnBill", 0.7f);
			relationSystem.AddLinkToPerson("Bill", new string[] { "John" }, TypeMask.interPers, "Noble", "BillJohn", 0.2f);

            #endregion InterPeople


			#region Rules in social masks

	//		relationSystem.AddRuleToMask("Bungary", "Bunce", relationSystem.pplAndMasks.GetPerson("Bill"), relationSystem.pplAndMasks.GetPerson("John"),"OrderJohn", "Order", 0.4f, new List<Rule>(), emptyCondition);
			relationSystem.AddRuleToMask("Bungary", "Bunce", "Lie", "Lie", -0.6f, new List<Rule>(), emptyCondition);
			relationSystem.AddRuleToMask("Bungary", "Bunsant", "Plead", "Plead", 0.4f, new List<Rule>(), pleadCondition);
			relationSystem.AddRuleToMask("Bungary", "Bunce", "punchPeasant", "punch", 1.0f, new List<Rule>(), punchCondition);	
			relationSystem.AddRuleToMask("Bungary", "Bunsant", "punchPrince", "punch", 1.0f, new List<Rule>(), punchCondition);	
			relationSystem.AddRuleToMask("Bungary", "Bunce", "ConvictfBill", "convict", -0.0f, new List<Rule>(), convictCondition);    
			relationSystem.AddRuleToMask("Bungary", "Bunsant", "flee", "flee", -0.0f, new List<Rule>(), fleeCondition);    
			relationSystem.AddRuleToMask("Bungary", "Bunsant", "fightback", "fightback", -0.0f, new List<Rule>(), fightBackCondition);    


			#endregion Rules in social masks
        }


        void PerformAction(Person target, MAction action)
        {
			action.DoAction(relationSystem.pplAndMasks.GetPerson("Player"), target,new Rule("Empty", new MAction("Empty", 0.0f), 0.0f, null, "Empty",null));
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
			ActionInvoker greet = (subject, direct) => 
			{
				Console.WriteLine(subject.name + " is greeting "+direct.name);
			};
			relationSystem.AddAction(new MAction("Greet", 0.1f, greet, relationSystem));

			ActionInvoker compliment = (subject, direct) => 
			{
				Console.WriteLine(subject.name + " is complimenting "+direct.name);
			};
			relationSystem.AddAction(new MAction("Compliment", 0.0f, compliment, relationSystem));

			ActionInvoker threaten = (subject, direct) => 
			{
				Console.WriteLine(subject.name + " is threatening "+direct.name);
			};
			relationSystem.AddAction(new MAction("Threaten", 0.0f, threaten, relationSystem));

			ActionInvoker accuse = (subject, direct) => 
			{
				Console.WriteLine(subject.name + " is accusing "+direct.name+" of heinous crimes!");
			};
			relationSystem.AddAction(new MAction("accuse", 0.0f, accuse, relationSystem));



	// ---------- CULTURAL ACTIONS

			ActionInvoker order = (subject, direct) => 
			{
				Console.WriteLine(subject.name + " is ordering "+direct.name+" to go away.");
			};
			relationSystem.AddAction(new MAction("Order", 0.1f, order, relationSystem));

			ActionInvoker lie = (subject, direct) => 
			{
				Console.WriteLine(subject.name + " is lying to "+direct.name+" about his own might");
			};
			relationSystem.AddAction(new MAction("Lie", 0.2f, lie, relationSystem));

			ActionInvoker plead_innocence = (subject, direct) => 
			{
				Console.WriteLine(subject.name + " is pleading innocence to "+direct.name);
				direct.absTraits.traits[TraitTypes.NiceNasty].value += Calculator.unboundAdd(-0.2f,direct.absTraits.traits[TraitTypes.NiceNasty].value);
			};
			relationSystem.AddAction(new MAction("Plead", 0.5f, plead_innocence, relationSystem));

			ActionInvoker punch = (subject, direct) => 
			{
				Console.WriteLine(subject.name + " is PUNCHING "+direct.name+"!! OUCH");
			};
			relationSystem.AddAction(new MAction("punch", 1.0f, punch, relationSystem));

			ActionInvoker convict = (subject, direct) => 
			{
				Console.WriteLine(subject.name + " is convicting "+direct.name+" of commiting a crime. To Jail with him!");
				direct.absTraits.traits[TraitTypes.ShyBolsterous].value += Calculator.unboundAdd(-0.2f,direct.absTraits.traits[TraitTypes.ShyBolsterous].value);
				direct.absTraits.traits[TraitTypes.NiceNasty].value += Calculator.unboundAdd(-0.4f,direct.absTraits.traits[TraitTypes.NiceNasty].value);
			};
			relationSystem.AddAction(new MAction("convict", 1.0f, convict, relationSystem));

			ActionInvoker flee = (subject, direct) => 
			{
				Console.WriteLine(subject.name + " is attempting to flee the scene!");
			};
			relationSystem.AddAction(new MAction("flee", 1.0f, flee, relationSystem));

			ActionInvoker fightBack = (subject, direct) => 
			{
				Console.WriteLine(subject.name + " is attempting to fight back against "+direct.name);
			};
			relationSystem.AddAction(new MAction("fightBack", 1.0f, fightBack, relationSystem));

		}
    }
}