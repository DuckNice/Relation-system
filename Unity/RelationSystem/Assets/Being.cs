using System;
using System.Collections.Generic;
using System.Linq;

    //Namespaces
using NRelationSystem;


namespace RelationSystemProgram
{
	public class Being
	{
		public string name;
		public Dictionary<Being, float> focus;
		public RelationSystem maskSystem;
		Dictionary<string, MAction> notPossibleActions;


		public Being (string _name, RelationSystem relsys)
		{
			name = _name;
			focus = new Dictionary<Being,float > ();
			notPossibleActions = new Dictionary<string, MAction> ();
			maskSystem = relsys;
		}


		//Use in beginning for all Beings, to set initial focus for all beings in the world.
		public void FindFocusToAll(List<Being> beingsInWorld){
			foreach (Being b in beingsInWorld) {
				focus.Add(b,0);
			}
		}


		public void SetFocusToOther(Being otherPerson, float f){
			focus [otherPerson] = f;
		}


		public void NPCAction(){
            Person self = maskSystem.pplAndMasks.GetPerson(name);
			Rule rule = self.GetAction(notPossibleActions.Values.ToList(), focus.Values.ToList());

			Console.WriteLine ("");
            Console.WriteLine("Doing action '" + rule.actionToTrigger.name + "' from " + name);

            if (rule.actionToTrigger.name.ToLower() != "empty")
            {
			    rule.DoAction (self, rule.selfOther[self], rule);
            }
        }
	}
}