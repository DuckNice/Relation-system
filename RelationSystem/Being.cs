
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
			Rule rule = maskSystem.pplAndMasks.GetPerson(name).GetAction(notPossibleActions.Values.ToList(), focus.Values.ToList());

            Console.WriteLine("Doing action '" + rule.actionToTrigger.name + "' from " + name);

			rule.DoAction (rule.self, rule.other);

			/*if (indiObject == null) {
				if (dirObject == null) {
					rule.DoAction (maskSystem.peopleAndMasks.GetPerson (name), " ");
				} else {
					rule.DoAction (maskSystem.peopleAndMasks.GetPerson (name), " ", dirObject);
				}
			} else {
				rule.DoAction(maskSystem.peopleAndMasks.GetPerson(name), " ", dirObject, indiObject);
			}
*/

        }
	}
}

