using System;
using System.Collections.Generic;


namespace NRelationSystem
{
    public class MAction
    {
        float gain;
        private string Name;
        public string name { get { return Name; } set { Name = value.ToLower(); } }
        public List<Rule> affectedRules;
        ActionInvoker actionInvoker;
        ActionInvoker sustainActionInvoker;
        public RelationSystem relationSystem;
        float duration = 0.0f;
        public float Duration { get { return duration; } set { duration = value; } }


        public MAction(string _efDesc, float _gain, RelationSystem _relationSystem, ActionInvoker _actionInvoker = null, ActionInvoker _sustainActionInvoker = null)
        {
            gain = _gain;
            name = _efDesc;
            affectedRules = new List<Rule>();
            relationSystem = _relationSystem;
            actionInvoker = _actionInvoker;
            sustainActionInvoker = _sustainActionInvoker;
        }


        public MAction(string _efDesc, float _gain)
        {
            gain = _gain;
            name = _efDesc;
            affectedRules = new List<Rule>();
        }


        public void DoAction(Person subject, Person direct, Rule _rule, Person[] indPpl = null, object[] misc = null)
        { //SUBJECT, VERB, OBB, DIROBJ    Setup

			if(actionInvoker != null)
            {
                actionInvoker(subject, direct, indPpl, misc);
			}
			else
            {
                debug.Write("Warning: No action to do in action '" + name + "'.");
            }

            relationSystem.DidAction(this, subject, direct, _rule);
        }


        public void DoSustainAction(Person subject, Person direct, Rule _rule, Person[] indPpl = null, object[] misc = null)
        { //SUBJECT, VERB, OBB, DIROBJ    Setup
            if (sustainActionInvoker != null)
            {
                sustainActionInvoker(subject, direct, indPpl, misc);
            }
            else
            {
                debug.Write("Warning: No action to do in action '" + name + "'.");
            }
        }


        public float EstimationOfSuccess(float ability)
        {
            return ability; //RIGHT now, just ability
        }


		public float GetGain(){ return gain; }
		public void SetGain(float inp){ gain = inp; }
		public void AddToGain(float inp){ gain += inp; }

    }
}
