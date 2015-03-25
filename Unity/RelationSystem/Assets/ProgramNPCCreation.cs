using UnityEngine;
using System.Collections;
using System.Collections.Generic;


	//Namespaces
using NRelationSystem;

public partial class Program : MonoBehaviour 
{
	public void CreateFirstRooms()
	{
		roomMan = new RoomManager();
		roomMan.NewRoom ("Indgang");
		roomMan.NewRoom ("Stue");
		roomMan.NewRoom ("Gang");
		roomMan.NewRoom ("Køkken");
	}


	public void CreateFirstBeings()
	{
		Being Bill = new Being ("Bill", relationSystem);
		Being Therese = new Being ("Therese", relationSystem);
		Being John = new Being ("John", relationSystem);
		Being Heather = new Being ("Heather", relationSystem);

		roomMan.EnterRoom ("Indgang", Bill);
		roomMan.EnterRoom ("Indgang", Therese);
		roomMan.EnterRoom ("Indgang", John);
		
		beings.Add (Bill);
		beings.Add (Therese);
		beings.Add (John);
		beings.Add (Heather);
		//Bill.FindFocusToAll (beings);
	}


	public void CreateFirstMasks()
	{
		relationSystem.CreateNewMask("Player", new float[]{}, new bool[]{}, TypeMask.selfPerc, new string[]{});

		relationSystem.CreateNewMask("Bungary", new float[] { 0.0f, -0.2f }, new bool[] { }, TypeMask.culture, new string[] { "Bunce", "Buncess", "Bunsant" });
		relationSystem.CreateNewMask("Cult", new float[] { 0.0f, -0.2f }, new bool[] { }, TypeMask.culture, new string[] { "Leader", "Follower", "Skeptic" });
		relationSystem.CreateNewMask("MerchantGuild", new float[] { 0.0f, -0.2f }, new bool[] { }, TypeMask.culture, new string[] { "Leader", "Rich", "Poor" });

		relationSystem.CreateNewMask("Bill", new float[] { 0.0f, 0.0f }, new bool[] { }, TypeMask.selfPerc, new string[] { "" });
		relationSystem.CreateNewMask("Therese", new float[] { 0.0f, 0.0f }, new bool[] { }, TypeMask.selfPerc, new string[] { "" });
		relationSystem.CreateNewMask("John", new float[] { 0.0f, 0.0f }, new bool[] { }, TypeMask.selfPerc, new string[] { "" });
		relationSystem.CreateNewMask("Heather", new float[] { 0.0f, 0.0f }, new bool[] { }, TypeMask.selfPerc, new string[] { "" });
		
		relationSystem.CreateNewMask("BillTherese", new float[] { 0.2f, -0.2f }, new bool[] { }, TypeMask.interPers, new string[] { "Partner" });
		relationSystem.CreateNewMask("ThereseBill", new float[] { 0.3f, 0.2f }, new bool[] { }, TypeMask.interPers, new string[] { "Partner" });
		
		relationSystem.CreateNewMask("BillJohn", new float[] { -0.2f, -0.2f }, new bool[] { }, TypeMask.interPers, new string[] { "Enemy" });
		relationSystem.CreateNewMask("JohnBill", new float[] { -0.2f, -0.2f }, new bool[] { }, TypeMask.interPers, new string[] { "Enemy" });
		
		relationSystem.CreateNewMask("JohnTherese", new float[] { 0.2f, -0.2f }, new bool[] { }, TypeMask.interPers, new string[] { "Enemy" });
		relationSystem.CreateNewMask("ThereseJohn", new float[] { -0.2f, -0.2f }, new bool[] { }, TypeMask.interPers, new string[] { "Enemy" });

		relationSystem.CreateNewMask("BillHeather", new float[] { 0.4f, 0.4f }, new bool[] { }, TypeMask.interPers, new string[] { "Friend" });
		relationSystem.CreateNewMask("HeatherBill", new float[] { 0.2f, 0.5f }, new bool[] { }, TypeMask.interPers, new string[] { "Friend" });

		relationSystem.CreateNewMask("HeatherTherese", new float[] { 0.2f, 0.2f }, new bool[] { }, TypeMask.interPers, new string[] { "Friend" });
		relationSystem.CreateNewMask("ThereseHeather", new float[] { 0.2f, 0.0f }, new bool[] { }, TypeMask.interPers, new string[] { "Friend" });

		relationSystem.CreateNewMask("JohnHeather", new float[] { 0.6f, 0.0f }, new bool[] { }, TypeMask.interPers, new string[] { "Partner" });
		relationSystem.CreateNewMask("HeatherJohn", new float[] { 0.2f, 0.0f }, new bool[] { }, TypeMask.interPers, new string[] { "Partner" });

		relationSystem.CreateNewMask("BillPlayer", new float[] { -0.4f, 0.0f }, new bool[] { }, TypeMask.interPers, new string[] { "Partner" });
		relationSystem.CreateNewMask("HeatherPlayer", new float[] { 0.5f, 0.0f }, new bool[] { }, TypeMask.interPers, new string[] { "Enemy" });
		relationSystem.CreateNewMask("TheresePlayer", new float[] { -0.2f, 0.0f }, new bool[] { }, TypeMask.interPers, new string[] { "Enemy" });
		relationSystem.CreateNewMask("JohnPlayer", new float[] { 0.2f, 0.0f }, new bool[] { }, TypeMask.interPers, new string[] { "Friend" });
	}



	public void CreateFirstPeople()
	{
		#region adding Conditions

// --------------------------- INTERPERSONAL RULE CONDITIONS
		RuleConditioner emptyCondition = (self, other, indPpl) => { 
			//UIFunctions.WriteGameLine("PassedCorrectly ");
			return true;
		};
		
		RuleConditioner GreetCondition = (self, other, indPpl) =>
		{ if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["greet"] && x.GetSubject()==self && x.GetDirect()==other)){
				return false; }
			return true; };

		RuleConditioner fleeCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["convict"] && x.GetDirect()==self) ){
				if(self.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue() >= 0.0){
					return true; 
				}
			}
			return false; };

		RuleConditioner kissCondition = (self, other, indPpl) =>
		{	if (self.interPersonal.Exists(x => x.roleName == "Married") && other.interPersonal.Exists(x => x.roleName == "Married"))
			{ return true; }
			
			if (self.interPersonal.Exists(x => x.roleName == "Peasant")) { return true; }
			
			return false;
		};

		RuleConditioner chooseAnotherAsPartnerCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.GetlvlOfInfl() > 0.5)){
				return true;
			}
			return false; };

		RuleConditioner stayAsPartnerCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.GetlvlOfInfl() > 0.5) && self.interPersonal.Exists(x => x.roleName == "Partner") && 
			     													  other.interPersonal.Exists(x => x.roleName == "Partner")){
				return true;
			}
			return false; };

		RuleConditioner LeavePartnerCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.GetlvlOfInfl() < 0.5) && self.interPersonal.Exists(x => x.roleName == "Partner") && 
			   														  other.interPersonal.Exists(x => x.roleName == "Partner")){
				return true;
			}
			return false; };

		RuleConditioner flirtCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.GetlvlOfInfl() > 0.5) && self.interPersonal.Exists(x => x.roleName != "Partner") && 
			 														  other.interPersonal.Exists(x => x.roleName != "Partner")){
				return true;
			}
			return false; };

		RuleConditioner chatCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.GetlvlOfInfl() > 0.2)){
				return true;
			}
			return false; };

		RuleConditioner giveGiftCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.GetlvlOfInfl() > 0.5f) && (self.interPersonal.Exists(x=>x.roleRef.Exists(y=>y.name == other.name)))){
				return true;
			}
			return false; };

		RuleConditioner poisonCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.GetlvlOfInfl() < 0.5f) && (self.interPersonal.Exists(x=>x.roleRef.Exists(y=>y.name == other.name)))){
				return true;
			}
			return false; };

		RuleConditioner gossipCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.GetlvlOfInfl() > 0.1f) && (self.interPersonal.Exists(x=>x.roleRef.Exists(y=>y.name == other.name)))){
				return true;
			}
			return false; };

		RuleConditioner argueCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.GetlvlOfInfl() > 0.1f) && (self.interPersonal.Exists(x=>x.roleRef.Exists(y=>y.name == other.name)))){
				return true;
			}
			return false; };

		RuleConditioner demandToStopBeingFriendWithCondition = (self, other, indPpl) =>
		{	foreach(Person p in indPpl){
				if( p.interPersonal.Exists(x=> x.GetlvlOfInfl() < 0.3) && self.interPersonal.Exists(x=>x.roleRef.Exists(y=>y.name == other.name)))
				   { return true; }
			}
			return false; };
		
		RuleConditioner makeDistractionCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["askForHelpInIllicitActivity"] && x.GetSubject() == self))
				{ return true; }
			return false; };

		RuleConditioner denyCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.GetlvlOfInfl() < 0.4f) && (self.interPersonal.Exists(x=>x.roleRef.Exists(y=>y.name == other.name)))) { return true; }
			return false; };

		RuleConditioner enthuseAboutGreatnessofPersonCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.GetlvlOfInfl() > 0.5f) && (self.interPersonal.Exists(x=>x.roleRef.Exists(y=>y.name == other.name)))) { return true; }
			return false; };


// --------------------------------- CULTURAL CONDITIONS
		
		RuleConditioner convictCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["steal"] && x.GetDirect()==self && (x.GetSubject()==other && x.GetSubject()!=self)) )
			{	return true; }
			return false; };

		RuleConditioner fightCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["convict"] && x.GetDirect()==self && x.GetSubject()==other) ){
				if(self.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue() < 0.0){
					return true; 
				}
			}
			return false; };
		
		RuleConditioner bribeCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["convict"] && x.GetDirect()==self && x.GetSubject()==other) ){
				return true; 
			}
			return false; };

		RuleConditioner argueInnocenceCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["convict"] && x.GetSubject()==other)){ return true; }
			return false; };

		RuleConditioner argueGuiltinessCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["convict"] && x.GetSubject()==other)){ return true; }
			return false; };

		RuleConditioner stealCondition = (self, other, indPpl) =>
		{	if(true){ return true; }
			return false; };

		RuleConditioner practiceStealingCondition = (self, other, indPpl) =>
		{	if(true){ return true; }
			return false; };

		RuleConditioner askForHelpInIllicitActivityCondition = (self, other, indPpl) =>
		{	if(true){ return true; }
			return false; };

		RuleConditioner searchForThiefCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["steal"])) { return true; }
			return false; };


// -------------- CULTURAL (CULT) ACTIONS

		RuleConditioner PraiseCultCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["steal"])) { return true; }
			return false; };

		RuleConditioner enterCultCondition = (self, other, indPpl) =>
		{	if(self.culture.Exists(x=>x.roleRef.Exists(y=>y.name=="cult") && x.GetlvlOfInfl() > 0.5f)) { return true; }
			return false; };

		RuleConditioner exitCultCondition = (self, other, indPpl) =>
		{	if(self.culture.Exists(x=>x.roleRef.Exists(y=>y.name=="cult") && x.GetlvlOfInfl() < 0.1f)) { return true; }
			return false; };

		RuleConditioner damnCultCondition = (self, other, indPpl) =>
		{	if(self.culture.Exists(x=>x.roleRef.Exists(y=>y.name=="cult") && x.GetlvlOfInfl() < 0.4f)) { return true; }
			return false; };

		RuleConditioner excommunicateFromCultCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["damnCult"] && x.GetSubject().name == other.name)) { return true; }
			return false; };

// --------------- CULTURAL (MERCHANT) ACTIONS

		RuleConditioner buyCompanyCondition = (self, other, indPpl) =>
		{	if(true) { return true; }
			return false; };

		RuleConditioner sellCompanyCondition = (self, other, indPpl) =>
		{	if(true) { return true; }
			return false; };

		RuleConditioner sabotageCondition = (self, other, indPpl) =>
		{	if(true) { return true; }
			return false; };

		RuleConditioner advertiseCondition = (self, other, indPpl) =>
		{	if(true) { return true; }
			return false; };

		RuleConditioner convinceToLeaveGuildCondition = (self, other, indPpl) =>
		{	if(true) { return true; }
			return false; };

		RuleConditioner DemandtoLeaveGuildCondition = (self, other, indPpl) =>
		{	if(true) { return true; }
			return false; };

		RuleConditioner askForHelpCondition = (self, other, indPpl) =>
		{	if(true) { return true; }
			return false; };

		RuleConditioner buyGoodsCondition = (self, other, indPpl) =>
		{	if(true) { return true; }
			return false; };

		RuleConditioner sellGoodsCondition = (self, other, indPpl) =>
		{	if(true) { return true; }
			return false; };

		#endregion adding Conditions


		#region AddingPlayer
		MaskAdds selfPersMask = new MaskAdds("Self", "Player", 0.0f, new List<Person>());
		
		relationSystem.CreateNewPerson(selfPersMask, new List<MaskAdds>(), new List<MaskAdds>(), 0f, 0f, 0f, new float[] { 0f, 0f, 0f },new float[]{0.0f,0.0f,0.0f});
		#endregion AddingPlayer
		
		#region AddingBill
		selfPersMask = new MaskAdds("Self", "Bill", 0.0f, new List<Person>());
		
		List<MaskAdds>  culture = new List<MaskAdds>();
		culture.Add(new MaskAdds("Bunce", "Bungary", 0.2f, new List<Person>()));
		culture.Add(new MaskAdds("Follower", "Cult", 0.4f,new List<Person>()));
		culture.Add(new MaskAdds("Rich", "MerchantGuild", 0.6f,new List<Person>()));
		
		relationSystem.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.6f, 0.4f, 0.7f, new float[] { -0.2f, 0.5f, 0.1f },new float[]{0.0f,0.0f,0.0f});
		#endregion AddingBill
		
		#region AddingTerese
		selfPersMask = new MaskAdds("Self", "Therese", 0.0f, new List<Person>());

		culture = new List<MaskAdds>();
		culture.Add(new MaskAdds("Buncess", "Bungary", 0.6f, new List<Person>()));
		culture.Add(new MaskAdds("Sceptic", "Cult", 0.1f,new List<Person>()));

		relationSystem.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.3f, 0.7f, 0.2f, new float[] { 0.6f, -0.5f, 0.6f },new float[]{0.0f,0.0f,0.0f});
		#endregion AddingTerese
		
		#region AddingJohn
		selfPersMask = new MaskAdds("Self", "John", 0.0f, new List<Person>());
		culture.Add(new MaskAdds("Follower", "Cult", 0.4f,new List<Person>()));

		culture = new List<MaskAdds>();
		culture.Add(new MaskAdds("Bunsant", "Bungary", 0.1f, new List<Person>()));
		
		relationSystem.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.5f, 0.5f, 0.4f, new float[] { 0.0f, 0.8f, -0.4f },new float[]{0.0f,0.0f,0.0f});
		#endregion AddingJohn

		#region AddingHeather
		selfPersMask = new MaskAdds("Self", "Heather", 0.0f, new List<Person>());
		
		culture = new List<MaskAdds>();
		culture.Add(new MaskAdds("Bunsant", "Bungary", 0.3f, new List<Person>()));
		culture.Add(new MaskAdds("Leader", "Cult", 0.9f,new List<Person>()));
		
		relationSystem.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.2f, 0.8f, 0.8f, new float[] { 0.2f, 0.4f, 0.0f },new float[]{0.0f,0.0f,0.0f});
		#endregion AddingHeather


		#region Rules
		relationSystem.CreateNewRule("kissfbunce", "kiss", kissCondition);
		relationSystem.CreateNewRule("kissfcess", "kiss", kissCondition);
		relationSystem.CreateNewRule("kissfbunsant", "kiss", kissCondition);

		// CULTURAL RULES
		relationSystem.CreateNewRule("convictfcess", "convict",  convictCondition);
		relationSystem.CreateNewRule("convictfbunce", "convict",  convictCondition);
		relationSystem.CreateNewRule("fight", "fight", fightCondition);
		relationSystem.CreateNewRule("bribefbunce", "bribe", bribeCondition);
		relationSystem.CreateNewRule("bribefcess", "bribe", bribeCondition);
		relationSystem.CreateNewRule("bribefbunsant", "bribe", bribeCondition);
		relationSystem.CreateNewRule("argueinnocencefbunce", "argueinnocence", argueInnocenceCondition);
		relationSystem.CreateNewRule("argueinnocencefcess", "argueinnocence", argueInnocenceCondition);
		relationSystem.CreateNewRule("argueguiltinessfbunce", "argueguiltiness", argueGuiltinessCondition);
		relationSystem.CreateNewRule("argueguiltinessfcess", "argueguiltiness", argueGuiltinessCondition);
		relationSystem.CreateNewRule("steal", "steal", stealCondition);
		relationSystem.CreateNewRule("practicestealing", "practicestealing", practiceStealingCondition);
		relationSystem.CreateNewRule("askforhelpinillicitactivity", "askforhelpinillicitactivity", askForHelpInIllicitActivityCondition);
		relationSystem.CreateNewRule("searchforthieffbunce", "searchforthief", searchForThiefCondition);
		relationSystem.CreateNewRule("searchforthieffcess", "searchforthief", searchForThiefCondition);

		relationSystem.CreateNewRule("praisecultfleader", "praisecult", PraiseCultCondition);
		relationSystem.CreateNewRule("praisecultffollower", "praisecult", PraiseCultCondition);
		relationSystem.CreateNewRule("entercult", "entercult", enterCultCondition);
		relationSystem.CreateNewRule("exitcult", "exitcult", exitCultCondition);
		relationSystem.CreateNewRule("damncult", "damncult", damnCultCondition);
		relationSystem.CreateNewRule("excommunicatefromcult", "excommunicatefromcult", excommunicateFromCultCondition);

		//SELF RULES
		relationSystem.CreateNewRule("donothing", "donothing", emptyCondition);
		relationSystem.CreateNewRule("flee", "flee", fleeCondition);


// -------------- ADDING RULES TO MASKS
	
	// SElF
		relationSystem.AddRuleToMask("John", "Self", "donothing", 0.0f);
		relationSystem.AddRuleToMask("Therese", "Self", "donothing", 0.0f);
		relationSystem.AddRuleToMask("Bill", "Self", "donothing", 0.0f);
		relationSystem.AddRuleToMask("Heather", "Self", "donothing", 0.0f);

		relationSystem.AddRuleToMask("John", "Self", "flee", 0.2f);
		relationSystem.AddRuleToMask("Heather", "Self", "flee", -0.1f);
	
		//THESE ARE RIGHT NOW CULTURE, BUT SHOULD PROBABLY BE CHANGED TO INTERPERSONAL
		relationSystem.AddRuleToMask("Bungary", "Bunce", "kissfbunce", 0.4f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "kissfcess", 0.4f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "kissfbunsant", 0.2f);
	
	//CULTURE
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "fight", -0.5f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "bribefbunsant", -0.1f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "steal", -0.5f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "practicestealing", -0.3f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "askforhelpinillicitactivity", -0.1f);

		relationSystem.AddRuleToMask("Bungary", "Bunce", "bribefbunce", 0.3f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "convictfbunce", 0.0f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "argueinnocencefbunce", 0.0f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "argueguiltinessfbunce", 0.0f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "searchforthieffbunce", 0.8f);

		relationSystem.AddRuleToMask("Bungary", "Buncess", "bribefcess", 0.3f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "convictfcess", 0.0f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "argueinnocencefcess", 0.2f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "argueguiltinessfcess", -0.1f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "searchforthieffcess", 0.3f);

		relationSystem.AddRuleToMask("Cult", "Leader", "praisecultfleader", 0.6f);
		relationSystem.AddRuleToMask("Cult", "Follower", "praisecultffollower", 0.4f);
		relationSystem.AddRuleToMask("Cult", "Follower", "entercult", 0.3f);
		relationSystem.AddRuleToMask("Cult", "Follower", "exitcult", -0.8f);
		relationSystem.AddRuleToMask("Cult", "Follower", "damncult", -0.4f);
		relationSystem.AddRuleToMask("Cult", "Leader", "excommunicatefromcult", 0.1f);


		#endregion Rules
		
		
		
		#region LINKS
		relationSystem.AddLinkToPerson("Bill", new string[] { "Therese" }, TypeMask.interPers, "Partner", "BillTherese", 0.6f);
		relationSystem.AddLinkToPerson("Bill", new string[] { "John" }, TypeMask.interPers, "Enemy", "BillJohn", 0.2f);
		relationSystem.AddLinkToPerson("Bill", new string[] { "Heather" }, TypeMask.interPers, "Friend", "BillHeather", 0.2f);
		relationSystem.AddLinkToPerson("Bill", new string[] { "Player" }, TypeMask.interPers, "Enemy", "BillPlayer", 0.2f);

		relationSystem.AddLinkToPerson("Therese", new string[] { "Bill" }, TypeMask.interPers, "Partner", "ThereseBill", 0.4f);
		relationSystem.AddLinkToPerson("Therese", new string[] { "John" }, TypeMask.interPers, "Enemy", "ThereseJohn", 0.2f);
		relationSystem.AddLinkToPerson("Therese", new string[] { "Heather" }, TypeMask.interPers, "Friend", "ThereseHeather", 0.6f);
		relationSystem.AddLinkToPerson("Therese", new string[] { "Player" }, TypeMask.interPers, "Enemy", "TheresePlayer", 0.3f);

		relationSystem.AddLinkToPerson("John", new string[] { "Bill" }, TypeMask.interPers, "Enemy", "JohnBill", 0.7f);
		relationSystem.AddLinkToPerson("John", new string[] { "Therese" }, TypeMask.interPers, "Enemy", "JohnTherese", 0.4f);
		relationSystem.AddLinkToPerson("John", new string[] { "Heather" }, TypeMask.interPers, "Friend", "JohnHeather", 0.8f);
		relationSystem.AddLinkToPerson("John", new string[] { "Player" }, TypeMask.interPers, "Enemy", "JohnPlayer", 0.5f);

		relationSystem.AddLinkToPerson("Heather", new string[] { "Bill" }, TypeMask.interPers, "Friend", "HeatherBill", 0.4f);
		relationSystem.AddLinkToPerson("Heather", new string[] { "Therese" }, TypeMask.interPers, "Enemy", "HeatherTherese", 0.6f);
		relationSystem.AddLinkToPerson("Heather", new string[] { "John" }, TypeMask.interPers, "Partner", "HeatherJohn", 0.5f);
		relationSystem.AddLinkToPerson("Heather", new string[] { "Player" }, TypeMask.interPers, "Partner", "HeatherPlayer", 0.5f);
		#endregion LINKS 
		
		
	}
}
