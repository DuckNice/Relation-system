using System;
using System.Collections.Generic;
using System.Threading;

    //Namespaces
using NRelationSystem;


namespace RelationSystemProgram
{
    partial class Program
    {
		volatile RelationSystem relationSystem = new RelationSystem ();
        
            //Threading work.
        Thread NPCThread;
        private volatile bool stopNPCLoop = false;

		public Program()
        {
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
			Bill.SetFocusToOther (Therese,1);


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
			Program main = new Program();
            

			//adding new beings for testing
		
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
            {
				if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["greet"] && x.GetSubject()==self)){
					return false;
				}
				return true;
			};

            RuleConditioner blameCondition = (self, other) =>
            {
				//Console.WriteLine( relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["compliment"]) );
				if(self.absTraits.traits[TraitTypes.NiceNasty].value < 0.0){ return true; }
				else 
					return false; 
			};

            RuleConditioner pleadCondition = (self, other) =>
            {
				if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["blame"] && x.GetDirect()==self) ) //if blame me
					{ return true; }
				else
					return false;
			};

            RuleConditioner punchCondition = (self, other) =>
            {
				if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["plead"] && x.GetDirect()==self) ){

					if(self.absTraits.traits[TraitTypes.NiceNasty].value < -0.2f){
						return true;
					}
				} //if plead to me

				return false;
			};

		//	RuleConditioner shriekCondition = pers => {

		//	};


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
			relationSystem.AddRuleToMask("BillTherese", "Married", "GreetSpouse","Greet", 0.4f, new List<Rule>(), GreetCondition);
			relationSystem.AddRuleToMask("ThereseBill", "Married", "GreetSpouse","Greet", 0.4f, new List<Rule>(), GreetCondition);

	//		relationSystem.AddRuleToMask("BillTherese", "Married", "ComplimentSpouse","Compliment", 0.3f, new List<Rule>(), emptyCondition);
          relationSystem.AddRuleToMask("ThereseBill", "Married",  "ComplimentSpouse","Compliment", 0.3f, new List<Rule>(), emptyCondition);

			relationSystem.AddRuleToMask("BillTherese", "Married", "ThreatenSpouse", "Threaten", -0.4f, new List<Rule>(), emptyCondition);
			relationSystem.AddRuleToMask("ThereseBill", "Married", "ThreatenSpouse", "Threaten", -0.4f, new List<Rule>(), emptyCondition);    
		
		//BILL JOHN RULES
			relationSystem.AddRuleToMask("BillJohn", "Noble", "BlameJohn", "Blame", 0.4f, new List<Rule>(), blameCondition);    
			relationSystem.AddRuleToMask("JohnBill", "Convicted", "ThreatenBill", "Threaten", 0.4f, new List<Rule>(), blameCondition);    


			relationSystem.AddLinkToPerson("Bill", new string[] { "Therese" }, TypeMask.interPers, "Married", "BillTherese", 0.6f);
            relationSystem.AddLinkToPerson("Therese", new string[] { "Bill" }, TypeMask.interPers, "Married", "ThereseBill", 0.4f);
			relationSystem.AddLinkToPerson("John", new string[] { "Bill" }, TypeMask.interPers, "Convicted", "JohnBill", 0.7f);
			relationSystem.AddLinkToPerson("Bill", new string[] { "John" }, TypeMask.interPers, "Noble", "BillJohn", 0.2f);
            #endregion InterPeople


			#region Rules in social masks

		//	relationSystem.AddRuleToMask("Bungary", "Bunce", relationSystem.pplAndMasks.GetPerson("Bill"), relationSystem.pplAndMasks.GetPerson("John"),"OrderJohn", "Order", 0.4f, new List<Rule>(), emptyCondition);
			relationSystem.AddRuleToMask("Bungary", "Bunce", "Lie", "Lie", -0.6f, new List<Rule>(), emptyCondition);
			relationSystem.AddRuleToMask("Bungary", "Bunsant", "Plead", "Plead", 0.4f, new List<Rule>(), pleadCondition);
			relationSystem.AddRuleToMask("Bungary", "Bunce", "Punch", "punch", 1.0f, new List<Rule>(), punchCondition);	

			#endregion Rules in social masks
        }


        void PerformAction(Person target, MAction action)
        {
            action.DoAction(relationSystem.pplAndMasks.GetPerson("Player"), target);

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

			ActionInvoker blame = (subject, direct) => 
			{
				Console.WriteLine(subject.name + " is blaming "+direct.name+" of heinous crimes!");
			};
			relationSystem.AddAction(new MAction("Blame", 0.0f, blame, relationSystem));



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


		}
    }
}