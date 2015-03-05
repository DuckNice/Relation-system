using System;


namespace NRelationSystem
{
    public enum typeMask
    {
        selfPerception,
        interPersonal,
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

}
