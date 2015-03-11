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

            while (!NPCThread.IsAlive);
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

            relationSystem.CreateNewMask("Bungary", new float[] { 0.2f, -0.3f }, new bool[] { }, TypeMask.culture, new string[] { "Bunce", "Buncess", "Bunsant" });

            relationSystem.CreateNewMask("Bill", new float[] { 0.2f, -0.3f }, new bool[] { }, TypeMask.selfPerc, new string[] { "" });

            relationSystem.CreateNewMask("Therese", new float[] { 0.2f, -0.3f }, new bool[] { }, TypeMask.selfPerc, new string[] { "" });

            relationSystem.CreateNewMask("John", new float[] { 0.2f, -0.3f }, new bool[] { }, TypeMask.selfPerc, new string[] { "" });

            relationSystem.CreateNewMask("BillTherese", new float[] { 0.2f, 0.2f }, new bool[] { }, TypeMask.interPers, new string[] { "Married" });

            relationSystem.CreateNewMask("ThereseBill", new float[] { 0.2f, 0.2f }, new bool[] { }, TypeMask.interPers, new string[] { "Married" });
        }


        public void CreateFirstPeople()
        {
            #region AddingPlayer
                MaskAdds selfPersMask = new MaskAdds("Self", "Player", 0.4f, new List<Person>());

                relationSystem.CreateNewPerson(selfPersMask, new List<MaskAdds>(), new List<MaskAdds>(), 0.2f, 0.2f, 0.2f);
            #endregion AddingPlayer

            #region AddingBill
                selfPersMask = new MaskAdds("Self", "Bill", 0.4f, new List<Person>());

                List<MaskAdds>  culture = new List<MaskAdds>();
                culture.Add(new MaskAdds("Bunce", "Bungary", 0.4f, new List<Person>()));

                relationSystem.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.2f, 0.2f, 0.2f);
            #endregion AddingBill

            #region AddingTerese
                selfPersMask = new MaskAdds("Self", "Therese", 0.4f, new List<Person>());

                culture = new List<MaskAdds>();
                culture.Add(new MaskAdds("Buncess", "Bungary", 0.4f, new List<Person>()));

                relationSystem.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.2f, 0.2f, 0.2f);
            #endregion AddingTerese

            #region AddingJohn
                selfPersMask = new MaskAdds("Self", "John", 0.4f, new List<Person>());

                culture = new List<MaskAdds>();
                culture.Add(new MaskAdds("Bunsant", "Bungary", 0.9f, new List<Person>()));

                relationSystem.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.2f, 0.2f, 0.2f);
            #endregion AddingJohn

            #region InterPeople
                relationSystem.AddRuleToMask("BillTherese", "Married", relationSystem.pplAndMasks.GetPerson("Bill"), relationSystem.pplAndMasks.GetPerson("Therese"), "Greet", 0.4f, new List<Rule>());
                relationSystem.AddRuleToMask("ThereseBill", "Married", relationSystem.pplAndMasks.GetPerson("Therese"), relationSystem.pplAndMasks.GetPerson("Bill"), "Greet", 0.4f, new List<Rule>());
				
			    relationSystem.AddRuleToMask("BillTherese", "Married", relationSystem.pplAndMasks.GetPerson("Bill"), relationSystem.pplAndMasks.GetPerson("Therese"), "Compliment", 0.7f, new List<Rule>());
		    	relationSystem.AddRuleToMask("ThereseBill", "Married", relationSystem.pplAndMasks.GetPerson("Therese"), relationSystem.pplAndMasks.GetPerson("Bill"), "Compliment", 0.5f, new List<Rule>());    

				relationSystem.AddRuleToMask("BillTherese", "Married", relationSystem.pplAndMasks.GetPerson("Bill"), relationSystem.pplAndMasks.GetPerson("Therese"), "Threaten", -0.4f, new List<Rule>());
				relationSystem.AddRuleToMask("ThereseBill", "Married", relationSystem.pplAndMasks.GetPerson("Therese"), relationSystem.pplAndMasks.GetPerson("Bill"), "Threaten", -0.4f, new List<Rule>());    

			
			relationSystem.AddLinkToPerson("Bill", new string[] { "Therese" }, TypeMask.interPers, "Married", "BillTherese", 0.4f);
                relationSystem.AddLinkToPerson("Therese", new string[] { "Bill" }, TypeMask.interPers, "Married", "ThereseBill", 0.4f);
            #endregion InterPeople


			#region Rules in social masks
			    relationSystem.AddRuleToMask("Bungary", "Bunce", relationSystem.pplAndMasks.GetPerson("Bill"), relationSystem.pplAndMasks.GetPerson("John"), "Order", 0.4f, new List<Rule>());
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



			/*	ActionInvoker ask_about_day = (subject, direct) => 
			{
				Console.WriteLine(subject.name + " Is asking " + direct.name + " About the time of day.");
			};

			maskSystem.AddAction(new MAction("Ask_about_day", 0.3f, ask_about_day));

*/
	// ---------- INTERPERSONAL ACTIONS

			ActionInvoker order = (subject, direct) => 
			{
				Console.WriteLine(subject.name + " is ordering "+direct.name+" to go away.");
			};
			
			relationSystem.AddAction(new MAction("Order", 0.1f, order, relationSystem));
		}
    }
}