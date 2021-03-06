﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using NRelationSystem;

public partial class Program: MonoBehaviour {

    public static string playerName { get { return RelationSystem.playerName; } }

	public Person GetPerson(string name){
        return relationSystem.GetPerson(name);
    }


    public Rule GetRule(string name){
        return relationSystem.pplAndMasks.GetRule(name);
    }


    public void AddPossibleRulesToRule(string ruleName, List<Rule> rules){
        relationSystem.pplAndMasks.AddPossibleRulesToRule(ruleName, rules);
    }


    public void AddRuleToMask(string person, string mask, string rule, float lvlOfInfl){
    	relationSystem.AddRuleToMask(person, mask, rule, lvlOfInfl);
    }


    public void CreateNewRule(string ruleName, string actName, RuleConditioner ruleCondition = null, RulePreference rulePreference = null, VisibilityCalculator visCalc = null){
        relationSystem.CreateNewRule(ruleName, actName, ruleCondition, rulePreference, visCalc);
    }


    public void CreateNewMask(string nameOfMask, float[] _traits = null, TypeMask maskType = TypeMask.interPers, string[] roles = null){
        relationSystem.CreateNewMask(nameOfMask, _traits, maskType, roles);
    }


    public void AddUpdateList(string name){
        relationSystem.AddUpdateList(name);
    }


    public string CapitalizeName(string s){
        return relationSystem.CapitalizeName(s);
    }


    public void AddAction(MAction action){
        relationSystem.AddAction(action);
    }


    public Being GetBeing(string name)
    {
        int index = beings.FindIndex(x => x.name == name);
        return (index == -1) ? null: beings[index];
    }
}