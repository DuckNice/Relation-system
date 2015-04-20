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
		John.possessions.Add (new Game ("Cards"));

		foreach (Being b in beings) {
			b.name = b.name.ToLower();
		}


	}


	public void CreateFirstMasks()
	{
		relationSystem.CreateNewMask("Player", new float[]{}, TypeMask.selfPerc, new string[]{});

		relationSystem.CreateNewMask("Bungary", new float[] { 0.0f, 0.0f, 0.0f }, TypeMask.culture, new string[] { "Bunce", "Buncess", "Bunsant" });
		relationSystem.CreateNewMask("Cult", new float[] { 0.0f, -0.2f, 0.1f }, TypeMask.culture, new string[] { "Leader", "Follower", "Skeptic" });
		relationSystem.CreateNewMask("MerchantGuild", new float[] { 0.0f, -0.3f, -0.2f }, TypeMask.culture, new string[] { "Member" });

		relationSystem.CreateNewMask("Bill", new float[] { 0.0f, 0.0f, 0.0f }, TypeMask.selfPerc, new string[] { "self" });
		relationSystem.CreateNewMask("Therese", new float[] { 0.0f, 0.0f, 0.0f }, TypeMask.selfPerc, new string[] { "self" });
		relationSystem.CreateNewMask("John", new float[] { 0.0f, 0.0f, 0.0f }, TypeMask.selfPerc, new string[] { "self" });
		relationSystem.CreateNewMask("Heather", new float[] { 0.0f, 0.0f, 0.0f }, TypeMask.selfPerc, new string[] { "self" });

		relationSystem.CreateNewMask("RomanticRelationship", new float[] { 0.2f, 0.2f, 0.2f }, TypeMask.interPers, new string[] { "Partner" });
		relationSystem.CreateNewMask("Friendship", new float[] { 0.1f, 0.1f, 0.0f }, TypeMask.interPers, new string[] { "Friend" });
		relationSystem.CreateNewMask("Rivalry", new float[] { -0.3f, -0.3f, -0.2f }, TypeMask.interPers, new string[] { "Enemy" });
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
				}
			}
			return false; };

		RuleConditioner fleeCondition = (self, other, indPpl) =>
		{	if(self.moods[MoodTypes.angryFear] < -0.8f){
					return true; 
				}
			return false; };

		RuleConditioner kissCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.GetRoleRefPpl().Exists(y=>y.name == other.name && y.interPersonal.Exists(z=>z.roleName == "partner" && z.GetRoleRefPpl().Exists(s=>s.name == self.name))) && x.roleName == "partner")){
				if(roomMan.IsPersonInSameRoomAsMe(self, other) )
					{ return true; }
			}
			else{
				if (self != other  && self.moods[MoodTypes.energTired] > -0.1f){ 
					if(self.moods[MoodTypes.arousDisgus] > 0.5f  && roomMan.IsPersonInSameRoomAsMe(self, other) ){ return true; }}
			}					
			return false;
		};

		RuleConditioner askAboutPartnerStatusCondition = (self, other, indPpl) =>
		{	if((relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["kiss"] && x.GetSubject() == other && x.GetDirect() != self)
			   && (self.interPersonal.Exists(x=>x.GetRoleRefPpl().Exists(y=>y.name == other.name && y.interPersonal.Exists(z=>z.roleName == "partner" && z.GetRoleRefPpl().Exists(s=>s.name == self.name))) && x.roleName == "partner")))
			   && roomMan.IsPersonInSameRoomAsMe(self, other))
			   		{ return true; }

			if(!(self.interPersonal.Exists(x=>x.roleName=="partner")) && self != other && roomMan.IsPersonInSameRoomAsMe(self, other)){
				if(self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.4f){
					return true;
				}
			}
			return false; };

		RuleConditioner chooseAnotherAsPartnerCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.GetRoleRefPpl().Exists(y=>y.name == other.name && y.interPersonal.Exists(z=>z.roleName == "partner" && z.GetRoleRefPpl().Exists(s=>s.name == self.name))) && x.roleName == "partner")){
				return false;
			}

			if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["askaboutpartnerstatus"] && x.GetSubject() == other && x.GetDirect() == self && x.GetTime() < 10f)){
				if(self.interPersonal.Exists(x=>x.GetlvlOfInfl(other) > 0.3f) && self != other  && roomMan.IsPersonInSameRoomAsMe(self, other)){
					if(self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.4f)
						{  return true; }
				}
			}
			if(self.interPersonal.Exists(x => x.roleName != "partner") && other.interPersonal.Exists(x => x.roleName != "partner")){
				if(self != other && roomMan.IsPersonInSameRoomAsMe(self, other)){
					if(self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.5f)
						{  return true; }
				}
			}
			return false; };

		RuleConditioner stayAsPartnerCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["askaboutpartnerstatus"] && x.GetSubject() == other && x.GetDirect() == self && x.GetTime() < 10f)){
				if(self != other && roomMan.IsPersonInSameRoomAsMe(self, other))
					{ return true; }
			}
			return false; };

		RuleConditioner LeavePartnerCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.GetRoleRefPpl().Exists(y=>y.name == other.name && y.interPersonal.Exists(z=>z.roleName == "partner" && z.GetRoleRefPpl().Exists(s=>s.name == self.name))) && x.roleName == "partner")){
				if(self != other && roomMan.IsPersonInSameRoomAsMe(self, other)){
					if(self.GetOpinionValue(TraitTypes.NiceNasty,other) < 0.5f){
						return true;
					}
				}
			}
			return false; };

		RuleConditioner flirtCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x => x.roleName != "partner") && other.interPersonal.Exists(x => x.roleName != "partner")){
				if(self.moods[MoodTypes.arousDisgus] > 0.0f  && self != other && roomMan.IsPersonInSameRoomAsMe(self, other)){
					if(self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.3f){
						return true;
					}
				}
			}
			else if(self.interPersonal.Exists(x => x.roleName != "partner") && other.interPersonal.Exists(x => x.roleName == "partner")){
				if(self.moods[MoodTypes.arousDisgus] > 0.3f  && self != other && roomMan.IsPersonInSameRoomAsMe(self, other)){
					if(self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.4f){
						return true;
					}
				}
			}
			else{
				if(self.moods[MoodTypes.arousDisgus] >= 0.0f  && self != other && roomMan.IsPersonInSameRoomAsMe(self, other)){
						return true;
				}
			}
			return false; };

		RuleConditioner chatCondition = (self, other, indPpl) =>
		{	if(self.interPersonal.Exists(x=>x.GetlvlOfInfl(other) >= 0.2) && self != other &&
			     self.moods[MoodTypes.energTired] > -0.5f && roomMan.IsPersonInSameRoomAsMe(self, other)){
					return true;
			}
			return false; };

		RuleConditioner giveGiftCondition = (self, other, indPpl) =>
		{	
			if(self.interPersonal.Exists(x=>x.GetlvlOfInfl(other) > 0.4f) && self.moods[MoodTypes.energTired] > -0.4f && self != other && roomMan.IsPersonInSameRoomAsMe(self, other)){
				if(beings.Find(x=>x.name == self.name).possessions.Exists(y=>y.Name=="game" || y.Name=="company") && self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.4f && self.GetOpinionValue(TraitTypes.CharitableGreedy,other) > 0.2f){
					if(beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="game" || y.Name=="company").value > 0f){
						return true;
					}
				}
			}
			return false; };

		RuleConditioner poisonCondition = (self, other, indPpl) =>
		{	if(self.moods[MoodTypes.angryFear] < -0.3f && self != other  && roomMan.IsPersonInSameRoomAsMe(self, other)){
				if(self.GetOpinionValue(TraitTypes.NiceNasty,other) < -0.4f)
				return true;
			}
			return false; };

		RuleConditioner gossipCondition = (self, other, indPpl) =>
		{	if(self != other  && self.moods[MoodTypes.energTired] > -0.4f && roomMan.IsPersonInSameRoomAsMe(self, other) && (self.moods[MoodTypes.angryFear] < 0.0f || self.moods[MoodTypes.hapSad] < 0.0f))
				{return true;}
			return false; };

		RuleConditioner argueCondition = (self, other, indPpl) =>
		{	if(self != other && roomMan.IsPersonInSameRoomAsMe(self, other)){
				if(self.GetOpinionValue(TraitTypes.NiceNasty,other) < 0.0f || self.moods[MoodTypes.angryFear] < -0.1f)
					return true;
			}
			return false; };

		RuleConditioner makeDistractionCondition = (self, other, indPpl) =>
		{	if(self.CalculateTraitType(TraitTypes.NiceNasty) < 0.3f  && self.moods[MoodTypes.energTired] > -0.4f && roomMan.IsPersonInSameRoomAsMe(self, other) && self != other)
					{ return true; }
			return false; };

		RuleConditioner reminisceCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["chat"] && x.GetSubject() == other && x.GetTime() < 10f) && self != other  &&
			     self.moods[MoodTypes.energTired] > -0.4f && roomMan.IsPersonInSameRoomAsMe(self, other))
					if(self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.2f)
						{ return true; }
			return false; };

		RuleConditioner denyCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Find(x=>x.GetDirect()==self && x.GetSubject()==other).GetTime() < 3f){
				if(self != other && roomMan.IsPersonInSameRoomAsMe(self, other)){
					if((self.moods[MoodTypes.arousDisgus] < 0.0f ||
					    self.moods[MoodTypes.hapSad] < 0.0f ||
					    self.moods[MoodTypes.angryFear] > 0.0f ||
					    self.GetOpinionValue(TraitTypes.NiceNasty,other) < -0.2f))
							{ return true; }
				}
			}
			return false; };

		RuleConditioner praiseCondition = (self, other, indPpl) =>
		{	if((self.interPersonal.Exists(x=>x.GetRoleRefPpl().Exists(y=>y.name == other.name))) && self.moods[MoodTypes.hapSad] > 0.0f && self.moods[MoodTypes.energTired] > -0.4f && roomMan.IsPersonInSameRoomAsMe(self, other)) 
				{ return true; }
			return false; };

		RuleConditioner cryCondition = (self, other, indPpl) =>
		{	if( self != other){
				if(relationSystem.historyBook.Exists(x=>(x.GetAction()==relationSystem.posActions["kill"] || x.GetAction()==relationSystem.posActions["flee"] ) && x.GetTime() < 10f))
					{ return true; }
				else if(relationSystem.historyBook.Exists(x=>(x.GetAction()==relationSystem.posActions["poison"] || x.GetAction()==relationSystem.posActions["sabotage"] || 
				                                              x.GetAction()==relationSystem.posActions["fight"]) && x.GetDirect() == other && x.GetTime() < 10f))
					{ return true; }
			}
			else{
				if(relationSystem.historyBook.Exists(x=>x.GetDirect() == self && (x.GetAction()==relationSystem.posActions["poison"] ||
				                                                                  x.GetAction()==relationSystem.posActions["sabotage"] || x.GetAction()==relationSystem.posActions["fight"]) && x.GetTime() < 10f))
					{ return true; }
			}
			if(self.moods[MoodTypes.hapSad] < -0.4f){
				return true;
			}
			return false; };

		RuleConditioner consoleCondition = (self, other, indPpl) =>
		{	if( self != other){
				if(relationSystem.historyBook.Exists(x=>(x.GetAction()==relationSystem.posActions["cry"] && x.GetSubject() == other && x.GetTime() < 10f))){
					return true;
				}
			}
			return false; };


// --------------------------------- CULTURAL CONDITIONS
		
		RuleConditioner convictCondition = (self, other, indPpl) =>
		{	if((relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["steal"] && (x.GetSubject()==other)) ||
			  relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["fight"] && (x.GetSubject()==other)) ||
			  relationSystem.historyBook.Exists(x=>x.GetRule().GetRuleStrength() < -0.4f && x.GetTime() < 10f)) && roomMan.IsPersonInSameRoomAsMe(self, other) && self != other)
				{ return true; }
			return false; };

		RuleConditioner fightCondition = (self, other, indPpl) =>
		{	if(self != other && roomMan.IsPersonInSameRoomAsMe(self, other)){
				if((self.CalculateTraitType(TraitTypes.NiceNasty) < 0.0 || self.moods[MoodTypes.angryFear] > 0.4f)  && self.moods[MoodTypes.energTired] > -0.8f){
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
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["convict"] && x.GetSubject()==other) && self.CalculateTraitType(TraitTypes.NiceNasty) > 0.0f
			     && self != other && roomMan.IsPersonInSameRoomAsMe(self, other))
				{ return true; }
			return false; };

		RuleConditioner argueGuiltinessCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["convict"] && x.GetSubject()==other) && self.CalculateTraitType(TraitTypes.NiceNasty) < 0.0f
			     && self != other && roomMan.IsPersonInSameRoomAsMe(self, other))
				{ return true; }
			return false; };

		RuleConditioner stealCondition = (self, other, indPpl) =>
		{	if(self != other  && self.moods[MoodTypes.energTired] > -0.4f && beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="money").value <= 50f
			   && beings.Find(x=>x.name == other.name).possessions.Find(y=>y.Name=="money").value >= 50f && self.CalculateTraitType(TraitTypes.CharitableGreedy) < 0.2f)
				  { return true; }
			return false; };

		RuleConditioner makefunofCondition = (self, other, indPpl) =>
		{	if(self != other  && self.moods[MoodTypes.energTired] > -0.6f && self.moods[MoodTypes.hapSad] > -0.8f && self.moods[MoodTypes.hapSad] < 0.4f && roomMan.IsPersonInSameRoomAsMe(self, other)) 
				{ return true; }
			return false; };

		RuleConditioner telljokeCondition = (self, other, indPpl) =>
		{	if(self != other  && self.moods[MoodTypes.energTired] > -0.5f && self.moods[MoodTypes.hapSad] > -0.4f && roomMan.IsPersonInSameRoomAsMe(self, other)) 
			{ return true; }
			return false; };

		RuleConditioner prankCondition = (self, other, indPpl) =>
		{	if(self != other  && self.moods[MoodTypes.energTired] > -0.5f && self.moods[MoodTypes.hapSad] > -0.4f && roomMan.IsPersonInSameRoomAsMe(self, other)) 
			{ if(self.GetOpinionValue(TraitTypes.NiceNasty,other) < 0.4f) {return true;} }
			return false; };

		RuleConditioner harassCondition = (self, other, indPpl) =>
		{	if(self != other  && self.moods[MoodTypes.energTired] > -0.5f && self.moods[MoodTypes.hapSad] > -0.4f && roomMan.IsPersonInSameRoomAsMe(self, other)) 
			{ if(self.GetOpinionValue(TraitTypes.NiceNasty,other) < 0.0f) {return true;} }
			return false; };

		RuleConditioner playgameCondition = (self, other, indPpl) =>
		{	if(beings.Find(x=>x.name == self.name).possessions.Exists(y=>y.Name=="game") && roomMan.IsPersonInSameRoomAsMe(self, other)){
				if(beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="game").value > 0){
					if(self != other  && self.moods[MoodTypes.energTired] > -0.2f) 
						{return true;} 
				}
			}
			return false; };

		RuleConditioner orderCondition = (self, other, indPpl) =>
		{	if(self != other && (self.culture.Exists(x => x.roleName == "bunce") || self.culture.Exists(x => x.roleName == "buncess")) && roomMan.IsPersonInSameRoomAsMe(self, other)){ 
					if(self.GetOpinionValue(TraitTypes.NiceNasty,other) < 0.4f) 
						{ return true;} 
			}
			if(self != other && roomMan.IsPersonInSameRoomAsMe(self, other)){
				if(self.CalculateTraitType(TraitTypes.CharitableGreedy) < 0.1f && self.GetOpinionValue(TraitTypes.NiceNasty,other) < 0.2f){
					return true;
				}
			}
			return false; };

		RuleConditioner killCondition = (self, other, indPpl) =>
		{	if((relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["steal"] && (x.GetSubject()==other)) ||
			       relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["fight"] && (x.GetSubject()==other)) ||
			       relationSystem.historyBook.Exists(x=>x.GetRule().GetRuleStrength() < -0.4f && x.GetTime() < 10f)) && roomMan.IsPersonInSameRoomAsMe(self, other) && self != other)
				   		{ return true; }

			if(self != other){
				if(self.GetOpinionValue(TraitTypes.NiceNasty,other) < -0.7 && self.CalculateTraitType(TraitTypes.NiceNasty) < -0.4){
					return true;
				}
			}

			return false; };

		RuleConditioner buyCompanyCondition = (self, other, indPpl) =>
		{	if(beings.Find(x=>x.name == other.name).possessions.Exists(y=>y.Name=="company")){
				if(beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="money").value >= 100f && beings.Find(x=>x.name == other.name).possessions.Find(y=>y.Name=="company").value >= 1f
				   && self != other && roomMan.IsPersonInSameRoomAsMe(self, other)) { return true; }
			}
			return false; };

		RuleConditioner sellCompanyCondition = (self, other, indPpl) =>
		{	if(beings.Find(x=>x.name == other.name).possessions.Exists(y=>y.Name=="company")){
				if(beings.Find(x=>x.name == other.name).possessions.Find(y=>y.Name=="money").value >= 100f && beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="company").value >= 1f
				   && self != other && roomMan.IsPersonInSameRoomAsMe(self, other)) { return true; }
			}
			return false; };

		RuleConditioner sabotageCondition = (self, other, indPpl) =>
		{	if(beings.Find(x=>x.name == other.name).possessions.Exists(y=>y.Name=="company")){
				if(beings.Find(x=>x.name == other.name).possessions.Find(y=>y.Name=="company").value > 0f){
					if(self.CalculateTraitType(TraitTypes.NiceNasty) < -0.1f && self != other  &&
					   self.moods[MoodTypes.energTired] > -0.4f){
						{ return true; }
					}
				}
			}
			return false; };

		RuleConditioner advertiseCondition = (self, other, indPpl) =>
		{	if(self != other  && self.moods[MoodTypes.energTired] > -0.4f && roomMan.IsPersonInSameRoomAsMe(self, other)) { return true; }
			return false; };

		RuleConditioner DemandtoLeaveGuildCondition = (self, other, indPpl) =>
		{	if(self.CalculateTraitType(TraitTypes.NiceNasty) < -0.3f && self != other &&
			    beings.Find(x=>x.name == other.name).possessions.Find(y=>y.Name=="money").value <= 0f && roomMan.IsPersonInSameRoomAsMe(self, other)){
						{ return true; }
			} 
			return false; };

		RuleConditioner buyGoodsCondition = (self, other, indPpl) =>
		{	if(beings.Find(x=>x.name == self.name).possessions.Exists(y=>y.Name=="goods")){
				if(beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="goods").value < 2f && beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="money").value > 30f
				   && self != other && self.moods[MoodTypes.energTired] > -0.4f && roomMan.IsPersonInSameRoomAsMe(self, other)) 
					{ return true; }
			}
			return false; };

		RuleConditioner sellGoodsCondition = (self, other, indPpl) =>
		{	if(beings.Find(x=>x.name == self.name).possessions.Exists(y=>y.Name=="goods")){
				if(beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="goods").value > 1f && self != other && self.moods[MoodTypes.energTired] > -0.4f && roomMan.IsPersonInSameRoomAsMe(self, other)) 
					{ return true; }
			}
			return false; };

		RuleConditioner movetolivingroomCondition = (self, other, indPpl) =>
		{	
			if(relationSystem.historyBook.Find(x=> (x.GetAction()==relationSystem.posActions["movetolivingroom"] || x.GetAction()==relationSystem.posActions["movetokitchen"] || x.GetAction()==relationSystem.posActions["movetoentryhall"]) && x.GetSubject()==self).GetTime() < 10f){
				return false;
			}
			if(self != null && !(roomMan.GetRoomIAmIn(self) == "Stue"))
				if((self.moods[MoodTypes.energTired] < -0.2f)){
					return true;
				}
				else if(roomMan.GetRoomIAmIn(other) == "Stue" && self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.0f){
					return true;
				}
			return false; };

		RuleConditioner movetoentryhallCondition = (self, other, indPpl) =>
        {
			if(relationSystem.historyBook.Find(x=> (x.GetAction()==relationSystem.posActions["movetolivingroom"] || x.GetAction()==relationSystem.posActions["movetokitchen"] || x.GetAction()==relationSystem.posActions["movetoentryhall"]) && x.GetSubject()==self).GetTime() < 10f){
				return false;
			}
			if (self != null && (roomMan.GetRoomIAmIn(self) == "Stue")) { 
				if((self.moods[MoodTypes.energTired] < -0.2f)){
					return true;
				}
				else if(roomMan.GetRoomIAmIn(other) == "Indgang" && self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.0f){
					return true;
				}
			}
			return false; };

		RuleConditioner movetokitchenCondition = (self, other, indPpl) =>
        {
			if(relationSystem.historyBook.Find(x=> (x.GetAction()==relationSystem.posActions["movetolivingroom"] || x.GetAction()==relationSystem.posActions["movetokitchen"] || x.GetAction()==relationSystem.posActions["movetoentryhall"]) && x.GetSubject()==self).GetTime() < 10f){
					return false;
				}
            if (self != null && (roomMan.GetRoomIAmIn(self) == "Stue")) { 
				if((self.moods[MoodTypes.energTired] < -0.2f)){
					return true;
				}
				else if(roomMan.GetRoomIAmIn(other) == "Køkken" && self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.0f){
					return true;
				}
			}
			return false; };

		#endregion adding Conditions




		RulePreference doNothingPreference = (self, other) => {
			return (-1 + Calculator.UnboundAdd((-self.moods[MoodTypes.energTired]),-1));
		};

		RulePreference fleePreference = (self, other) => {
			return -Calculator.UnboundAdd(self.CalculateTraitType(TraitTypes.HonestFalse),self.moods[MoodTypes.angryFear]);
		};

		RulePreference greetPreference = (self, other) => {
			return self.GetOpinionValue(TraitTypes.NiceNasty,other);
		};

		RulePreference kissPreference = (self, other) => {
			if(self.interPersonal.Exists(x=>x.GetRoleRefPpl().Exists(y=>y.name == other.name && y.interPersonal.Exists(z=>z.roleName == "partner" && z.GetRoleRefPpl().Exists(s=>s.name == self.name))) && x.roleName == "partner")){
				return Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),self.moods[MoodTypes.arousDisgus]);
			}
			else{
				float ret = 0 + Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),(0.5f*self.GetOpinionValue(TraitTypes.CharitableGreedy,other)));
				ret += Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.HonestFalse,other),ret);
				ret += Calculator.UnboundAdd(self.moods[MoodTypes.arousDisgus],ret);
				return ret;
			}
		};

		RulePreference flirtPreference = (self, other) => { 
			float r = Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),self.moods[MoodTypes.arousDisgus]);
			r += Calculator.UnboundAdd(Calculator.NegPosTransform(self.interPersonal.Find(x=>x.GetRoleRefPpl().Exists(y=>y==other)).GetlvlOfInfl(other)),r);
			return r;
		};

		RulePreference chatPreference = (self, other) => { 
			return Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),Calculator.NegPosTransform(self.interPersonal.Find(x=>x.GetRoleRefPpl().Exists(y=>y==other)).GetlvlOfInfl(other)));
		};

		RulePreference giveGiftPreference = (self, other) => { 
			float r = Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),self.GetOpinionValue(TraitTypes.CharitableGreedy,other));
			r += Calculator.UnboundAdd(self.CalculateTraitType(TraitTypes.CharitableGreedy),r);
			return r;
		};

		RulePreference poisonPreference = (self, other) => { 
			float r = Calculator.UnboundAdd(-(self.GetOpinionValue(TraitTypes.NiceNasty,other)),-(self.CalculateTraitType(TraitTypes.NiceNasty)));
			r += Calculator.UnboundAdd(-self.CalculateTraitType(TraitTypes.HonestFalse),r);
			r += Calculator.UnboundAdd(-self.moods[MoodTypes.angryFear],r);
			return r;
		};

		RulePreference gossipPreference = (self, other) => { 
			float r = Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),-self.CalculateTraitType(TraitTypes.NiceNasty));
			r += Calculator.UnboundAdd(-self.CalculateTraitType(TraitTypes.HonestFalse),r);
			return r;
		};

		RulePreference arguePreference = (self, other) => { 
			float r = Calculator.UnboundAdd(-(self.GetOpinionValue(TraitTypes.NiceNasty,other)),-self.GetOpinionValue(TraitTypes.HonestFalse,other));
			r += Calculator.UnboundAdd(-self.CalculateTraitType(TraitTypes.NiceNasty),r);
			r += Calculator.UnboundAdd(-self.GetOpinionValue(TraitTypes.HonestFalse,other),r);
			return r;
		};

		RulePreference makeDistractionPreference = (self, other) => {
			float r = Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),self.CalculateTraitType(TraitTypes.NiceNasty));
			r += Calculator.UnboundAdd(-self.moods[MoodTypes.angryFear],r);
			return -r;
		};

		RulePreference reminiscePreference = (self, other) => { 
			float r = Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),Calculator.NegPosTransform(self.interPersonal.Find(x=>x.GetRoleRefPpl().Exists(y=>y==other)).GetlvlOfInfl(other)));
			r += Calculator.UnboundAdd(-self.moods[MoodTypes.energTired],r);
			return r;
		};

		RulePreference denyPreference = (self, other) => { 
			float r = Calculator.UnboundAdd(-self.GetOpinionValue(TraitTypes.NiceNasty,other),-self.GetOpinionValue(TraitTypes.HonestFalse,other));
			r += Calculator.UnboundAdd((0.5f*(-self.CalculateTraitType(TraitTypes.NiceNasty))),r);
			r += Calculator.UnboundAdd(-(Calculator.NegPosTransform(self.interPersonal.Find(x=>x.GetRoleRefPpl().Exists(y=>y==other)).GetlvlOfInfl(other))),r);
			return r;
		};

		RulePreference praisePreference = (self, other) => { 
			float r = Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),self.GetOpinionValue(TraitTypes.HonestFalse,other));
			r += Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.CharitableGreedy,other),r);
			r += Calculator.UnboundAdd(self.moods[MoodTypes.hapSad],r);
			r += Calculator.UnboundAdd(self.CalculateTraitType(TraitTypes.NiceNasty),r);
			return r;
		};

		RulePreference cryPreference = (self, other) => { 
			float r = 0;
			if(self != other){
				if(relationSystem.historyBook.Exists(x=>(x.GetAction()==relationSystem.posActions["kill"] || x.GetAction()==relationSystem.posActions["flee"] ) && x.GetTime() < 10f)){
					//debug.Write("THIS ONE "+self.moods[MoodTypes.hapSad]+" "+self.CalculateTraitType(TraitTypes.NiceNasty));
					r = Calculator.UnboundAdd(self.CalculateTraitType(TraitTypes.NiceNasty),self.moods[MoodTypes.hapSad]);
				}
				else if(relationSystem.historyBook.Exists(x=>(x.GetAction()==relationSystem.posActions["sabotage"] || x.GetAction()==relationSystem.posActions["poison"]
				                                              || x.GetAction()==relationSystem.posActions["fight"]) && x.GetTime() < 10f)){
					//debug.Write("OR THIS ONE "+self.moods[MoodTypes.hapSad]+" "+self.CalculateTraitType(TraitTypes.NiceNasty));
					r = Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),self.moods[MoodTypes.hapSad]);
					r += Calculator.UnboundAdd(self.CalculateTraitType(TraitTypes.NiceNasty),r);
				}
				else{
					//debug.Write("OR END "+self.moods[MoodTypes.hapSad]+" "+self.CalculateTraitType(TraitTypes.NiceNasty));
					r = Calculator.UnboundAdd(self.CalculateTraitType(TraitTypes.NiceNasty),self.moods[MoodTypes.hapSad]);
				}
			}
			else{
				r = Calculator.UnboundAdd(self.moods[MoodTypes.hapSad],-0.5f);
			}

			return r;
		};

		RulePreference consolePreference = (self, other) => {
			float r = Calculator.UnboundAdd (self.GetOpinionValue (TraitTypes.NiceNasty, other), self.CalculateTraitType (TraitTypes.NiceNasty));
			return r;
		};

		RulePreference askAboutPartnerStatusPreference = (self, other) => {
			if(self.interPersonal.Exists(x=>x.GetRoleRefPpl().Exists(y=>y.name == other.name && y.interPersonal.Exists(z=>z.roleName == "partner" && z.GetRoleRefPpl().Exists(s=>s.name == self.name))) && x.roleName == "partner")){
				return Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),Calculator.NegPosTransform(self.interPersonal.Find(x=>x.GetRoleRefPpl().Exists(y=>y==other)).GetlvlOfInfl(other)));
			}
			else{
				float r = Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),Calculator.NegPosTransform(self.interPersonal.Find(x=>x.GetRoleRefPpl().Exists(y=>y==other)).GetlvlOfInfl(other)));
				r += Calculator.UnboundAdd(self.moods[MoodTypes.arousDisgus],r);
				return r;
			}
		};

		RulePreference chooseAnotherAsPartnerPreference = (self, other) => {
			float r = Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),self.GetOpinionValue(TraitTypes.HonestFalse,other));
			r += Calculator.UnboundAdd(Calculator.NegPosTransform(self.interPersonal.Find(x=>x.GetRoleRefPpl().Exists(y=>y==other)).GetlvlOfInfl(other)),r);
			r += Calculator.UnboundAdd(self.moods[MoodTypes.arousDisgus],r);
			return r;
		};

		RulePreference StayAsPartnerPreference = (self, other) => { 
			float r = Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),self.GetOpinionValue(TraitTypes.HonestFalse,other));
			r += Calculator.UnboundAdd(Calculator.NegPosTransform(self.interPersonal.Find(x=>x.GetRoleRefPpl().Exists(y=>y==other)).GetlvlOfInfl(other)),r);
			return r;
		};

		RulePreference LeavePartnerPreference = (self, other) => { 
			float r = Calculator.UnboundAdd(-self.GetOpinionValue(TraitTypes.NiceNasty,other),-self.GetOpinionValue(TraitTypes.HonestFalse,other));
			r += Calculator.UnboundAdd(-Calculator.NegPosTransform(self.interPersonal.Find(x=>x.GetRoleRefPpl().Exists(y=>y==other)).GetlvlOfInfl(other)),r);
			return r;
		};

		RulePreference convictPreference = (self, other) => { 
			float r = Calculator.UnboundAdd(-0.5f,((Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),(self.moods[MoodTypes.hapSad]))*0.5f)));
			r += Calculator.UnboundAdd(Mathf.Abs(self.moods[MoodTypes.angryFear]),r);
			return r;
		};

		RulePreference fightPreference = (self, other) => { 
			float r = Calculator.UnboundAdd(-self.GetOpinionValue(TraitTypes.NiceNasty,other),-self.CalculateTraitType(TraitTypes.NiceNasty));
			r += Calculator.UnboundAdd(-self.moods[MoodTypes.angryFear],r);
			return r;
		};

		RulePreference bribePreference = (self, other) => {
			float r = Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.HonestFalse,other),-self.CalculateTraitType(TraitTypes.HonestFalse));
			r += Calculator.UnboundAdd(self.moods[MoodTypes.angryFear],r);
			r += Calculator.UnboundAdd(-self.moods[MoodTypes.energTired],r);
			return r;
		};

		RulePreference argueInnocencePreference = (self, other) => {
			float r = Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),self.CalculateTraitType(TraitTypes.NiceNasty));
			r += Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.HonestFalse,other),r);
			return r;
		};

		RulePreference argueGuiltinessPreference = (self, other) => { 
			float r = Calculator.UnboundAdd(-self.GetOpinionValue(TraitTypes.NiceNasty,other),-self.CalculateTraitType(TraitTypes.NiceNasty));
			r += Calculator.UnboundAdd(-self.GetOpinionValue(TraitTypes.HonestFalse,other),r);
			return r;
		};

		RulePreference stealPreference = (self, other) => { 
			float r = Calculator.UnboundAdd(-self.GetOpinionValue(TraitTypes.NiceNasty,other),-self.CalculateTraitType(TraitTypes.NiceNasty));
			r += Calculator.UnboundAdd(self.CalculateTraitType(TraitTypes.CharitableGreedy),r);
			return r;
		};

		RulePreference makeFunOfPreference = (self, other) => { 
			float r = Calculator.UnboundAdd(-self.GetOpinionValue(TraitTypes.NiceNasty,other),-self.CalculateTraitType(TraitTypes.NiceNasty));
			r += Calculator.UnboundAdd(-self.moods[MoodTypes.angryFear],r);
			r += Calculator.UnboundAdd(-self.moods[MoodTypes.hapSad],r);
			return r;
		};

		RulePreference telljokePreference = (self, other) => { 
			float r = Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),self.moods[MoodTypes.hapSad]);
			r += Calculator.UnboundAdd(self.moods[MoodTypes.energTired],r);
			return r;
		};

		RulePreference harassPreference = (self, other) => { 
			float r = Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),self.CalculateTraitType(TraitTypes.NiceNasty));
			r += Calculator.UnboundAdd(self.moods[MoodTypes.angryFear],r);
			r += Calculator.UnboundAdd(self.moods[MoodTypes.hapSad],r);
			return -r;
		};

		RulePreference prankPreference = (self, other) => {
			float r = Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),self.CalculateTraitType(TraitTypes.NiceNasty));
			r += Calculator.UnboundAdd(self.moods[MoodTypes.angryFear],r);
			r += Calculator.UnboundAdd(self.moods[MoodTypes.hapSad],r);
			return -r;
		};

		RulePreference playgamePreference = (self, other) => { 
			float r = Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),self.CalculateTraitType(TraitTypes.NiceNasty));
			r += Calculator.UnboundAdd(self.moods[MoodTypes.hapSad],r);
			r += Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.HonestFalse,other),r);
			return r;
		};


		RulePreference orderPreference = (self, other) => { 
			float r = Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),self.CalculateTraitType(TraitTypes.NiceNasty));
			r += Calculator.UnboundAdd(-(self.moods[MoodTypes.energTired]),r);
			return -r;
		};

		RulePreference killPreference = (self, other) => { 
			float ret = Calculator.UnboundAdd(-self.GetOpinionValue(TraitTypes.NiceNasty,other),-self.CalculateTraitType(TraitTypes.NiceNasty));
			ret += Calculator.UnboundAdd(-self.moods[MoodTypes.angryFear],ret);
			ret += Calculator.UnboundAdd(-self.moods[MoodTypes.arousDisgus],ret);
			ret += Calculator.UnboundAdd(-self.moods[MoodTypes.hapSad],ret);
			return ret;
		};


		RulePreference buyCompanyPreference = (self, other) => {
			return Calculator.UnboundAdd(self.CalculateTraitType(TraitTypes.CharitableGreedy),(-self.GetOpinionValue(TraitTypes.NiceNasty,other)));
		};

		RulePreference sellCompanyPreference = (self, other) => {
			return -(Calculator.UnboundAdd(self.CalculateTraitType(TraitTypes.CharitableGreedy),(-self.GetOpinionValue(TraitTypes.NiceNasty,other))));
		};

		RulePreference advertisePreference = (self, other) => {
			return Calculator.UnboundAdd(self.CalculateTraitType(TraitTypes.CharitableGreedy),-self.CalculateTraitType(TraitTypes.NiceNasty));
		};

		RulePreference sabotagePreference = (self, other) => { 
			float r = Calculator.UnboundAdd(-self.CalculateTraitType(TraitTypes.CharitableGreedy),self.moods[MoodTypes.angryFear]);
			r += Calculator.UnboundAdd(-self.GetOpinionValue(TraitTypes.NiceNasty,other),r);
			r += Calculator.UnboundAdd(-self.CalculateTraitType(TraitTypes.NiceNasty),r);
			return r;
		};

		RulePreference demandoLeaveGuildPreference = (self, other) => { 
			float r = Calculator.UnboundAdd(-self.CalculateTraitType(TraitTypes.CharitableGreedy),-self.GetOpinionValue(TraitTypes.NiceNasty,other));
			r += Calculator.UnboundAdd(-self.CalculateTraitType(TraitTypes.NiceNasty),r);
			return r;
		};

		RulePreference buyGoodsPreference = (self, other) => { 
			return 0.5f*self.CalculateTraitType(TraitTypes.CharitableGreedy);
		};

		RulePreference sellGoodsPreference = (self, other) => { 
			return (0.5f*self.CalculateTraitType(TraitTypes.CharitableGreedy));
		};

		RulePreference moveToLivingRoomPreference = (self, other) => { 
			float r = Calculator.UnboundAdd(0.1f,-self.moods[MoodTypes.energTired]);
			r += Calculator.UnboundAdd(-self.moods[MoodTypes.hapSad],r);
			r += Calculator.UnboundAdd(-self.moods[MoodTypes.angryFear],r);
			return r;
		};

		RulePreference moveToKitchenPreference = (self, other) => { 
			float r = Calculator.UnboundAdd(0.1f,-self.moods[MoodTypes.energTired]);
			r += Calculator.UnboundAdd(-self.moods[MoodTypes.hapSad],r);
			r += Calculator.UnboundAdd(-self.moods[MoodTypes.angryFear],r);
			return r;
		};

		RulePreference moveToEntryHallPreference = (self, other) => { 
			float r = Calculator.UnboundAdd(0.1f,-self.moods[MoodTypes.energTired]);
			r += Calculator.UnboundAdd(-self.moods[MoodTypes.hapSad],r);
			r += Calculator.UnboundAdd(-self.moods[MoodTypes.angryFear],r);
			return r;
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
		relationSystem.CreateNewRule("makedistraction", "makedistraction", makeDistractionCondition,makeDistractionPreference);
		relationSystem.CreateNewRule("reminisce", "reminisce", reminisceCondition,reminiscePreference);
		relationSystem.CreateNewRule("praise", "praise", praiseCondition,praisePreference);
		relationSystem.CreateNewRule("makefunof", "makefunof", makefunofCondition,makeFunOfPreference);
		relationSystem.CreateNewRule("telljoke", "telljoke", telljokeCondition,telljokePreference);
		relationSystem.CreateNewRule("prank", "prank", prankCondition,prankPreference);
		relationSystem.CreateNewRule("harass", "harass", harassCondition,harassPreference);
		relationSystem.CreateNewRule("cry", "cry", cryCondition,cryPreference);
		relationSystem.CreateNewRule("console", "console", consoleCondition,consolePreference);

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

		relationSystem.CreateNewRule("movetolivingroomfbunce", "movetolivingroom", movetolivingroomCondition,moveToLivingRoomPreference);
		relationSystem.CreateNewRule("movetolivingroomfcess", "movetolivingroom", movetolivingroomCondition,moveToLivingRoomPreference);
		relationSystem.CreateNewRule("movetolivingroomfbunsant", "movetolivingroom", movetolivingroomCondition,moveToLivingRoomPreference);
		relationSystem.CreateNewRule("movetokitchenfbunce", "movetokitchen", movetokitchenCondition,moveToKitchenPreference);
		relationSystem.CreateNewRule("movetokitchenfcess", "movetokitchen", movetokitchenCondition,moveToKitchenPreference);
		relationSystem.CreateNewRule("movetokitchenfbunsant", "movetokitchen", movetokitchenCondition,moveToKitchenPreference);
		relationSystem.CreateNewRule("movetoentryhallfbunce", "movetoentryhall", movetoentryhallCondition,moveToEntryHallPreference);
		relationSystem.CreateNewRule("movetoentryhallfcess", "movetoentryhall", movetoentryhallCondition,moveToEntryHallPreference);
		relationSystem.CreateNewRule("movetoentryhallfbunsant", "movetoentryhall", movetoentryhallCondition,moveToEntryHallPreference);
		relationSystem.CreateNewRule("buycompany", "buycompany", buyCompanyCondition,buyCompanyPreference);
		relationSystem.CreateNewRule("sabotage", "sabotage", sabotageCondition,sabotagePreference);
		relationSystem.CreateNewRule("advertise", "advertise", advertiseCondition,advertisePreference);
		relationSystem.CreateNewRule("demandtoleaveguild", "demandtoleaveguild", DemandtoLeaveGuildCondition,demandoLeaveGuildPreference);
		relationSystem.CreateNewRule("sellcompany", "sellcompany", sellCompanyCondition,sellCompanyPreference);
		relationSystem.CreateNewRule("buygoods", "buygoods", buyGoodsCondition,buyCompanyPreference);
		relationSystem.CreateNewRule("sellgoods", "sellgoods", sellGoodsCondition,sellGoodsPreference);

		//SELF RULES
		relationSystem.CreateNewRule("donothing", "donothing", emptyCondition, doNothingPreference);
		relationSystem.CreateNewRule("flee", "flee", fleeCondition,fleePreference);


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

		List<Rule> makedistractionRulesToTrigger = new List<Rule>(); makedistractionRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue")); makedistractionRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("poison")); 
		makedistractionRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("steal")); makedistractionRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("deny")); makedistractionRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("sabotage"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("makedistraction",makedistractionRulesToTrigger);

		List<Rule> reminisceRulesToTrigger = new List<Rule>(); reminisceRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("chat")); reminisceRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("gossip"));
		reminisceRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("flirt"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("reminisce",reminisceRulesToTrigger);

		List<Rule> praiseRulesToTrigger = new List<Rule>(); praiseRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("chat"));
		praiseRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("flirt")); praiseRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("fight"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("praise",praiseRulesToTrigger);

		List<Rule> makefunofRulesToTrigger = new List<Rule>(); makefunofRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("makefunof")); makefunofRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue"));
		makefunofRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("harass")); makefunofRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("deny"));
		makefunofRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("telljoke"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("makefunof",makefunofRulesToTrigger);

		List<Rule> telljokeRulesToTrigger = new List<Rule>(); telljokeRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("makefunof")); telljokeRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("telljoke")); 
		telljokeRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("chat")); telljokeRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("praise")); 
		relationSystem.pplAndMasks.AddPossibleRulesToRule("telljoke",telljokeRulesToTrigger);

		List<Rule> prankRulesToTrigger = new List<Rule>(); prankRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("makefunof")); prankRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("deny"));
		prankRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("convict")); prankRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue")); prankRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("order"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("prank",prankRulesToTrigger);

		List<Rule> harassRulesToTrigger = new List<Rule>(); harassRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("telljoke")); harassRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("deny"));
		harassRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue")); harassRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("fight")); harassRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("order"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("harass",harassRulesToTrigger);

		List<Rule> cryRulesToTrigger = new List<Rule>(); cryRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("cry")); cryRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("reminisce"));
		cryRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("givegift")); cryRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("kiss")); cryRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("console")); 
		relationSystem.pplAndMasks.AddPossibleRulesToRule("cry",cryRulesToTrigger);

		List<Rule> consoleRulesToTrigger = new List<Rule>(); consoleRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("chat")); consoleRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("reminisce"));
		consoleRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("kiss"));
		relationSystem.pplAndMasks.AddPossibleRulesToRule("console",consoleRulesToTrigger);

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

		List<Rule> demandtoleaveguildRulesToTrigger = new List<Rule>();
		demandtoleaveguildRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("argue")); demandtoleaveguildRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("poison")); 
		demandtoleaveguildRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("fight")); demandtoleaveguildRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("deny")); 
		demandtoleaveguildRulesToTrigger.Add(relationSystem.pplAndMasks.GetRule("order")); 
		relationSystem.pplAndMasks.AddPossibleRulesToRule("demandtoleaveguild",demandtoleaveguildRulesToTrigger);

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
		relationSystem.AddRuleToMask("Player", "Self", "donothing", -1.0f);

		relationSystem.AddRuleToMask("John", "Self", "flee", 0.2f);
		relationSystem.AddRuleToMask("Heather", "Self", "flee", -0.1f);
		relationSystem.AddRuleToMask("Player", "Self", "flee", -0.1f);

		relationSystem.AddRuleToMask("John", "Self", "chooseanotheraspartner", -0.2f);
		relationSystem.AddRuleToMask("Therese", "Self", "chooseanotheraspartner", -0.2f);
		relationSystem.AddRuleToMask("Bill", "Self", "chooseanotheraspartner", 0.4f);
		relationSystem.AddRuleToMask("Heather", "Self", "chooseanotheraspartner", 0.5f);
		relationSystem.AddRuleToMask("Player", "Self", "chooseanotheraspartner", -0.4f);
		
	// INTERPERSONAL
		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "kiss", 0.4f);
		
		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "askAboutPartnerStatus", 0.5f);
		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "stayaspartner", 0.2f);
		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "leavepartner", 0.0f);

		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "flirt", 0.4f);
		relationSystem.AddRuleToMask("Friendship", "Friend", "flirt", -0.4f);

		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "chat", 0.0f);
		relationSystem.AddRuleToMask("Friendship", "Friend", "chat", 0.0f);
		relationSystem.AddRuleToMask("Rivalry", "Enemy", "chat", -0.2f);

		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "givegift", 0.4f);
		relationSystem.AddRuleToMask("Rivalry", "Enemy", "givegift", 0.2f);
		relationSystem.AddRuleToMask("Friendship", "Friend", "givegift", -0.3f);

		relationSystem.AddRuleToMask("Rivalry", "Enemy", "gossip", -0.2f);
		relationSystem.AddRuleToMask("Friendship", "Friend", "gossip", 0.1f);
		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "gossip", -0.2f);

		relationSystem.AddRuleToMask("Rivalry", "Enemy", "argue", 0.2f);
		relationSystem.AddRuleToMask("Friendship", "Friend", "argue", -0.2f);
		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "argue", -0.4f);

		relationSystem.AddRuleToMask("Rivalry", "Enemy", "deny", 0.3f);
		relationSystem.AddRuleToMask("Friendship", "Friend", "deny", -0.1f);
		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "deny", -0.2f);

		relationSystem.AddRuleToMask("Rivalry", "Enemy", "makedistraction", -0.1f);
		relationSystem.AddRuleToMask("Friendship", "Friend", "makedistraction", -0.3f);
		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "makedistraction", -0.5f);

		relationSystem.AddRuleToMask("Rivalry", "Enemy", "makefunof", 0.3f);
		relationSystem.AddRuleToMask("Friendship", "Friend", "makefunof", -0.1f);
		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "makefunof", -0.4f);

		relationSystem.AddRuleToMask("Rivalry", "Enemy", "telljoke", -0.2f);
		relationSystem.AddRuleToMask("Friendship", "Friend", "telljoke", 0.4f);
		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "telljoke", 0.2f);

		relationSystem.AddRuleToMask("Rivalry", "Enemy", "prank", 0.2f);
		relationSystem.AddRuleToMask("Friendship", "Friend", "prank", 0.0f);
		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "prank", -0.3f);

		relationSystem.AddRuleToMask("Rivalry", "Enemy", "harass", 0.3f);
		relationSystem.AddRuleToMask("Friendship", "Friend", "harass", -0.4f);
		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "harass", -0.6f);

		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "reminisce", 0.3f);
		relationSystem.AddRuleToMask("Friendship", "Friend", "reminisce", 0.1f);

		relationSystem.AddRuleToMask("Friendship", "Friend", "praise", 0.3f);
		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "praise", 0.2f);

		relationSystem.AddRuleToMask("Friendship", "Friend", "cry", 0.8f);
		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "cry", 1.0f);
		relationSystem.AddRuleToMask("Rivalry", "Enemy", "cry", 0.6f);

		relationSystem.AddRuleToMask("Friendship", "Friend", "console", 0.8f);
		relationSystem.AddRuleToMask("RomanticRelationship", "Partner", "console", 1.0f);
		relationSystem.AddRuleToMask("Rivalry", "Enemy", "console", -0.4f);

	// CULTURE
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "fight", -0.5f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "bribefbunsant", -0.1f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "steal", -0.5f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "askforhelpinillicitactivity", -0.1f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "poisonfbunsant", -0.8f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "greetfbunsant", 0.5f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "playgamefbunsant", 0.2f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "orderfbunsant", -0.5f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "movetolivingroomfbunsant", 0.0f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "movetokitchenfbunsant", 0.0f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "movetoentryhallfbunsant", 0.0f);
		relationSystem.AddRuleToMask("Bungary", "Bunsant", "killfbunsant", -0.9f);

		relationSystem.AddRuleToMask("Bungary", "Bunce", "bribefbunce", 0.3f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "convictfbunce", 1.0f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "argueinnocencefbunce", 0.0f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "argueguiltinessfbunce", 0.0f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "poisonfbunce", -0.8f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "greetfbunce", 1.0f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "playgamefbunce", 0.2f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "orderfbunce", 0.5f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "movetolivingroomfbunce", 0.5f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "movetokitchenfbunce", 0.1f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "movetoentryhallfbunce", 0.3f);
		relationSystem.AddRuleToMask("Bungary", "Bunce", "killfbunce", -0.8f);

		relationSystem.AddRuleToMask("Bungary", "Buncess", "bribefcess", 0.3f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "convictfcess", 0.8f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "argueinnocencefcess", 0.2f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "argueguiltinessfcess", -0.1f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "poisonfcess", -0.8f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "greetfcess", 1.0f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "playgamefbuncess", 0.2f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "orderfcess", 0.5f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "movetolivingroomfcess", 0.6f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "movetokitchenfcess", 0.2f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "movetoentryhallfcess", 0.3f);
		relationSystem.AddRuleToMask("Bungary", "Buncess", "killfcess", -0.9f);

		relationSystem.AddRuleToMask("MerchantGuild", "Member", "buycompany", 0.2f);
		relationSystem.AddRuleToMask("MerchantGuild", "Member", "sabotage", -0.5f);
		relationSystem.AddRuleToMask("MerchantGuild", "Member", "advertise", 0.5f);
		relationSystem.AddRuleToMask("MerchantGuild", "Member", "demandtoleaveguild", -0.3f);
		relationSystem.AddRuleToMask("MerchantGuild", "Member", "sellcompany", -0.5f);
		relationSystem.AddRuleToMask("MerchantGuild", "Member", "buygoods", 0.5f);
		relationSystem.AddRuleToMask("MerchantGuild", "Member", "sellgoods", 0.7f);

		#endregion Rules


//  ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		//PEOPLE

		
		#region AddingPlayer
		MaskAdds selfPersMask = new MaskAdds("Self", "Player", 0.0f);
	
		List<MaskAdds>  culture = new List<MaskAdds>();
		culture.Add(new MaskAdds("Bunce", "Bungary", 0.6f));
		culture.Add(new MaskAdds("Member", "MerchantGuild", 0.4f));

		relationSystem.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.4f, 0.6f, 0.5f, new float[] { 0.4f, 0.2f, 0.5f },new float[]{0.0f,0.0f,0.0f});
		#endregion AddingPlayer
		
		#region AddingBill
		selfPersMask = new MaskAdds("Self", "Bill", 0.0f);
		
		culture = new List<MaskAdds>();
		culture.Add(new MaskAdds("Bunce", "Bungary", 0.4f));
		//culture.Add(new MaskAdds("Follower", "Cult", 0.4f,new List<Person>()));
		culture.Add(new MaskAdds("Member", "MerchantGuild", 0.5f));
		
		relationSystem.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.6f, 0.4f, 0.7f, new float[] { -0.4f, -0.5f, -0.1f },new float[]{0.0f,0.0f,0.0f});
		#endregion AddingBill
		
		#region AddingTerese
		selfPersMask = new MaskAdds("Self", "Therese", 0.0f);
		
		culture = new List<MaskAdds>();
		culture.Add(new MaskAdds("Buncess", "Bungary", 0.6f));
		//culture.Add(new MaskAdds("Sceptic", "Cult", 0.1f,new List<Person>()));
		
		relationSystem.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.3f, 0.7f, 0.2f, new float[] { 0.6f, 0.2f, 0.6f },new float[]{0.0f,0.0f,0.0f});
		#endregion AddingTerese
		
		#region AddingJohn
		selfPersMask = new MaskAdds("Self", "John", 0.0f);
		
		culture = new List<MaskAdds>();
		//culture.Add(new MaskAdds("Follower", "Cult", 0.4f,new List<Person>()));
		culture.Add(new MaskAdds("Bunsant", "Bungary", 0.1f));
		
		relationSystem.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.5f, 0.5f, 0.4f, new float[] { 0.0f, -0.5f, -0.3f },new float[]{0.0f,0.0f,0.0f});
		#endregion AddingJohn
		
		#region AddingHeather
		selfPersMask = new MaskAdds("Self", "Heather", 0.0f);
		
		culture = new List<MaskAdds>();
		culture.Add(new MaskAdds("Bunsant", "Bungary", 0.4f));
		//culture.Add(new MaskAdds("Leader", "Cult", 0.6f,new List<Person>()));
		
		relationSystem.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.2f, 0.8f, 0.8f, new float[] { 0.2f, 0.3f, 0.2f },new float[]{0.0f,0.0f,0.0f});
		#endregion AddingHeather
		
		#region rolerefs
		//relationSystem.pplAndMasks.GetPerson("bill").GetLinks(TypeMask.culture).Find(x=>x.roleMask.GetMaskName() == "cult").AddRoleRef(relationSystem.pplAndMasks.GetPerson("heather"));
		//relationSystem.pplAndMasks.GetPerson("john").GetLinks(TypeMask.culture).Find(x=>x.roleMask.GetMaskName() == "cult").AddRoleRef(relationSystem.pplAndMasks.GetPerson("heather"));
		#endregion rolerefs

		#region LINKS
	    relationSystem.AddLinkToPerson("Bill", TypeMask.interPers, "Partner", "RomanticRelationship", 0, "Therese", 0.4f);
	    relationSystem.AddLinkToPerson("Bill", TypeMask.interPers, "Enemy", "Rivalry", 0, "John", 0.6f);
	    relationSystem.AddLinkToPerson("Bill", TypeMask.interPers, "Friend", "Friendship",0, "Heather", 0.3f);
		relationSystem.pplAndMasks.GetPerson ("Bill").interPersonal.Find(x=>x.roleMask.GetMaskName()=="rivalry").AddRoleRef(relationSystem.pplAndMasks.GetPerson ("Player"),0.5f);
	   // relationSystem.AddLinkToPerson("Bill", TypeMask.interPers, "Enemy", "Rivalry", 0, "Player", 0.5f);
	
	    relationSystem.AddLinkToPerson("Therese", TypeMask.interPers, "Partner", "RomanticRelationship", 0, "Bill", 0.5f);
	    relationSystem.AddLinkToPerson("Therese", TypeMask.interPers, "Enemy", "Rivalry", 0, "John", 0.2f);
	    relationSystem.AddLinkToPerson("Therese", TypeMask.interPers, "Friend", "Friendship", 0, "Heather", 0.6f);
		relationSystem.pplAndMasks.GetPerson ("Therese").interPersonal.Find(x=>x.roleMask.GetMaskName()=="rivalry").AddRoleRef(relationSystem.pplAndMasks.GetPerson ("Player"),0.3f);
	    //relationSystem.AddLinkToPerson("Therese", TypeMask.interPers, "Enemy", "Rivalry", 0, "Player", 0.3f);
		
	    relationSystem.AddLinkToPerson("John", TypeMask.interPers, "Enemy", "Rivalry", 0, "Bill", 0.7f);
		relationSystem.pplAndMasks.GetPerson ("John").interPersonal.Find(x=>x.roleMask.GetMaskName()=="rivalry").AddRoleRef(relationSystem.pplAndMasks.GetPerson ("Therese"),0.4f);
		//relationSystem.AddLinkToPerson("John", TypeMask.interPers, "Enemy", "Rivalry", 0, "Therese", 0.4f);
	    relationSystem.AddLinkToPerson("John", TypeMask.interPers, "Partner", "Romanticrelationship", 0, "Heather", 0.8f);
	    relationSystem.AddLinkToPerson("John", TypeMask.interPers, "Friend", "Friendship", 0, "Player", 0.5f);
	
	    relationSystem.AddLinkToPerson("Heather", TypeMask.interPers, "Friend", "Friendship", 0, "Bill", 0.3f);
		relationSystem.pplAndMasks.GetPerson ("Heather").interPersonal.Find(x=>x.roleMask.GetMaskName()=="friendship").AddRoleRef(relationSystem.pplAndMasks.GetPerson ("Therese"),0.7f);    
		//relationSystem.AddLinkToPerson("Heather", TypeMask.interPers, "Friend", "Friendship", 0, "Therese", 0.7f);
	    relationSystem.AddLinkToPerson("Heather", TypeMask.interPers, "Partner", "RomanticRelationship", 0, "John", 0.5f);
		relationSystem.pplAndMasks.GetPerson ("Heather").interPersonal.Find(x=>x.roleMask.GetMaskName()=="romanticrelationship").AddRoleRef(relationSystem.pplAndMasks.GetPerson ("Player"),0.5f);
		//relationSystem.AddLinkToPerson("Heather", TypeMask.interPers, "Partner", "RomanticRelationship", 0, "Player", 0.5f);
	
	    relationSystem.AddLinkToPerson("Player", TypeMask.interPers, "Enemy", "Rivalry", 0, "Bill", 0.5f);
		relationSystem.pplAndMasks.GetPerson ("Player").interPersonal.Find(x=>x.roleMask.GetMaskName()=="rivalry").AddRoleRef(relationSystem.pplAndMasks.GetPerson ("Therese"),0.3f);
	    //relationSystem.AddLinkToPerson("Player", TypeMask.interPers, "Enemy", "Rivalry", 0, "Therese", 0.3f);
        relationSystem.AddLinkToPerson("Player", TypeMask.interPers, "Friend", "Friendship", 0, "John", 0.5f);
        relationSystem.AddLinkToPerson("Player", TypeMask.interPers, "Partner", "RomanticRelationship", 0, "Heather", 0.6f);
		#endregion LINKS 

		#region Opinions
		//BILL OPINIONS
		relationSystem.pplAndMasks.GetPerson("bill").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("therese"),0.6f));
		relationSystem.pplAndMasks.GetPerson("bill").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("therese"),0.3f));
		relationSystem.pplAndMasks.GetPerson("bill").opinions.Add(new Opinion(TraitTypes.CharitableGreedy,relationSystem.pplAndMasks.GetPerson("therese"),-0.2f));
		relationSystem.pplAndMasks.GetPerson("bill").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("john"),-0.4f));
		relationSystem.pplAndMasks.GetPerson("bill").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("john"),-0.6f));
		relationSystem.pplAndMasks.GetPerson("bill").opinions.Add(new Opinion(TraitTypes.CharitableGreedy,relationSystem.pplAndMasks.GetPerson("john"),0.3f));
		relationSystem.pplAndMasks.GetPerson("bill").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("heather"),0.4f));
		relationSystem.pplAndMasks.GetPerson("bill").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("heather"),0.6f));
		relationSystem.pplAndMasks.GetPerson("bill").opinions.Add(new Opinion(TraitTypes.CharitableGreedy,relationSystem.pplAndMasks.GetPerson("heather"),-0.1f));
		relationSystem.pplAndMasks.GetPerson("bill").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("player"),-0.2f));
		relationSystem.pplAndMasks.GetPerson("bill").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("player"),-0.1f));
		relationSystem.pplAndMasks.GetPerson("bill").opinions.Add(new Opinion(TraitTypes.CharitableGreedy,relationSystem.pplAndMasks.GetPerson("player"),0.1f));
		relationSystem.pplAndMasks.GetPerson("therese").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("bill"),0.7f));
		relationSystem.pplAndMasks.GetPerson("therese").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("bill"),0.5f));
		relationSystem.pplAndMasks.GetPerson("therese").opinions.Add(new Opinion(TraitTypes.CharitableGreedy,relationSystem.pplAndMasks.GetPerson("bill"),0.3f));
		relationSystem.pplAndMasks.GetPerson("therese").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("john"),-0.4f));
		relationSystem.pplAndMasks.GetPerson("therese").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("john"),-0.7f));
		relationSystem.pplAndMasks.GetPerson("therese").opinions.Add(new Opinion(TraitTypes.CharitableGreedy,relationSystem.pplAndMasks.GetPerson("john"),-0.3f));
		relationSystem.pplAndMasks.GetPerson("therese").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("heather"),0.5f));
		relationSystem.pplAndMasks.GetPerson("therese").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("heather"),-0.1f));
		relationSystem.pplAndMasks.GetPerson("therese").opinions.Add(new Opinion(TraitTypes.CharitableGreedy,relationSystem.pplAndMasks.GetPerson("heather"),0.2f));
		relationSystem.pplAndMasks.GetPerson("therese").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("player"),-0.3f));
		relationSystem.pplAndMasks.GetPerson("therese").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("player"),0.1f));
		relationSystem.pplAndMasks.GetPerson("therese").opinions.Add(new Opinion(TraitTypes.CharitableGreedy,relationSystem.pplAndMasks.GetPerson("player"),0.1f));
		relationSystem.pplAndMasks.GetPerson("john").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("bill"),-0.5f));
		relationSystem.pplAndMasks.GetPerson("john").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("bill"),-0.5f));
		relationSystem.pplAndMasks.GetPerson("john").opinions.Add(new Opinion(TraitTypes.CharitableGreedy,relationSystem.pplAndMasks.GetPerson("bill"),-0.4f));
		relationSystem.pplAndMasks.GetPerson("john").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("therese"),-0.3f));
		relationSystem.pplAndMasks.GetPerson("john").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("therese"),0.2f));
		relationSystem.pplAndMasks.GetPerson("john").opinions.Add(new Opinion(TraitTypes.CharitableGreedy,relationSystem.pplAndMasks.GetPerson("therese"),0.3f));
		relationSystem.pplAndMasks.GetPerson("john").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("heather"),0.6f));
		relationSystem.pplAndMasks.GetPerson("john").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("heather"),0.4f));
		relationSystem.pplAndMasks.GetPerson("john").opinions.Add(new Opinion(TraitTypes.CharitableGreedy,relationSystem.pplAndMasks.GetPerson("heather"),0.4f));
		relationSystem.pplAndMasks.GetPerson("john").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("player"),0.5f));
		relationSystem.pplAndMasks.GetPerson("john").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("player"),07f));
		relationSystem.pplAndMasks.GetPerson("john").opinions.Add(new Opinion(TraitTypes.CharitableGreedy,relationSystem.pplAndMasks.GetPerson("player"),-0.1f));
		relationSystem.pplAndMasks.GetPerson("heather").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("bill"),0.4f));
		relationSystem.pplAndMasks.GetPerson("heather").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("bill"),-0.1f));
		relationSystem.pplAndMasks.GetPerson("heather").opinions.Add(new Opinion(TraitTypes.CharitableGreedy,relationSystem.pplAndMasks.GetPerson("bill"),-0.2f));
		relationSystem.pplAndMasks.GetPerson("heather").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("therese"),0.6f));
		relationSystem.pplAndMasks.GetPerson("heather").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("therese"),0.4f));
		relationSystem.pplAndMasks.GetPerson("heather").opinions.Add(new Opinion(TraitTypes.CharitableGreedy,relationSystem.pplAndMasks.GetPerson("therese"),0.2f));
		relationSystem.pplAndMasks.GetPerson("heather").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("john"),0.4f));
		relationSystem.pplAndMasks.GetPerson("heather").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("john"), -0.4f));
		relationSystem.pplAndMasks.GetPerson("heather").opinions.Add(new Opinion(TraitTypes.CharitableGreedy,relationSystem.pplAndMasks.GetPerson("john"), 0.2f));
		relationSystem.pplAndMasks.GetPerson("heather").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("player"),0.7f));
		relationSystem.pplAndMasks.GetPerson("heather").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("player"),0.3f));
		relationSystem.pplAndMasks.GetPerson("heather").opinions.Add(new Opinion(TraitTypes.CharitableGreedy,relationSystem.pplAndMasks.GetPerson("player"),0.1f));
		relationSystem.pplAndMasks.GetPerson("player").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("bill"),-0.4f));
		relationSystem.pplAndMasks.GetPerson("player").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("bill"),-0.2f));
		relationSystem.pplAndMasks.GetPerson("player").opinions.Add(new Opinion(TraitTypes.CharitableGreedy,relationSystem.pplAndMasks.GetPerson("bill"),-0.7f));
		relationSystem.pplAndMasks.GetPerson("player").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("therese"),0.3f));
		relationSystem.pplAndMasks.GetPerson("player").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("therese"),0.1f));
		relationSystem.pplAndMasks.GetPerson("player").opinions.Add(new Opinion(TraitTypes.CharitableGreedy,relationSystem.pplAndMasks.GetPerson("therese"),0.2f));
		relationSystem.pplAndMasks.GetPerson("player").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("john"),0.4f));
		relationSystem.pplAndMasks.GetPerson("player").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("john"), -0.1f));
		relationSystem.pplAndMasks.GetPerson("player").opinions.Add(new Opinion(TraitTypes.CharitableGreedy,relationSystem.pplAndMasks.GetPerson("john"), -0.2f));
		relationSystem.pplAndMasks.GetPerson("player").opinions.Add(new Opinion(TraitTypes.NiceNasty,relationSystem.pplAndMasks.GetPerson("heather"),0.6f));
		relationSystem.pplAndMasks.GetPerson("player").opinions.Add(new Opinion(TraitTypes.HonestFalse,relationSystem.pplAndMasks.GetPerson("heather"),0.4f));
		relationSystem.pplAndMasks.GetPerson("player").opinions.Add(new Opinion(TraitTypes.CharitableGreedy,relationSystem.pplAndMasks.GetPerson("heather"),0.5f));
		#endregion Opinions
	}


}
