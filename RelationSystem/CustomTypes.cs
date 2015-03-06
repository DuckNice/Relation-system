using System;

namespace NRelationSystem
{
    public enum typeMask
    {
        selfPerc,
        interPers,
        culture
    };

    public struct actionAndStrength
    {
        public MAction chosenAction;
        public float strengthOfAction;
    };

    public enum traitTypes
    {
        NiceNasty,
        ShyBolsterous
    }

    public delegate void ActionInvoker(object subject, string verb, object direct, object indirect);
}
