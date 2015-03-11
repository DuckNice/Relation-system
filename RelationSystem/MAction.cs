using System;
using System.Collections.Generic;


namespace NRelationSystem
{
    public class MAction
    {
        public float gain;
        public string name;
        public List<Rule> affectedRules;
        ActionInvoker actionInvoker;
        private RelationSystem relationSystem;

        public MAction(string _efDesc, float _gain, ActionInvoker _actionInvoker, RelationSystem _relationSystem)
        {
            gain = _gain;
            name = _efDesc;
            affectedRules = new List<Rule>();
            relationSystem = _relationSystem;
            actionInvoker = _actionInvoker;
        }


        public MAction(string _efDesc, float _gain)
        {
            gain = _gain;
            name = _efDesc;
            affectedRules = new List<Rule>();
        }


        public void DoAction(Person subject, Person direct)
        { //SUBJECT, VERB, OBB, DIROBJ    Setup
            if(actionInvoker != null)
            {
                actionInvoker(subject, direct);
                 //Console.WriteLine("Did action: " + name);
                relationSystem.DidAction(this, subject, direct);
            }
            else
            {
                Console.WriteLine("No action to do in action '" + name + "'.");
            }
        }

        public float EstimationOfSuccess(float ability)
        {
            return ability; //RIGHT now, just ability
        }
    }
}
