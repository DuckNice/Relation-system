﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

    //namespaces
using NRelationSystem;

namespace ConsoleApplication2
{
    class Program
    {
		RelationSystem maskSystem = new RelationSystem ();
		List<Being> beings  = new List<Being>();

		public Program(){


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
		}

        static void Main(string[] args)
        {
			Program main = new Program();
            

			//adding new beings for testing
		
            main.Update();
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
                        if(sepInput[1] == "bill")
                        {
                            if(sepInput.Length > 2)
                            {
                                if (sepInput[2] == "bill")
                                {
                                    PerformAction();
                                }
                            }
                        }
                        else if (sepInput[1] == "terese")
                        {
                            if (sepInput.Length > 2)
                            {
                                if (sepInput[2] == "bill")
                                {
                                    PerformAction();
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error: 'do' keyword recognized, but second parameter not recognized.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: 'do' keyword recognized, but second parameter not found.");
                    }
                }
                else if(sepInput[0] == "help")
                {
                    Console.WriteLine("No help to get.");
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
            maskSystem.CreateNewMask("Bungary", new float[] { 0.2f, -0.3f }, new bool[] { }, typeMask.culture, new string[] { "Bunce", "Buncess", "Bunsant" });

            maskSystem.CreateNewMask("Bill", new float[] { 0.2f, -0.3f }, new bool[] { }, typeMask.selfPerc, new string[] { "" });

            maskSystem.CreateNewMask("Therese", new float[] { 0.2f, -0.3f }, new bool[] { }, typeMask.selfPerc, new string[] { "" });

            maskSystem.CreateNewMask("John", new float[] { 0.2f, -0.3f }, new bool[] { }, typeMask.selfPerc, new string[] { "" });

            maskSystem.CreateNewMask("BillTherese", new float[] { 0.2f, 0.2f }, new bool[] { }, typeMask.interPers, new string[] { "Married" });

            maskSystem.AddRuleToMask("BillTherese", "Married", "Greet", 0.7f, new List<Rule>());

            maskSystem.CreateNewMask("ThereseBill", new float[] { 0.2f, 0.2f }, new bool[] { }, typeMask.interPers, new string[] { "Married" });

            maskSystem.AddRuleToMask("ThereseBill", "Married", "Greet", 0.7f, new List<Rule>());
        }

        public void CreateFirstPeople()
        {
            #region AddingBill
                MaskAdds selfPersMask = new MaskAdds("Self", "Bill", 0.4f, new List<Person>());

                List<MaskAdds> culture = new List<MaskAdds>();
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
                maskSystem.AddLinkToPerson("Bill", new string[] { "Therese" }, typeMask.interPers, "Married", "BillTherese", 0.4f);
                maskSystem.AddLinkToPerson("Therese", new string[] { "Bill" }, typeMask.interPers, "Married", "ThereseBill", 0.4f);
            #endregion InterPeople
        }


        void PerformAction()
        {
            NPCActions();
        }


        void NPCActions() 
        {
            foreach(Being being in beings)
            {
				being.NPCAction();
            }
            //MAction action = maskSystem.peopleAndMasks.GetPerson("Bill").GetAction(maskSystem.posActions.Values.ToList());

          //  Console.WriteLine("Doing action:");

          //  Console.WriteLine(action.name);
        }

		void SetupActions()
		{
			ActionInvoker greet = (subject, verb, direct, indirect) => 
			{
				Person sub = (Person)subject;


				Console.WriteLine(sub.name+" is greeting ");
				
			};
			maskSystem.AddAction(new MAction("Greet", 0.1f, greet));

			
			ActionInvoker ask_about_day = (subject, verb, direct, indirect) => 
			{
				Person sub = (Person)subject;

				Console.WriteLine(sub);
				
			};
			maskSystem.AddAction(new MAction("Ask_about_day", 0.3f, ask_about_day));
		}

    }
}