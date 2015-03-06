using System;

namespace NRelationSystem
{
    public enum typeMask
    {
        selfPerception,
        interPersonal,
        culture
    };

    public struct RuleAndStrength
    {
        public Rule chosenRule;
        public float strengthOfAction;
    };

    public enum traitTypes
    {
        NiceNasty,
        ShyBolsterous
    }

    public delegate void ActionInvoker(object subject, string verb, object direct, object indirect);
}
