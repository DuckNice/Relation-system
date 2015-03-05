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

        public MAction(string _efDesc, float _gain)
        {
            gain = _gain;
            name = _efDesc;
            affectedRules = new List<Rule>();
        }

        public void DoAction()
        { //SUBJECT, VERB, OBB, DIROBJ    Setup
            Console.WriteLine("Did action: " + name);
            
        }

        public float EstimationOfSuccess(float ability)
        {
            return ability; //RIGHT now, just ability
        }
    }
}
