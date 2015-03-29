using UnityEngine;
using System.Collections;
using System.Collections.Generic;


	//Namespaces
using NRelationSystem;

public partial class Program : MonoBehaviour 
{

	string statsString;

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
		Being Player = new Being ("Player", relationSystem);

		roomMan.EnterRoom ("Indgang", Bill);
		roomMan.EnterRoom ("Indgang", Therese);
		roomMan.EnterRoom ("Indgang", John);
		roomMan.EnterRoom ("Indgang", Player);
		
		beings.Add (Bill);
		beings.Add (Therese);
		beings.Add (John);
		beings.Add (Heather);
		beings.Add (Player);
		Bill.FindFocusToAll (beings);
		Therese.FindFocusToAll (beings);
		John.FindFocusToAll (beings);
		Heather.FindFocusToAll (beings);
		Player.FindFocusToAll (beings);

		Bill.possessions.Add (new Money (100f));
		Bill.possessions.Add (new Goods (5f));
		Bill.possessions.Add (new Company("Bill's Wares"));
		Therese.possessions.Add (new Money (70f));
		John.possessions.Add (new Money (5f));
		Heather.possessions.Add (new Money (20f));
		Player.possessions.Add (new Money (30f));
		Player.possessions.Add (new Goods (2f));
		Player.possessions.Add (new Company("A Poor Excuse for A Company"));

		foreach (Being b in beings) {
			b.name = b.name.ToLower();
		}


	}


	public void CreateFirstMasks()
	{
		relationSystem.CreateNewMask("Player", new float[]{}, new bool[]{}, TypeMask.selfPerc, new string[]{});

		relationSystem.CreateNewMask("Bungary", new float[] { 0.0f, -0.2f }, new bool[] { }, TypeMask.culture, new string[] { "Bunce", "Buncess", "Bunsant" });
		relationSystem.CreateNewMask("Cult", new float[] { 0.0f, -0.2f }, new bool[] { }, TypeMask.culture, new string[] { "Leader", "Follower", "Skeptic" });
		relationSystem.CreateNewMask("MerchantGuild", new float[] { 0.0f, -0.2f }, new bool[] { }, TypeMask.culture, new string[] { "Member" });

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
				if(self.moods[MoodTypes.angryFear] < -0.2f){
					return true; 
				}
			}
			return false; };

		RuleConditioner kissCondition = (self, other, indPpl) =>
		{	if (self.interPersonal.Exists(x => x.roleName == "Partner") && other.interPersonal.Exists(x => x.roleName == "Partner"))
			{ return true; }
			
			if (self.moods[MoodTypes.arousDisgus] > 0.5f) { return true; }
			return false;
		};

		RuleConditioner chooseAnotherAsPartnerCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.GetlvlOfInfl() > 0.5) && self.moods[MoodTypes.arousDisgus] > 0.3f){
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
			 	other.interPersonal.Exists(x => x.roleName != "Partner") && self.moods[MoodTypes.arousDisgus] > 0.1f)
				{ return true;}
			return false; };

		RuleConditioner chatCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.GetlvlOfInfl() > 0.2) && self.moods[MoodTypes.hapSad] > 0.0f){
				return true;
			}
			return false; };

		RuleConditioner giveGiftCondition = (self, other, indPpl) =>
		{	
			//AND IF YOU HAVE SOMETHING TO GIVE
			if(self.interPersonal.Exists(x=>x.GetlvlOfInfl() > 0.5f) && (self.interPersonal.Exists(x=>x.roleRef.Exists(y=>y.name == other.name)))){
				if(beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="money").value > 30f){
					return true;
				}
			}
			return false; };

		RuleConditioner poisonCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.GetlvlOfInfl() < 0.5f) && (self.interPersonal.Exists(x=>x.roleRef.Exists(y=>y.name == other.name))) && self.moods[MoodTypes.angryFear] < -0.7f){
				return true;
			}
			return false; };

		RuleConditioner gossipCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.GetlvlOfInfl() > 0.1f) && (self.interPersonal.Exists(x=>x.roleRef.Exists(y=>y.name == other.name)))){
				return true;
			}
			return false; };

		RuleConditioner argueCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.GetlvlOfInfl() > 0.1f) && (self.interPersonal.Exists(x=>x.roleRef.Exists(y=>y.name == other.name))) && self.moods[MoodTypes.angryFear] < -0.1f){
				return true;
			}
			return false; };

		RuleConditioner demandToStopBeingFriendWithCondition = (self, other, indPpl) =>
		{	
			//PROBABLY NEED OPINIONS FOR THIS ONE
			//foreach(Person p in indPpl){
			//	if( p.interPersonal.Exists(x=>x.GetlvlOfInfl() < 0.3))
			//	   { return true; }
			//}
			return false; };
		
		RuleConditioner makeDistractionCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["askforhelpinillicitactivity"] && x.GetSubject()==other && x.GetDirect()==self) 
			     && self.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue() < 0.0f)
				{ return true; }
			return false; };

		RuleConditioner reminisceCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["chat"] && x.GetSubject() == other))
			{ return true; }
			return false; };

		RuleConditioner denyCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.GetlvlOfInfl() < 0.4f) && (self.interPersonal.Exists(x=>x.roleRef.Exists(y=>y.name == other.name))) && 
			     (self.moods[MoodTypes.arousDisgus] < 0.0f ||
			      self.moods[MoodTypes.hapSad] < 0.0f ||
			      self.moods[MoodTypes.angryFear] < 0.0f)) 
				{ return true; }
			return false; };

		RuleConditioner enthuseAboutGreatnessofPersonCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.GetlvlOfInfl() > 0.5f) && (self.interPersonal.Exists(x=>x.roleRef.Exists(y=>y.name == other.name))) && self.moods[MoodTypes.hapSad] > 0.0f) 
				{ return true; }
			return false; };


// --------------------------------- CULTURAL CONDITIONS
		
		RuleConditioner convictCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["steal"] && x.GetDirect()==self && (x.GetSubject()==other && x.GetSubject()!=self)) )
			{	return true; }
			return false; };

		RuleConditioner fightCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["convict"] && x.GetDirect()==self && x.GetSubject()==other) ){
				if(self.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue() < 0.0 || self.moods[MoodTypes.angryFear] < -0.7f){
					return true; 
				}
			}
			return false; };
		
		RuleConditioner bribeCondition = (self, other, indPpl) =>
		{	//MONEY
			if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["convict"] && x.GetDirect()==self && x.GetSubject()==other) ){
				return true; 
			}
			return false; };

		RuleConditioner argueInnocenceCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["convict"] && x.GetSubject()==other) && self.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue() > 0.0f)
				{ return true; }
			return false; };

		RuleConditioner argueGuiltinessCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["convict"] && x.GetSubject()==other) && self.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue() < 0.0f)
				{ return true; }
			return false; };

		RuleConditioner stealCondition = (self, other, indPpl) =>
		{	//MONEY
			if(self.moods[MoodTypes.hapSad] < -0.3f){ return true; }
			return false; };

		RuleConditioner practiceStealingCondition = (self, other, indPpl) =>
		{	if(self.moods[MoodTypes.hapSad] < -0.2f && self.GetAbilityy() < 1.0f){ return true; }
			return false; };

		RuleConditioner askForHelpInIllicitActivityCondition = (self, other, indPpl) =>
		{	if(self.moods[MoodTypes.angryFear] < -0.2f && self.GetAbilityy() < 1.0f){ return true; }
			return false; };

		RuleConditioner searchForThiefCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["steal"])) { return true; }
			return false; };


// -------------- CULTURAL (CULT) ACTIONS

		RuleConditioner PraiseCultCondition = (self, other, indPpl) =>
		{	if(self.culture.Exists(x=>x.GetlvlOfInfl() > 0.5f && x.roleName == "Follower")) { return true; }
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
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["damncult"] && x.GetSubject() == other)) { return true; }
			return false; };

// --------------- CULTURAL (MERCHANT) ACTIONS

		RuleConditioner buyCompanyCondition = (self, other, indPpl) =>
		{	if(beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="money").value >= 100f && beings.Find(x=>x.name == other.name).possessions.Find(y=>y.Name=="company").value >= 1f && self != other) { return true; }
			return false; };

		RuleConditioner sellCompanyCondition = (self, other, indPpl) =>
		{	if(beings.Find(x=>x.name == other.name).possessions.Find(y=>y.Name=="money").value >= 100f && beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="company").value >= 1f) { return true; }
			return false; };

		RuleConditioner sabotageCondition = (self, other, indPpl) =>
		{	if(self.moods[MoodTypes.angryFear] > 0.5f || self.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue() < -0.5f) { return true; }
			return false; };

		RuleConditioner advertiseCondition = (self, other, indPpl) =>
		{	if(true) { return true; }
			return false; };

		RuleConditioner convinceToLeaveGuildCondition = (self, other, indPpl) =>
		{	if(true) { return true; }
			return false; };

		RuleConditioner DemandtoLeaveGuildCondition = (self, other, indPpl) =>
		{	if(self.moods[MoodTypes.angryFear] < -0.3f || self.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue() < -0.3f) { return true; }
			return false; };

		RuleConditioner askForHelpCondition = (self, other, indPpl) =>
		{	if(true) { return true; }
			return false; };

		RuleConditioner buyGoodsCondition = (self, other, indPpl) =>
		{	if(beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="goods").value < 2f && beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="money").value > 30f) { return true; }
			return false; };

		RuleConditioner sellGoodsCondition = (self, other, indPpl) =>
		{	if(beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="goods").value > 1f) { return true; }
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
		culture.Add(new MaskAdds("Member", "MerchantGuild", 0.6f,new List<Person>()));
		
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
		// INTERPERSONAL RULES
		relationSystem.CreateNewRule("kiss", "kiss", kissCondition);
		relationSystem.CreateNewRule("chooseanotheraspartner", "chooseanotheraspartner", chooseAnotherAsPartnerCondition);
		relationSystem.CreateNewRule("stayaspartner", "stayaspartner", stayAsPartnerCondition);
		relationSystem.CreateNewRule("leavepartner", "leavepartner", LeavePartnerCondition);
		relationSystem.CreateNewRule("flirt", "flirt", flirtCondition);
		relationSystem.CreateNewRule("chat", "chat", chatCondition);
		relationSystem.CreateNewRule("givegift", "givegift", giveGiftCondition);
		relationSystem.CreateNewRule("gossip", "gossip", gossipCondition);
		relationSystem.CreateNewRule("argue", "argue", argueCondition);
		relationSystem.CreateNewRule("deny", "deny", denyCondition);
		relationSystem.CreateNewRule("demandtostopbeingfriendwith", "demandtostopbeingfriendwith", demandToStopBeingFriendWithCondition);
		relationSystem.CreateNewRule("makedistraction", "makedistraction", makeDistractionCondition);
		relationSystem.CreateNewRule("reminisce", "reminisce", reminisceCondition);
		relationSystem.CreateNewRule("enthuseaboutgreatnessofperson", "enthuseaboutgreatnessofperson", enthuseAboutGreatnessofPersonCondition);

		// CULTURAL RULES
		relationSystem.CreateNewRule("greetfbunce", "greet",  GreetCondition);
		relationSystem.CreateNewRule("greetfcess", "greet",  GreetCondition);
		relationSystem.CreateNewRule("greetfbunsant", "greet",  GreetCondition);
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
		relationSystem.CreateNewRule("poisonfbunce", "poison", poisonCondition);
		relationSystem.CreateNewRule("poisonfcess", "poison", poisonCondition);
		relationSystem.CreateNewRule("poisonfbunsant", "poison", poisonCondition);

		relationSystem.CreateNewRule("praisecultfleader", "praisecult", PraiseCultCondition);
		relationSystem.CreateNewRule("praisecultffollower", "praisecult", PraiseCultCondition);
		relationSystem.CreateNewRule("entercult", "entercult", enterCultCondition);
		relationSystem.CreateNewRule("exitcult", "exitcult", exitCultCondition);
		relationSystem.CreateNewRule("damncult", "damncult", damnCultCondition);
		relationSystem.CreateNewRule("excommunicatefromcult", "excommunicatefromcult", excommunicateFromCultCondition);

		relationSystem.CreateNewRule("buycompany", "buycompany", buyCompanyCondition);
		relationSystem.CreateNewRule("sabotage", "sabotage", sabotageCondition);
		relationSystem.CreateNewRule("advertise", "advertise", advertiseCondition);
		relationSystem.CreateNewRule("convincetoleaveguild", "convincetoleaveguild", convinceToLeaveGuildCondition);
		relationSystem.CreateNewRule("demandtoleaveguild", "demandtoleaveguild", DemandtoLeaveGuildCondition);
		relationSystem.CreateNewRule("askforhelp", "askforhelp", askForHelpCondition);
		relationSystem.CreateNewRule("sellcompany", "sellcompany", sellCompanyCondition);
		relationSystem.CreateNewRule("buygoods", "buygoods", buyGoodsCondition);
		relationSystem.CreateNewRule("sellgoods", "sellgoods", sellGoodsCondition);

		//SELF RULES
		relationSystem.CreateNewRule("donothing", "donothing", emptyCondition);
		relationSystem.CreateNewRule("flee", "flee", fleeCondition);


// -----------------

		List<Rule> kissRulesToTrigger = new List<Rule>(); kissRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("stayaspartner")); kissRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("givegift"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("kiss",kissRulesToTrigger);

		List<Rule> giveGiftRulesToTrigger = new List<Rule>(); giveGiftRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("reminisce")); giveGiftRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("flirt"));
		giveGiftRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("deny")); giveGiftRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("chooseanotheraspartner"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("givegift",giveGiftRulesToTrigger);

		List<Rule> gossipRulesToTrigger = new List<Rule>(); gossipRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("reminisce")); gossipRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("flirt"));
		gossipRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("deny")); gossipRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("demandtostopbeingfriendwith"));
		gossipRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("gossip",gossipRulesToTrigger);

		List<Rule> denyRulesToTrigger = new List<Rule>(); denyRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue")); denyRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("poison")); 
		denyRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("fight"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("deny",denyRulesToTrigger);

		List<Rule> stealRulesToTrigger = new List<Rule>(); stealRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("searchforthief")); stealRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("poison")); 
		stealRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("fight")); stealRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("convict"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("steal",stealRulesToTrigger);

		List<Rule> convictRulesToTrigger = new List<Rule>(); convictRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("bribe")); convictRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("poison")); 
		convictRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("fight")); convictRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("flee")); 
		relationSystem.pplAndMasks.AddPossibleRulesToRule("convict",convictRulesToTrigger);

// -------------- ADDING RULES TO MASKS
	
	// SElF
		relationSystem.AddRuleToMask("John", "Self", "donothing", 0.0f);
		relationSystem.AddRuleToMask("Therese", "Self", "donothing", 0.0f);
		relationSystem.AddRuleToMask("Bill", "Self", "donothing", 0.0f);
		relationSystem.AddRuleToMask("Heather", "Self", "donothing", 0.0f);

		relationSystem.AddRuleToMask("John", "Self", "flee", 0.2f);
		relationSystem.AddRuleToMask("Heather", "Self", "flee", -0.1f);
		
	// INTERPERSONAL

		relationSystem.AddRuleToMask("BillTherese", "Partner", "kiss", 0.4f);
		relationSystem.AddRuleToMask("ThereseBill", "Partner", "kiss", 0.4f);
		relationSystem.AddRuleToMask("JohnHeather", "Partner", "kiss", 0.4f);
		relationSystem.AddRuleToMask("HeatherJohn", "Partner", "kiss", 0.4f);
		relationSystem.AddRuleToMask("HeatherPlayer", "Partner", "kiss", 0.4f);

		relationSystem.AddRuleToMask("HeatherJohn", "Partner", "chooseanotheraspartner", -0.3f);
		relationSystem.AddRuleToMask("HeatherPlayer", "Partner", "chooseanotheraspartner", -0.3f);
		relationSystem.AddRuleToMask("JohnHeather", "Partner", "chooseanotheraspartner", -0.3f);
		relationSystem.AddRuleToMask("HeatherJohn", "Partner", "stayaspartner", 0.4f);
		relationSystem.AddRuleToMask("HeatherPlayer", "Partner", "stayaspartner", 0.4f);
		relationSystem.AddRuleToMask("JohnHeather", "Partner", "stayaspartner", 0.4f);
		relationSystem.AddRuleToMask("BillTherese", "Partner", "stayaspartner", 0.4f);
		relationSystem.AddRuleToMask("ThereseBill", "Partner", "stayaspartner", 0.4f);
		relationSystem.AddRuleToMask("HeatherJohn", "Partner", "leavepartner", -0.2f);
		relationSystem.AddRuleToMask("HeatherPlayer", "Partner", "leavepartner", -0.2f);
		relationSystem.AddRuleToMask("JohnHeather", "Partner", "leavepartner", -0.2f);
		relationSystem.AddRuleToMask("BillTherese", "Partner", "leavepartner", -0.2f);
		relationSystem.AddRuleToMask("ThereseBill", "Partner", "leavepartner", -0.2f);

		relationSystem.AddRuleToMask("JohnHeather", "Partner", "flirt", -0.4f);
		relationSystem.AddRuleToMask("JohnBill", "Enemy", "flirt", -0.4f);
		relationSystem.AddRuleToMask("JohnTherese", "Enemy", "flirt", -0.4f);
		relationSystem.AddRuleToMask("JohnPlayer", "Friend", "flirt", -0.4f);
		relationSystem.AddRuleToMask("HeatherJohn", "Partner", "flirt", -0.4f);
		relationSystem.AddRuleToMask("HeatherPlayer", "Partner", "flirt", 0.4f);
		relationSystem.AddRuleToMask("HeatherBill", "Friend", "flirt", -0.4f);
		relationSystem.AddRuleToMask("HeatherTherese", "Friend", "flirt", -0.4f);
		relationSystem.AddRuleToMask("ThereseBill", "Partner", "flirt", 0.4f);
		relationSystem.AddRuleToMask("ThereseJohn", "Enemy", "flirt", -0.4f);
		relationSystem.AddRuleToMask("ThereseHeather", "Friend", "flirt", -0.4f);
		relationSystem.AddRuleToMask("TheresePlayer", "Enemy", "flirt", -0.4f);
		relationSystem.AddRuleToMask("BillTherese", "Partner", "flirt", 0.4f);
		relationSystem.AddRuleToMask("BillJohn", "Enemy", "flirt", -0.4f);
		relationSystem.AddRuleToMask("BillHeather", "Friend", "flirt", -0.4f);
		relationSystem.AddRuleToMask("BillPlayer", "Enemy", "flirt", -0.4f);

		relationSystem.AddRuleToMask("JohnHeather", "Partner", "chat", 0.0f);
		relationSystem.AddRuleToMask("JohnBill", "Enemy", "chat", 0.0f);
		relationSystem.AddRuleToMask("JohnTherese", "Enemy", "chat", 0.0f);
		relationSystem.AddRuleToMask("JohnPlayer", "Friend", "chat", 0.0f);
		relationSystem.AddRuleToMask("HeatherJohn", "Partner", "chat", 0.0f);
		relationSystem.AddRuleToMask("HeatherPlayer", "Partner", "chat",0.0f);
		relationSystem.AddRuleToMask("HeatherBill", "Friend", "chat", 0.0f);
		relationSystem.AddRuleToMask("HeatherTherese", "Friend", "chat", 0.0f);
		relationSystem.AddRuleToMask("ThereseBill", "Partner", "chat", 0.0f);
		relationSystem.AddRuleToMask("ThereseJohn", "Enemy", "chat", 0.0f);
		relationSystem.AddRuleToMask("ThereseHeather", "Friend", "chat", 0.0f);
		relationSystem.AddRuleToMask("TheresePlayer", "Enemy", "chat", 0.0f);
		relationSystem.AddRuleToMask("BillTherese", "Partner", "chat",0.0f);
		relationSystem.AddRuleToMask("BillJohn", "Enemy", "chat", 0.0f);
		relationSystem.AddRuleToMask("BillHeather", "Friend", "chat", 0.0f);
		relationSystem.AddRuleToMask("BillPlayer", "Enemy", "chat", 0.0f);



		relationSystem.AddRuleToMask("JohnHeather", "Partner", "givegift", 0.0f);
		relationSystem.AddRuleToMask("JohnBill", "Enemy", "givegift", 0.0f);
		relationSystem.AddRuleToMask("JohnTherese", "Enemy", "givegift", 0.0f);
		relationSystem.AddRuleToMask("JohnPlayer", "Friend", "givegift", 0.0f);
		relationSystem.AddRuleToMask("HeatherJohn", "Partner", "givegift", 0.0f);
		relationSystem.AddRuleToMask("HeatherPlayer", "Partner", "givegift",0.0f);
		relationSystem.AddRuleToMask("HeatherBill", "Friend", "givegift", 0.0f);
		relationSystem.AddRuleToMask("HeatherTherese", "Friend", "givegift", 0.0f);
		relationSystem.AddRuleToMask("ThereseBill", "Partner", "givegift", 0.0f);
		relationSystem.AddRuleToMask("ThereseJohn", "Enemy", "givegift", 0.0f);
		relationSystem.AddRuleToMask("ThereseHeather", "Friend", "givegift", 0.0f);
		relationSystem.AddRuleToMask("TheresePlayer", "Enemy", "givegift", 0.0f);
		relationSystem.AddRuleToMask("BillTherese", "Partner", "givegift",0.0f);
		relationSystem.AddRuleToMask("BillJohn", "Enemy", "givegift", 0.0f);
		relationSystem.AddRuleToMask("BillHeather", "Friend", "givegift", 0.0f);
		relationSystem.AddRuleToMask("BillPlayer", "Enemy", "givegift", 0.0f);

		relationSystem.AddRuleToMask("JohnHeather", "Partner", "gossip", -0.2f);
		relationSystem.AddRuleToMask("JohnBill", "Enemy", "gossip", -0.2f);
		relationSystem.AddRuleToMask("JohnTherese", "Enemy", "gossip", -0.2f);
		relationSystem.AddRuleToMask("JohnPlayer", "Friend", "gossip", -0.2f);
		relationSystem.AddRuleToMask("HeatherJohn", "Partner", "gossip", -0.2f);
		relationSystem.AddRuleToMask("HeatherPlayer", "Partner", "gossip", -0.2f);
		relationSystem.AddRuleToMask("HeatherBill", "Friend", "gossip", -0.2f);
		relationSystem.AddRuleToMask("HeatherTherese", "Friend", "gossip", -0.1f);
		relationSystem.AddRuleToMask("ThereseBill", "Partner", "gossip", -0.2f);
		relationSystem.AddRuleToMask("ThereseJohn", "Enemy", "gossip", -0.2f);
		relationSystem.AddRuleToMask("ThereseHeather", "Friend", "gossip", -0.1f);
		relationSystem.AddRuleToMask("TheresePlayer", "Enemy", "gossip", -0.2f);
		relationSystem.AddRuleToMask("BillTherese", "Partner", "gossip", -0.2f);
		relationSystem.AddRuleToMask("BillJohn", "Enemy", "gossip", -0.2f);
		relationSystem.AddRuleToMask("BillHeather", "Friend", "gossip", -0.2f);
		relationSystem.AddRuleToMask("BillPlayer", "Enemy", "gossip", -0.2f);

		relationSystem.AddRuleToMask("JohnHeather", "Partner", "argue", -0.2f);
		relationSystem.AddRuleToMask("JohnBill", "Enemy", "argue", -0.2f);
		relationSystem.AddRuleToMask("JohnTherese", "Enemy", "argue", -0.2f);
		relationSystem.AddRuleToMask("JohnPlayer", "Friend", "argue", -0.2f);
		relationSystem.AddRuleToMask("HeatherJohn", "Partner", "argue", -0.2f);
		relationSystem.AddRuleToMask("HeatherPlayer", "Partner", "argue", -0.2f);
		relationSystem.AddRuleToMask("HeatherBill", "Friend", "argue", -0.2f);
		relationSystem.AddRuleToMask("HeatherTherese", "Friend", "argue", -0.2f);
		relationSystem.AddRuleToMask("ThereseBill", "Partner", "argue", -0.2f);
		relationSystem.AddRuleToMask("ThereseJohn", "Enemy", "argue", -0.2f);
		relationSystem.AddRuleToMask("ThereseHeather", "Friend", "argue", -0.2f);
		relationSystem.AddRuleToMask("TheresePlayer", "Enemy", "argue", -0.2f);
		relationSystem.AddRuleToMask("BillTherese", "Partner", "argue", -0.2f);
		relationSystem.AddRuleToMask("BillJohn", "Enemy", "argue", -0.2f);
		relationSystem.AddRuleToMask("BillHeather", "Friend", "argue", -0.2f);
		relationSystem.AddRuleToMask("BillPlayer", "Enemy", "argue", -0.2f);

		relationSystem.AddRuleToMask("JohnHeather", "Partner", "deny", -0.1f);
		relationSystem.AddRuleToMask("JohnBill", "Enemy", "deny", -0.1f);
		relationSystem.AddRuleToMask("JohnTherese", "Enemy", "deny", -0.1f);
		relationSystem.AddRuleToMask("JohnPlayer", "Friend", "deny", -0.1f);
		relationSystem.AddRuleToMask("HeatherJohn", "Partner", "deny", -0.1f);
		relationSystem.AddRuleToMask("HeatherPlayer", "Partner", "deny", -0.1f);
		relationSystem.AddRuleToMask("HeatherBill", "Friend", "deny", -0.1f);
		relationSystem.AddRuleToMask("HeatherTherese", "Friend", "deny", -0.1f);
		relationSystem.AddRuleToMask("ThereseBill", "Partner", "deny", -0.1f);
		relationSystem.AddRuleToMask("ThereseJohn", "Enemy", "deny", -0.1f);
		relationSystem.AddRuleToMask("ThereseHeather", "Friend", "deny", -0.1f);
		relationSystem.AddRuleToMask("TheresePlayer", "Enemy", "deny", -0.1f);
		relationSystem.AddRuleToMask("BillTherese", "Partner", "deny", -0.1f);
		relationSystem.AddRuleToMask("BillJohn", "Enemy", "deny", -0.1f);
		relationSystem.AddRuleToMask("BillHeather", "Friend", "deny", -0.1f);
		relationSystem.AddRuleToMask("BillPlayer", "Enemy", "deny", -0.1f);

		relationSystem.AddRuleToMask("JohnHeather", "Partner", "demandtostopbeingfriendwith", -0.4f);
		relationSystem.AddRuleToMask("JohnBill", "Enemy", "demandtostopbeingfriendwith", -0.4f);
		relationSystem.AddRuleToMask("JohnTherese", "Enemy", "demandtostopbeingfriendwith", -0.4f);
		relationSystem.AddRuleToMask("JohnPlayer", "Friend", "demandtostopbeingfriendwith", -0.4f);
		relationSystem.AddRuleToMask("HeatherJohn", "Partner", "demandtostopbeingfriendwith", -0.4f);
		relationSystem.AddRuleToMask("HeatherPlayer", "Partner", "demandtostopbeingfriendwith", -0.4f);
		relationSystem.AddRuleToMask("HeatherBill", "Friend", "demandtostopbeingfriendwith",  -0.4f);
		relationSystem.AddRuleToMask("HeatherTherese", "Friend", "demandtostopbeingfriendwith", -0.4f);
		relationSystem.AddRuleToMask("ThereseBill", "Partner", "demandtostopbeingfriendwith", -0.4f);
		relationSystem.AddRuleToMask("ThereseJohn", "Enemy", "demandtostopbeingfriendwith", -0.4f);
		relationSystem.AddRuleToMask("ThereseHeather", "Friend", "demandtostopbeingfriendwith", -0.4f);
		relationSystem.AddRuleToMask("TheresePlayer", "Enemy", "demandtostopbeingfriendwith", -0.4f);
		relationSystem.AddRuleToMask("BillTherese", "Partner", "demandtostopbeingfriendwith", -0.4f);
		relationSystem.AddRuleToMask("BillJohn", "Enemy", "demandtostopbeingfriendwith", -0.4f);
		relationSystem.AddRuleToMask("BillHeather", "Friend", "demandtostopbeingfriendwith", -0.4f);
		relationSystem.AddRuleToMask("BillPlayer", "Enemy", "demandtostopbeingfriendwith", -0.4f);

		relationSystem.AddRuleToMask("JohnHeather", "Partner", "makedistraction", -0.2f);
		relationSystem.AddRuleToMask("JohnBill", "Enemy", "makedistraction", -0.2f);
		relationSystem.AddRuleToMask("JohnTherese", "Enemy", "makedistraction", -0.2f);
		relationSystem.AddRuleToMask("JohnPlayer", "Friend", "makedistraction", -0.2f);
		relationSystem.AddRuleToMask("HeatherJohn", "Partner", "makedistraction", -0.2f);
		relationSystem.AddRuleToMask("HeatherPlayer", "Partner", "makedistraction", -0.2f);
		relationSystem.AddRuleToMask("HeatherBill", "Friend", "makedistraction",  -0.2f);
		relationSystem.AddRuleToMask("HeatherTherese", "Friend", "makedistraction", -0.2f);
		relationSystem.AddRuleToMask("ThereseBill", "Partner", "makedistraction", -0.2f);
		relationSystem.AddRuleToMask("ThereseJohn", "Enemy", "makedistraction", -0.2f);
		relationSystem.AddRuleToMask("ThereseHeather", "Friend", "makedistraction", -0.2f);
		relationSystem.AddRuleToMask("TheresePlayer", "Enemy", "makedistraction", -0.2f);
		relationSystem.AddRuleToMask("BillTherese", "Partner", "makedistraction", -0.2f);
		relationSystem.AddRuleToMask("BillJohn", "Enemy", "makedistraction", -0.2f);
		relationSystem.AddRuleToMask("BillHeather", "Friend", "makedistraction", -0.2f);
		relationSystem.AddRuleToMask("BillPlayer", "Enemy", "makedistraction", -0.2f);

		relationSystem.AddRuleToMask("JohnHeather", "Partner", "reminisce", 0.2f);
		relationSystem.AddRuleToMask("JohnPlayer", "Friend", "reminisce", 0.2f);
		relationSystem.AddRuleToMask("HeatherJohn", "Partner", "reminisce", 0.2f);
		relationSystem.AddRuleToMask("HeatherPlayer", "Partner", "reminisce", 0.2f);
		relationSystem.AddRuleToMask("HeatherBill", "Friend", "reminisce",  0.2f);
		relationSystem.AddRuleToMask("HeatherTherese", "Friend", "reminisce", 0.2f);
		relationSystem.AddRuleToMask("ThereseBill", "Partner", "reminisce", 0.2f);
		relationSystem.AddRuleToMask("ThereseHeather", "Friend", "reminisce", 0.2f);
		relationSystem.AddRuleToMask("BillTherese", "Partner", "reminisce", 0.2f);
		relationSystem.AddRuleToMask("BillHeather", "Friend", "reminisce", 0.2f);

		relationSystem.AddRuleToMask("JohnHeather", "Partner", "enthuseaboutgreatnessofperson", 0.4f);
		relationSystem.AddRuleToMask("JohnPlayer", "Friend", "enthuseaboutgreatnessofperson", 0.2f);
		relationSystem.AddRuleToMask("HeatherJohn", "Partner", "enthuseaboutgreatnessofperson", 0.4f);
		relationSystem.AddRuleToMask("HeatherPlayer", "Partner", "enthuseaboutgreatnessofperson", 0.4f);
		relationSystem.AddRuleToMask("HeatherBill", "Friend", "enthuseaboutgreatnessofperson",  0.2f);
		relationSystem.AddRuleToMask("HeatherTherese", "Friend", "enthuseaboutgreatnessofperson", 0.2f);
		relationSystem.AddRuleToMask("ThereseBill", "Partner", "enthuseaboutgreatnessofperson", 0.4f);
		relationSystem.AddRuleToMask("ThereseHeather", "Friend", "enthuseaboutgreatnessofperson", 0.2f);
		relationSystem.AddRuleToMask("BillTherese", "Partner", "enthuseaboutgreatnessofperson", 0.4f);
		relationSystem.AddRuleToMask("BillHeather", "Friend", "enthuseaboutgreatnessofperson", 0.2f);


	// CULTURE
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "fight", -0.5f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "bribefbunsant", -0.1f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "steal", -0.5f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "practicestealing", -0.3f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "askforhelpinillicitactivity", -0.1f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "poisonfbunsant", -0.8f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "greetfbunsant", 0.5f);


		relationSystem.AddRuleToMask("Bungary", "Bunce", "bribefbunce", 0.3f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "convictfbunce", 0.0f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "argueinnocencefbunce", 0.0f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "argueguiltinessfbunce", 0.0f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "searchforthieffbunce", 0.8f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "poisonfbunce", -0.8f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "greetfbunce", 1.0f);

		relationSystem.AddRuleToMask("Bungary", "Buncess", "bribefcess", 0.3f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "convictfcess", 0.0f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "argueinnocencefcess", 0.2f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "argueguiltinessfcess", -0.1f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "searchforthieffcess", 0.3f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "poisonfcess", -0.8f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "greetfcess", 1.0f);

		relationSystem.AddRuleToMask("Cult", "Leader", "praisecultfleader", 0.6f);
		relationSystem.AddRuleToMask("Cult", "Follower", "praisecultffollower", 0.4f);
		relationSystem.AddRuleToMask("Cult", "Follower", "entercult", 0.3f);
		relationSystem.AddRuleToMask("Cult", "Follower", "exitcult", -0.8f);
		relationSystem.AddRuleToMask("Cult", "Follower", "damncult", -0.4f);
		relationSystem.AddRuleToMask("Cult", "Leader", "excommunicatefromcult", 0.1f);

		relationSystem.AddRuleToMask("MerchantGuild", "Member", "buycompany", 0.2f);
		relationSystem.AddRuleToMask("MerchantGuild", "Member", "sabotage", -0.5f);
		relationSystem.AddRuleToMask("MerchantGuild", "Member", "advertise", 0.5f);
		relationSystem.AddRuleToMask("MerchantGuild", "Member", "convincetoleaveguild", -0.1f);
		relationSystem.AddRuleToMask("MerchantGuild", "Member", "demandtoleaveguild", -0.3f);
		relationSystem.AddRuleToMask("MerchantGuild", "Member", "askforhelp", 0.2f);
		relationSystem.AddRuleToMask("MerchantGuild", "Member", "sellcompany", -0.5f);
		relationSystem.AddRuleToMask("MerchantGuild", "Member", "buygoods", 0.5f);
		relationSystem.AddRuleToMask("MerchantGuild", "Member", "sellgoods", 0.7f);



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


	void UpdateStats(){
		statsString = "";

		foreach (Person p in relationSystem.pplAndMasks.people.Values) {
			if(p.name != "player"){
				statsString += p.name+"\n";
				statsString += "AngFea: "+p.moods[MoodTypes.angryFear]+"\n";
				statsString += "aroDis: "+p.moods[MoodTypes.arousDisgus]+"\n";
				statsString += "EnrTir: "+p.moods[MoodTypes.energTired]+"\n";
				statsString += "HapSad: "+p.moods[MoodTypes.hapSad]+"\n";
			}
			statsString += "\n";
		}
		statsString += "Money: \n";


		foreach (Being b in beings) {
			statsString +=  b.name+"  "+ b.possessions.Find(x=>x.Name == "money").value+"\n";
		}

	}





}
