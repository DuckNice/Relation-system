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
            UIFunctions.WriteGameLine(subject.name + " is fleeing the scene!");
        };

        relationSystem.AddAction(new MAction("flee", 1.0f, -0.5f, relationSystem, flee, 10f));

        ActionInvoker doNothing = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is doing nothing. What a bore.");
            subject.moods[MoodTypes.energTired] += Calculator.unboundAdd(0.2f, subject.moods[MoodTypes.energTired]);
        };

        relationSystem.AddAction(new MAction("doNothing", -1.0f, -1.0f, relationSystem, doNothing, 5f));

        // ---------- INTERPERSONAL ACTIONS
        ActionInvoker greet = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is greeting " + direct.name);
        };

        relationSystem.AddAction(new MAction("Greet", 0.1f, 0.1f, relationSystem, greet, 2f));

        ActionInvoker kiss = (subject, direct, indPpl, misc) =>
        {
            if (subject.interPersonal.Exists(x => x.roleRef.Exists(y => y.name == direct.name && y.interPersonal.Exists(z => z.roleName == "partner" && z.roleRef.Exists(s => s.name == subject.name))) && x.roleName == "partner"))
            {
                UIFunctions.WriteGameLine(subject.name + " is kissing " + direct.name);
            }
            else
            {
                UIFunctions.WriteGameLine(subject.name + " is kissing " + direct.name + " outside a relationship!");
            }

            direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.5f, direct.moods[MoodTypes.hapSad]);
            direct.moods[MoodTypes.arousDisgus] += Calculator.unboundAdd(0.5f, direct.moods[MoodTypes.arousDisgus]);
            subject.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.5f, subject.moods[MoodTypes.hapSad]);
            subject.moods[MoodTypes.arousDisgus] += Calculator.unboundAdd(0.5f, subject.moods[MoodTypes.arousDisgus]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, 0.2f);
        };
        relationSystem.AddAction(new MAction("kiss", 0.3f, 0.3f, relationSystem, kiss, 5f));

        ActionInvoker askAboutPartnerStatus = (subject, direct, indPpl, misc) =>
        {
            if (subject.interPersonal.Exists(x => x.roleRef.Exists(y => y.name == direct.name && y.interPersonal.Exists(z => z.roleName == "partner" && z.roleRef.Exists(s => s.name == subject.name))) && x.roleName == "partner"))
            {
                UIFunctions.WriteGameLine(subject.name + " is asking if " + direct.name + " still wants to be their partner after what they've done.");
            }
            else
            {
                UIFunctions.WriteGameLine(subject.name + " is asking if " + direct.name + " wants to be their partner. Is it love?");
            }

            direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.6f, direct.moods[MoodTypes.hapSad]);
            direct.moods[MoodTypes.arousDisgus] += Calculator.unboundAdd(0.6f, direct.moods[MoodTypes.arousDisgus]);
            subject.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.6f, subject.moods[MoodTypes.hapSad]);
            subject.moods[MoodTypes.arousDisgus] += Calculator.unboundAdd(0.6f, subject.moods[MoodTypes.arousDisgus]);

            if (indPpl != null)
            {
                foreach (Person p in indPpl)
                {
                    p.moods[MoodTypes.hapSad] += Calculator.unboundAdd(-0.4f, p.moods[MoodTypes.hapSad]);
                    p.moods[MoodTypes.arousDisgus] += Calculator.unboundAdd(-0.4f, p.moods[MoodTypes.arousDisgus]);
                }
            }

        };
        relationSystem.AddAction(new MAction("askAboutPartnerStatus", 1.0f, 0.5f, relationSystem, askAboutPartnerStatus, 5f));

        ActionInvoker chooseAnotherAsPartner = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " has chosen " + direct.name + " as their partner! How romantic.");
            direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.6f, direct.moods[MoodTypes.hapSad]);
            direct.moods[MoodTypes.arousDisgus] += Calculator.unboundAdd(0.6f, direct.moods[MoodTypes.arousDisgus]);
            subject.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.6f, subject.moods[MoodTypes.hapSad]);
            subject.moods[MoodTypes.arousDisgus] += Calculator.unboundAdd(0.6f, subject.moods[MoodTypes.arousDisgus]);

            relationSystem.AddLinkToPerson(subject.name, new string[] { direct.name }, TypeMask.interPers, "partner", "" + subject.name + direct.name + "new", subject.GetOpinionValue(TraitTypes.NiceNasty, direct));
            relationSystem.AddLinkToPerson(direct.name, new string[] { subject.name }, TypeMask.interPers, "partner", "" + direct.name + subject.name + "new", direct.GetOpinionValue(TraitTypes.NiceNasty, subject));

            if (indPpl != null)
            {
                foreach (Person p in indPpl)
                {
                    p.moods[MoodTypes.hapSad] += Calculator.unboundAdd(-0.4f, p.moods[MoodTypes.hapSad]);
                    p.moods[MoodTypes.arousDisgus] += Calculator.unboundAdd(-0.4f, p.moods[MoodTypes.arousDisgus]);
                }
            }

        };
        relationSystem.AddAction(new MAction("chooseAnotherAsPartner", 0.4f, 0.6f, relationSystem, chooseAnotherAsPartner, 5f));

        ActionInvoker stayAsPartner = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is staying with " + direct.name + ". Nothing separates these two.");
            direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.3f, direct.moods[MoodTypes.hapSad]);
            subject.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.1f, subject.moods[MoodTypes.hapSad]);
        };
        relationSystem.AddAction(new MAction("stayAsPartner", 0.3f, 0.3f, relationSystem, stayAsPartner, 4f));

        ActionInvoker LeavePartner = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is leaving " + direct.name + "!");
            direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(-0.7f, direct.moods[MoodTypes.hapSad]);
            subject.moods[MoodTypes.hapSad] += Calculator.unboundAdd(-0.2f, subject.moods[MoodTypes.hapSad]);

            subject.RemoveLink(TypeMask.interPers, subject.interPersonal.Find(x => x.roleName == "partner" && x.roleRef.Exists(y => y.name == direct.name)));
            direct.RemoveLink(TypeMask.interPers, direct.interPersonal.Find(x => x.roleName == "partner" && x.roleRef.Exists(y => y.name == subject.name)));
        };
        relationSystem.AddAction(new MAction("LeavePartner", 0.3f, -0.7f, relationSystem, LeavePartner, 5f));

        ActionInvoker flirt = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is flirting with " + direct.name + ".");
            direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.2f, direct.moods[MoodTypes.hapSad]);
            direct.moods[MoodTypes.arousDisgus] += Calculator.unboundAdd(0.2f, direct.moods[MoodTypes.arousDisgus]);
            subject.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.2f, subject.moods[MoodTypes.hapSad]);
            subject.moods[MoodTypes.arousDisgus] += Calculator.unboundAdd(0.2f, subject.moods[MoodTypes.arousDisgus]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, 0.1f);

            foreach (Link l in subject.interPersonal)
            {
                if (l.roleRef.Exists(x => x.name == direct.name))
                {
                    l.AddToLvlOfInfl(0.1f);
                }
            }
        };
        relationSystem.AddAction(new MAction("flirt", 0.1f, 0.1f, relationSystem, flirt, 4f));

        ActionInvoker chat = (subject, direct, indPpl, misc) =>
        {
            debug.Write(subject.name + "   " + direct.name);
            UIFunctions.WriteGameLine(subject.name + " is chatting with " + direct.name + ".");
            direct.moods[MoodTypes.energTired] += Calculator.unboundAdd(-0.2f, direct.moods[MoodTypes.energTired]);
            subject.moods[MoodTypes.energTired] += Calculator.unboundAdd(-0.2f, subject.moods[MoodTypes.energTired]);

            foreach (Link l in subject.interPersonal)
            {
                if (l.roleRef.Exists(x => x.name == direct.name))
                {
                    l.AddToLvlOfInfl(0.1f);
                }
            }
        };
        relationSystem.AddAction(new MAction("chat", 0.0f, 0.0f, relationSystem, chat, 10f));

        ActionInvoker giveGift = (subject, direct, indPpl, misc) =>
        {
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, 0.1f);
            if (misc != null)
            {
                UIFunctions.WriteGameLine(subject.name + " is giving the gift of " + misc + " to " + direct.name + ".");
            }
            else
            {
                UIFunctions.WriteGameLine(subject.name + " is giving a gift to " + direct.name + ".");
            }
            direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.5f, direct.moods[MoodTypes.hapSad]);

            beings.Find(x => x.name == subject.name).possessions.Find(y => y.Name == "money").value -= 10f;
            beings.Find(x => x.name == direct.name).possessions.Find(y => y.Name == "money").value += 10f;

        };
        relationSystem.AddAction(new MAction("giveGift", 0.2f, 0.4f, relationSystem, giveGift, 5f));

        ActionInvoker poison = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is poisoning " + direct.name + "! Oh no!");
            direct.moods[MoodTypes.angryFear] += Calculator.unboundAdd(-0.8f, direct.moods[MoodTypes.angryFear]);
            direct.moods[MoodTypes.energTired] += Calculator.unboundAdd(-0.5f, direct.moods[MoodTypes.energTired]);
            subject.moods[MoodTypes.energTired] += Calculator.unboundAdd(-0.2f, subject.moods[MoodTypes.energTired]);

            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.5f);


            foreach (Link l in subject.interPersonal)
            {
                if (l.roleRef.Exists(x => x.name == direct.name))
                {
                    l.AddToLvlOfInfl(-0.4f);
                }
            }
        };
        relationSystem.AddAction(new MAction("poison", 0.1f, -0.9f, relationSystem, poison, 10f));

        ActionInvoker gossip = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is gossiping with " + direct.name);
            direct.moods[MoodTypes.energTired] += Calculator.unboundAdd(-0.2f, direct.moods[MoodTypes.energTired]);
            subject.moods[MoodTypes.energTired] += Calculator.unboundAdd(-0.2f, subject.moods[MoodTypes.energTired]);
        };
        relationSystem.AddAction(new MAction("gossip", 0.1f, 0.1f, relationSystem, gossip, 7f));

        ActionInvoker argue = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is arguing with " + direct.name + "!");
            direct.moods[MoodTypes.angryFear] += Calculator.unboundAdd(0.3f, direct.moods[MoodTypes.angryFear]);
            subject.moods[MoodTypes.angryFear] += Calculator.unboundAdd(0.3f, subject.moods[MoodTypes.angryFear]);
            direct.moods[MoodTypes.energTired] += Calculator.unboundAdd(0.3f, direct.moods[MoodTypes.energTired]);
            subject.moods[MoodTypes.energTired] += Calculator.unboundAdd(0.3f, subject.moods[MoodTypes.energTired]);
            direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(-0.1f, direct.moods[MoodTypes.hapSad]);
            subject.moods[MoodTypes.hapSad] += Calculator.unboundAdd(-0.1f, subject.moods[MoodTypes.hapSad]);

            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.2f);

            foreach (Link l in subject.interPersonal)
            {
                if (l.roleRef.Exists(x => x.name == direct.name))
                {
                    l.AddToLvlOfInfl(0.3f);
                }
            }
        };
        relationSystem.AddAction(new MAction("argue", -0.2f, -0.4f, relationSystem, argue, 8f));

        /*		ActionInvoker demandToStopBeingFriendWith = (subject, direct, indPpl, misc) =>
                {
                    UIFunctions.WriteGameLine(subject.name + " is demanding that " + direct.name+" stops being friends with ");
                    //I HAVE NO WAY OF TESTING IF THIS WORKS
                    foreach(Person p in indPpl){
                        UIFunctions.WriteGameLine(p.name+" ");
                        foreach(Link l in p.interPersonal){
                            if(l.roleRef.Contains(direct)){ direct.interPersonal.Remove(l); }
                        }
                    }
                };
                relationSystem.AddAction(new MAction("demandToStopBeingFriendWith", 0.3f, -0.5f, relationSystem, demandToStopBeingFriendWith,4f));
        */
        ActionInvoker makeDistraction = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is making a distraction for " + direct.name + "!");
            int rand = UnityEngine.Random.Range(0, 2); //SHOULD PROBABLY BASE THIS ON ABILITY
            if (rand == 0)
            {
                UIFunctions.WriteGameLine("It was a success! They are very distracted");
                direct.moods[MoodTypes.energTired] += Calculator.unboundAdd(-0.5f, direct.moods[MoodTypes.energTired]);
                subject.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.3f, subject.moods[MoodTypes.hapSad]);
                foreach (Link l in direct.interPersonal)
                {
                    if (l.roleRef.Exists(x => x.name == subject.name))
                    {
                        l.AddToLvlOfInfl(0.4f);
                    }
                }
            }
            else
            {
                UIFunctions.WriteGameLine("It failed. They are now more wary than before.");
                direct.moods[MoodTypes.energTired] += Calculator.unboundAdd(0.3f, direct.moods[MoodTypes.energTired]);
                direct.moods[MoodTypes.angryFear] += Calculator.unboundAdd(-0.3f, direct.moods[MoodTypes.angryFear]);
                subject.moods[MoodTypes.hapSad] += Calculator.unboundAdd(-0.4f, subject.moods[MoodTypes.hapSad]);
                foreach (Link l in direct.interPersonal)
                {
                    if (l.roleRef.Exists(x => x.name == subject.name))
                    {
                        l.AddToLvlOfInfl(-0.6f);
                    }
                }
            }
        };
        relationSystem.AddAction(new MAction("makeDistraction", 0.2f, -0.5f, relationSystem, makeDistraction, 6f));

        ActionInvoker reminisce = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is reminiscing about old times with " + direct.name + "!");
            direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.2f, direct.moods[MoodTypes.hapSad]);
            subject.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.2f, subject.moods[MoodTypes.hapSad]);
            subject.moods[MoodTypes.energTired] += Calculator.unboundAdd(0.1f, subject.moods[MoodTypes.energTired]);
            direct.moods[MoodTypes.energTired] += Calculator.unboundAdd(0.1f, direct.moods[MoodTypes.energTired]);
        };
        relationSystem.AddAction(new MAction("reminisce", 0.1f, 0.1f, relationSystem, reminisce, 9f));

        ActionInvoker deny = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is denying " + direct.name + " their wishes.");
            direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(-0.4f, direct.moods[MoodTypes.hapSad]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.2f);
            foreach (Link l in subject.interPersonal)
            {
                if (l.roleRef.Exists(x => x.name == direct.name))
                {
                    l.AddToLvlOfInfl(0.2f);
                }
            }
        };
        relationSystem.AddAction(new MAction("deny", 0.4f, -0.4f, relationSystem, deny, 2f));

        ActionInvoker enthuseAboutGreatnessofPerson = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is saying how great a person " + direct.name + " is!");
            direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.4f, direct.moods[MoodTypes.hapSad]);
            subject.moods[MoodTypes.energTired] += Calculator.unboundAdd(-0.1f, subject.moods[MoodTypes.energTired]);
            direct.moods[MoodTypes.energTired] += Calculator.unboundAdd(0.1f, direct.moods[MoodTypes.energTired]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, 0.1f);

            subject.GetRule("enthuseaboutgreatnessofperson").SetRuleStrength(-1.0f);
            foreach (Link l in direct.interPersonal)
            {
                if (l.roleRef.Exists(x => x.name == subject.name))
                {
                    l.AddToLvlOfInfl(0.1f);
                }
            }

        };
        relationSystem.AddAction(new MAction("enthuseAboutGreatnessofPerson", 0.4f, 0.2f, relationSystem, enthuseAboutGreatnessofPerson, 4f));

        // --------------- CULTURAL ACTIONS

        ActionInvoker convict = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is convicting " + direct.name + " of commiting a crime. To Jail with him!");
            direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(-0.8f, direct.moods[MoodTypes.hapSad]);
            direct.moods[MoodTypes.angryFear] += Calculator.unboundAdd(0.6f, direct.moods[MoodTypes.angryFear]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.4f);
            foreach (Link l in direct.interPersonal)
            {
                if (l.roleRef.Exists(x => x.name == subject.name))
                {
                    l.AddToLvlOfInfl(0.3f);
                }
            }
        };
        relationSystem.AddAction(new MAction("convict", 1.0f, -0.5f, relationSystem, convict, 6f));

        ActionInvoker fight = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is attempting to fight " + direct.name);
            direct.moods[MoodTypes.angryFear] += Calculator.unboundAdd(0.6f, direct.moods[MoodTypes.angryFear]);
            direct.moods[MoodTypes.energTired] += Calculator.unboundAdd(0.6f, direct.moods[MoodTypes.energTired]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.2f);
            subject.AddToOpinionValue(TraitTypes.NiceNasty, direct, -0.2f);
            foreach (Link l in direct.interPersonal)
            {
                if (l.roleRef.Exists(x => x.name == subject.name))
                {
                    l.AddToLvlOfInfl(0.4f);
                }
            }
        };
        relationSystem.AddAction(new MAction("fight", 0.6f, -0.4f, relationSystem, fight, 12f));

        ActionInvoker bribe = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is attempting to bribe " + direct.name);
            direct.moods[MoodTypes.angryFear] += Calculator.unboundAdd(-0.4f, direct.moods[MoodTypes.angryFear]);
            direct.AddToOpinionValue(TraitTypes.HonestFalse, subject, -0.3f);

            beings.Find(x => x.name == subject.name).possessions.Find(y => y.Name == "money").value -= 30f;
            beings.Find(x => x.name == direct.name).possessions.Find(y => y.Name == "money").value += 30f;
            foreach (Link l in direct.interPersonal)
            {
                if (l.roleRef.Exists(x => x.name == subject.name))
                {
                    l.AddToLvlOfInfl(-0.1f);
                }
            }
        };
        relationSystem.AddAction(new MAction("bribe", 0.2f, 0.6f, relationSystem, bribe, 5f));

        ActionInvoker argueInnocence = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is arguing " + direct.name + "'s innocence.");
            direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.3f, direct.moods[MoodTypes.hapSad]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, 0.2f);
        };
        relationSystem.AddAction(new MAction("argueInnocence", 0.0f, 0.3f, relationSystem, argueInnocence, 7f));

        ActionInvoker argueGuiltiness = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is arguing " + direct.name + "'s guilt!");
            direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(-0.3f, direct.moods[MoodTypes.hapSad]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.2f);
        };
        relationSystem.AddAction(new MAction("argueGuiltiness", 0.0f, -0.3f, relationSystem, argueGuiltiness, 7f));

        ActionInvoker steal = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is stealing from " + direct.name + ". Will they get caught?");
            subject.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.6f, subject.moods[MoodTypes.hapSad]);
            subject.moods[MoodTypes.angryFear] += Calculator.unboundAdd(-0.3f, subject.moods[MoodTypes.angryFear]);
            subject.moods[MoodTypes.energTired] += Calculator.unboundAdd(-0.3f, subject.moods[MoodTypes.energTired]);

            beings.Find(x => x.name == subject.name).possessions.Find(y => y.Name == "money").value += 50f;
            beings.Find(x => x.name == direct.name).possessions.Find(y => y.Name == "money").value -= 50f;
            foreach (Link l in direct.interPersonal)
            {
                if (l.roleRef.Exists(x => x.name == subject.name))
                {
                    l.AddToLvlOfInfl(-0.1f);
                }
            }
        };
        relationSystem.AddAction(new MAction("steal", 0.7f, -0.5f, relationSystem, steal, 10f));

        ActionInvoker practiceStealing = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is practicing the arts of stealth. What are they intending!");
            subject.AddToAbility(Calculator.unboundAdd(0.2f, subject.GetAbilityy()));
            subject.moods[MoodTypes.energTired] += Calculator.unboundAdd(-0.2f, subject.moods[MoodTypes.energTired]);
        };
        relationSystem.AddAction(new MAction("practiceStealing", 0.2f, 0.0f, relationSystem, practiceStealing, 6f));

        ActionInvoker askForHelpInIllicitActivity = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is asking " + direct.name + " for help in something... dangerous.");
        };
        relationSystem.AddAction(new MAction("askForHelpInIllicitActivity", 0.4f, 0.1f, relationSystem, askForHelpInIllicitActivity, 8f));

        /*ActionInvoker searchForThief = (subject, direct, indPpl, misc) => 
        {
            UIFunctions.WriteGameLine(subject.name + " is searching for the thief!");
            subject.moods[MoodTypes.energTired] += Calculator.unboundAdd(-0.3f,subject.moods[MoodTypes.energTired]);
        };
        relationSystem.AddAction(new MAction("searchForThief", 0.6f,-0.5f, relationSystem, searchForThief,10f));*/

        ActionInvoker makefunof = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is making fun of " + direct.name);

            subject.moods[MoodTypes.energTired] += Calculator.unboundAdd(-0.2f, subject.moods[MoodTypes.energTired]);
            subject.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.2f, subject.moods[MoodTypes.hapSad]);
            direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(-0.4f, direct.moods[MoodTypes.hapSad]);
            direct.moods[MoodTypes.angryFear] += Calculator.unboundAdd(0.2f, direct.moods[MoodTypes.angryFear]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.2f);
            direct.AddToOpinionValue(TraitTypes.HonestFalse, subject, -0.1f);
        };
        relationSystem.AddAction(new MAction("makefunof", 0.4f, -0.6f, relationSystem, makefunof, 4f));

        ActionInvoker telljoke = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is telling a joke to " + direct.name + ". It's funny!");

            subject.moods[MoodTypes.energTired] += Calculator.unboundAdd(-0.2f, subject.moods[MoodTypes.energTired]);
            subject.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.2f, subject.moods[MoodTypes.hapSad]);
            direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.1f, direct.moods[MoodTypes.hapSad]);
            direct.moods[MoodTypes.energTired] += Calculator.unboundAdd(-0.2f, direct.moods[MoodTypes.energTired]);
        };
        relationSystem.AddAction(new MAction("telljoke", 0.1f, 0.1f, relationSystem, telljoke, 5f));

        ActionInvoker harass = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is harassing " + direct.name + ". Ugh, how annoying.");

            subject.moods[MoodTypes.energTired] += Calculator.unboundAdd(-0.3f, subject.moods[MoodTypes.energTired]);
            subject.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.2f, subject.moods[MoodTypes.hapSad]);
            subject.moods[MoodTypes.arousDisgus] += Calculator.unboundAdd(0.2f, subject.moods[MoodTypes.arousDisgus]);
            direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(-0.4f, direct.moods[MoodTypes.hapSad]);
            direct.moods[MoodTypes.energTired] += Calculator.unboundAdd(-0.2f, direct.moods[MoodTypes.energTired]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.3f);
        };
        relationSystem.AddAction(new MAction("harass", 0.4f, -0.4f, relationSystem, harass, 4f));

        ActionInvoker prank = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is pulling a prank on " + direct.name);

            subject.moods[MoodTypes.energTired] += Calculator.unboundAdd(-0.1f, subject.moods[MoodTypes.energTired]);
            subject.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.3f, subject.moods[MoodTypes.hapSad]);
            subject.moods[MoodTypes.arousDisgus] += Calculator.unboundAdd(0.2f, subject.moods[MoodTypes.arousDisgus]);
            direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(-0.5f, direct.moods[MoodTypes.hapSad]);
            direct.moods[MoodTypes.energTired] += Calculator.unboundAdd(-0.3f, direct.moods[MoodTypes.energTired]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.3f);
            direct.AddToOpinionValue(TraitTypes.HonestFalse, subject, -0.3f);
        };
        relationSystem.AddAction(new MAction("prank", 0.3f, -0.4f, relationSystem, prank, 5f));

        ActionInvoker playgame = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is playing a game with " + direct.name);

            subject.moods[MoodTypes.energTired] += Calculator.unboundAdd(-0.3f, subject.moods[MoodTypes.energTired]);
            subject.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.3f, subject.moods[MoodTypes.hapSad]);
            direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.3f, direct.moods[MoodTypes.hapSad]);
            direct.moods[MoodTypes.energTired] += Calculator.unboundAdd(-0.3f, direct.moods[MoodTypes.energTired]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, 0.1f);
            direct.AddToOpinionValue(TraitTypes.HonestFalse, subject, 0.1f);
        };
        relationSystem.AddAction(new MAction("playgame", 0.3f, 0.3f, relationSystem, playgame, 10f));

        ActionInvoker order = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is ordering " + direct.name + " to do something! How dare they?");

            subject.moods[MoodTypes.arousDisgus] += Calculator.unboundAdd(0.2f, subject.moods[MoodTypes.arousDisgus]);
            subject.moods[MoodTypes.hapSad] += Calculator.unboundAdd(0.3f, subject.moods[MoodTypes.hapSad]);
            direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(-0.5f, direct.moods[MoodTypes.hapSad]);
            direct.moods[MoodTypes.angryFear] += Calculator.unboundAdd(0.4f, direct.moods[MoodTypes.angryFear]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.3f);
        };
        relationSystem.AddAction(new MAction("order", 0.5f, -0.5f, relationSystem, order, 10f));




        // ----------- CULTURAL (CULT) ACTIONS
        /*
                ActionInvoker praiseCult = (subject, direct, indPpl, misc) => 
                {
                    UIFunctions.WriteGameLine(subject.name + " is saying how great this cult is to "+direct.name);

                    if(direct.culture.Exists(x=>x.roleMask.GetMaskName() == "cult" && (x.roleName == "follower" || x.roleName == "leader"))){
                        direct.culture.Find(x=>x.roleMask.GetMaskName() == "cult").AddToLvlOfInfl(0.1f);
                    }
                };
                relationSystem.AddAction(new MAction("praiseCult", 0.1f,-0.1f, relationSystem, praiseCult,4f));

                ActionInvoker enterCult = (subject, direct, indPpl, misc) => 
                {
                    UIFunctions.WriteGameLine(subject.name + " is entering the cult.");
                    foreach(Link l in subject.interPersonal){
                        if(l.roleRef.Exists(x=>x.name == direct.name)){
                            l.AddToLvlOfInfl(0.4f);
                        }
                    }
                };
                relationSystem.AddAction(new MAction("enterCult", 0.0f,0.0f, relationSystem, enterCult,10f));

                ActionInvoker exitCult = (subject, direct, indPpl, misc) => 
                {
                    UIFunctions.WriteGameLine(subject.name + " is exiting the cult!");
                    foreach(Link l in subject.interPersonal){
                        if(l.roleRef.Exists(x=>x.name == direct.name)){
                            l.AddToLvlOfInfl(-0.4f);
                        }
                    }
                };
                relationSystem.AddAction(new MAction("exitCult", 0.0f,0.0f, relationSystem, exitCult,5f));

                ActionInvoker damnCult = (subject, direct, indPpl, misc) => 
                {
                    UIFunctions.WriteGameLine(subject.name + " is damning the cult!");
                    foreach(Link l in subject.interPersonal){
                        if(l.roleRef.Exists(x=>x.name == direct.name)){
                            l.AddToLvlOfInfl(-0.4f);
                        }
                    }
                };
                relationSystem.AddAction(new MAction("damnCult", -0.2f,0.0f, relationSystem, damnCult,4f));

                ActionInvoker excommunicateFromCult = (subject, direct, indPpl, misc) => 
                {
                    UIFunctions.WriteGameLine(subject.name + " is excommunicating "+direct.name+" from the cult");
                    foreach(Link l in subject.interPersonal){
                        if(l.roleRef.Exists(x=>x.name == direct.name)){
                            l.AddToLvlOfInfl(-0.9f);
                        }
                    }
                };
                relationSystem.AddAction(new MAction("excommunicateFromCult", 0.0f,-0.6f, relationSystem, excommunicateFromCult,6f));
        */


        // ------------ CULTURAL (MERCHANT) ACTIONS

        ActionInvoker buyCompany = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is making a deal to buy " + direct.name + "'s company");
            subject.moods[MoodTypes.energTired] += Calculator.unboundAdd(-0.1f, subject.moods[MoodTypes.energTired]);
            beings.Find(x => x.name == subject.name).possessions.Find(y => y.Name == "money").value -= 100f;
            beings.Find(x => x.name == direct.name).possessions.Find(y => y.Name == "money").value += 100f;
            beings.Find(x => x.name == direct.name).possessions.Find(y => y.Name == "company").value -= 1f;
            beings.Find(x => x.name == subject.name).possessions.Find(y => y.Name == "company").value += 1f;
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.4f);
        };
        relationSystem.AddAction(new MAction("buyCompany", 0.4f, -0.4f, relationSystem, buyCompany, 6f));

        ActionInvoker sellCompany = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is making a deal to sell a company to " + direct.name);
            subject.moods[MoodTypes.energTired] += Calculator.unboundAdd(-0.1f, subject.moods[MoodTypes.energTired]);
            beings.Find(x => x.name == subject.name).possessions.Find(y => y.Name == "money").value += 100f;
            beings.Find(x => x.name == direct.name).possessions.Find(y => y.Name == "money").value -= 100f;
            beings.Find(x => x.name == direct.name).possessions.Find(y => y.Name == "company").value += 1f;
            beings.Find(x => x.name == subject.name).possessions.Find(y => y.Name == "company").value -= 1f;
        };
        relationSystem.AddAction(new MAction("sellCompany", -0.4f, 0.4f, relationSystem, sellCompany, 6f));

        ActionInvoker sabotage = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is sabotaging " + direct.name);
            direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(-0.6f, direct.moods[MoodTypes.hapSad]);
            direct.moods[MoodTypes.angryFear] += Calculator.unboundAdd(0.4f, direct.moods[MoodTypes.angryFear]);
            subject.moods[MoodTypes.energTired] += Calculator.unboundAdd(-0.3f, subject.moods[MoodTypes.energTired]);

            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.4f);
            beings.Find(x => x.name == direct.name).possessions.Find(y => y.Name == "company").value -= 1f;
            foreach (Link l in subject.interPersonal)
            {
                if (l.roleRef.Exists(x => x.name == direct.name))
                {
                    l.AddToLvlOfInfl(0.4f);
                }
            }
        };
        relationSystem.AddAction(new MAction("sabotage", 0.5f, -0.5f, relationSystem, sabotage, 10f));

        ActionInvoker advertise = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is advertising for their wares!");
            subject.moods[MoodTypes.energTired] += Calculator.unboundAdd(-0.1f, subject.moods[MoodTypes.energTired]);
            direct.moods[MoodTypes.energTired] += Calculator.unboundAdd(-0.1f, direct.moods[MoodTypes.energTired]);
            direct.moods[MoodTypes.angryFear] += Calculator.unboundAdd(0.2f, direct.moods[MoodTypes.angryFear]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.1f);
        };
        relationSystem.AddAction(new MAction("advertise", 0.3f, -0.1f, relationSystem, advertise, 7f));

        ActionInvoker convinceToLeaveGuild = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is convincing " + direct.name + " to leave the merchant's guild!");
            direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(-0.2f, direct.moods[MoodTypes.hapSad]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.2f);
            foreach (Link l in subject.interPersonal)
            {
                if (l.roleRef.Exists(x => x.name == direct.name))
                {
                    l.AddToLvlOfInfl(-0.6f);
                }
            }
        };
        relationSystem.AddAction(new MAction("convinceToLeaveGuild", 0.4f, -0.3f, relationSystem, convinceToLeaveGuild, 5f));

        ActionInvoker DemandtoLeaveGuild = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is demanding " + direct.name + " to leave the merchant's guild!");
            direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(-0.5f, direct.moods[MoodTypes.hapSad]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, -0.4f);
            foreach (Link l in subject.interPersonal)
            {
                if (l.roleRef.Exists(x => x.name == direct.name))
                {
                    l.AddToLvlOfInfl(-0.9f);
                }
            }
        };
        relationSystem.AddAction(new MAction("DemandtoLeaveGuild", 0.4f, -0.5f, relationSystem, DemandtoLeaveGuild, 4f));

        ActionInvoker askForHelp = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is asking " + direct.name + " for help");
            direct.moods[MoodTypes.hapSad] += Calculator.unboundAdd(-0.6f, direct.moods[MoodTypes.hapSad]);
            direct.AddToOpinionValue(TraitTypes.NiceNasty, subject, 0.1f);
            direct.AddToOpinionValue(TraitTypes.ShyBolsterous, subject, -0.4f);
            foreach (Link l in subject.interPersonal)
            {
                if (l.roleRef.Exists(x => x.name == direct.name))
                {
                    l.AddToLvlOfInfl(0.2f);
                }
            }
        };
        relationSystem.AddAction(new MAction("askForHelp", 0.5f, 0.2f, relationSystem, askForHelp, 4f));

        ActionInvoker buyGoods = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is buying goods from " + direct.name);
            beings.Find(x => x.name == subject.name).possessions.Find(y => y.Name == "money").value -= 30f;
            beings.Find(x => x.name == subject.name).possessions.Find(y => y.Name == "goods").value += 1f;
            subject.moods[MoodTypes.energTired] += Calculator.unboundAdd(-0.1f, subject.moods[MoodTypes.energTired]);
        };
        relationSystem.AddAction(new MAction("buyGoods", 0.4f, 0.2f, relationSystem, buyGoods, 3f));

        ActionInvoker sellGoods = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is selling goods to " + direct.name);
            beings.Find(x => x.name == subject.name).possessions.Find(y => y.Name == "money").value += 30f;
            beings.Find(x => x.name == subject.name).possessions.Find(y => y.Name == "goods").value -= 1f;
            subject.moods[MoodTypes.energTired] += Calculator.unboundAdd(-0.1f, subject.moods[MoodTypes.energTired]);
        };
        relationSystem.AddAction(new MAction("sellGoods", 0.7f, 0.2f, relationSystem, sellGoods, 3f));



        // ROOM ACTIONS

        ActionInvoker moveToStue = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is going into the Living Room.");
            roomMan.EnterRoom("Stue", relationSystem.pplAndMasks.GetPerson(name));
        };
        relationSystem.AddAction(new MAction("moveToStue", 0.4f, 0.0f, relationSystem, moveToStue, 5f));

        ActionInvoker moveToKøkken = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is going into the Kitchen.");
            roomMan.EnterRoom("Køkken", relationSystem.pplAndMasks.GetPerson(name));
        };
        relationSystem.AddAction(new MAction("moveToKøkken", 0.4f, 0.0f, relationSystem, moveToKøkken, 5f));

        ActionInvoker moveToIndgang = (subject, direct, indPpl, misc) =>
        {
            UIFunctions.WriteGameLine(subject.name + " is going into the Entry Hallway.");
            roomMan.EnterRoom("Indgang", relationSystem.pplAndMasks.GetPerson(name));
        };
        relationSystem.AddAction(new MAction("moveToIndgang", 0.4f, 0.0f, relationSystem, moveToIndgang, 5f));


    }
}