using System;
using System.Collections.Generic;
using System.Threading;

    //Namespaces
using NRelationSystem;


namespace RelationSystemProgram
{
    partial class Program
    {
		volatile RelationSystem maskSystem = new RelationSystem ();
        
            //Threading work.
        Thread NPCThread;
        private volatile bool stopNPCLoop = false;

		public Program()
        {
			SetupActions ();
			CreateFirstMasks();
			CreateFirstPeople();

			Being Bill = new Being ("Bill", maskSystem);
			Being Therese = new Being ("Therese", maskSystem);
			Being John = new Being ("John", maskSystem);
			
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
                    maskSystem.PrintPersonStatus();
                }
                else if(sepInput[0] == "close" || input == "exit")
                {
                    break;
                }
                else if(sepInput[0] == "do")
                {
                    if(sepInput.Length > 1)
                    {
                        Person target = maskSystem.pplAndMasks.GetPerson(sepInput[1]);

                        if(target != null)
                        {
                            if(sepInput.Length > 2)
                            {
                                if (maskSystem.posActions.ContainsKey(sepInput[2]))
                                {
                                    MAction actionToDo = maskSystem.posActions[sepInput[2]];

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

                                foreach(string actionNames in maskSystem.posActions.Keys)
                                    Console.WriteLine("  " + actionNames + ".");

                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("'display <person>': Get information about character.");
                        Console.WriteLine("'do <person> <action>': Perform the mentioned <action> interacting with the stated <person>");
                        Console.WriteLine("'exit' or 'close': Properly close the application and all related threads.");
                    }
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
            maskSystem.CreateNewMask("Player", new float[]{}, new bool[]{}, TypeMask.selfPerc, new string[]{});

            maskSystem.CreateNewMask("Bungary", new float[] { 0.2f, -0.3f }, new bool[] { }, TypeMask.culture, new string[] { "Bunce", "Buncess", "Bunsant" });

            maskSystem.CreateNewMask("Bill", new float[] { 0.2f, -0.3f }, new bool[] { }, TypeMask.selfPerc, new string[] { "" });

            maskSystem.CreateNewMask("Therese", new float[] { 0.2f, -0.3f }, new bool[] { }, TypeMask.selfPerc, new string[] { "" });

            maskSystem.CreateNewMask("John", new float[] { 0.2f, -0.3f }, new bool[] { }, TypeMask.selfPerc, new string[] { "" });

            maskSystem.CreateNewMask("BillTherese", new float[] { 0.2f, 0.2f }, new bool[] { }, TypeMask.interPers, new string[] { "Married" });

            maskSystem.CreateNewMask("ThereseBill", new float[] { 0.2f, 0.2f }, new bool[] { }, TypeMask.interPers, new string[] { "Married" });
        }


        public void CreateFirstPeople()
        {
            #region AddingPlayer
                MaskAdds selfPersMask = new MaskAdds("Self", "Player", 0.4f, new List<Person>());

                maskSystem.CreateNewPerson(selfPersMask, new List<MaskAdds>(), new List<MaskAdds>(), 0.2f, 0.2f, 0.2f);
            #endregion AddingPlayer

            #region AddingBill
                selfPersMask = new MaskAdds("Self", "Bill", 0.4f, new List<Person>());

                List<MaskAdds>  culture = new List<MaskAdds>();
                culture.Add(new MaskAdds("Bunce", "Bungary", 0.4f, new List<Person>()));

                maskSystem.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.2f, 0.2f, 0.2f);
            #endregion AddingBill

            #region AddingTerese
                selfPersMask = new MaskAdds("Self", "Therese", 0.4f, new List<Person>());

                culture = new List<MaskAdds>();
                culture.Add(new MaskAdds("Buncess", "Bungary", 0.4f, new List<Person>()));

                maskSystem.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.2f, 0.2f, 0.2f);
            #endregion AddingTerese

            #region AddingJohn
                selfPersMask = new MaskAdds("Self", "John", 0.4f, new List<Person>());

                culture = new List<MaskAdds>();
                culture.Add(new MaskAdds("Bunsant", "Bungary", 0.4f, new List<Person>()));

                maskSystem.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.2f, 0.2f, 0.2f);
            #endregion AddingJohn

            #region InterPeople
                maskSystem.AddRuleToMask("BillTherese", "Married", maskSystem.pplAndMasks.GetPerson("Bill"), maskSystem.pplAndMasks.GetPerson("Therese"), "Greet", 0.7f, new List<Rule>());
                maskSystem.AddRuleToMask("ThereseBill", "Married", maskSystem.pplAndMasks.GetPerson("Therese"), maskSystem.pplAndMasks.GetPerson("Bill"), "Greet", 0.7f, new List<Rule>());

                maskSystem.AddLinkToPerson("Bill", new string[] { "Therese" }, TypeMask.interPers, "Married", "BillTherese", 0.4f);
                maskSystem.AddLinkToPerson("Therese", new string[] { "Bill" }, TypeMask.interPers, "Married", "ThereseBill", 0.4f);
            #endregion InterPeople
        }


        void PerformAction(Person target, MAction action)
        {
            action.DoAction(maskSystem.pplAndMasks.GetPerson("Player"), target);

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
			ActionInvoker greet = (subject, direct) => 
			{
				Console.WriteLine(subject.name + " is greeting ");
			};

			maskSystem.AddAction(new MAction("Greet", 0.1f, greet));

			ActionInvoker ask_about_day = (subject, direct) => 
			{
				Console.WriteLine(subject.name + " Is asking " + direct.name + " About the time of day.");
			};

			maskSystem.AddAction(new MAction("Ask_about_day", 0.3f, ask_about_day));
		}
    }
}