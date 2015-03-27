using System;
using System.Collections.Generic;


namespace NRelationSystem
{
    public class MAction
    {
        float selfGain;
        float againstGain;
        private string Name;
        public string name { get { return Name; } set { Name = value.ToLower(); } }
        public List<Rule> affectedRules;
        ActionInvoker actionInvoker;
        ActionInvoker sustainActionInvoker;
        public RelationSystem relationSystem;
        float Duration = 0.0f;
        public float duration { get { return Duration; } set { Duration = value; } }


        public MAction(string _efDesc, float _selfGain, float _againstGain, RelationSystem _relationSystem, ActionInvoker _actionInvoker = null, ActionInvoker _sustainActionInvoker = null)
        {
            selfGain = _selfGain;
            againstGain = _againstGain;
            name = _efDesc;
            affectedRules = new List<Rule>();
            relationSystem = _relationSystem;
            actionInvoker = _actionInvoker;
            sustainActionInvoker = _sustainActionInvoker;
        }


        public MAction(string _efDesc, float _selfGain, float _againstGain)
        {
            selfGain = _selfGain;
            againstGain = _againstGain;
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


		public float GetGain(bool selfGain){ if(selfGain){return this.selfGain;} return againstGain; }
        public void SetGain(float inp, bool selfGain) { if (selfGain) { this.selfGain = inp; } else { againstGain = inp; } }
        public void AddToGain(float inp, bool selfGain) { if (selfGain) { this.selfGain += inp; } else { againstGain += inp; } }

    }
}
