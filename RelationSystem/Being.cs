
using System;
using System.Collections.Generic;
using System.Linq;

using NRelationSystem;

namespace ConsoleApplication2
{
	public class Being
	{
		public string name;
		public Dictionary<Being, float> focus;
		RelationSystem maskSystem;
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
			MAction action = maskSystem.peopleAndMasks.GetPerson(name).GetAction(maskSystem.posActions.Values.ToList(), focus.Values.ToList());

			Console.WriteLine ("Doing action '" + action.name + "' from " + name);
		}
	}
}

