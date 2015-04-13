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
		roomMan = new RoomManager(relationSystem);

		relationSystem.AddUpdateList ("Indgang");
		relationSystem.AddUpdateList ("Stue");
		relationSystem.AddUpdateList ("Gang");
		relationSystem.AddUpdateList ("Køkken");
		relationSystem.AddUpdateList ("Jail");
	}


	public void CreateFirstBeings()
	{
		Being Bill = new Being ("Bill", relationSystem);
		Being Therese = new Being ("Therese", relationSystem);
		Being John = new Being ("John", relationSystem);
		Being Heather = new Being ("Heather", relationSystem);
		Being Player = new Being ("Player", relationSystem);

		roomMan.EnterRoom ("Indgang", relationSystem.pplAndMasks.GetPerson("Bill"));
        roomMan.EnterRoom("Indgang", relationSystem.pplAndMasks.GetPerson("Therese"));
        roomMan.EnterRoom("Indgang", relationSystem.pplAndMasks.GetPerson("John"));
		roomMan.EnterRoom("Indgang", relationSystem.pplAndMasks.GetPerson("Heather"));
        roomMan.EnterRoom("Indgang", relationSystem.pplAndMasks.GetPerson("Player"));

        relationSystem.AddListToActives("Indgang");
		relationSystem.AddListToActives("Stue");
		relationSystem.AddListToActives("Køkken");

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
		Heather.possessions.Add (new Game ("StarCraft"));

		foreach (Being b in beings) {
			b.name = b.name.ToLower();
		}


	}


	public void CreateFirstMasks()
	{
		relationSystem.CreateNewMask("Player", new float[]{}, TypeMask.selfPerc, new string[]{});

		relationSystem.CreateNewMask("Bungary", new float[] { 0.0f, -0.2f }, TypeMask.culture, new string[] { "Bunce", "Buncess", "Bunsant" });
		relationSystem.CreateNewMask("Cult", new float[] { 0.0f, -0.2f }, TypeMask.culture, new string[] { "Leader", "Follower", "Skeptic" });
		relationSystem.CreateNewMask("MerchantGuild", new float[] { 0.0f, -0.2f }, TypeMask.culture, new string[] { "Member" });

		relationSystem.CreateNewMask("Bill", new float[] { 0.0f, 0.0f }, TypeMask.selfPerc, new string[] { "" });
		relationSystem.CreateNewMask("Therese", new float[] { 0.0f, 0.0f }, TypeMask.selfPerc, new string[] { "" });
		relationSystem.CreateNewMask("John", new float[] { 0.0f, 0.0f }, TypeMask.selfPerc, new string[] { "" });
		relationSystem.CreateNewMask("Heather", new float[] { 0.0f, 0.0f }, TypeMask.selfPerc, new string[] { "" });
		
		relationSystem.CreateNewMask("BillTherese", new float[] { 0.2f, -0.2f }, TypeMask.interPers, new string[] { "Partner" });
		relationSystem.CreateNewMask("ThereseBill", new float[] { 0.3f, 0.2f }, TypeMask.interPers, new string[] { "Partner" });
		
		relationSystem.CreateNewMask("BillJohn", new float[] { -0.2f, -0.2f }, TypeMask.interPers, new string[] { "Enemy" });
		relationSystem.CreateNewMask("JohnBill", new float[] { -0.2f, -0.2f }, TypeMask.interPers, new string[] { "Enemy" });
		
		relationSystem.CreateNewMask("JohnTherese", new float[] { 0.2f, -0.2f }, TypeMask.interPers, new string[] { "Enemy" });
		relationSystem.CreateNewMask("ThereseJohn", new float[] { -0.2f, -0.2f }, TypeMask.interPers, new string[] { "Enemy" });

		relationSystem.CreateNewMask("BillHeather", new float[] { 0.4f, 0.4f }, TypeMask.interPers, new string[] { "Friend" });
		relationSystem.CreateNewMask("HeatherBill", new float[] { 0.2f, 0.5f }, TypeMask.interPers, new string[] { "Friend" });

		relationSystem.CreateNewMask("HeatherTherese", new float[] { 0.2f, 0.2f }, TypeMask.interPers, new string[] { "Friend" });
		relationSystem.CreateNewMask("ThereseHeather", new float[] { 0.2f, 0.0f }, TypeMask.interPers, new string[] { "Friend" });

		relationSystem.CreateNewMask("JohnHeather", new float[] { 0.6f, 0.0f }, TypeMask.interPers, new string[] { "Partner" });
		relationSystem.CreateNewMask("HeatherJohn", new float[] { 0.2f, 0.0f }, TypeMask.interPers, new string[] { "Partner" });

		relationSystem.CreateNewMask("BillPlayer", new float[] { -0.4f, 0.0f }, TypeMask.interPers, new string[] { "Partner" });
		relationSystem.CreateNewMask("HeatherPlayer", new float[] { 0.5f, 0.0f }, TypeMask.interPers, new string[] { "Partner" });
		relationSystem.CreateNewMask("TheresePlayer", new float[] { -0.2f, 0.0f }, TypeMask.interPers, new string[] { "Enemy" });
		relationSystem.CreateNewMask("JohnPlayer", new float[] { 0.2f, 0.0f }, TypeMask.interPers, new string[] { "Friend" });

		relationSystem.CreateNewMask("PlayerBill", new float[] { -0.4f, 0.0f }, TypeMask.interPers, new string[] { "Enemy" });
		relationSystem.CreateNewMask("PlayerHeather", new float[] { 0.5f, 0.0f }, TypeMask.interPers, new string[] { "Partner" });
		relationSystem.CreateNewMask("PlayerTherese", new float[] { -0.2f, 0.0f }, TypeMask.interPers, new string[] { "Enemy" });
		relationSystem.CreateNewMask("PlayerJohn", new float[] { 0.2f, 0.0f }, TypeMask.interPers, new string[] { "Friend" });

		relationSystem.CreateNewMask("RomanticRelationship", new float[] { 0.2f, 0.0f, 0.0f }, TypeMask.interPers, new string[] { "Partner" });
		relationSystem.CreateNewMask("Friendship", new float[] { 0.1f, 0.2f, 0.0f }, TypeMask.interPers, new string[] { "Friend" });
		relationSystem.CreateNewMask("Rivalry", new float[] { -0.2f, -0.2f, 0.2f }, TypeMask.interPers, new string[] { "Enemy" });
	}



	public void CreateFirstPeople()
	{
		#region adding Conditions

// ------------------------------------------------------------------------------------------------------------------------------------------------ CREATING CONDITIONS

// --------------------------- INTERPERSONAL RULE CONDITIONS
		RuleConditioner emptyCondition = (self, other, indPpl) => { 
			//UIFunctions.WriteGameLine("PassedCorrectly ");
			return true;
		};
		
		RuleConditioner GreetCondition = (self, other, indPpl) =>
		{ if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["greet"] && x.GetSubject()==self && x.GetDirect()==other))
			{ return false; }

		  if(self != other && roomMan.IsPersonInSameRoomAsMe(self, other)){
				if(self.GetOpinionValue(TraitTypes.NiceNasty,other) > -0.5f){
					return true;
				} else{ return false; }
			}
			else{
				return false;
			}
		};

		RuleConditioner fleeCondition = (self, other, indPpl) =>
		{	if(self.moods[MoodTypes.angryFear] < -0.9f){
					return true; 
				}
			return false; };

		RuleConditioner kissCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.roleRef.Exists(y=>y.name == other.name && y.interPersonal.Exists(z=>z.roleName == "partner" && z.roleRef.Exists(s=>s.name == self.name))) && x.roleName == "partner")){
				if(self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.4f && self.moods[MoodTypes.arousDisgus] > 0.4f  && roomMan.IsPersonInSameRoomAsMe(self, other) ){ return true; }
			}
			else{
				if (self != other  && self.moods[MoodTypes.energTired] > 0.2f)
				{ if(self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.7f && self.moods[MoodTypes.arousDisgus] > 0.5f  && roomMan.IsPersonInSameRoomAsMe(self, other) ){ return true; }}
			}					
			return false;
		};

		RuleConditioner askAboutPartnerStatusCondition = (self, other, indPpl) =>
		{	if((relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["kiss"] && x.GetSubject() == other && x.GetDirect() != self)
			      && (self.interPersonal.Exists(x=>x.roleRef.Exists(y=>y.name == other.name && y.interPersonal.Exists(z=>z.roleName == "partner" && z.roleRef.Exists(s=>s.name == self.name))) && x.roleName == "partner")))
			     && roomMan.IsPersonInSameRoomAsMe(self, other)){
				return true;
			}
			if(!(self.interPersonal.Exists(x=>x.roleName=="partner")) && self != other && roomMan.IsPersonInSameRoomAsMe(self, other)){
				if(self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.7f){
					return true;
				}
			}
			return false; };

		RuleConditioner chooseAnotherAsPartnerCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.roleRef.Exists(y=>y.name == other.name && y.interPersonal.Exists(z=>z.roleName == "partner" && z.roleRef.Exists(s=>s.name == self.name))) && x.roleName == "partner")){
				return false;
			}

			if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["askaboutpartnerstatus"] && x.GetSubject() == other && x.GetDirect() == self && x.GetTime() < 10f)){
				if(self.interPersonal.Exists(x=>x.GetlvlOfInfl() > 0.3) && self.moods[MoodTypes.arousDisgus] > 0.3f && self != other  && roomMan.IsPersonInSameRoomAsMe(self, other)){
					if(self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.5f)
						{  return true; }
				}
			}
			if(self.interPersonal.Exists(x => x.roleName != "partner") && other.interPersonal.Exists(x => x.roleName != "partner")){
				if(self.interPersonal.Exists(x=>x.GetlvlOfInfl() > 0.5f) && self.moods[MoodTypes.arousDisgus] > 0.3f && self != other && roomMan.IsPersonInSameRoomAsMe(self, other)){
					if(self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.5f)
						{  return true; }
				}
			}
			return false; };

		RuleConditioner stayAsPartnerCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["askaboutpartnerstatus"] && x.GetSubject() == other && x.GetDirect() == self && x.GetTime() < 10f)){
				if(self.interPersonal.Exists(x=>x.GetlvlOfInfl() > 0.3) && self.interPersonal.Exists(x => x.roleName == "partner") && other.interPersonal.Exists(x => x.roleName == "partner") && self != other
				   && roomMan.IsPersonInSameRoomAsMe(self, other)){
					if(self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.5f)
						{ return true; }
				}
			}
			return false; };

		RuleConditioner LeavePartnerCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.roleRef.Exists(y=>y.name == other.name && y.interPersonal.Exists(z=>z.roleName == "partner" && z.roleRef.Exists(s=>s.name == self.name))) && x.roleName == "partner")){
				if((self.interPersonal.Exists(x=>x.GetlvlOfInfl() < 0.3f) && self != other) && roomMan.IsPersonInSameRoomAsMe(self, other)){
					return true;
				}
			}
			return false; };

		RuleConditioner flirtCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.GetlvlOfInfl() > 0.5) && self.interPersonal.Exists(x => x.roleName != "partner") && 
			     other.interPersonal.Exists(x => x.roleName != "partner") && self.moods[MoodTypes.arousDisgus] > 0.1f  && self != other && roomMan.IsPersonInSameRoomAsMe(self, other))
			{ if(self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.3f) 
				return true;}
			return false; };

		RuleConditioner chatCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.GetlvlOfInfl() >= 0.2) && self.moods[MoodTypes.hapSad] >= -0.2f && self != other &&
			     self.moods[MoodTypes.energTired] > -0.4f && roomMan.IsPersonInSameRoomAsMe(self, other)){
				if(self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.0f){
					return true;
				}
			}
			return false; };

		RuleConditioner giveGiftCondition = (self, other, indPpl) =>
		{	
			if(self.interPersonal.Exists(x=>x.GetlvlOfInfl() > 0.5f) && (self.interPersonal.Exists(x=>x.roleRef.Exists(y=>y.name == other.name)))  &&
			   self.moods[MoodTypes.energTired] > -0.4f && self != other && roomMan.IsPersonInSameRoomAsMe(self, other)){
				if(beings.Find(x=>x.name == self.name).possessions.Exists(y=>y.Name=="game" || y.Name=="company") && self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.4f){
					if(beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="game" || y.Name=="company").value > 0f){
						return true;
					}

				}
			}
			return false; };

		RuleConditioner poisonCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.GetlvlOfInfl() < 0.5f) && (self.interPersonal.Exists(x=>x.roleRef.Exists(y=>y.name == other.name))) && self.moods[MoodTypes.angryFear] < -0.7f
			     && self != other  && roomMan.IsPersonInSameRoomAsMe(self, other)){
				if(self.GetOpinionValue(TraitTypes.NiceNasty,other) < -0.3f)
				return true;
			}
			return false; };

		RuleConditioner gossipCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.GetlvlOfInfl() > 0.1f) && (self.interPersonal.Exists(x=>x.roleRef.Exists(y=>y.name == other.name))) && self != other  &&
			     self.moods[MoodTypes.energTired] > -0.4f && roomMan.IsPersonInSameRoomAsMe(self, other)){
				return true;
			}
			return false; };

		RuleConditioner argueCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.GetlvlOfInfl() > 0.1f) && (self.interPersonal.Exists(x=>x.roleRef.Exists(y=>y.name == other.name))) && self != other){
				if(self.GetOpinionValue(TraitTypes.NiceNasty,other) < 0.0f || self.moods[MoodTypes.angryFear] < -0.1f && roomMan.IsPersonInSameRoomAsMe(self, other))
					return true;
			}
			return false; };

		//RuleConditioner demandToStopBeingFriendWithCondition = (self, other, indPpl) =>
		//{	
			//PROBABLY NEED OPINIONS FOR THIS ONE
			//foreach(Person p in indPpl){
			//	if( p.interPersonal.Exists(x=>x.GetlvlOfInfl() < 0.3))
			//	   { return true; }
			//}
			//return false; };
		
		RuleConditioner makeDistractionCondition = (self, other, indPpl) =>
		{	
		    if(self.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue() < 0.0f  && self.moods[MoodTypes.energTired] > -0.4f && roomMan.IsPersonInSameRoomAsMe(self, other))
				if(self.GetOpinionValue(TraitTypes.NiceNasty,other) < 0.0f)
					{ return true; }
			return false; };

		RuleConditioner reminisceCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["chat"] && x.GetSubject() == other && 
			     self.interPersonal.Exists(z=>z.GetlvlOfInfl() > 0.4f) && (self.interPersonal.Exists(y=>y.roleRef.Exists(yy=>yy.name == other.name)))) && self != other  &&
			     self.moods[MoodTypes.energTired] > -0.4f && roomMan.IsPersonInSameRoomAsMe(self, other))
				if(self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.2f)
					{ return true; }
			return false; };

		RuleConditioner denyCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.GetlvlOfInfl() < 0.8f) && (self.interPersonal.Exists(x=>x.roleRef.Exists(y=>y.name == other.name))) && 
			     self != other && roomMan.IsPersonInSameRoomAsMe(self, other))
				if((self.moods[MoodTypes.arousDisgus] < 0.0f ||
				    self.moods[MoodTypes.hapSad] < 0.0f ||
				    self.moods[MoodTypes.angryFear] < 0.0f ||
				    self.GetOpinionValue(TraitTypes.NiceNasty,other) < -0.4f))
					{ return true; }
			return false; };

		RuleConditioner enthuseAboutGreatnessofPersonCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.GetlvlOfInfl() > 0.5f) && (self.interPersonal.Exists(x=>x.roleRef.Exists(y=>y.name == other.name))) && self.moods[MoodTypes.hapSad] > 0.0f && roomMan.IsPersonInSameRoomAsMe(self, other)) 
				if(self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.6f)
					{ return true; }
			return false; };


// --------------------------------- CULTURAL CONDITIONS
		
		RuleConditioner convictCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["steal"] && x.GetDirect()==self && (x.GetSubject()==other && x.GetSubject()!=self)) ||
			   relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["fight"] && x.GetDirect()==self && (x.GetSubject()==other && x.GetSubject()!=self))   && roomMan.IsPersonInSameRoomAsMe(self, other))
			{	return true; }
			return false; };

		RuleConditioner fightCondition = (self, other, indPpl) =>
		{	if(self != other && roomMan.IsPersonInSameRoomAsMe(self, other)){
				if((self.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue() < 0.0 || self.moods[MoodTypes.angryFear] < -0.7f) && self.GetOpinionValue(TraitTypes.NiceNasty,other) < -0.5f  &&
				   self.moods[MoodTypes.energTired] > -0.6f){
					return true; 
				}
			}
			return false; };
		
		RuleConditioner bribeCondition = (self, other, indPpl) =>
		{	//MONEY
			if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["convict"] && x.GetDirect()==self && x.GetSubject()==other)  && self != other && roomMan.IsPersonInSameRoomAsMe(self, other)){
				return true; 
			}
			return false; };

		RuleConditioner argueInnocenceCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["convict"] && x.GetSubject()==other) && self.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue() > 0.0f
			     && self != other && roomMan.IsPersonInSameRoomAsMe(self, other))
				{ return true; }
			return false; };

		RuleConditioner argueGuiltinessCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["convict"] && x.GetSubject()==other) && self.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue() < 0.0f
			     && self != other && roomMan.IsPersonInSameRoomAsMe(self, other))
				{ return true; }
			return false; };

		RuleConditioner stealCondition = (self, other, indPpl) =>
		{	if(self.moods[MoodTypes.hapSad] < -0.3f  && self != other  &&
			   self.moods[MoodTypes.energTired] > -0.4f && beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="money").value <= 50f
			   && beings.Find(x=>x.name == other.name).possessions.Find(y=>y.Name=="money").value >= 50f)
				  { return true; }
			return false; };

	//	RuleConditioner practiceStealingCondition = (self, other, indPpl) =>
	//	{	if(self.moods[MoodTypes.hapSad] < -0.2f && self.GetAbilityy() < 1.0f  &&
	//		     self.moods[MoodTypes.energTired] > -0.4f){ return true; }
	//		return false; };

	/*	RuleConditioner askForHelpInIllicitActivityCondition = (self, other, indPpl) =>
		{	if(self.moods[MoodTypes.angryFear] < -0.2f && self.GetAbilityy() < 1.0f && self != other && roomMan.IsPersonInSameRoomAsMe(self, other))
			if(self.GetOpinionValue(TraitTypes.HonestFalse,other) > 0.4f)
				{ return true; }
			return false; };
*/
	/*	RuleConditioner searchForThiefCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["steal"]) && self != other  &&
			     self.moods[MoodTypes.energTired] > -0.4f) { return true; }
			return false; };*/

		RuleConditioner makefunofCondition = (self, other, indPpl) =>
		{	if(self != other  && self.moods[MoodTypes.energTired] > -0.6f && self.moods[MoodTypes.hapSad] > -0.8f && self.moods[MoodTypes.hapSad] < 0.0f && roomMan.IsPersonInSameRoomAsMe(self, other)) 
			{ return true; }
			return false; };

		RuleConditioner telljokeCondition = (self, other, indPpl) =>
		{	if(self != other  && self.moods[MoodTypes.energTired] > -0.2f && self.moods[MoodTypes.hapSad] > -0.2f && roomMan.IsPersonInSameRoomAsMe(self, other)) 
			{ return true; }
			return false; };

		RuleConditioner prankCondition = (self, other, indPpl) =>
		{	if(self != other  && self.moods[MoodTypes.energTired] > -0.5f && self.moods[MoodTypes.hapSad] > -0.4f && roomMan.IsPersonInSameRoomAsMe(self, other)) 
			{ if(self.GetOpinionValue(TraitTypes.NiceNasty,other) < 0.0f) {return true;} }
			return false; };

		RuleConditioner harassCondition = (self, other, indPpl) =>
		{	if(self != other  && self.moods[MoodTypes.energTired] > -0.7f && self.moods[MoodTypes.hapSad] > -0.8f && self.moods[MoodTypes.hapSad] < 0.0f && roomMan.IsPersonInSameRoomAsMe(self, other)) 
			{ if(self.GetOpinionValue(TraitTypes.NiceNasty,other) < 0.0f) {return true;} }
			return false; };

		RuleConditioner playgameCondition = (self, other, indPpl) =>
		{	if(beings.Find(x=>x.name == self.name).possessions.Exists(y=>y.Name=="game") && roomMan.IsPersonInSameRoomAsMe(self, other)){
				if(beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="game").value > 0){
					if(self != other  && self.moods[MoodTypes.energTired] > -0.2f) 
					{ if(self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.4f) {return true;} }
				}
			}
			return false; };

		RuleConditioner orderCondition = (self, other, indPpl) =>
		{	if(self != other && 
			     (self.interPersonal.Exists(x => x.roleName != "bunce") || self.interPersonal.Exists(x => x.roleName != "buncess")) && 
			     other.interPersonal.Exists(x => x.roleName != "bunsant") && roomMan.IsPersonInSameRoomAsMe(self, other))
			{ if(self.GetOpinionValue(TraitTypes.NiceNasty,other) < 0.4f) {return true;} }
			return false; };

		RuleConditioner killCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["steal"] && x.GetDirect()==self && (x.GetSubject()==other && x.GetSubject()!=self)) ||
			     relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["fight"] && x.GetDirect()==self && (x.GetSubject()==other && x.GetSubject()!=self))   && roomMan.IsPersonInSameRoomAsMe(self, other))
			{	return true; }

			if(self != other){
				if(self.GetOpinionValue(TraitTypes.NiceNasty,other) < -0.7 && self.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue() < -0.6){
					return true;
				}
			}

			return false; };


/*
// -------------- CULTURAL (CULT) ACTIONS

		RuleConditioner PraiseCultCondition = (self, other, indPpl) =>
		{	if(self.culture.Exists(x=>x.GetlvlOfInfl() > 0.5f && x.roleName == "follower" || x.roleName == "leader")  &&
			     self.moods[MoodTypes.energTired] > -0.4f && other.culture.Exists(x=>x.roleMask.GetMaskName() == "cult" && (x.roleName == "follower" || x.roleName == "leader")) && self != other) 
				{ return true; }
			if(self.culture.Exists(x=>x.GetlvlOfInfl() > 0.5f && x.roleName == "follower" || x.roleName == "leader")  &&
			   self.moods[MoodTypes.energTired] > -0.4f && other.culture.Exists(x=>x.roleMask.GetMaskName() == "cult" && (x.roleName != "follower" || x.roleName != "leader")) && 
			   self != other && self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.5f)
				{ return true; }

			return false; };

		RuleConditioner enterCultCondition = (self, other, indPpl) =>
		{	if(self.culture.Exists(x=>x.roleRef.Exists(y=>y.name=="cult") && (x.roleName != "follower" || x.roleName != "leader") && x.GetlvlOfInfl() > 0.5f)) { return true; }
			return false; };

		RuleConditioner exitCultCondition = (self, other, indPpl) =>
		{	if(self.culture.Exists(x=>x.roleRef.Exists(y=>y.name=="cult") && (x.roleName == "follower" || x.roleName == "leader") && x.GetlvlOfInfl() < 0.1f)) { return true; }
			return false; };

		RuleConditioner damnCultCondition = (self, other, indPpl) =>
		{	if(self.culture.Exists(x=>x.roleRef.Exists(y=>y.name=="cult") && x.GetlvlOfInfl() < 0.4f) && self != other) { return true; }
			return false; };

		RuleConditioner excommunicateFromCultCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["damncult"] && x.GetSubject() == other) && self != other) { return true; }
			return false; };*/

// --------------- CULTURAL (MERCHANT) ACTIONS

		RuleConditioner buyCompanyCondition = (self, other, indPpl) =>
		{	if(beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="money").value >= 100f && beings.Find(x=>x.name == other.name).possessions.Find(y=>y.Name=="company").value >= 1f
			     && self != other && roomMan.IsPersonInSameRoomAsMe(self, other)) { return true; }
			return false; };

		RuleConditioner sellCompanyCondition = (self, other, indPpl) =>
		{	if(beings.Find(x=>x.name == other.name).possessions.Find(y=>y.Name=="money").value >= 100f && beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="company").value >= 1f
			     && self != other && roomMan.IsPersonInSameRoomAsMe(self, other)) { return true; }
			return false; };

		RuleConditioner sabotageCondition = (self, other, indPpl) =>
		{	if(self.moods[MoodTypes.angryFear] > 0.5f || self.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue() < -0.5f && self != other  &&
			     self.moods[MoodTypes.energTired] > -0.4f) 
				if(self.GetOpinionValue(TraitTypes.NiceNasty,other) < -0.2f)
					{ return true; }
			return false; };

		RuleConditioner advertiseCondition = (self, other, indPpl) =>
		{	if(self != other  && self.moods[MoodTypes.energTired] > -0.4f && roomMan.IsPersonInSameRoomAsMe(self, other)) { return true; }
			return false; };

	//	RuleConditioner convinceToLeaveGuildCondition = (self, other, indPpl) =>
	//	{	if(self != other  &&  self.moods[MoodTypes.energTired] > -0.4f && beings.Find(x=>x.name == other.name).possessions.Find(y=>y.Name=="money").value <= 0f && roomMan.IsPersonInSameRoomAsMe(self, other)){
	//			if(self.GetOpinionValue(TraitTypes.NiceNasty,other) < 0)
	//				{ return true; }
	//		} 
	//		return false; };

		RuleConditioner DemandtoLeaveGuildCondition = (self, other, indPpl) =>
		{	if((self.moods[MoodTypes.angryFear] > 0.3f || self.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue() < -0.3f) && self != other &&
			    beings.Find(x=>x.name == other.name).possessions.Find(y=>y.Name=="money").value <= 0f && roomMan.IsPersonInSameRoomAsMe(self, other)){
					if(self.GetOpinionValue(TraitTypes.NiceNasty,other) < 0)
						{ return true; }
			} 
				
			return false; };

	/*	RuleConditioner askForHelpCondition = (self, other, indPpl) =>
		{	if(self != other && beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="goods").value <= 0f && roomMan.IsPersonInSameRoomAsMe(self, other)) 
				if(self.GetOpinionValue(TraitTypes.HonestFalse,other) > 0.2f)
					{ return true; }
			return false; };
*/
		RuleConditioner buyGoodsCondition = (self, other, indPpl) =>
		{	if(beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="goods").value < 2f && beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="money").value > 30f
			     && self != other && self.moods[MoodTypes.energTired] > -0.4f && roomMan.IsPersonInSameRoomAsMe(self, other)) { return true; }
			return false; };

		RuleConditioner sellGoodsCondition = (self, other, indPpl) =>
		{	if(beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="goods").value > 1f && self != other && self.moods[MoodTypes.energTired] > -0.4f && roomMan.IsPersonInSameRoomAsMe(self, other)) { return true; }
			return false; };

		RuleConditioner moveToStueCondition = (self, other, indPpl) =>
		{	
			if(relationSystem.historyBook.Find(x=> (x.GetAction()==relationSystem.posActions["movetostue"] || x.GetAction()==relationSystem.posActions["movetokøkken"] || x.GetAction()==relationSystem.posActions["movetoindgang"]) && x.GetSubject()==self).GetTime() < 10f){
				return false;
			}
			if(self != null && !(roomMan.GetRoomIAmIn(self) == "Stue"))
				if((self.moods[MoodTypes.energTired] < -0.4f)){
					return true;
				}
				else if(roomMan.GetRoomIAmIn(other) == "Stue" && self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.5f){
					return true;
				}
			return false; };

		RuleConditioner moveToIndgangCondition = (self, other, indPpl) =>
        {
			if(relationSystem.historyBook.Find(x=> (x.GetAction()==relationSystem.posActions["movetostue"] || x.GetAction()==relationSystem.posActions["movetokøkken"] || x.GetAction()==relationSystem.posActions["movetoindgang"]) && x.GetSubject()==self).GetTime() < 10f){
				return false;
			}
			if (self != null && (roomMan.GetRoomIAmIn(self) == "Stue")) { 
				if((self.moods[MoodTypes.energTired] < -0.4f)){
					return true;
				}
				else if(roomMan.GetRoomIAmIn(other) == "Indgang" && self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.5f){
					return true;
				}
			}
			return false; };

		RuleConditioner moveToKøkkenCondition = (self, other, indPpl) =>
        {
			if(relationSystem.historyBook.Find(x=> (x.GetAction()==relationSystem.posActions["movetostue"] || x.GetAction()==relationSystem.posActions["movetokøkken"] || x.GetAction()==relationSystem.posActions["movetoindgang"]) && x.GetSubject()==self).GetTime() < 10f){
					return false;
				}
            if (self != null && (roomMan.GetRoomIAmIn(self) == "Stue")) { 
				if((self.moods[MoodTypes.energTired] < -0.4f || self.moods[MoodTypes.angryFear] < -0.6f || self.moods[MoodTypes.angryFear] > 0.6f)){
					return true;
				}
				else if(roomMan.GetRoomIAmIn(other) == "Køkken" && self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.5f){
					return true;
				}
			}
			return false; };

		#endregion adding Conditions

		RulePreference doNothingPreference = (self, other) => { 
			return (-1 + Calculator.unboundAdd((-self.moods[MoodTypes.energTired]),-1));
		};

		RulePreference greetPreference = (self, other) => { 
			return self.GetOpinionValue(TraitTypes.NiceNasty,other);
		};

		RulePreference kissPreference = (self, other) => { 
			if(self.interPersonal.Exists(x=>x.roleRef.Exists(y=>y.name == other.name && y.interPersonal.Exists(z=>z.roleName == "partner" && z.roleRef.Exists(s=>s.name == self.name))) && x.roleName == "partner")){
				return self.GetOpinionValue(TraitTypes.NiceNasty,other);
			}
			else{
				float ret = 0 + Calculator.unboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),(0.5f*self.GetOpinionValue(TraitTypes.ShyBolsterous,other)));
				ret += Calculator.unboundAdd(self.GetOpinionValue(TraitTypes.HonestFalse,other),ret);
				return ret;
			}
		};

		RulePreference flirtPreference = (self, other) => { 
			return self.GetOpinionValue(TraitTypes.NiceNasty,other)*self.moods[MoodTypes.arousDisgus]*Calculator.NegPosTransform(self.interPersonal.Find(x=>x.roleRef.Exists(y=>y==other)).GetlvlOfInfl());
		};

		RulePreference chatPreference = (self, other) => { 
			return self.GetOpinionValue(TraitTypes.NiceNasty,other)*Calculator.NegPosTransform(self.interPersonal.Find(x=>x.roleRef.Exists(y=>y==other)).GetlvlOfInfl());
		};

		RulePreference giveGiftPreference = (self, other) => { 
			return self.GetOpinionValue(TraitTypes.NiceNasty,other)*(0.5f*self.GetOpinionValue(TraitTypes.ShyBolsterous,other)*self.GetOpinionValue(TraitTypes.HonestFalse,other))*self.interPersonal.Find(x=>x.roleRef.Exists(y=>y==other)).GetlvlOfInfl();
		};

		RulePreference poisonPreference = (self, other) => { 
			return -(self.GetOpinionValue(TraitTypes.NiceNasty,other)*self.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue());
		};

		RulePreference gossipPreference = (self, other) => { 
			return self.GetOpinionValue(TraitTypes.NiceNasty,other)*(-self.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue()*self.absTraits.traits[TraitTypes.HonestFalse].GetTraitValue());
		};

		RulePreference arguePreference = (self, other) => { 
			return -(self.GetOpinionValue(TraitTypes.NiceNasty,other)*self.GetOpinionValue(TraitTypes.HonestFalse,other)*self.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue());
		};

		RulePreference reminiscePreference = (self, other) => { 
			return self.GetOpinionValue(TraitTypes.NiceNasty,other)*Calculator.NegPosTransform(self.interPersonal.Find(x=>x.roleRef.Exists(y=>y==other)).GetlvlOfInfl()*(-self.moods[MoodTypes.energTired])*self.moods[MoodTypes.hapSad]);
		};

		RulePreference denyPreference = (self, other) => { 
			return -(self.GetOpinionValue(TraitTypes.NiceNasty,other)*self.GetOpinionValue(TraitTypes.HonestFalse,other)*Calculator.NegPosTransform(self.interPersonal.Find(x=>x.roleRef.Exists(y=>y==other)).GetlvlOfInfl()));
		};

		RulePreference enthuseAboutGreatnessOfPersonPreference = (self, other) => { 
			return (self.GetOpinionValue(TraitTypes.NiceNasty,other)*self.GetOpinionValue(TraitTypes.HonestFalse,other)*Calculator.NegPosTransform(self.interPersonal.Find(x=>x.roleRef.Exists(y=>y==other)).GetlvlOfInfl()));
		};

		RulePreference askAboutPartnerStatusPreference = (self, other) => { 
			return (self.GetOpinionValue(TraitTypes.NiceNasty,other)*Calculator.NegPosTransform(self.interPersonal.Find(x=>x.roleRef.Exists(y=>y==other)).GetlvlOfInfl()));
		};

		RulePreference chooseAnotherAsPartnerPreference = (self, other) => { 
			return (self.GetOpinionValue(TraitTypes.NiceNasty,other)*self.GetOpinionValue(TraitTypes.HonestFalse,other)*Calculator.NegPosTransform(self.interPersonal.Find(x=>x.roleRef.Exists(y=>y==other)).GetlvlOfInfl())*
			        self.moods[MoodTypes.arousDisgus]);
		};

		RulePreference StayAsPartnerPreference = (self, other) => { 
			return (self.GetOpinionValue(TraitTypes.NiceNasty,other)*self.GetOpinionValue(TraitTypes.HonestFalse,other)*Calculator.NegPosTransform(self.interPersonal.Find(x=>x.roleRef.Exists(y=>y==other)).GetlvlOfInfl()));
		};

		RulePreference LeavePartnerPreference = (self, other) => { 
			debug.Write(self.GetOpinionValue(TraitTypes.NiceNasty,other)+"   "+self.GetOpinionValue(TraitTypes.HonestFalse,other)+"   "+self.interPersonal.Find(x=>x.roleRef.Exists(y=>y==other)).GetlvlOfInfl());
			return -(self.GetOpinionValue(TraitTypes.NiceNasty,other)*self.GetOpinionValue(TraitTypes.HonestFalse,other)*Calculator.NegPosTransform(self.interPersonal.Find(x=>x.roleRef.Exists(y=>y==other)).GetlvlOfInfl()));
		};

		RulePreference convictPreference = (self, other) => { 
			return -Calculator.unboundAdd(-0.5f,(self.GetOpinionValue(TraitTypes.NiceNasty,other)*(self.moods[MoodTypes.hapSad]*0.5f)));
		};

		RulePreference fightPreference = (self, other) => { 
			return -(self.GetOpinionValue(TraitTypes.NiceNasty,other)*self.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue()*self.moods[MoodTypes.angryFear]);
		};

		RulePreference bribePreference = (self, other) => { 
			return (self.GetOpinionValue(TraitTypes.HonestFalse,other)*-(self.absTraits.traits[TraitTypes.HonestFalse].GetTraitValue()*self.moods[MoodTypes.angryFear]*self.moods[MoodTypes.energTired]));
		};

		RulePreference argueInnocencePreference = (self, other) => { 
			return (self.GetOpinionValue(TraitTypes.NiceNasty,other)*self.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue()*self.GetOpinionValue(TraitTypes.HonestFalse,other));
		};

		RulePreference argueGuiltinessPreference = (self, other) => { 
			return -(self.GetOpinionValue(TraitTypes.NiceNasty,other)*self.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue()*self.GetOpinionValue(TraitTypes.HonestFalse,other));
		};

		RulePreference stealPreference = (self, other) => { 
			return -(self.GetOpinionValue(TraitTypes.NiceNasty,other)*self.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue());
		};

	//	RulePreference practiceStealingPreference = (self, other) => { 
	//		return self.absTraits.traits[TraitTypes.ShyBolsterous].GetTraitValue()*self.moods[MoodTypes.angryFear];
	//	};

		//MAKE FUN OF, HARASS, PRANK, PLAYGAME
		RulePreference makeFunOfPreference = (self, other) => { 
			return -(self.GetOpinionValue(TraitTypes.NiceNasty,other)*self.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue()*self.moods[MoodTypes.angryFear]*self.moods[MoodTypes.hapSad]);
		};

		RulePreference telljokePreference = (self, other) => { 
			return (self.GetOpinionValue(TraitTypes.NiceNasty,other)*self.moods[MoodTypes.hapSad]*self.moods[MoodTypes.energTired]);
		};

		RulePreference harassPreference = (self, other) => { 
			return -(self.GetOpinionValue(TraitTypes.NiceNasty,other)*self.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue()*self.moods[MoodTypes.angryFear]*self.moods[MoodTypes.hapSad]);
		};

		RulePreference prankPreference = (self, other) => { 
			return -(self.GetOpinionValue(TraitTypes.NiceNasty,other)*self.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue()*self.moods[MoodTypes.angryFear]*self.moods[MoodTypes.hapSad]);
		};

		RulePreference playgamePreference = (self, other) => { 
			return (self.GetOpinionValue(TraitTypes.NiceNasty,other)*self.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue()*self.moods[MoodTypes.hapSad]);
		};


		RulePreference orderPreference = (self, other) => { 
			return -(self.GetOpinionValue(TraitTypes.NiceNasty,other)*self.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue()*self.moods[MoodTypes.energTired]);
		};

		RulePreference killPreference = (self, other) => { 
			float ret = Calculator.unboundAdd(-self.GetOpinionValue(TraitTypes.NiceNasty,other),0);
			 	 ret += Calculator.unboundAdd(-self.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue(),ret);
			 	 ret += Calculator.unboundAdd(-self.moods[MoodTypes.angryFear],ret);
			return ret;
		};


		RulePreference buyCompanyPreference = (self, other) => { 
			return self.absTraits.traits[TraitTypes.ShyBolsterous].GetTraitValue()*(-self.GetOpinionValue(TraitTypes.NiceNasty,other));
		};

		RulePreference sellCompanyPreference = (self, other) => { 
			return -(self.absTraits.traits[TraitTypes.ShyBolsterous].GetTraitValue()*(-self.GetOpinionValue(TraitTypes.NiceNasty,other)));
		};

		RulePreference advertisePreference = (self, other) => { 
			return self.absTraits.traits[TraitTypes.ShyBolsterous].GetTraitValue()*self.moods[MoodTypes.hapSad];
		};

		RulePreference sabotagePreference = (self, other) => { 
			return self.absTraits.traits[TraitTypes.ShyBolsterous].GetTraitValue()*-(self.moods[MoodTypes.angryFear]*self.GetOpinionValue(TraitTypes.NiceNasty,other)*self.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue());
		};

		RulePreference demandoLeaveGuildPreference = (self, other) => { 
			return self.absTraits.traits[TraitTypes.ShyBolsterous].GetTraitValue()*-(self.GetOpinionValue(TraitTypes.NiceNasty,other)*self.absTraits.traits[TraitTypes.NiceNasty].GetTraitValue());
		};

		RulePreference buyGoodsPreference = (self, other) => { 
			return 0.5f*self.absTraits.traits[TraitTypes.ShyBolsterous].GetTraitValue();
		};

		RulePreference sellGoodsPreference = (self, other) => { 
			return -(0.5f*self.absTraits.traits[TraitTypes.ShyBolsterous].GetTraitValue());
		};


// ---------------------------------------------------------------------------------------------------------------------- CREATING RULES


		#region Rules
		// INTERPERSONAL RULES
		relationSystem.CreateNewRule("kiss", "kiss", kissCondition,kissPreference);
		relationSystem.CreateNewRule("chooseanotheraspartner", "chooseanotheraspartner", chooseAnotherAsPartnerCondition,chooseAnotherAsPartnerPreference);
		relationSystem.CreateNewRule("stayaspartner", "stayaspartner", stayAsPartnerCondition,StayAsPartnerPreference);
		relationSystem.CreateNewRule("leavepartner", "leavepartner", LeavePartnerCondition,LeavePartnerPreference);
		relationSystem.CreateNewRule("askAboutPartnerStatus", "askAboutPartnerStatus", askAboutPartnerStatusCondition,askAboutPartnerStatusPreference);

		relationSystem.CreateNewRule("flirt", "flirt", flirtCondition,flirtPreference);
		relationSystem.CreateNewRule("chat", "chat", chatCondition,chatPreference);
		relationSystem.CreateNewRule("givegift", "givegift", giveGiftCondition,giveGiftPreference);
		relationSystem.CreateNewRule("gossip", "gossip", gossipCondition,gossipPreference);
		relationSystem.CreateNewRule("argue", "argue", argueCondition,arguePreference);
		relationSystem.CreateNewRule("deny", "deny", denyCondition,denyPreference);
		//relationSystem.CreateNewRule("demandtostopbeingfriendwith", "demandtostopbeingfriendwith", demandToStopBeingFriendWithCondition);
		relationSystem.CreateNewRule("makedistraction", "makedistraction", makeDistractionCondition);
		relationSystem.CreateNewRule("reminisce", "reminisce", reminisceCondition,reminiscePreference);
		relationSystem.CreateNewRule("enthuseaboutgreatnessofperson", "enthuseaboutgreatnessofperson", enthuseAboutGreatnessofPersonCondition,enthuseAboutGreatnessOfPersonPreference);
		relationSystem.CreateNewRule("makefunof", "makefunof", makefunofCondition,makeFunOfPreference);
		relationSystem.CreateNewRule("telljoke", "telljoke", telljokeCondition,telljokePreference);
		relationSystem.CreateNewRule("prank", "prank", prankCondition,prankPreference);
		relationSystem.CreateNewRule("harass", "harass", harassCondition,harassPreference);


		// CULTURAL RULES
		relationSystem.CreateNewRule("greet", "greet",  GreetCondition,greetPreference);
		relationSystem.CreateNewRule("greetfbunce", "greet",  GreetCondition,greetPreference);
		relationSystem.CreateNewRule("greetfcess", "greet",  GreetCondition,greetPreference);
		relationSystem.CreateNewRule("greetfbunsant", "greet",  GreetCondition,greetPreference);
		relationSystem.CreateNewRule("convict", "convict",  convictCondition,convictPreference);
		relationSystem.CreateNewRule("convictfcess", "convict",  convictCondition,convictPreference);
		relationSystem.CreateNewRule("convictfbunce", "convict",  convictCondition,convictPreference);
		relationSystem.CreateNewRule("fight", "fight", fightCondition,fightPreference);
		relationSystem.CreateNewRule("bribe", "bribe", bribeCondition,bribePreference);
		relationSystem.CreateNewRule("bribefbunce", "bribe", bribeCondition,bribePreference);
		relationSystem.CreateNewRule("bribefcess", "bribe", bribeCondition,bribePreference);
		relationSystem.CreateNewRule("bribefbunsant", "bribe", bribeCondition,bribePreference);
		relationSystem.CreateNewRule("argueinnocence", "argueinnocence", argueInnocenceCondition,argueInnocencePreference);
		relationSystem.CreateNewRule("argueinnocencefbunce", "argueinnocence", argueInnocenceCondition,argueInnocencePreference);
		relationSystem.CreateNewRule("argueinnocencefcess", "argueinnocence", argueInnocenceCondition,argueInnocencePreference);
		relationSystem.CreateNewRule("argueguiltiness", "argueguiltiness", argueGuiltinessCondition,argueGuiltinessPreference);
		relationSystem.CreateNewRule("argueguiltinessfbunce", "argueguiltiness", argueGuiltinessCondition,argueGuiltinessPreference);
		relationSystem.CreateNewRule("argueguiltinessfcess", "argueguiltiness", argueGuiltinessCondition,argueGuiltinessPreference);
		relationSystem.CreateNewRule("steal", "steal", stealCondition,stealPreference);
		relationSystem.CreateNewRule("kill", "kill",killCondition,killPreference);
		relationSystem.CreateNewRule("killfbunsant", "kill",killCondition,killPreference);
		relationSystem.CreateNewRule("killfbunce", "kill",killCondition,killPreference);
		relationSystem.CreateNewRule("killfcess", "kill",killCondition,killPreference);
	//	relationSystem.CreateNewRule("practicestealing", "practicestealing", practiceStealingCondition,practiceStealingPreference);
	//	relationSystem.CreateNewRule("askforhelpinillicitactivity", "askforhelpinillicitactivity", askForHelpInIllicitActivityCondition);
	//	relationSystem.CreateNewRule("searchforthief", "searchforthief", searchForThiefCondition);
	//	relationSystem.CreateNewRule("searchforthieffbunce", "searchforthief", searchForThiefCondition);
	//	relationSystem.CreateNewRule("searchforthieffcess", "searchforthief", searchForThiefCondition);
		relationSystem.CreateNewRule("poison", "poison", poisonCondition,poisonPreference);
		relationSystem.CreateNewRule("poisonfbunce", "poison", poisonCondition,poisonPreference);
		relationSystem.CreateNewRule("poisonfcess", "poison", poisonCondition,poisonPreference);
		relationSystem.CreateNewRule("poisonfbunsant", "poison", poisonCondition,poisonPreference);
		relationSystem.CreateNewRule("playgame", "playgame", playgameCondition,playgamePreference);
		relationSystem.CreateNewRule("playgamefbunce", "playgame", playgameCondition,playgamePreference);
		relationSystem.CreateNewRule("playgamefcess", "playgame", playgameCondition,playgamePreference);
		relationSystem.CreateNewRule("playgamefbunsant", "playgame", playgameCondition,playgamePreference);
		relationSystem.CreateNewRule("order", "order", orderCondition,orderPreference);
		relationSystem.CreateNewRule("orderfbunce", "order", orderCondition,orderPreference);
		relationSystem.CreateNewRule("orderfcess", "order", orderCondition,orderPreference);
		relationSystem.CreateNewRule("orderfbunsant", "order", orderCondition,orderPreference);

		relationSystem.CreateNewRule("moveToStuefbunce", "moveToStue", moveToStueCondition);
		relationSystem.CreateNewRule("moveToStuefcess", "moveToStue", moveToStueCondition);
		relationSystem.CreateNewRule("moveToStuefbunsant", "moveToStue", moveToStueCondition);
		relationSystem.CreateNewRule("moveToKøkkenfbunce", "moveToKøkken", moveToKøkkenCondition);
		relationSystem.CreateNewRule("moveToKøkkenfcess", "moveToKøkken", moveToKøkkenCondition);
		relationSystem.CreateNewRule("moveToKøkkenfbunsant", "moveToKøkken", moveToKøkkenCondition);
		relationSystem.CreateNewRule("moveToIndgangfbunce", "moveToIndgang", moveToIndgangCondition);
		relationSystem.CreateNewRule("moveToIndgangfcess", "moveToIndgang", moveToIndgangCondition);
		relationSystem.CreateNewRule("moveToIndgangfbunsant", "moveToIndgang", moveToIndgangCondition);
		/*
		relationSystem.CreateNewRule("praisecult", "praisecult", PraiseCultCondition);
		relationSystem.CreateNewRule("praisecultfleader", "praisecult", PraiseCultCondition);
		relationSystem.CreateNewRule("praisecultffollower", "praisecult", PraiseCultCondition);
		relationSystem.CreateNewRule("entercult", "entercult", enterCultCondition);
		relationSystem.CreateNewRule("exitcult", "exitcult", exitCultCondition);
		relationSystem.CreateNewRule("damncult", "damncult", damnCultCondition);
		relationSystem.CreateNewRule("excommunicatefromcult", "excommunicatefromcult", excommunicateFromCultCondition);
		*/
		relationSystem.CreateNewRule("buycompany", "buycompany", buyCompanyCondition,buyCompanyPreference);
		relationSystem.CreateNewRule("sabotage", "sabotage", sabotageCondition,sabotagePreference);
		relationSystem.CreateNewRule("advertise", "advertise", advertiseCondition,advertisePreference);
	//	relationSystem.CreateNewRule("convincetoleaveguild", "convincetoleaveguild", convinceToLeaveGuildCondition);
		relationSystem.CreateNewRule("demandtoleaveguild", "demandtoleaveguild", DemandtoLeaveGuildCondition,demandoLeaveGuildPreference);
	//	relationSystem.CreateNewRule("askforhelp", "askforhelp", askForHelpCondition);
		relationSystem.CreateNewRule("sellcompany", "sellcompany", sellCompanyCondition,sellCompanyPreference);
		relationSystem.CreateNewRule("buygoods", "buygoods", buyGoodsCondition,buyCompanyPreference);
		relationSystem.CreateNewRule("sellgoods", "sellgoods", sellGoodsCondition,sellGoodsPreference);

		//SELF RULES
		relationSystem.CreateNewRule("donothing", "donothing", emptyCondition, doNothingPreference);
		relationSystem.CreateNewRule("flee", "flee", fleeCondition);


// ------------------------------------------------------------------------------------------------------------------------------------------ RULES THAT MIGHT HAPPEN (REACTION RULES)

		// ------------- INTERPERSONAL
		List<Rule> kissRulesToTrigger = new List<Rule>(); kissRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("givegift"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("kiss",kissRulesToTrigger); relationSystem.pplAndMasks.AddPossibleRulesToRule("deny",kissRulesToTrigger); 
		relationSystem.pplAndMasks.AddPossibleRulesToRule("deny",kissRulesToTrigger);

		List<Rule> askAboutPartnerStatusRulesToTrigger = new List<Rule>(); askAboutPartnerStatusRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("chooseanotheraspartner"));
		askAboutPartnerStatusRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("stayaspartner")); askAboutPartnerStatusRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("leavepartner"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("askAboutPartnerStatus",askAboutPartnerStatusRulesToTrigger);

		List<Rule> chooseanotheraspartnerRulesToTrigger = new List<Rule>(); chooseanotheraspartnerRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("kiss")); chooseanotheraspartnerRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("flirt")); 
		chooseanotheraspartnerRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("deny")); chooseanotheraspartnerRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("fight"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("chooseanotheraspartner",chooseanotheraspartnerRulesToTrigger);

		List<Rule> stayaspartnerRulesToTrigger = new List<Rule>(); stayaspartnerRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("kiss")); stayaspartnerRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("flirt")); 
		relationSystem.pplAndMasks.AddPossibleRulesToRule("stayaspartner",stayaspartnerRulesToTrigger);

		List<Rule> leavepartnerRulesToTrigger = new List<Rule>(); leavepartnerRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("deny")); leavepartnerRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("poison"));
		leavepartnerRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("fight"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("leavepartner",leavepartnerRulesToTrigger);

		List<Rule> flirtRulesToTrigger = new List<Rule>(); flirtRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("deny")); flirtRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("flirt"));
		flirtRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("kiss")); flirtRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("fight"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("flirt",flirtRulesToTrigger);

		List<Rule> chatRulesToTrigger = new List<Rule>(); chatRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("chat")); chatRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("deny"));
		chatRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("gossip")); chatRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("flirt")); chatRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("askAboutPartnerStatus"));
		chatRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("reminisce")); chatRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("chat",chatRulesToTrigger);

		List<Rule> giveGiftRulesToTrigger = new List<Rule>(); giveGiftRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("reminisce")); giveGiftRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("flirt"));
		giveGiftRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("deny")); giveGiftRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("askAboutPartnerStatus"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("givegift",giveGiftRulesToTrigger);

		List<Rule> gossipRulesToTrigger = new List<Rule>(); gossipRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("reminisce")); gossipRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("flirt"));
		gossipRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("deny")); gossipRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("gossip",gossipRulesToTrigger);

		List<Rule> argueRulesToTrigger = new List<Rule>(); argueRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue")); argueRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("fight"));
		argueRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("sabotage")); argueRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("order")); 
		argueRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("deny")); argueRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("harass"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("argue",argueRulesToTrigger);

		List<Rule> denyRulesToTrigger = new List<Rule>(); denyRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue")); denyRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("poison")); 
		denyRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("fight"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("deny",denyRulesToTrigger);

		//List<Rule> demandtostopbeingfriendwithRulesToTrigger = new List<Rule>(); demandtostopbeingfriendwithRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue")); demandtostopbeingfriendwithRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("poison")); 
		//demandtostopbeingfriendwithRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("fight"));
	

		List<Rule> makedistractionRulesToTrigger = new List<Rule>(); makedistractionRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue")); makedistractionRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("poison")); 
		makedistractionRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("steal")); makedistractionRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("deny")); makedistractionRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("sabotage"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("makedistraction",makedistractionRulesToTrigger);

		List<Rule> reminisceRulesToTrigger = new List<Rule>(); reminisceRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("chat")); reminisceRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("gossip"));
		reminisceRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("flirt"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("reminisce",reminisceRulesToTrigger);

		List<Rule> enthuseaboutgreatnessofpersonRulesToTrigger = new List<Rule>(); enthuseaboutgreatnessofpersonRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("chat"));
		enthuseaboutgreatnessofpersonRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("flirt")); enthuseaboutgreatnessofpersonRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("fight"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("enthuseaboutgreatnessofperson",enthuseaboutgreatnessofpersonRulesToTrigger);

		List<Rule> makefunofRulesToTrigger = new List<Rule>(); makefunofRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("makefunof")); makefunofRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue"));
		makefunofRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("harass")); makefunofRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("deny"));
		makefunofRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("telljoke"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("makefunof",makefunofRulesToTrigger);

		List<Rule> telljokeRulesToTrigger = new List<Rule>(); telljokeRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("makefunof")); telljokeRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("telljoke")); 
		telljokeRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("chat")); telljokeRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("enthuseaboutgreatnessofperson")); 
		relationSystem.pplAndMasks.AddPossibleRulesToRule("telljoke",telljokeRulesToTrigger);

		List<Rule> prankRulesToTrigger = new List<Rule>(); prankRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("makefunof")); prankRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("deny"));
		prankRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("convict")); prankRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue")); prankRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("order"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("prank",prankRulesToTrigger);

		List<Rule> harassRulesToTrigger = new List<Rule>(); harassRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("telljoke")); harassRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("deny"));
		harassRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue")); harassRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("fight")); harassRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("order"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("harass",harassRulesToTrigger);


		// ------------- CULTURE
		List<Rule> greetRulesToTrigger = new List<Rule>(); greetRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("chat")); greetRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("kiss"));
		greetRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("greet"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("greet",greetRulesToTrigger);
		relationSystem.pplAndMasks.AddPossibleRulesToRule("greetfcess",greetRulesToTrigger);
		relationSystem.pplAndMasks.AddPossibleRulesToRule("greetfbunce",greetRulesToTrigger);
		relationSystem.pplAndMasks.AddPossibleRulesToRule("greetfbunsant",greetRulesToTrigger);

		List<Rule> convictRulesToTrigger = new List<Rule>(); convictRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("bribe")); convictRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("poison")); 
		convictRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("fight")); convictRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("flee")); 
		relationSystem.pplAndMasks.AddPossibleRulesToRule("convict",convictRulesToTrigger);
		relationSystem.pplAndMasks.AddPossibleRulesToRule("convictfcess",convictRulesToTrigger);
		relationSystem.pplAndMasks.AddPossibleRulesToRule("convictfbunce",convictRulesToTrigger);

		List<Rule> fightRulesToTrigger = new List<Rule>(); fightRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("fight")); fightRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("convict"));
		fightRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("poison")); fightRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("sabotage")); fightRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("fight",fightRulesToTrigger);

		List<Rule> bribeRulesToTrigger = new List<Rule>(); bribeRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("fight")); bribeRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("convict"));
		bribeRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("gossip")); bribeRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("deny")); bribeRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("deny"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("bribe",bribeRulesToTrigger);
		relationSystem.pplAndMasks.AddPossibleRulesToRule("bribefbunce",bribeRulesToTrigger);
		relationSystem.pplAndMasks.AddPossibleRulesToRule("bribefcess",bribeRulesToTrigger);
		relationSystem.pplAndMasks.AddPossibleRulesToRule("bribefbunsant",bribeRulesToTrigger);

		List<Rule> argueinnocenceRulesToTrigger = new List<Rule>(); argueinnocenceRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("chat")); argueinnocenceRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue"));
		argueinnocenceRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("deny"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("argueinnocence",argueinnocenceRulesToTrigger);
		relationSystem.pplAndMasks.AddPossibleRulesToRule("argueinnocencefbunce",argueinnocenceRulesToTrigger);
		relationSystem.pplAndMasks.AddPossibleRulesToRule("argueinnocencefcess",argueinnocenceRulesToTrigger);

		List<Rule> argueguiltinessRulesToTrigger = new List<Rule>(); argueinnocenceRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("chat")); argueinnocenceRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue"));
		argueinnocenceRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("deny"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("argueguiltiness",argueguiltinessRulesToTrigger);
		relationSystem.pplAndMasks.AddPossibleRulesToRule("argueguiltinessfbunce",argueguiltinessRulesToTrigger);
		relationSystem.pplAndMasks.AddPossibleRulesToRule("argueguiltinessfcess",argueguiltinessRulesToTrigger);

		List<Rule> stealRulesToTrigger = new List<Rule>();  stealRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("poison")); 
		stealRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("fight")); stealRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("convict"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("steal",stealRulesToTrigger); relationSystem.pplAndMasks.AddPossibleRulesToRule("buygoods",stealRulesToTrigger);

/*		List<Rule> practicestealingRulesToTrigger = new List<Rule>(); practicestealingRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("steal"));
		practicestealingRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("practicestealing",practicestealingRulesToTrigger);
*/
//		List<Rule> askforhelpinillicitactivityRulesToTrigger = new List<Rule>(); askforhelpinillicitactivityRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("steal"));
//		askforhelpinillicitactivityRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("makedistraction"));
//		relationSystem.pplAndMasks.AddPossibleRulesToRule("askforhelpinillicitactivity",askforhelpinillicitactivityRulesToTrigger);

	/*	List<Rule> searchforthiefRulesToTrigger = new List<Rule>(); searchforthiefRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("searchforthief",searchforthiefRulesToTrigger);
		relationSystem.pplAndMasks.AddPossibleRulesToRule("searchforthieffbunce",searchforthiefRulesToTrigger);
		relationSystem.pplAndMasks.AddPossibleRulesToRule("searchforthieffcess",searchforthiefRulesToTrigger);
*/
		List<Rule> poisonRulesToTrigger = new List<Rule>(); poisonRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("fight")); poisonRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue"));
		poisonRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("sabotage"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("poison",poisonRulesToTrigger);
		relationSystem.pplAndMasks.AddPossibleRulesToRule("poisonfbunce",poisonRulesToTrigger);
		relationSystem.pplAndMasks.AddPossibleRulesToRule("poisonfcess",poisonRulesToTrigger);
		relationSystem.pplAndMasks.AddPossibleRulesToRule("poisonfbunsant",poisonRulesToTrigger);

		List<Rule> playgameRulesToTriger = new List<Rule>(); playgameRulesToTriger.Add(relationSystem.pplAndMasks.GetRule("telljoke")); playgameRulesToTriger.Add(relationSystem.pplAndMasks.GetRule("chat")); 
		playgameRulesToTriger.Add(relationSystem.pplAndMasks.GetRule("reminisce")); 
		relationSystem.pplAndMasks.AddPossibleRulesToRule("playgame",playgameRulesToTriger);
		relationSystem.pplAndMasks.AddPossibleRulesToRule("playgamefbunce",playgameRulesToTriger);
		relationSystem.pplAndMasks.AddPossibleRulesToRule("playgamefbunsant",playgameRulesToTriger);
		relationSystem.pplAndMasks.AddPossibleRulesToRule("playgamefcess",playgameRulesToTriger);

		List<Rule> orderRulesToTriger = new List<Rule>(); orderRulesToTriger.Add(relationSystem.pplAndMasks.GetRule("deny")); orderRulesToTriger.Add(relationSystem.pplAndMasks.GetRule("fight"));
		orderRulesToTriger.Add(relationSystem.pplAndMasks.GetRule("flee"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("order",orderRulesToTriger);
		relationSystem.pplAndMasks.AddPossibleRulesToRule("orderfbunce",orderRulesToTriger);
		relationSystem.pplAndMasks.AddPossibleRulesToRule("orderfcess",orderRulesToTriger);
		relationSystem.pplAndMasks.AddPossibleRulesToRule("orderfbunsant",orderRulesToTriger);

		List<Rule> killRulesToTrigger = new List<Rule>(); killRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("convict")); killRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("fight"));
		killRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("kill"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("kill",killRulesToTrigger);
		relationSystem.pplAndMasks.AddPossibleRulesToRule("killfbunsant",killRulesToTrigger);
		relationSystem.pplAndMasks.AddPossibleRulesToRule("killfcess",killRulesToTrigger);
		relationSystem.pplAndMasks.AddPossibleRulesToRule("killfbunce",killRulesToTrigger);

/*		// ------------- CULT RULES

		List<Rule> praisecultRulesToTrigger = new List<Rule>(); praisecultRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("enthuseaboutgreatnessofperson"));
		praisecultRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue")); praisecultRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("deny")); praisecultRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("leavecult"));  
		praisecultRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("entercult"));  
		relationSystem.pplAndMasks.AddPossibleRulesToRule("praisecult",praisecultRulesToTrigger);
		relationSystem.pplAndMasks.AddPossibleRulesToRule("praisecultffollower",praisecultRulesToTrigger);
		List<Rule> praisecultfleaderRulesToTrigger = new List<Rule>(); poisonRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("enthuseaboutgreatnessofperson"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("praisecultfleader",praisecultfleaderRulesToTrigger);

		List<Rule> entercultRulesToTrigger = new List<Rule>(); entercultRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("enthuseaboutgreatnessofperson"));
		entercultRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue")); entercultRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("fight"));
		entercultRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("leavepartner"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("entercult",entercultRulesToTrigger);

		List<Rule> exitcultRulesToTrigger = new List<Rule>(); exitcultRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("enthuseaboutgreatnessofperson"));
		exitcultRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue")); exitcultRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("fight"));
		exitcultRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("askAboutPartnerStatus"));
		exitcultRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("poison"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("exitcult",exitcultRulesToTrigger);

		List<Rule> damncultRulesToTrigger = new List<Rule>();
		damncultRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue"));
		damncultRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("askAboutPartnerStatus"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("damncult",damncultRulesToTrigger);

		List<Rule> excommunicatefromcultRulesToTrigger = new List<Rule>();
		excommunicatefromcultRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue")); excommunicatefromcultRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("fight"));
		excommunicatefromcultRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("askAboutPartnerStatus"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("excommunicatefromcult",excommunicatefromcultRulesToTrigger);
*/
		// ------------- MERCHANT GUILD RULES

		List<Rule> buycompanyRulesToTrigger = new List<Rule>();
		buycompanyRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue"));
		buycompanyRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("sabotage")); buycompanyRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("demandtoleaveguild"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("buycompany",buycompanyRulesToTrigger);

		List<Rule> sellcompanyRulesToTrigger = new List<Rule>();
		sellcompanyRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue"));
		sellcompanyRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("sabotage")); sellcompanyRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("demandtoleaveguild"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("sellcompany",sellcompanyRulesToTrigger);

		List<Rule> advertiseRulesToTrigger = new List<Rule>();
		sellcompanyRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue")); sellcompanyRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("chat"));
		sellcompanyRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("deny"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("advertise",advertiseRulesToTrigger);

	//	List<Rule> convincetoleaveguildRulesToTrigger = new List<Rule>();
	//	convincetoleaveguildRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue")); convincetoleaveguildRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("poison")); 
	//	convincetoleaveguildRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("order")); convincetoleaveguildRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("deny")); 
	//	relationSystem.pplAndMasks.AddPossibleRulesToRule("demandtoleaveguild",convincetoleaveguildRulesToTrigger); 

		List<Rule> demandtoleaveguildRulesToTrigger = new List<Rule>();
		demandtoleaveguildRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue")); demandtoleaveguildRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("poison")); 
		demandtoleaveguildRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("fight")); demandtoleaveguildRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("deny")); 
		demandtoleaveguildRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("order")); 
		relationSystem.pplAndMasks.AddPossibleRulesToRule("demandtoleaveguild",demandtoleaveguildRulesToTrigger);

/*		List<Rule> askforhelpRulesToTrigger = new List<Rule>(); 
		askforhelpRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue")); askforhelpRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("chat"));
		askforhelpRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("sellgoods")); askforhelpRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("buygoods"));
		askforhelpRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("order"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("askforhelp",askforhelpRulesToTrigger);
*/
		List<Rule> buygoodsRulesToTrigger = new List<Rule>(); 
		buygoodsRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("chat"));
		buygoodsRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("advertise"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("buygoods",buygoodsRulesToTrigger);

		List<Rule> sellgoodsRulesToTrigger = new List<Rule>(); 
		buygoodsRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("chat")); buygoodsRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue")); buygoodsRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("sabotage"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("sellgoods",sellgoodsRulesToTrigger);


// ----------------------------------------------------------------------------------------------- ADDING RULES TO MASKS
	
	// SElF
		relationSystem.AddRuleToMask("John", "Self", "donothing", -1.0f);
		relationSystem.AddRuleToMask("Therese", "Self", "donothing", -1.0f);
		relationSystem.AddRuleToMask("Bill", "Self", "donothing", -1.0f);
		relationSystem.AddRuleToMask("Heather", "Self", "donothing", -1.0f);

		relationSystem.AddRuleToMask("John", "Self", "flee", 0.2f);
		relationSystem.AddRuleToMask("Heather", "Self", "flee", -0.1f);

		relationSystem.AddRuleToMask("John", "Self", "chooseanotheraspartner", 0.5f);
		relationSystem.AddRuleToMask("Therese", "Self", "chooseanotheraspartner", 0.5f);
		relationSystem.AddRuleToMask("Bill", "Self", "chooseanotheraspartner", 0.5f);
		relationSystem.AddRuleToMask("Heather", "Self", "chooseanotheraspartner", 0.5f);
		
	// INTERPERSONAL
		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "kiss", 0.4f);
		
		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "askAboutPartnerStatus", 0.5f);
		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "stayaspartner", 0.5f);
		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "leavepartner", 0.5f);

		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "flirt", -0.4f);
		relationSystem.AddRuleToMask("Friendship", "Friend", "flirt", -0.4f);

		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "chat", 0.0f);
		relationSystem.AddRuleToMask("Friendship", "Friend", "chat", 0.0f);
		relationSystem.AddRuleToMask("Rivalry", "Enemy", "chat", 0.0f);

		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "givegift", 0.0f);
		relationSystem.AddRuleToMask("Rivalry", "Enemy", "givegift", 0.0f);
		relationSystem.AddRuleToMask("Friendship", "Friend", "givegift", 0.0f);

		relationSystem.AddRuleToMask("Rivalry", "Enemy", "gossip", -0.2f);
		relationSystem.AddRuleToMask("Friendship", "Friend", "gossip", -0.2f);
		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "gossip", -0.2f);

		relationSystem.AddRuleToMask("Rivalry", "Enemy", "argue", -0.2f);
		relationSystem.AddRuleToMask("Friendship", "Friend", "argue", -0.2f);
		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "argue", -0.2f);

		relationSystem.AddRuleToMask("Rivalry", "Enemy", "deny", -0.1f);
		relationSystem.AddRuleToMask("Friendship", "Friend", "deny", -0.1f);
		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "deny", -0.1f);

		relationSystem.AddRuleToMask("Rivalry", "Enemy", "makedistraction", -0.2f);
		relationSystem.AddRuleToMask("Friendship", "Friend", "makedistraction", -0.2f);
		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "makedistraction", -0.2f);

		relationSystem.AddRuleToMask("Rivalry", "Enemy", "makefunof", 0.0f);
		relationSystem.AddRuleToMask("Friendship", "Friend", "makefunof", 0.0f);
		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "makefunof", 0.0f);

		relationSystem.AddRuleToMask("Rivalry", "Enemy", "telljoke", 0.2f);
		relationSystem.AddRuleToMask("Friendship", "Friend", "telljoke", 0.2f);
		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "telljoke", 0.2f);

		relationSystem.AddRuleToMask("Rivalry", "Enemy", "prank", 0.0f);
		relationSystem.AddRuleToMask("Friendship", "Friend", "prank", 0.0f);
		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "prank", 0.0f);

		relationSystem.AddRuleToMask("Rivalry", "Enemy", "harass", -0.2f);
		relationSystem.AddRuleToMask("Friendship", "Friend", "harass", -0.2f);
		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "harass", -0.2f);

		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "reminisce", 0.2f);
		relationSystem.AddRuleToMask("Friendship", "Friend", "reminisce", 0.2f);

		relationSystem.AddRuleToMask("Friendship", "Friend", "enthuseaboutgreatnessofperson", 0.2f);
		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "enthuseaboutgreatnessofperson", 0.2f);




	// CULTURE
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "fight", -0.5f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "bribefbunsant", -0.1f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "steal", -0.5f);
//		relationSystem.AddRuleToMask("Bungary", "Bunsant", "practicestealing", -0.3f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "askforhelpinillicitactivity", -0.1f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "poisonfbunsant", -0.8f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "greetfbunsant", 0.5f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "playgamefbunsant", 0.2f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "orderfbunsant", -0.5f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "moveToStuefbunsant", 0.0f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "moveToKøkkenfbunsant", 0.0f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "moveToIndgangfbunsant", 0.0f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "killfbunsant", -0.9f);

		relationSystem.AddRuleToMask("Bungary", "Bunce", "bribefbunce", 0.3f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "convictfbunce", 0.0f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "argueinnocencefbunce", 0.0f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "argueguiltinessfbunce", 0.0f);
		//relationSystem.AddRuleToMask("Bungary", "Bunce", "searchforthieffbunce", 0.8f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "poisonfbunce", -0.8f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "greetfbunce", 1.0f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "playgamefbunce", 0.2f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "orderfbunce", 0.5f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "moveToStuefbunce", 0.5f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "moveToKøkkenfbunce", 0.1f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "moveToIndgangfbunce", 0.3f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "killfbunce", -0.8f);

		relationSystem.AddRuleToMask("Bungary", "Buncess", "bribefcess", 0.3f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "convictfcess", 0.0f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "argueinnocencefcess", 0.2f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "argueguiltinessfcess", -0.1f);
		//relationSystem.AddRuleToMask("Bungary", "Buncess", "searchforthieffcess", 0.3f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "poisonfcess", -0.8f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "greetfcess", 1.0f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "playgamefbuncess", 0.2f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "orderfcess", 0.5f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "moveToStuefcess", 0.6f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "moveToKøkkenfcess", 0.2f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "moveToIndgangfcess", 0.3f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "killfcess", -0.9f);
/*
		relationSystem.AddRuleToMask("Cult", "Leader", "praisecultfleader", 0.6f);
		relationSystem.AddRuleToMask("Cult", "Follower", "praisecultffollower", 0.4f);
		relationSystem.AddRuleToMask("Cult", "Follower", "entercult", 0.3f);
		relationSystem.AddRuleToMask("Cult", "Follower", "exitcult", -0.8f);
		relationSystem.AddRuleToMask("Cult", "Follower", "damncult", -0.4f);
		relationSystem.AddRuleToMask("Cult", "Leader", "excommunicatefromcult", 0.1f);
*/
		relationSystem.AddRuleToMask("MerchantGuild", "Member", "buycompany", 0.2f);
		relationSystem.AddRuleToMask("MerchantGuild", "Member", "sabotage", -0.5f);
		relationSystem.AddRuleToMask("MerchantGuild", "Member", "advertise", 0.5f);
	//	relationSystem.AddRuleToMask("MerchantGuild", "Member", "convincetoleaveguild", -0.1f);
		relationSystem.AddRuleToMask("MerchantGuild", "Member", "demandtoleaveguild", -0.3f);
	//	relationSystem.AddRuleToMask("MerchantGuild", "Member", "askforhelp", 0.2f);
		relationSystem.AddRuleToMask("MerchantGuild", "Member", "sellcompany", -0.5f);
		relationSystem.AddRuleToMask("MerchantGuild", "Member", "buygoods", 0.5f);
		relationSystem.AddRuleToMask("MerchantGuild", "Member", "sellgoods", 0.7f);

		#endregion Rules



//  ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		//PEOPLE


		
		#region AddingPlayer
		MaskAdds selfPersMask = new MaskAdds("Self", "Player", 0.0f, new List<Person>());
		
		relationSystem.CreateNewPerson(selfPersMask, new List<MaskAdds>(), new List<MaskAdds>(), 0f, 0f, 0f, new float[] { 0f, 0f, 0f },new float[]{0.0f,0.0f,0.0f});
		#endregion AddingPlayer
		
		#region AddingBill
		selfPersMask = new MaskAdds("Self", "Bill", 0.0f, new List<Person>());
		
		List<MaskAdds>  culture = new List<MaskAdds>();
		culture.Add(new MaskAdds("Bunce", "Bungary", 0.5f, new List<Person>()));
		//culture.Add(new MaskAdds("Follower", "Cult", 0.4f,new List<Person>()));
		culture.Add(new MaskAdds("Member", "MerchantGuild", 0.6f,new List<Person>()));
		
		relationSystem.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.6f, 0.4f, 0.7f, new float[] { -0.2f, 0.5f, 0.1f },new float[]{0.0f,0.0f,0.0f});
		#endregion AddingBill
		
		#region AddingTerese
		selfPersMask = new MaskAdds("Self", "Therese", 0.0f, new List<Person>());
		
		culture = new List<MaskAdds>();
		culture.Add(new MaskAdds("Buncess", "Bungary", 0.6f, new List<Person>()));
		//culture.Add(new MaskAdds("Sceptic", "Cult", 0.1f,new List<Person>()));
		
		relationSystem.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.3f, 0.7f, 0.2f, new float[] { 0.6f, -0.5f, 0.6f },new float[]{0.0f,0.0f,0.0f});
		#endregion AddingTerese
		
		#region AddingJohn
		selfPersMask = new MaskAdds("Self", "John", 0.0f, new List<Person>());
		
		culture = new List<MaskAdds>();
		//culture.Add(new MaskAdds("Follower", "Cult", 0.4f,new List<Person>()));
		culture.Add(new MaskAdds("Bunsant", "Bungary", 0.1f, new List<Person>()));
		
		relationSystem.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.5f, 0.5f, 0.4f, new float[] { 0.0f, 0.8f, -0.4f },new float[]{0.0f,0.0f,0.0f});
		#endregion AddingJohn
		
		#region AddingHeather
		selfPersMask = new MaskAdds("Self", "Heather", 0.0f, new List<Person>());
		
		culture = new List<MaskAdds>();
		culture.Add(new MaskAdds("Bunsant", "Bungary", 0.3f, new List<Person>()));
		//culture.Add(new MaskAdds("Leader", "Cult", 0.6f,new List<Person>()));
		
		relationSystem.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.2f, 0.8f, 0.8f, new float[] { 0.2f, 0.4f, 0.0f },new float[]{0.0f,0.0f,0.0f});
		#endregion AddingHeather
		
		#region rolerefs
		//relationSystem.pplAndMasks.GetPerson("bill").GetLinks(TypeMask.culture).Find(x=>x.roleMask.GetMaskName() == "cult").AddRoleRef(relationSystem.pplAndMasks.GetPerson("heather"));
		//relationSystem.pplAndMasks.GetPerson("john").GetLinks(TypeMask.culture).Find(x=>x.roleMask.GetMaskName() == "cult").AddRoleRef(relationSystem.pplAndMasks.GetPerson("heather"));
		#endregion rolerefs

		
		#region LINKS
		relationSystem.AddLinkToPerson("Bill", new string[] { "Therese" }, TypeMask.interPers, "Partner", "RomanticRelationship", 0.3f);
		relationSystem.AddLinkToPerson("Bill", new string[] { "John" }, TypeMask.interPers, "Enemy", "Rivalry", 0.4f);
		relationSystem.AddLinkToPerson("Bill", new string[] { "Heather" }, TypeMask.interPers, "Friend", "Friendship", 0.2f);
		relationSystem.AddLinkToPerson("Bill", new string[] { "Player" }, TypeMask.interPers, "Enemy", "Rivalry", 0.4f);
		
		relationSystem.AddLinkToPerson("Therese", new string[] { "Bill" }, TypeMask.interPers, "Partner", "RomanticRelationship", 0.5f);
		relationSystem.AddLinkToPerson("Therese", new string[] { "John" }, TypeMask.interPers, "Enemy", "Rivalry", 0.2f);
		relationSystem.AddLinkToPerson("Therese", new string[] { "Heather" }, TypeMask.interPers, "Friend", "Friendship", 0.6f);
		relationSystem.AddLinkToPerson("Therese", new string[] { "Player" }, TypeMask.interPers, "Enemy", "Rivalry", 0.3f);
		
		relationSystem.AddLinkToPerson("John", new string[] { "Bill" }, TypeMask.interPers, "Enemy", "Rivalry", 0.7f);
		relationSystem.AddLinkToPerson("John", new string[] { "Therese" }, TypeMask.interPers, "Enemy", "Rivalry", 0.4f);
		relationSystem.AddLinkToPerson("John", new string[] { "Heather" }, TypeMask.interPers, "Partner", "RomanticRelationship", 0.8f);
		relationSystem.AddLinkToPerson("John", new string[] { "Player" }, TypeMask.interPers, "Friend", "Friendship", 0.5f);
		
		relationSystem.AddLinkToPerson("Heather", new string[] { "Bill" }, TypeMask.interPers, "Friend", "Friendship", 0.4f);
		relationSystem.AddLinkToPerson("Heather", new string[] { "Therese" }, TypeMask.interPers, "Friend", "Friendship", 0.6f);
		relationSystem.AddLinkToPerson("Heather", new string[] { "John" }, TypeMask.interPers, "Partner", "RomanticRelationship", 0.5f);
		relationSystem.AddLinkToPerson("Heather", new string[] { "Player" }, TypeMask.interPers, "Partner", "RomanticRelationship", 0.5f);
		
		relationSystem.AddLinkToPerson("Player", new string[] { "Bill" }, TypeMask.interPers, "Enemy", "Rivalry", 0.4f);
		relationSystem.AddLinkToPerson("Player", new string[] { "Therese" }, TypeMask.interPers, "Enemy", "Rivalry", 0.6f);
		relationSystem.AddLinkToPerson("Player", new string[] { "John" }, TypeMask.interPers, "Friend", "Friendship", 0.5f);
		relationSystem.AddLinkToPerson("Player", new string[] { "Heather" }, TypeMask.interPers, "Partner", "RomanticRelationship", 0.5f);
		#endregion LINKS 

		#region Opinions
		//BILL OPINIONS
		relationSystem.pplAndMasks.GetPerson("bill").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("therese"),0.6f));
		relationSystem.pplAndMasks.GetPerson("bill").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("therese"),0.3f));
		relationSystem.pplAndMasks.GetPerson("bill").opinions.Add(new Opinion(TraitTypes.ShyBolsterous,relationSystem.pplAndMasks.GetPerson("therese"),-0.1f));
		relationSystem.pplAndMasks.GetPerson("bill").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("john"),-0.4f));
		relationSystem.pplAndMasks.GetPerson("bill").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("john"),-0.6f));
		relationSystem.pplAndMasks.GetPerson("bill").opinions.Add(new Opinion(TraitTypes.ShyBolsterous,relationSystem.pplAndMasks.GetPerson("john"),0.3f));
		relationSystem.pplAndMasks.GetPerson("bill").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("heather"),0.4f));
		relationSystem.pplAndMasks.GetPerson("bill").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("heather"),0.6f));
		relationSystem.pplAndMasks.GetPerson("bill").opinions.Add(new Opinion(TraitTypes.ShyBolsterous,relationSystem.pplAndMasks.GetPerson("heather"),0.4f));
		relationSystem.pplAndMasks.GetPerson("bill").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("player"),-0.2f));
		relationSystem.pplAndMasks.GetPerson("bill").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("player"),-0.1f));
		relationSystem.pplAndMasks.GetPerson("bill").opinions.Add(new Opinion(TraitTypes.ShyBolsterous,relationSystem.pplAndMasks.GetPerson("player"),0.4f));
		relationSystem.pplAndMasks.GetPerson("therese").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("bill"),0.7f));
		relationSystem.pplAndMasks.GetPerson("therese").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("bill"),0.5f));
		relationSystem.pplAndMasks.GetPerson("therese").opinions.Add(new Opinion(TraitTypes.ShyBolsterous,relationSystem.pplAndMasks.GetPerson("bill"),0.8f));
		relationSystem.pplAndMasks.GetPerson("therese").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("john"),-0.4f));
		relationSystem.pplAndMasks.GetPerson("therese").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("john"),-0.7f));
		relationSystem.pplAndMasks.GetPerson("therese").opinions.Add(new Opinion(TraitTypes.ShyBolsterous,relationSystem.pplAndMasks.GetPerson("john"),-0.3f));
		relationSystem.pplAndMasks.GetPerson("therese").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("heather"),0.5f));
		relationSystem.pplAndMasks.GetPerson("therese").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("heather"),-0.1f));
		relationSystem.pplAndMasks.GetPerson("therese").opinions.Add(new Opinion(TraitTypes.ShyBolsterous,relationSystem.pplAndMasks.GetPerson("heather"),0.0f));
		relationSystem.pplAndMasks.GetPerson("therese").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("player"),-0.3f));
		relationSystem.pplAndMasks.GetPerson("therese").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("player"),0.1f));
		relationSystem.pplAndMasks.GetPerson("therese").opinions.Add(new Opinion(TraitTypes.ShyBolsterous,relationSystem.pplAndMasks.GetPerson("player"),0.5f));
		relationSystem.pplAndMasks.GetPerson("john").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("bill"),-0.5f));
		relationSystem.pplAndMasks.GetPerson("john").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("bill"),-0.5f));
		relationSystem.pplAndMasks.GetPerson("john").opinions.Add(new Opinion(TraitTypes.ShyBolsterous,relationSystem.pplAndMasks.GetPerson("bill"),-0.1f));
		relationSystem.pplAndMasks.GetPerson("john").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("therese"),-0.3f));
		relationSystem.pplAndMasks.GetPerson("john").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("therese"),0.2f));
		relationSystem.pplAndMasks.GetPerson("john").opinions.Add(new Opinion(TraitTypes.ShyBolsterous,relationSystem.pplAndMasks.GetPerson("therese"),-0.3f));
		relationSystem.pplAndMasks.GetPerson("john").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("heather"),0.6f));
		relationSystem.pplAndMasks.GetPerson("john").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("heather"),0.4f));
		relationSystem.pplAndMasks.GetPerson("john").opinions.Add(new Opinion(TraitTypes.ShyBolsterous,relationSystem.pplAndMasks.GetPerson("heather"),0.4f));
		relationSystem.pplAndMasks.GetPerson("john").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("player"),0.5f));
		relationSystem.pplAndMasks.GetPerson("john").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("player"),07f));
		relationSystem.pplAndMasks.GetPerson("john").opinions.Add(new Opinion(TraitTypes.ShyBolsterous,relationSystem.pplAndMasks.GetPerson("player"),0.3f));
		relationSystem.pplAndMasks.GetPerson("heather").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("bill"),0.4f));
		relationSystem.pplAndMasks.GetPerson("heather").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("bill"),-0.1f));
		relationSystem.pplAndMasks.GetPerson("heather").opinions.Add(new Opinion(TraitTypes.ShyBolsterous,relationSystem.pplAndMasks.GetPerson("bill"),0.2f));
		relationSystem.pplAndMasks.GetPerson("heather").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("therese"),0.6f));
		relationSystem.pplAndMasks.GetPerson("heather").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("therese"),0.4f));
		relationSystem.pplAndMasks.GetPerson("heather").opinions.Add(new Opinion(TraitTypes.ShyBolsterous,relationSystem.pplAndMasks.GetPerson("therese"),-0.1f));
		relationSystem.pplAndMasks.GetPerson("heather").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("john"),0.4f));
		relationSystem.pplAndMasks.GetPerson("heather").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("john"), -0.4f));
		relationSystem.pplAndMasks.GetPerson("heather").opinions.Add(new Opinion(TraitTypes.ShyBolsterous,relationSystem.pplAndMasks.GetPerson("john"), 0.2f));
		relationSystem.pplAndMasks.GetPerson("heather").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("player"),0.7f));
		relationSystem.pplAndMasks.GetPerson("heather").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("player"),0.3f));
		relationSystem.pplAndMasks.GetPerson("heather").opinions.Add(new Opinion(TraitTypes.ShyBolsterous,relationSystem.pplAndMasks.GetPerson("player"),0.4f));
		relationSystem.pplAndMasks.GetPerson("player").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("bill"),0.0f));
		relationSystem.pplAndMasks.GetPerson("player").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("bill"),0.0f));
		relationSystem.pplAndMasks.GetPerson("player").opinions.Add(new Opinion(TraitTypes.ShyBolsterous,relationSystem.pplAndMasks.GetPerson("bill"),0.0f));
		relationSystem.pplAndMasks.GetPerson("player").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("therese"),0.0f));
		relationSystem.pplAndMasks.GetPerson("player").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("therese"),0.0f));
		relationSystem.pplAndMasks.GetPerson("player").opinions.Add(new Opinion(TraitTypes.ShyBolsterous,relationSystem.pplAndMasks.GetPerson("therese"),0.0f));
		relationSystem.pplAndMasks.GetPerson("player").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("john"),0.0f));
		relationSystem.pplAndMasks.GetPerson("player").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("john"), 0.0f));
		relationSystem.pplAndMasks.GetPerson("player").opinions.Add(new Opinion(TraitTypes.ShyBolsterous,relationSystem.pplAndMasks.GetPerson("john"), 0.0f));
		relationSystem.pplAndMasks.GetPerson("player").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("heather"),0.0f));
		relationSystem.pplAndMasks.GetPerson("player").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("heather"),0.0f));
		relationSystem.pplAndMasks.GetPerson("player").opinions.Add(new Opinion(TraitTypes.ShyBolsterous,relationSystem.pplAndMasks.GetPerson("heather"),0.0f));
		#endregion Opinions
	}


}
