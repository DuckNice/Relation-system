﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using NRelationSystem;

public partial class Program : MonoBehaviour {

    void SetupActions()
    {
        // ---------- SELF ACTIONS

        ActionInvoker flee = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " flees the scene!");
			roomMan.EnterRoom("Jail",subject);
        };
        relationSystem.AddAction(new MAction("flee", 1.0f, -0.5f, relationSystem, flee, 10f, _needsDirect:false));

        ActionInvoker doNothing = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " is doing nothing. What a bore.");
            subject.moods[MoodTypes.energTired] += Calculator.UnboundAdd(0.2f, subject.moods[MoodTypes.energTired]);
        };
        relationSystem.AddAction(new MAction("doNothing", -1.0f, -1.0f, relationSystem, doNothing, 5f, _needsDirect:false));

        // ---------- INTERPERSONAL ACTIONS
        ActionInvoker greet = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " greets " + relationSystem.CapitalizeName(direct.name));
        };
        relationSystem.AddAction(new MAction("Greet", 0.7f, 0.5f, relationSystem, greet, 2f));

        ActionInvoker kiss = (subject, direct, indPpl, misc) =>
        {
			//debug.Write(""+subject.CheckRoleName("partner",direct)+
			   //         " "+subject.interPersonal.Exists(x => x.GetRoleRefPpl().Exists(y => y.name == direct.name && y.interPersonal.Exists(z => z.roleName == "partner" && z.GetRoleRefPpl().Exists(s => s.name == subject.name))) && x.roleName == "partner"));
			if (subject.CheckRoleName("partner",direct))
            {
                UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " kisses " + relationSystem.CapitalizeName(direct.name));
            }
            else
            {
                UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " kisses " + relationSystem.CapitalizeName(direct.name) + " outside a relationship!");
            }

            direct.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.3f, direct.moods[MoodTypes.hapSad]);
            direct.moods[MoodTypes.arousDisgus] += Calculator.UnboundAdd(0.3f, direct.moods[MoodTypes.arousDisgus]);
            subject.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.3f, subject.moods[MoodTypes.hapSad]);
            subject.moods[MoodTypes.arousDisgus] += Calculator.UnboundAdd(0.3f, subject.moods[MoodTypes.arousDisgus]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, 0.2f);
        };
        relationSystem.AddAction(new MAction("kiss", 0.4f, 0.4f, relationSystem, kiss, 5f));

        ActionInvoker askAboutPartnerStatus = (subject, direct, indPpl, misc) =>
        {
			if (subject.CheckRoleName("partner",direct))
            {
                UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " asks if " + relationSystem.CapitalizeName(direct.name) + " still wants to be their partner after what they've done.");
				direct.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(-0.3f, direct.moods[MoodTypes.hapSad]);
				subject.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(-0.3f, subject.moods[MoodTypes.hapSad]);
				subject.AddToOpinionValue(TraitTypes.NiceNasty,direct,-0.3f);
				subject.AddToOpinionValue(TraitTypes.HonestFalse,direct,-0.4f);
				direct.AddToOpinionValue(TraitTypes.NiceNasty,subject,-0.3f);
            }
            else
            {
                UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " asks if " + relationSystem.CapitalizeName(direct.name) + " wants to be their partner. Is it love?");
				direct.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.6f, direct.moods[MoodTypes.hapSad]);
				direct.moods[MoodTypes.arousDisgus] += Calculator.UnboundAdd(0.6f, direct.moods[MoodTypes.arousDisgus]);
				subject.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.6f, subject.moods[MoodTypes.hapSad]);
				subject.moods[MoodTypes.arousDisgus] += Calculator.UnboundAdd(0.6f, subject.moods[MoodTypes.arousDisgus]);
            }
        };
        relationSystem.AddAction(new MAction("askAboutPartnerStatus", 1.0f, 0.5f, relationSystem, askAboutPartnerStatus, 5f));

        ActionInvoker chooseAnotherAsPartner = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " chooses " + relationSystem.CapitalizeName(direct.name) + " as their partner! How romantic.");
            direct.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.6f, direct.moods[MoodTypes.hapSad]);
            direct.moods[MoodTypes.arousDisgus] += Calculator.UnboundAdd(0.2f, direct.moods[MoodTypes.arousDisgus]);
            subject.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.6f, subject.moods[MoodTypes.hapSad]);
            subject.moods[MoodTypes.arousDisgus] += Calculator.UnboundAdd(0.2f, subject.moods[MoodTypes.arousDisgus]);

			direct.AddToOpinionValue(TraitTypes.NiceNasty,subject,0.2f);
			subject.AddToOpinionValue(TraitTypes.NiceNasty,direct,0.1f);
			direct.AddToInterPersonalLvlOfInfl(subject,0.2f);
			subject.AddToInterPersonalLvlOfInfl(direct,0.2f);

//            relationSystem.AddLinkToPerson(relationSystem.CapitalizeName(subject.name), TypeMask.interPers, "partner", "RomanticRelationship", 0, relationSystem.CapitalizeName(direct.name), 0.5f);
//            relationSystem.AddLinkToPerson(relationSystem.CapitalizeName(direct.name), TypeMask.interPers, "partner", "RomanticRelationship", 0, relationSystem.CapitalizeName(subject.name), 0.5f);
        };
        relationSystem.AddAction(new MAction("chooseAnotherAsPartner", 0.4f, 0.6f, relationSystem, chooseAnotherAsPartner, 5f));

        ActionInvoker stayAsPartner = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " stays with " + relationSystem.CapitalizeName(direct.name) + ". Nothing separates these two.");
            direct.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.3f, direct.moods[MoodTypes.hapSad]);
            subject.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.1f, subject.moods[MoodTypes.hapSad]);
        };
        relationSystem.AddAction(new MAction("stayAsPartner", 0.4f, 0.3f, relationSystem, stayAsPartner, 4f));

        ActionInvoker LeavePartner = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " leaves " + relationSystem.CapitalizeName(direct.name) + "!");
            direct.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(-0.7f, direct.moods[MoodTypes.hapSad]);
            subject.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(-0.2f, subject.moods[MoodTypes.hapSad]);

          //  subject.RemoveLink(TypeMask.interPers, subject.interPersonal.Find(x => x.roleName == "partner" && x.GetRoleRefPpl().Exists(y => y.name == direct.name)));
          //  direct.RemoveLink(TypeMask.interPers, direct.interPersonal.Find(x => x.roleName == "partner" && x.GetRoleRefPpl().Exists(y => y.name == subject.name)));
			//relationSystem.AddLinkToPerson (relationSystem.CapitalizeName(subject.name),TypeMask.interPers,"enemy","Rivalry",0,direct.name,0.3f);
			//relationSystem.AddLinkToPerson (relationSystem.CapitalizeName(direct.name),TypeMask.interPers,"enemy","Rivalry",0,subject.name,0.5f);
        };
        relationSystem.AddAction(new MAction("LeavePartner", -0.3f, -0.7f, relationSystem, LeavePartner, 5f));

        ActionInvoker flirt = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " flirts with " + relationSystem.CapitalizeName(direct.name) + ".");
            direct.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.2f, direct.moods[MoodTypes.hapSad]);
            direct.moods[MoodTypes.arousDisgus] += Calculator.UnboundAdd(0.2f, direct.moods[MoodTypes.arousDisgus]);
            subject.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.2f, subject.moods[MoodTypes.hapSad]);
            subject.moods[MoodTypes.arousDisgus] += Calculator.UnboundAdd(0.2f, subject.moods[MoodTypes.arousDisgus]);
			subject.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.1f, subject.moods[MoodTypes.energTired]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, 0.1f);
			subject.AddToOpinionValue(TraitTypes.NiceNasty, direct, 0.1f);

			subject.AddToInterPersonalLvlOfInfl(direct,0.05f);
			direct.AddToInterPersonalLvlOfInfl(subject,0.05f);
        };
        relationSystem.AddAction(new MAction("flirt", 0.1f, 0.1f, relationSystem, flirt, 4f));

        ActionInvoker chat = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " chats with " + relationSystem.CapitalizeName(direct.name) + ".");
            direct.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.1f, direct.moods[MoodTypes.energTired]);
            subject.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.1f, subject.moods[MoodTypes.energTired]);
			subject.AddToInterPersonalLvlOfInfl(direct,0.05f);
			direct.AddToInterPersonalLvlOfInfl(subject,0.05f);
        };
        relationSystem.AddAction(new MAction("chat", 0.0f, 0.0f, relationSystem, chat, 10f));

        ActionInvoker giveGift = (subject, direct, indPpl, misc) =>
        {
            Possession giftToGive = new Possession();
            List<Possession> gifts = ((Possession[])misc).ToList();
            giftToGive = gifts.Find(x => x.Name == "game" || x.Name == "company");
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " gives the gift of " + giftToGive.objectName + " to " + relationSystem.CapitalizeName(direct.name) + ".");

            beings.Find(x => x.name == subject.name).possessions.Find(y => y.Name == giftToGive.Name).value -= 1f;
            if (beings.Find(x => x.name == direct.name).possessions.Exists(x => x.Name == giftToGive.Name))
            {
                beings.Find(x => x.name == subject.name).possessions.Find(y => y.Name == giftToGive.Name).value += 1f;
            }
            else
            {
                beings.Find(x => x.name == direct.name).possessions.Add(giftToGive);
            }

            direct.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.3f, direct.moods[MoodTypes.hapSad]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, 0.1f);
			direct.AddToOpinionValue(TraitTypes.CharitableGreedy, subject, 0.2f);
        };
        relationSystem.AddAction(new MAction("giveGift", 0.2f, 0.4f, relationSystem, giveGift, 5f));

        ActionInvoker poison = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGame(relationSystem.CapitalizeName(subject.name) + " poisons " + relationSystem.CapitalizeName(direct.name) + "! Oh no!");

			float rand = UnityEngine.Random.Range (0,1);
			rand += subject.GetAbility();

			if(rand > 0.5f){
				UIFunctions.WriteGameLine(" It's a success! "+relationSystem.CapitalizeName(direct.name)+" is poisoned!");
				direct.moods[MoodTypes.angryFear] += Calculator.UnboundAdd(-0.8f, direct.moods[MoodTypes.angryFear]);
				direct.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.5f, direct.moods[MoodTypes.energTired]);
				subject.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.2f, subject.moods[MoodTypes.energTired]);
				
				direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.5f);
				direct.AddToOpinionValue(TraitTypes.HonestFalse, subject, -0.5f);
			}
			else{
				UIFunctions.WriteGameLine(" It fails! "+relationSystem.CapitalizeName(direct.name)+" is angry!");
			}
            direct.moods[MoodTypes.angryFear] += Calculator.UnboundAdd(0.8f, direct.moods[MoodTypes.angryFear]);
            direct.moods[MoodTypes.energTired] += Calculator.UnboundAdd(0.5f, direct.moods[MoodTypes.energTired]);
            subject.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.2f, subject.moods[MoodTypes.energTired]);

            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.5f);
			direct.AddToOpinionValue(TraitTypes.HonestFalse, subject, -0.5f);
        };
        relationSystem.AddAction(new MAction("poison", 0.1f, -0.9f, relationSystem, poison, 10f));

        ActionInvoker gossip = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " gossips with " + relationSystem.CapitalizeName(direct.name));
            direct.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.1f, direct.moods[MoodTypes.energTired]);
            subject.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.1f, subject.moods[MoodTypes.energTired]);
			direct.AddToOpinionValue(TraitTypes.HonestFalse, subject, -0.1f);
			subject.AddToOpinionValue(TraitTypes.HonestFalse, direct, -0.1f);
        };
        relationSystem.AddAction(new MAction("gossip", 0.1f, 0.1f, relationSystem, gossip, 7f));

        ActionInvoker argue = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGame(relationSystem.CapitalizeName(subject.name) + " argues with " + relationSystem.CapitalizeName(direct.name) + "! ");

			if(subject.GetAbility() > direct.GetAbility()){
				UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " is winning!");
				direct.moods[MoodTypes.angryFear] += Calculator.UnboundAdd(0.3f, direct.moods[MoodTypes.angryFear]);
				subject.moods[MoodTypes.angryFear] += Calculator.UnboundAdd(0.1f, subject.moods[MoodTypes.angryFear]);
				direct.moods[MoodTypes.energTired] += Calculator.UnboundAdd(0.3f, direct.moods[MoodTypes.energTired]);
				subject.moods[MoodTypes.energTired] += Calculator.UnboundAdd(0.1f, subject.moods[MoodTypes.energTired]);
				direct.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(-0.2f, direct.moods[MoodTypes.hapSad]);
				subject.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.1f, subject.moods[MoodTypes.hapSad]);
				direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.3f);
				subject.AddToOpinionValue(TraitTypes.NiceNasty, direct, -0.2f);
				subject.AddToInterPersonalLvlOfInfl(direct,0.3f);
				direct.AddToInterPersonalLvlOfInfl(subject,0.3f);
			}
			else{
				UIFunctions.WriteGameLine(relationSystem.CapitalizeName(direct.name) + " is winning!");
				direct.moods[MoodTypes.angryFear] += Calculator.UnboundAdd(0.1f, direct.moods[MoodTypes.angryFear]);
				subject.moods[MoodTypes.angryFear] += Calculator.UnboundAdd(0.3f, subject.moods[MoodTypes.angryFear]);
				direct.moods[MoodTypes.energTired] += Calculator.UnboundAdd(0.1f, direct.moods[MoodTypes.energTired]);
				subject.moods[MoodTypes.energTired] += Calculator.UnboundAdd(0.3f, subject.moods[MoodTypes.energTired]);
				direct.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(-0.1f, direct.moods[MoodTypes.hapSad]);
				subject.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(-0.2f, subject.moods[MoodTypes.hapSad]);
				direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.2f);
				subject.AddToOpinionValue(TraitTypes.NiceNasty, direct, -0.3f);
				subject.AddToInterPersonalLvlOfInfl(direct,0.3f);
				direct.AddToInterPersonalLvlOfInfl(subject,0.3f);
			}
        };
        relationSystem.AddAction(new MAction("argue", -0.2f, -0.4f, relationSystem, argue, 8f));

		ActionInvoker makeDistraction = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " makes a distraction for " + relationSystem.CapitalizeName(direct.name) + "!");
            float rand = UnityEngine.Random.Range(0, 2); //SHOULD PROBABLY BASE THIS ON ABILITY
			rand += subject.GetAbility();
            if (rand > 1f)
            {
                UIFunctions.WriteGameLine("It's a success! They are very distracted");
                direct.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.5f, direct.moods[MoodTypes.energTired]);
                subject.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.3f, subject.moods[MoodTypes.hapSad]);

				direct.AddToInterPersonalLvlOfInfl(subject,-0.4f);
            }
            else
            {
                UIFunctions.WriteGameLine("It fails. "+relationSystem.CapitalizeName(direct.name)+" is now more wary than before.");
                direct.moods[MoodTypes.energTired] += Calculator.UnboundAdd(0.3f, direct.moods[MoodTypes.energTired]);
                direct.moods[MoodTypes.angryFear] += Calculator.UnboundAdd(-0.3f, direct.moods[MoodTypes.angryFear]);
                subject.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(-0.4f, subject.moods[MoodTypes.hapSad]);

				direct.AddToInterPersonalLvlOfInfl(subject,0.6f);
            }
			subject.AddToInterPersonalLvlOfInfl(direct,0.3f);
        };
        relationSystem.AddAction(new MAction("makeDistraction", 0.2f, -0.5f, relationSystem, makeDistraction, 6f));

        ActionInvoker reminisce = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " reminisces about old times with " + relationSystem.CapitalizeName(direct.name) + ".");
            direct.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.2f, direct.moods[MoodTypes.hapSad]);
            subject.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.2f, subject.moods[MoodTypes.hapSad]);
            subject.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.1f, subject.moods[MoodTypes.energTired]);
            direct.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.1f, direct.moods[MoodTypes.energTired]);
			direct.AddToOpinionValue(TraitTypes.NiceNasty,subject,0.1f);
			subject.AddToOpinionValue(TraitTypes.NiceNasty,direct,0.1f);
        };
        relationSystem.AddAction(new MAction("reminisce", 0.1f, 0.1f, relationSystem, reminisce, 9f));

        ActionInvoker deny = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " denies " + relationSystem.CapitalizeName(direct.name) + " their wishes.");
            direct.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(-0.4f, direct.moods[MoodTypes.hapSad]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.2f);
			direct.AddToOpinionValue(TraitTypes.HonestFalse, subject, -0.1f);
			subject.AddToOpinionValue(TraitTypes.NiceNasty, direct, -0.1f);
			direct.AddToInterPersonalLvlOfInfl(subject,0.2f);
        };
        relationSystem.AddAction(new MAction("deny", 0.4f, -0.4f, relationSystem, deny, 3f));

        ActionInvoker praise = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " praises " + relationSystem.CapitalizeName(direct.name) + ".");
            direct.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.3f, direct.moods[MoodTypes.hapSad]);
            subject.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.2f, subject.moods[MoodTypes.energTired]);
            direct.moods[MoodTypes.energTired] += Calculator.UnboundAdd(0.1f, direct.moods[MoodTypes.energTired]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, 0.1f);

            // subject.GetRule("enthuseaboutgreatnessofperson").SetRuleStrength(-1.0f);
			direct.AddToInterPersonalLvlOfInfl(subject,0.1f);
			subject.AddToInterPersonalLvlOfInfl(direct,0.1f);
        };
		relationSystem.AddAction(new MAction("praise", 0.4f, 0.2f, relationSystem, praise, 5f));

		ActionInvoker cry = (subject, direct, indPpl, misc) =>
		{
			UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " cries!");
			subject.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(-0.3f, subject.moods[MoodTypes.hapSad]);
			subject.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.2f, subject.moods[MoodTypes.energTired]);
			direct.AddToOpinionValue(TraitTypes.NiceNasty,subject,0.2f);

			subject.GetRule("cry").AddToRuleStrength(-0.2f);
			// subject.GetRule("enthuseaboutgreatnessofperson").SetRuleStrength(-1.0f);
		};
		relationSystem.AddAction(new MAction("cry", 0.7f, -0.5f, relationSystem, cry, 5f));

		ActionInvoker console = (subject, direct, indPpl, misc) =>
		{
			UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " consoles " + relationSystem.CapitalizeName(direct.name) + ".");
			subject.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.1f, subject.moods[MoodTypes.hapSad]);
			subject.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.1f, subject.moods[MoodTypes.energTired]);
			direct.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.1f, direct.moods[MoodTypes.hapSad]);
			direct.AddToOpinionValue(TraitTypes.NiceNasty,subject,0.2f);
			direct.AddToOpinionValue(TraitTypes.HonestFalse,subject,0.1f);
			
			direct.GetRule("cry").AddToRuleStrength(-0.2f);
			// subject.GetRule("enthuseaboutgreatnessofperson").SetRuleStrength(-1.0f);
		};
		relationSystem.AddAction(new MAction("console", 0.4f, 0.4f, relationSystem, console, 4f));


        // --------------- CULTURAL ACTIONS

        ActionInvoker convict = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " convicts " + relationSystem.CapitalizeName(direct.name) + " of commiting a crime. To Jail with him!");
            direct.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(-0.8f, direct.moods[MoodTypes.hapSad]);
            direct.moods[MoodTypes.angryFear] += Calculator.UnboundAdd(0.6f, direct.moods[MoodTypes.angryFear]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.4f);

			subject.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.2f, subject.moods[MoodTypes.hapSad]);
			subject.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.2f, subject.moods[MoodTypes.energTired]);

			roomMan.EnterRoom("Jail",direct);
			if(direct.name == "player"){
				UIFunctions.WriteGameLine("Player is in Jail! Game Over!");
			}
        };
        relationSystem.AddAction(new MAction("convict", 1.0f, -0.5f, relationSystem, convict, 6f));

        ActionInvoker fight = (subject, direct, indPpl, misc) =>
        {
			UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " fights " + relationSystem.CapitalizeName(direct.name)+"!");
			if(subject.GetAbility() > direct.GetAbility()){
				UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name)+" is winning!");
			}
			else{
				UIFunctions.WriteGameLine(relationSystem.CapitalizeName(direct.name)+" is winning!");
			}

            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " is attempting to fight " + relationSystem.CapitalizeName(direct.name));
            direct.moods[MoodTypes.angryFear] += Calculator.UnboundAdd(0.4f, direct.moods[MoodTypes.angryFear]);
            direct.moods[MoodTypes.energTired] += Calculator.UnboundAdd(0.5f, direct.moods[MoodTypes.energTired]);
			subject.moods[MoodTypes.angryFear] += Calculator.UnboundAdd(0.4f, subject.moods[MoodTypes.angryFear]);
			subject.moods[MoodTypes.energTired] += Calculator.UnboundAdd(0.5f, subject.moods[MoodTypes.energTired]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.4f);
            subject.AddToOpinionValue(TraitTypes.NiceNasty, direct, -0.4f);
			direct.AddToInterPersonalLvlOfInfl(subject,0.4f);
        };
        relationSystem.AddAction(new MAction("fight", 0.6f, -0.4f, relationSystem, fight, 12f));

        ActionInvoker bribe = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " attempts to bribe " + relationSystem.CapitalizeName(direct.name));
            direct.moods[MoodTypes.angryFear] += Calculator.UnboundAdd(-0.4f, direct.moods[MoodTypes.angryFear]);
            direct.AddToOpinionValue(TraitTypes.HonestFalse, subject, -0.3f);
			direct.AddToOpinionValue(TraitTypes.CharitableGreedy, subject, 0.2f);
			direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, 0.1f);
			subject.AddToOpinionValue(TraitTypes.NiceNasty, direct, -0.1f);
			subject.AddToOpinionValue(TraitTypes.CharitableGreedy, direct, -0.1f);

            beings.Find(x => x.name == subject.name).possessions.Find(y => y.Name == "money").value -= 30f;
            beings.Find(x => x.name == direct.name).possessions.Find(y => y.Name == "money").value += 30f;
			direct.AddToInterPersonalLvlOfInfl(subject,-0.1f);
        };
        relationSystem.AddAction(new MAction("bribe", 0.2f, 0.6f, relationSystem, bribe, 5f));

        ActionInvoker argueInnocence = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " argues " + relationSystem.CapitalizeName(direct.name) + "'s innocence.");
            direct.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.3f, direct.moods[MoodTypes.hapSad]);
			subject.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.1f, subject.moods[MoodTypes.hapSad]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, 0.2f);
			direct.AddToOpinionValue(TraitTypes.HonestFalse, subject, 0.1f);
        };
        relationSystem.AddAction(new MAction("argueInnocence", 0.0f, 0.3f, relationSystem, argueInnocence, 7f));

        ActionInvoker argueGuiltiness = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " argues " + relationSystem.CapitalizeName(direct.name) + "'s guilt!");
            direct.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(-0.3f, direct.moods[MoodTypes.hapSad]);
			subject.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.1f, subject.moods[MoodTypes.hapSad]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.2f);
			direct.AddToOpinionValue(TraitTypes.HonestFalse, subject, -0.2f);
        };
        relationSystem.AddAction(new MAction("argueGuiltiness", 0.0f, -0.3f, relationSystem, argueGuiltiness, 7f));

        ActionInvoker steal = (subject, direct, indPpl, misc) =>
        {
            
            subject.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.4f, subject.moods[MoodTypes.hapSad]);
            subject.moods[MoodTypes.angryFear] += Calculator.UnboundAdd(-0.3f, subject.moods[MoodTypes.angryFear]);
            subject.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.3f, subject.moods[MoodTypes.energTired]);
			direct.AddToOpinionValue(TraitTypes.CharitableGreedy,subject,-0.3f);
			direct.AddToOpinionValue(TraitTypes.NiceNasty,subject,-0.3f);

			float stealAmount = UnityEngine.Random.Range(1,20);
			stealAmount += subject.GetAbility()*30f;
			UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " steals "+stealAmount+" bungarian rupees from " + relationSystem.CapitalizeName(direct.name) + ".");
            beings.Find(x => x.name == subject.name).possessions.Find(y => y.Name == "money").value += stealAmount;
            beings.Find(x => x.name == direct.name).possessions.Find(y => y.Name == "money").value -= stealAmount;
			direct.AddToInterPersonalLvlOfInfl(subject,0.2f); 
        };
        relationSystem.AddAction(new MAction("steal", 0.7f, -0.5f, relationSystem, steal, 10f));

        ActionInvoker makefunof = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " makes fun of " + relationSystem.CapitalizeName(direct.name));

			if(direct.GetOpinionValue(TraitTypes.NiceNasty,subject) > 0.4f){
				subject.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.2f, subject.moods[MoodTypes.hapSad]);
				direct.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(-0.1f, direct.moods[MoodTypes.hapSad]);
				direct.moods[MoodTypes.angryFear] += Calculator.UnboundAdd(0.1f, direct.moods[MoodTypes.angryFear]);
				direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.2f);
				direct.AddToOpinionValue(TraitTypes.HonestFalse, subject, -0.1f);
				direct.AddToInterPersonalLvlOfInfl(direct,0.1f);
			}
			else if(direct.GetOpinionValue(TraitTypes.NiceNasty,subject) < 0.4f){
				subject.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.2f, subject.moods[MoodTypes.hapSad]);
				direct.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(-0.4f, direct.moods[MoodTypes.hapSad]);
				direct.moods[MoodTypes.angryFear] += Calculator.UnboundAdd(0.3f, direct.moods[MoodTypes.angryFear]);
				direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.2f);
				direct.AddToOpinionValue(TraitTypes.HonestFalse, subject, -0.1f);
				direct.AddToInterPersonalLvlOfInfl(direct,0.1f);
			}
            subject.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.2f, subject.moods[MoodTypes.energTired]);
        };
        relationSystem.AddAction(new MAction("makefunof", 0.4f, -0.6f, relationSystem, makefunof, 4f));

        ActionInvoker telljoke = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " tells a joke to " + relationSystem.CapitalizeName(direct.name) + ". It's funny!");

            subject.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.1f, subject.moods[MoodTypes.energTired]);
            subject.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.2f, subject.moods[MoodTypes.hapSad]);
            direct.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.1f, direct.moods[MoodTypes.hapSad]);
            direct.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.1f, direct.moods[MoodTypes.energTired]);
			subject.AddToOpinionValue(TraitTypes.NiceNasty,direct,0.1f);
			direct.AddToOpinionValue(TraitTypes.NiceNasty,subject,0.2f);

        };
        relationSystem.AddAction(new MAction("telljoke", 0.1f, 0.1f, relationSystem, telljoke, 5f));

        ActionInvoker harass = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " harasses " + relationSystem.CapitalizeName(direct.name) + ". Ugh, how annoying.");
			subject.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.2f, subject.moods[MoodTypes.energTired]);
            subject.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.2f, subject.moods[MoodTypes.hapSad]);
            direct.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(-0.4f, direct.moods[MoodTypes.hapSad]);
            direct.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.2f, direct.moods[MoodTypes.energTired]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.3f);
			direct.AddToOpinionValue(TraitTypes.HonestFalse, subject, -0.2f);
        };
        relationSystem.AddAction(new MAction("harass", 0.4f, -0.4f, relationSystem, harass, 4f));

        ActionInvoker prank = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " pulls a prank on " + relationSystem.CapitalizeName(direct.name));
            subject.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.1f, subject.moods[MoodTypes.energTired]);
            subject.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.3f, subject.moods[MoodTypes.hapSad]);
			direct.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.1f, direct.moods[MoodTypes.energTired]);
			direct.moods[MoodTypes.angryFear] += Calculator.UnboundAdd(0.2f, direct.moods[MoodTypes.angryFear]);
            direct.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(-0.5f, direct.moods[MoodTypes.hapSad]);
            direct.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.2f, direct.moods[MoodTypes.energTired]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.3f);
            direct.AddToOpinionValue(TraitTypes.HonestFalse, subject, -0.3f);
        };
        relationSystem.AddAction(new MAction("prank", 0.3f, -0.4f, relationSystem, prank, 5f));

        ActionInvoker playgame = (subject, direct, indPpl, misc) =>
        {
			Possession gameToPlay = new Possession();
			List<Possession> games = ((Possession[])misc).ToList();
			gameToPlay = games.Find(x => x.Name == "game");
			UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " plays a game of "+gameToPlay.objectName+" with " + relationSystem.CapitalizeName(direct.name));
            subject.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.2f, subject.moods[MoodTypes.energTired]);
            subject.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.3f, subject.moods[MoodTypes.hapSad]);
            direct.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.3f, direct.moods[MoodTypes.hapSad]);
            direct.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.2f, direct.moods[MoodTypes.energTired]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, 0.1f);
            direct.AddToOpinionValue(TraitTypes.HonestFalse, subject, 0.1f);
        };
        relationSystem.AddAction(new MAction("playgame", 0.3f, 0.3f, relationSystem, playgame, 10f));

        ActionInvoker order = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " orders " + relationSystem.CapitalizeName(direct.name) + " to do something! How dare they?");

			if(direct.CheckRoleName("bunce") || direct.CheckRoleName("buncess")){
				subject.moods[MoodTypes.arousDisgus] += Calculator.UnboundAdd(0.3f, subject.moods[MoodTypes.arousDisgus]);
				subject.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.3f, subject.moods[MoodTypes.hapSad]);
				direct.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(-0.5f, direct.moods[MoodTypes.hapSad]);
				direct.moods[MoodTypes.angryFear] += Calculator.UnboundAdd(0.5f, direct.moods[MoodTypes.angryFear]);
				direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.5f);
				direct.AddToOpinionValue(TraitTypes.CharitableGreedy, subject, -0.6f);
			}
			else{
				subject.moods[MoodTypes.arousDisgus] += Calculator.UnboundAdd(0.1f, subject.moods[MoodTypes.arousDisgus]);
				subject.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.3f, subject.moods[MoodTypes.hapSad]);
				direct.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(-0.4f, direct.moods[MoodTypes.hapSad]);
				direct.moods[MoodTypes.angryFear] += Calculator.UnboundAdd(0.3f, direct.moods[MoodTypes.angryFear]);
				direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.3f);
				direct.AddToOpinionValue(TraitTypes.CharitableGreedy, subject, -0.3f);
			}
        };
        relationSystem.AddAction(new MAction("order", 0.5f, -0.5f, relationSystem, order, 5f));


		ActionInvoker kill = (subject, direct, indPpl, misc) =>
		{
			UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " KILLS " + relationSystem.CapitalizeName(direct.name) + "!!");
			subject.moods[MoodTypes.arousDisgus] += Calculator.UnboundAdd(-0.5f, subject.moods[MoodTypes.arousDisgus]);
			subject.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(-0.5f, subject.moods[MoodTypes.hapSad]);
			subject.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.5f, subject.moods[MoodTypes.hapSad]);
			roomMan.EnterRoom("Jail",direct);
		};
		relationSystem.AddAction(new MAction("kill", -0.9f, -0.8f, relationSystem, kill, 7f));

        // ------------ CULTURAL (MERCHANT) ACTIONS

        ActionInvoker buyCompany = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " makes a deal to buy " + relationSystem.CapitalizeName(direct.name) + "'s company");
            subject.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.1f, subject.moods[MoodTypes.energTired]);
            beings.Find(x => x.name == subject.name).possessions.Find(y => y.Name == "money").value -= 100f;
            beings.Find(x => x.name == direct.name).possessions.Find(y => y.Name == "money").value += 100f;
            beings.Find(x => x.name == direct.name).possessions.Find(y => y.Name == "company").value -= 1f;
            beings.Find(x => x.name == subject.name).possessions.Find(y => y.Name == "company").value += 1f;
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.4f);
			direct.AddToOpinionValue(TraitTypes.CharitableGreedy, subject, -0.2f);
        };
        relationSystem.AddAction(new MAction("buyCompany", 0.4f, -0.4f, relationSystem, buyCompany, 6f));

        ActionInvoker sellCompany = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " makes a deal to sell a company to " + relationSystem.CapitalizeName(direct.name));
            subject.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.1f, subject.moods[MoodTypes.energTired]);
            beings.Find(x => x.name == subject.name).possessions.Find(y => y.Name == "money").value += 100f;
            beings.Find(x => x.name == direct.name).possessions.Find(y => y.Name == "money").value -= 100f;
            beings.Find(x => x.name == direct.name).possessions.Find(y => y.Name == "company").value += 1f;
            beings.Find(x => x.name == subject.name).possessions.Find(y => y.Name == "company").value -= 1f;
			direct.AddToOpinionValue(TraitTypes.CharitableGreedy, subject, -0.2f);
        };
        relationSystem.AddAction(new MAction("sellCompany", -0.4f, 0.4f, relationSystem, sellCompany, 6f));

        ActionInvoker sabotage = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " sabotages " + relationSystem.CapitalizeName(direct.name));
            direct.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(-0.6f, direct.moods[MoodTypes.hapSad]);
            direct.moods[MoodTypes.angryFear] += Calculator.UnboundAdd(0.4f, direct.moods[MoodTypes.angryFear]);
            subject.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.3f, subject.moods[MoodTypes.energTired]);
			subject.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(0.1f, subject.moods[MoodTypes.hapSad]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.4f);
			direct.AddToOpinionValue(TraitTypes.CharitableGreedy, subject, -0.4f);

			direct.AddToInterPersonalLvlOfInfl(direct,0.4f);
			beings.Find(x => x.name == direct.name).possessions.Find(y => y.Name == "company").value -= 1f;
        };
        relationSystem.AddAction(new MAction("sabotage", 0.5f, -0.5f, relationSystem, sabotage, 10f));

        ActionInvoker advertise = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " advertises their wares to "+relationSystem.CapitalizeName(direct.name));
            subject.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.1f, subject.moods[MoodTypes.energTired]);
            direct.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.1f, direct.moods[MoodTypes.energTired]);
            direct.moods[MoodTypes.angryFear] += Calculator.UnboundAdd(0.2f, direct.moods[MoodTypes.angryFear]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.1f);
			direct.AddToOpinionValue(TraitTypes.CharitableGreedy, subject, -0.1f);
			direct.AddToInterPersonalLvlOfInfl(direct,0.1f);
        };
        relationSystem.AddAction(new MAction("advertise", 0.3f, -0.1f, relationSystem, advertise, 7f));

		ActionInvoker DemandtoLeaveGuild = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " demands " + relationSystem.CapitalizeName(direct.name) + " to leave the merchant's guild!");
            direct.moods[MoodTypes.hapSad] += Calculator.UnboundAdd(-0.5f, direct.moods[MoodTypes.hapSad]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.4f);
			direct.AddToOpinionValue(TraitTypes.CharitableGreedy, subject, -0.4f);
			subject.AddToOpinionValue(TraitTypes.NiceNasty, direct, -0.1f);
			direct.moods[MoodTypes.angryFear] += Calculator.UnboundAdd(0.3f, direct.moods[MoodTypes.angryFear]);

		//	direct.RemoveLink(TypeMask.culture,subject.culture.Find(x => x.droleMask.GetMaskName() == "merchantguild"));
        };
        relationSystem.AddAction(new MAction("DemandtoLeaveGuild", 0.4f, -0.5f, relationSystem, DemandtoLeaveGuild, 4f));

		ActionInvoker buyGoods = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " buys goods from " + relationSystem.CapitalizeName(direct.name));
            beings.Find(x => x.name == relationSystem.CapitalizeName(subject.name)).possessions.Find(y => y.Name == "money").value -= 30f;
            beings.Find(x => x.name == relationSystem.CapitalizeName(subject.name)).possessions.Find(y => y.Name == "goods").value += 1f;
            subject.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.1f, subject.moods[MoodTypes.energTired]);
			subject.AddToOpinionValue(TraitTypes.CharitableGreedy,direct,0.1f);
			direct.AddToOpinionValue(TraitTypes.CharitableGreedy,subject,0.1f);
        };
        relationSystem.AddAction(new MAction("buyGoods", 0.4f, 0.2f, relationSystem, buyGoods, 3f));

        ActionInvoker sellGoods = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " sells goods to " + relationSystem.CapitalizeName(direct.name));
            beings.Find(x => x.name == subject.name).possessions.Find(y => y.Name == "money").value += 30f;
            beings.Find(x => x.name == subject.name).possessions.Find(y => y.Name == "goods").value -= 1f;
            subject.moods[MoodTypes.energTired] += Calculator.UnboundAdd(-0.1f, subject.moods[MoodTypes.energTired]);
			subject.AddToOpinionValue(TraitTypes.CharitableGreedy,direct,0.1f);
			direct.AddToOpinionValue(TraitTypes.CharitableGreedy,subject,0.1f);
        };
        relationSystem.AddAction(new MAction("sellGoods", 0.7f, 0.2f, relationSystem, sellGoods, 3f));


        // ROOM ACTIONS

        ActionInvoker moveToLivingRoom = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " goes into the Living Room.");
			subject.moods[MoodTypes.energTired] += Calculator.UnboundAdd(0.2f, subject.moods[MoodTypes.energTired]);
            roomMan.EnterRoom("Stue", relationSystem.pplAndMasks.GetPerson(subject.name));
        };
		relationSystem.AddAction(new MAction("moveToLivingRoom", 0.4f, 0.0f, relationSystem, moveToLivingRoom, 5f, _needsDirect: false));

        ActionInvoker moveToKitchen = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " goes into the Kitchen.");
			subject.moods[MoodTypes.energTired] += Calculator.UnboundAdd(0.2f, subject.moods[MoodTypes.energTired]);
            roomMan.EnterRoom("Køkken",  relationSystem.pplAndMasks.GetPerson(subject.name));
        };
		relationSystem.AddAction(new MAction("moveToKitchen", 0.2f, 0.0f, relationSystem, moveToKitchen, 5f, _needsDirect: false));

        ActionInvoker moveToEntryHall = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(relationSystem.CapitalizeName(subject.name) + " goes into the Entry Hallway.");
			subject.moods[MoodTypes.energTired] += Calculator.UnboundAdd(0.2f, subject.moods[MoodTypes.energTired]);
            roomMan.EnterRoom("Indgang", relationSystem.pplAndMasks.GetPerson(subject.name));
        };
		relationSystem.AddAction(new MAction("moveToEntryHall", 0.1f, 0.0f, relationSystem, moveToEntryHall, 5f, _needsDirect:false));


    }
}
