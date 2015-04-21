using System;
using System.Collections.Generic;
using System.Linq;

namespace NRelationSystem
{
    public class Link
    {
        public Dictionary<Person, Dictionary<string, float>> _roleRef = new Dictionary<Person, Dictionary<string, float>>();
        public Mask _roleMask;
        public Person empty;

        public Link(string _genRoleName, Mask roleMask, float lvlOfInfl) 
        {
            empty = new Person("none");
            
            _roleRef.Add(empty, new Dictionary<string, float>());
            _roleRef[empty].Add(_genRoleName, lvlOfInfl);
            _roleMask = roleMask;
        }


        public void AddRoleRef(Dictionary<Person, Dictionary<string, float>> roleRefs)
        {
            foreach(KeyValuePair<Person, Dictionary<string, float>> roleref in roleRefs)
            {
                foreach (KeyValuePair<string, float> influence in roleref.Value)
                    AddRoleRef(influence.Key, influence.Value, roleref.Key);
            }
        }


        public void AddRoleRef(string roleName, float lvlOfInfl, Person roleRef = null)
        {
            if(roleRef == null)
                roleRef = empty;

            if (!_roleRef.Keys.ToList().Exists(x => x.name == roleRef.name))
            {
                _roleRef.Add(roleRef, new Dictionary<string, float>());
                _roleRef[roleRef].Add(roleName, lvlOfInfl);
            }
            else
            {
                if (!_roleRef[roleRef].ContainsKey(roleName))
                    _roleRef[roleRef].Add(roleName, lvlOfInfl);
                
                else
                    debug.Write("Warning: roleName already associated with this character in link. Not adding role reference.");
            }
        }


        public RuleAndStr actionForLink(List<MAction> notPosActions, List<PosActionItem> possibleActions, Person self, float rat, float mor, float imp, float abi) 
        {
            RuleAndStr actionToSend;

            try
            {
				actionToSend = _roleMask.CalculateActionToUse(notPosActions, possibleActions, self, rat, mor, imp, abi, empty, _roleRef);
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
            return _roleRef.Keys.ToList();
        }


		public float GetlvlOfInfl(string roleName, Person roleRef = null)
        { 
            if(roleRef == null){
                roleRef = empty;    
            }
            
            if(_roleRef.ContainsKey(roleRef) && _roleRef[roleRef].ContainsKey(roleName))
            {
                return _roleRef[roleRef][roleName]; 
            }
            else
            {
                return 0;
            }
        }


		public bool SetlvlOfInfl(float inp, string roleName, Person roleRef = null)
        {
            if(roleRef == null){
                roleRef = empty;
            }
            else if(_roleRef.ContainsKey(roleRef) && _roleRef[roleRef].ContainsKey(roleName))
            {
                _roleRef[roleRef][roleName] = inp;
            }
            else
            {
                debug.Write("Warning: Person " + roleRef.name + " does not exist in link. Not setting lvlOfInfl.");
                return false;
            }

            return true;
        }


		public bool AddToLvlOfInfl(float inp, string roleName, Person roleRef = null)
        {
            if (roleRef == null)
            {
                roleRef = empty;
            }
            else if (_roleRef.ContainsKey(roleRef) && _roleRef[roleRef].ContainsKey(roleName))
            {
                _roleRef[roleRef][roleName] += Calculator.UnboundAdd(inp, _roleRef[roleRef][roleName]);
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