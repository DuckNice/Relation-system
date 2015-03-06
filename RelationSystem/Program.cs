using System;
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
			maskSystem.CreateFirstMasks();
			maskSystem.CreateFirstPeople();

			Being Bill = new Being ("Bill", maskSystem);
			Being Therese = new Being ("Therese", maskSystem);
			Being John = new Being ("John", maskSystem);
			
			beings.Add (Bill);
			beings.Add (Therese);
			beings.Add (John);
			Bill.FindFocusToAll (beings);
			Console.WriteLine (beings[0].focus[Therese]);
			Bill.SetFocusToOther (Therese,1);
			Console.WriteLine (beings[0].focus[Therese]);

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