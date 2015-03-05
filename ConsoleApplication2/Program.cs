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
        RelationSystem maskSystem = new RelationSystem();

        static void Main(string[] args)
        {
            Program main = new Program();

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
            MAction action = maskSystem.peopleAndMasks.GetPerson("Bill").GetAction(maskSystem.posActions.Values.ToList());

            Console.WriteLine("Doing action:");

            Console.WriteLine(action.name);
        }
    }
}