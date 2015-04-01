using System;
using System.Collections.Generic;


namespace NRelationSystem
{
    public class Link
    {
        public string roleName;
        public List<Person> roleRef;
        public Mask roleMask;
        float lvlOfInfl;


        public Link(string _roleName, List<Person> _roleRef, Mask _roleMask, float _lvlOfInfl) 
        {
            roleName = _roleName;
            roleRef = _roleRef;
            roleMask = _roleMask;
            lvlOfInfl = _lvlOfInfl;
        }


        public void AddRoleRef(Person _roleRef)
        {
            roleRef.Add(_roleRef);
        }


        public RuleAndStr actionForLink(List<MAction> notPosActions, List<PosActionItem> possibleActions, Person self, float rat, float mor, float imp, float abi, List<float> foci) 
        {
            RuleAndStr actionToSend;

            try
            {
				actionToSend = roleMask.CalculateActionToUse(notPosActions, possibleActions, self, rat, mor, imp, abi, lvlOfInfl,foci, roleName);
					//debug.Write ("Trying from link "+self.name+" Maskname: "+ roleMask.GetMaskName() +" Rolename: "+roleName);
            }
            catch
            {
				debug.Write("Catching actionForLink Error.");
                actionToSend = new RuleAndStr();
                actionToSend.chosenRule = new Rule("Empty", new MAction("Empty", 0.0f,0.0f), null);
                actionToSend.strOfAct = 0.0f;
            }

			//Console.WriteLine ("From LINK from "+roleName+" ::: " + actionToSend.chosenRule.ruleName);
            return actionToSend;
        }


		public float GetlvlOfInfl(){ return lvlOfInfl; }

		public void SetlvlOfInfl(float inp){ lvlOfInfl = inp; }

		public void AddToLvlOfInfl(float inp){ lvlOfInfl += inp; }

    }
}
