using System;
using System.Collections.Generic;


namespace NRelationSystem
{
    public enum TypeMask
    {
        selfPerc,
        interPers,
        culture
    }
    

    public struct RuleAndStr
    {
        public Rule chosenRule;
        public float strOfAct;
    };


    public enum TraitTypes
    {
        NiceNasty,
        ShyBolsterous,
        HonestFalse
    }


    public struct MaskAdds
    {
        public string role;
        public string mask;
        public float lvlOfInfl;
        public List<Person> linkPpl;

        public MaskAdds(string _role, string _mask, float _lvlOIf, List<Person> _linkPeople)
        {
            role = _role;
            mask = _mask;
            lvlOfInfl = _lvlOIf;
            linkPpl = _linkPeople;
        }
    };


    public delegate void ActionInvoker(Person subject, Person direct);
}
