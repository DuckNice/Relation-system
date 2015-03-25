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

// --------------------------- INTERPERSONAL ACTIONS
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
		culture.Add(new MaskAdds("Bunce", "Bungary", 0.8f, new List<Person>()));
		
		relationSystem.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.6f, 0.4f, 0.7f, new float[] { -0.2f, 0.5f, 0.1f },new float[]{0.0f,0.0f,0.0f});
		#endregion AddingBill
		
		#region AddingTerese
		selfPersMask = new MaskAdds("Self", "Therese", 0.0f, new List<Person>());
		
		culture = new List<MaskAdds>();
		culture.Add(new MaskAdds("Buncess", "Bungary", 0.6f, new List<Person>()));

		relationSystem.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.3f, 0.7f, 0.2f, new float[] { 0.6f, -0.5f, 0.6f },new float[]{0.0f,0.0f,0.0f});
		#endregion AddingTerese
		
		#region AddingJohn
		selfPersMask = new MaskAdds("Self", "John", 0.0f, new List<Person>());
		
		culture = new List<MaskAdds>();
		culture.Add(new MaskAdds("Bunsant", "Bungary", 0.1f, new List<Person>()));
		
		relationSystem.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.5f, 0.5f, 0.4f, new float[] { 0.0f, 0.8f, -0.4f },new float[]{0.0f,0.0f,0.0f});
		#endregion AddingJohn

		#region AddingHeather
		selfPersMask = new MaskAdds("Self", "Heather", 0.0f, new List<Person>());
		
		culture = new List<MaskAdds>();
		culture.Add(new MaskAdds("Bunsant", "Bungary", 0.3f, new List<Person>()));
		
		relationSystem.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.2f, 0.8f, 0.8f, new float[] { 0.2f, 0.4f, 0.0f },new float[]{0.0f,0.0f,0.0f});
		#endregion AddingHeather


		#region Rules
		
		//BILL THERESE RULES
		//	relationSystem.AddRuleToMask("BillTherese", "Married", "GreetfBill","Greet", 0.2f, new List<Rule>(), GreetCondition);
		//	relationSystem.AddRuleToMask("ThereseBill", "Married", "GreetfTherese","Greet", 0.2f, new List<Rule>(), GreetCondition);
		//	relationSystem.AddRuleToMask("John", "Self", "GreetfJohn","Greet", 0.0f, new List<Rule>(), GreetCondition);
		
		//	relationSystem.AddRuleToMask("BillTherese", "Married", "ComplimentSpouse","Compliment", 0.3f, new List<Rule>(), emptyCondition);
		//  relationSystem.AddRuleToMask("ThereseBill", "Married",  "ComplimentSpouse","Compliment", 0.3f, new List<Rule>(), emptyCondition);

		
		relationSystem.CreateNewRule("KissfTherese", "kiss", kissCondition);
		relationSystem.AddRuleToMask("ThereseBill", "Married", "KissfTherese", 0.4f);
		
		relationSystem.CreateNewRule("KissfBill", "kiss", kissCondition);
		relationSystem.AddRuleToMask("BillTherese", "Married", "KissfBill", 0.4f);    
		
		//BILL JOHN RULES
		
		//THERESE JOHN RULES
		relationSystem.CreateNewRule("KissfJohn", "kiss", kissCondition);
		relationSystem.AddRuleToMask("JohnTherese", "Peasant", "KissfJohn", -0.6f);
		
		relationSystem.AddRuleToMask("ThereseJohn", "Princess", "accusefJohn", 0.1f);
		
		// CULTURAL RULES

		relationSystem.CreateNewRule("ConvictfBill", "convict",  convictCondition);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "ConvictfBill", -0.0f);
		
		relationSystem.CreateNewRule("flee", "flee", fleeCondition);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "flee", 0.2f);
		
		relationSystem.CreateNewRule("fight", "fight", fightCondition);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "fight", 0.1f);
		
		relationSystem.CreateNewRule("bribe", "bribe", bribeCondition);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "bribe", 0.3f);
		
		
		//SELF RULES
		relationSystem.CreateNewRule("doNothingfSant", "doNothing", emptyCondition);
		relationSystem.AddRuleToMask("John", "Self", "doNothingfSant", 0.0f);
		
		relationSystem.CreateNewRule("doNothingfcess", "doNothing", emptyCondition);
		relationSystem.AddRuleToMask("Therese", "Self", "doNothingfcess", 0.0f);
		
		relationSystem.CreateNewRule("doNothingfbuncce", "doNothing", emptyCondition);
		relationSystem.AddRuleToMask("Bill", "Self", "doNothingfbuncce", 0.0f);
		
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
