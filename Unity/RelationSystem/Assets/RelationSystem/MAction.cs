using System;
using System.Collections.Generic;


namespace NRelationSystem
{
    public class MAction
    {
        float selfGain;
        float againstGain;
        public string name = "";
        ActionInvoker actionInvoker;
        ActionInvoker sustainActionInvoker;
        public static RelationSystem relationSystem;
        float Duration = 2.0f;
        protected bool needsDirect = true;
        protected bool needsIndirect = false;

        public bool NeedsIndirect { get { return needsIndirect; } }
        public bool NeedsDirect { get { return needsDirect; } }
        public float duration { get { return Duration; } set { Duration = value; } }


		public MAction(string _efDesc, float _selfGain, float _againstGain, RelationSystem _relationSystem, ActionInvoker _actionInvoker = null, float _duration = 2.0f, ActionInvoker _sustainActionInvoker = null, bool _needsDirect = true, bool _needsIndirect = false)
        {
            selfGain = _selfGain;
            againstGain = _againstGain;
            name = _efDesc;
            relationSystem = _relationSystem;
            actionInvoker = _actionInvoker;
            sustainActionInvoker = _sustainActionInvoker;
			duration = _duration;
            needsDirect = _needsDirect;
            needsIndirect = _needsIndirect;
        }


        public MAction(string _efDesc, float _selfGain, float _againstGain)
        {
            selfGain = _selfGain;
            againstGain = _againstGain;
            name = _efDesc;
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
                //debug.Write("Warning: No action to do in action '" + name + "'.");
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
