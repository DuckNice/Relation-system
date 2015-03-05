using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRelationSystem
{
    public class Calculator
    {
        public static float calculateEgo(float impulsivity, Rule curRule, List<Rule> rulesThatWillTrigger)
        {
            float tempEgo = 1.0f;

            if (rulesThatWillTrigger != null)
            {
                foreach (Rule r in rulesThatWillTrigger)
                {
                    tempEgo += r.strength * r.actionToTrigger.EstimationOfSuccess(0.1f);   //MISSING visibility,
                    //probability is just r.strength for now. let's leave it like that for simplicity
                }
            }

            tempEgo *= (1 - impulsivity);

            float ego = impulsivity * CalculateGain(curRule) + tempEgo;
            Console.WriteLine("Ego: " + ego);

            return ego;
        }


        public static float calculateSuperEgo(Rule rule, List<Rule> rules, float maskInfl)
        {
            float superEgo = 1.0f;

                //own rules morality:
            superEgo += rule.strength * maskInfl;

                //consequent rules morality:
            if (rules != null)
            {
                foreach (Rule r in rules)
                {
                    superEgo += r.strength * maskInfl;
                }
            }
            Console.WriteLine("SuperEgo: " + superEgo);
            return superEgo;
        }


        public static float CalculateRule(float rationality, float morality, float impulsivity, Rule rule, List<Rule> rulesThatWillTrigger, float maskInfl)
        {
            return (calculateEgo(impulsivity, rule, rulesThatWillTrigger) * rationality)
                 + (calculateSuperEgo(rule, rulesThatWillTrigger, maskInfl) * morality);
        }


        public static float CalculateGain(Rule rule)
        {
            return rule.actionToTrigger.gain;
        }


        float Blend(float x, float y, float WeightingFactor)
        {
            float uWeightingFactor = 1 - ((1 - WeightingFactor) / 2);
            float blend = y * uWeightingFactor + x * (1 - uWeightingFactor);
            return blend;
        }


        float unboundAdd(float unboundedNumber, float curValue)
        {
            if (curValue > 0)
            {
                double dist;
                if (unboundedNumber > 0)
                {
                    dist = Math.Abs((1) - curValue);
                }
                else
                {
                    dist = Math.Abs((-1) - curValue);
                }
                return unboundedNumber * (float)dist;
            }
            else
            {
                double dist;
                if (unboundedNumber > 0)
                {
                    dist = Math.Abs((1) - curValue);
                }
                else
                {
                    dist = Math.Abs((-1) - curValue);
                }
                return unboundedNumber * (float)dist;
            }
        }
    }
}
