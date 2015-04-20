using System;
using System.Collections.Generic;
using System.Linq;

namespace NRelationSystem
{
    public class Link
    {
        public Dictionary<Person, Dictionary<string, float>> roleRef_LvlOfInfl = new Dictionary<Person, Dictionary<string, float>>();
        public Mask roleMask;

        public Link(string _genRoleName, Mask _roleMask, float _genLvlOfInfl, Person _roleRef = null, string _roleName = "", float _lvlOfInfl = 0) 
        {
            if (_roleRef != null && _lvlOfInfl > 0 && _roleRef != null && _roleName != "")
            {
                roleRef_LvlOfInfl.Add(_roleRef, new Dictionary<string, float>());
                roleRef_LvlOfInfl[_roleRef].Add(_roleName, _lvlOfInfl);
            }

            Person newPerson = new Person("none");

            roleRef_LvlOfInfl.Add(newPerson, new Dictionary<string, float>());
            roleRef_LvlOfInfl[newPerson].Add(_genRoleName, _genLvlOfInfl);
            roleMask = _roleMask;
        }


        public void AddRoleRef(string roleName, float lvlOfInfl, Person roleRef = null)
        {
            
            if(roleRef == null){
                roleRef = new Person("none");

                if (!roleRef_LvlOfInfl.Keys.ToList().Exists(x => x.name == roleRef.name))
                {
                    roleRef_LvlOfInfl.Add(roleRef, new Dictionary<string, float>());
                    roleRef_LvlOfInfl[roleRef].Add(roleName, lvlOfInfl);
                }
                else
                {
                    if(!roleRef_LvlOfInfl[roleRef].ContainsKey(roleName))
                    {
                        roleRef_LvlOfInfl[roleRef].Add(roleName, lvlOfInfl);
                    }
                }
            }
            else
            {

            }
        }


        public RuleAndStr actionForLink(List<MAction> notPosActions, List<PosActionItem> possibleActions, Person self, float rat, float mor, float imp, float abi) 
        {
            RuleAndStr actionToSend;

            try
            {
				actionToSend = roleMask.CalculateActionToUse(notPosActions, possibleActions, self, rat, mor, imp, abi, roleName, genLvlOfInfl, roleRef_LvlOfInfl);
					//debug.Write ("Trying from link "+self.name+" Maskname: "+ roleMask.GetMaskName() +" Rolename: "+roleName);
            }
            catch(Exception e)
            {
				debug.Write("Catching actionForLink error with code: '" + e + "'.");
                actionToSend = new RuleAndStr();
                actionToSend.chosenRule = new Rule("Empty", new MAction("Empty", 0.0f, 0.0f), null, null);
                actionToSend.strOfAct = 0.0f;
            }

			//Console.WriteLine ("From LINK from "+roleName+" ::: " + actionToSend.chosenRule.ruleName);
            return actionToSend;
        }


        public List<Person> GetRoleRefPpl()
        {
            return roleRef_LvlOfInfl.Keys.ToList();
        }


		public float GetlvlOfInfl(Person roleRef = null)
        { 
            if(roleRef == null){
                return genLvlOfInfl;
            }
            else if(roleRef_LvlOfInfl.ContainsKey(roleRef))
            {
                return roleRef_LvlOfInfl[roleRef]; 
            }
            else
            {
                return 0;
            }
        }

		public bool SetlvlOfInfl(float inp, Person roleRef = null)//{ lvlOfInfl = inp; }
        { 
            if(roleRef == null){
                genLvlOfInfl = inp;
            }
            else if(roleRef_LvlOfInfl.ContainsKey(roleRef))
            {
                roleRef_LvlOfInfl[roleRef] = inp;
            }
            else
            {
                debug.Write("Warning: Person " + roleRef.name + " does not exist in link. Not setting lvlOfInfl.");
                return false;
            }

            return true;
        }

		public bool AddToLvlOfInfl(float inp, Person roleRef = null)//{ lvlOfInfl += Calculator.unboundAdd(inp,lvlOfInfl); }
        {
            if (roleRef == null)
            {
                genLvlOfInfl += Calculator.UnboundAdd(inp, genLvlOfInfl);
            }
            else if (roleRef_LvlOfInfl.ContainsKey(roleRef))
            {
                roleRef_LvlOfInfl[roleRef] += Calculator.UnboundAdd(inp, roleRef_LvlOfInfl[roleRef]);
            }
            else
            {
                debug.Write("Warning: Person " + roleRef.name + " does not exist in link. Not setting lvlOfInfl.");
                return false;
            }

            return true;
        }

    }
}
