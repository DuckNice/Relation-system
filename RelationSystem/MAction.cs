using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NRelationSystem
{
    public class MAction
    {
        public float gain;
        public string name;
        public List<Rule> affectedRules;
        ActionInvoker actionInvoker;


        public MAction(string _efDesc, float _gain, ActionInvoker _actionInvoker)
        {
            gain = _gain;
            name = _efDesc;
            affectedRules = new List<Rule>();
            actionInvoker = _actionInvoker;
        }


        public MAction(string _efDesc, float _gain)
        {
            gain = _gain;
            name = _efDesc;
            affectedRules = new List<Rule>();
        }


        public void DoAction(object subject, string verb, object direct, object indirect)
        { //SUBJECT, VERB, OBB, DIROBJ    Setup
            if(actionInvoker != null)
            {
                actionInvoker(subject, verb, direct, indirect);
                 Console.WriteLine("Did action: " + name);
            }
            else{
                Console.WriteLine("No action to do in action '" + name + "'.");
            }
        }


        public float EstimationOfSuccess(float ability)
        {
            return ability; //RIGHT now, just ability
        }
    }
}
