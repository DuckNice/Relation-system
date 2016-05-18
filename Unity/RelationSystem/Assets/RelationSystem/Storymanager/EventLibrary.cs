using UnityEngine;
using System.Collections.Generic;

public class EventLibrary {

    public static Dictionary<string, float> ActionDramas = new Dictionary<string, float>();

	public EventLibrary()
    {
        ActionDramas.Add("flee", 0.1f);
        ActionDramas.Add("doNothing", 0.001f);
        ActionDramas.Add("greet", 0.01f);
        ActionDramas.Add("kiss", 0.05f);
        ActionDramas.Add("askIfShouldBePartner", 0.2f);
        ActionDramas.Add("chooseAnotherAsPartner", 0.3f);
        ActionDramas.Add("stayAsPartner", 0.01f);
        ActionDramas.Add("leavePartner", 0.2f);
        ActionDramas.Add("flirt", 0.01f);
        ActionDramas.Add("chat", 0.001f);
        ActionDramas.Add("giveGift", 0.05f);
        ActionDramas.Add("poison", 0.3f);
        ActionDramas.Add("gossip", 0.01f);
        ActionDramas.Add("argue", 0.01f);
        ActionDramas.Add("makeDistraction", 0.2f);
        ActionDramas.Add("reminisce", 0.01f);
        ActionDramas.Add("deny", 0.05f);
        ActionDramas.Add("praise", 0.03f);
        ActionDramas.Add("cry", 0.1f);
        ActionDramas.Add("console", 0.07f);
        ActionDramas.Add("convict", 0.3f);
        ActionDramas.Add("fight", 0.2f);
        ActionDramas.Add("bribe", 0.05f);
        ActionDramas.Add("argueInnocence", 0.1f);
        ActionDramas.Add("argueGuiltiness", 0.2f);
        ActionDramas.Add("steal", 0.3f);
        ActionDramas.Add("makeFunOf", 0.01f);
        ActionDramas.Add("tellJoke", 0.001f);
        ActionDramas.Add("harass", 0.1f);
        ActionDramas.Add("prank", 0.01f);
        ActionDramas.Add("playGame", 0.01f);
        ActionDramas.Add("order", 0.1f);
        ActionDramas.Add("kill", 0.5f);
        ActionDramas.Add("buyCompany", 0.1f);
        ActionDramas.Add("sellCompany", 0.1f);
        ActionDramas.Add("sabotage", 0.3f);
        ActionDramas.Add("advertise", 0.01f);
        ActionDramas.Add("demandToLeaveGuild", 0.05f);
        ActionDramas.Add("buyGoods", 0.01f);
        ActionDramas.Add("sellGoods", 0.01f);
    }
}