using System;
using System.Collections.Generic;

namespace NRelationSystem
{
    public enum typeMask
    {
        selfPerc,
        interPers,
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

    public struct MaskAdds
    {
        public string role;
        public string mask;
        public float levelOfInfluence;
        public List<Person> linkPpl;

        public MaskAdds(string _role, string _mask, float _lvlOIf, List<Person> _linkPeople)
        {
            role = _role;
            mask = _mask;
            levelOfInfluence = _lvlOIf;
            linkPpl = _linkPeople;
        }
    };

    public delegate void ActionInvoker(Person subject, Person direct);
}
