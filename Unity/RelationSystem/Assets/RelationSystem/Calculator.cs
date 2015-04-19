using System;
using System.Collections.Generic;


namespace NRelationSystem
{
    public class Calculator
    {
        public static float CalculateEgo(float impulsivity, float ability, Rule curRule, List<Rule> rulesThatWillTrigger)
        {
            float tempEgo = 1.0f;

            if (rulesThatWillTrigger != null)
            {
                foreach (Rule rule in rulesThatWillTrigger)
                {
                    float visibility = rule.visCalc();

					tempEgo += rule.GetRuleStrength() * rule.actionToTrigger.EstimationOfSuccess(ability) * visibility * CalculateGain(rule, false);

                    //probability is just r.strength for now. let's leave it like that for simplicity
					//right now it just check visibility for all people in world, not just the people involved in the action considered.
                }
            }

            tempEgo *= (1 - impulsivity);

            float ego = impulsivity * CalculateGain(curRule, true) + tempEgo;

          //  debug.Write("Ego: " + ego);

            return ego;
        }


        public static float CalculateSuperEgo(Rule rule, List<Rule> rules, float maskInfl)
        {
            float superEgo = 0.0f;

                //own rules morality:
            superEgo += rule.GetRuleStrength() * maskInfl;

                //consequent rules morality:
            if (rules != null)
            {
                foreach (Rule r in rules)
                {
                    superEgo += r.GetRuleStrength() * maskInfl;
                }
            }

			//debug.Write("SuperEgo: " + superEgo);

            return superEgo;
        }


        public static float CalculateRule(float rationality, float morality, float impulsivity, float ability, Rule rule, List<Rule> rulesThatWillTrigger, float maskInfl)
        {
			float returner = (CalculateEgo(impulsivity, ability, rule, rulesThatWillTrigger) * rationality) + (CalculateSuperEgo(rule, rulesThatWillTrigger, maskInfl) * morality);

			//debug.Write("L: "+returner);

			return returner;
        }


        public static float CalculateGain(Rule rule, bool selfGain)
        {
            return rule.actionToTrigger.GetGain(selfGain);
        }


        float Blend(float x, float y, float WeightingFactor)
        {
            float uWeightingFactor = 1 - ((1 - WeightingFactor) / 2);
            float blend = y * uWeightingFactor + x * (1 - uWeightingFactor);

            return blend;
        }


        public static float UnboundAdd(float unboundedNumber, float curValue)
        {
            float dist;
            if (unboundedNumber > 0)
            {
                dist = Math.Abs((1) - curValue);
            }
            else
            {
                dist = Math.Abs((-1) - curValue);
            }
            return unboundedNumber * dist;
        }

		public static float NegPosTransform(float inp){
			return inp-(1-inp);
		}


    }
}
