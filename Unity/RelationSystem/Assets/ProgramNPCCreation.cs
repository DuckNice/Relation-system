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

		AddUpdateList ("Indgang");
		AddUpdateList ("Stue");
		AddUpdateList ("Gang");
		AddUpdateList ("Køkken");
		AddUpdateList ("Jail");


	}


	public void CreateFirstBeings()
	{
		Being Bill = new Being ("Bill", relationSystem);
		Being Therese = new Being ("Therese", relationSystem);
		Being John = new Being ("John", relationSystem);
		Being Heather = new Being ("Heather", relationSystem);
		Being Player = new Being ("Player", relationSystem);

		relationSystem.AddListToActives("Indgang");
		relationSystem.AddListToActives("Stue");
		relationSystem.AddListToActives("Køkken");

		roomMan.EnterRoom("Indgang", GetPerson("Bill"));
        roomMan.EnterRoom("Indgang", GetPerson("Therese"));
        roomMan.EnterRoom("Indgang", GetPerson("John"));
		roomMan.EnterRoom("Indgang", GetPerson("Heather"));
        roomMan.EnterRoom("Indgang", GetPerson("Player"));

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
		CreateNewMask("Player", new float[]{}, TypeMask.selfPerc, new string[]{});

		CreateNewMask("Bungary", new float[] { 0.0f, 0.0f, 0.0f }, TypeMask.culture, new string[] { "Bunce", "Buncess", "Bunsant" });
		CreateNewMask("Cult", new float[] { 0.0f, -0.2f, 0.1f }, TypeMask.culture, new string[] { "Leader", "Follower", "Skeptic" });
		CreateNewMask("MerchantGuild", new float[] { 0.0f, -0.3f, -0.2f }, TypeMask.culture, new string[] { "Member" });

		CreateNewMask("Bill", new float[] { 0.0f, 0.0f, 0.0f }, TypeMask.selfPerc, new string[] { "self" });
		CreateNewMask("Therese", new float[] { 0.0f, 0.0f, 0.0f }, TypeMask.selfPerc, new string[] { "self" });
		CreateNewMask("John", new float[] { 0.0f, 0.0f, 0.0f }, TypeMask.selfPerc, new string[] { "self" });
		CreateNewMask("Heather", new float[] { 0.0f, 0.0f, 0.0f }, TypeMask.selfPerc, new string[] { "self" });

		CreateNewMask("RomanticRelationship", new float[] { 0.2f, 0.2f, 0.2f }, TypeMask.interPers, new string[] { "Partner" });
		CreateNewMask("Friendship", new float[] { 0.1f, 0.1f, 0.0f }, TypeMask.interPers, new string[] { "Friend" });
		CreateNewMask("Rivalry", new float[] { -0.3f, -0.3f, -0.2f }, TypeMask.interPers, new string[] { "Enemy" });
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
		
		RuleConditioner GreetCondition = (self, other, indPpl) => { 
            if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["greet"] && x.GetSubject()==self && x.GetDirect()==other))
			{ return false; }

		    if(self != other && roomMan.IsPersonInSameRoomAsMe(self, other)){
				if(self.GetOpinionValue(TraitTypes.NiceNasty,other) > -0.5f){
					return true;
				}
			}
			return false; 
        };

		RuleConditioner fleeCondition = (self, other, indPpl) =>
		{	if(self.moods[MoodTypes.angryFear] < -0.8f){
					return true; 
				}
			return false; 
        };

		RuleConditioner kissCondition = (self, other, indPpl) =>
		{	
			if(self != other){
				if(self.CheckRoleName("partner",other)){
					if(roomMan.IsPersonInSameRoomAsMe(self, other) )
					{ return true; }
				}
			}
			if (self != other  && self.moods[MoodTypes.energTired] > -0.1f){ 
				if(self.moods[MoodTypes.arousDisgus] > 0.5f  && roomMan.IsPersonInSameRoomAsMe(self, other) )
					{ return true; }}
			return false;
		};

		RuleConditioner askAboutPartnerStatusCondition = (self, other, indPpl) =>
		{	if( self != other){
				if((relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["kiss"] && x.GetSubject() == other && x.GetDirect() != self)
				    && (self.CheckRoleName("partner",other)))
				   && roomMan.IsPersonInSameRoomAsMe(self, other))
				{ debug.Write("PARTNERS"); return true; }
			}

			if(self != other){
				if(!(self.CheckRoleName("partner",other)) && roomMan.IsPersonInSameRoomAsMe(self, other)){
					if(self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.4f){
						debug.Write("NOT PARTNERS");
						return true;
					}
				}
			}

			return false; 
        };

		RuleConditioner chooseAnotherAsPartnerCondition = (self, other, indPpl) =>
		{	if(self != other){
				if(self.CheckRoleName("partner")){
					return false;
				}
			}

			if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["askaboutpartnerstatus"] && x.GetSubject() == other && x.GetDirect() == self && HowLongAgo(x.GetTime()) < 10f)){
				if(self != other  && roomMan.IsPersonInSameRoomAsMe(self, other)){ //LVLOFINFL0.3
					if(self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.4f)
						{  return true; }
				}
			}
			if(self != other){
				if(!self.CheckRoleName("partner") && !other.CheckRoleName("partner")){
					if(self != other && roomMan.IsPersonInSameRoomAsMe(self, other)){
						if(self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.5f)
						{  return true; }
					}
				}
			}

			return false; 
        };

		RuleConditioner stayAsPartnerCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["askaboutpartnerstatus"] && x.GetSubject() == other && x.GetDirect() == self && HowLongAgo(x.GetTime()) < 10f)){
				if(self != other && roomMan.IsPersonInSameRoomAsMe(self, other))
					{ return true; }
			}
			return false; 
        };

		RuleConditioner LeavePartnerCondition = (self, other, indPpl) =>
		{	if(self != other){
				if(self.CheckRoleName("partner",other)){
					if(self != other && roomMan.IsPersonInSameRoomAsMe(self, other)){
						if(self.GetOpinionValue(TraitTypes.NiceNasty,other) < 0.5f){
							return true;
						}
					}
				}
			}

			return false; 
        };

		RuleConditioner flirtCondition = (self, other, indPpl) =>
		{	if(self.CheckRoleName("partner",other)){
				if(self.moods[MoodTypes.arousDisgus] > 0.0f  && self != other && roomMan.IsPersonInSameRoomAsMe(self, other)){
					if(self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.3f){
						return true;
					}
				}
			}
			if(self != other){
				if(!self.CheckRoleName("partner") && other.CheckRoleName ("partner")){
					if(self.moods[MoodTypes.arousDisgus] > 0.3f  && self != other && roomMan.IsPersonInSameRoomAsMe(self, other)){
						if(self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.4f){
							return true;
						}
					}
				}
			}

			if(self.moods[MoodTypes.arousDisgus] >= 0.0f  && self != other && roomMan.IsPersonInSameRoomAsMe(self, other)){
				return true;
			}
			return false; 
        };

		RuleConditioner chatCondition = (self, other, indPpl) =>
		{	if(self != other && 
			     self.moods[MoodTypes.energTired] > -0.5f && roomMan.IsPersonInSameRoomAsMe(self, other)){ //LvlOfInfl0.2
					return true;
			}
			return false; 
        };

		RuleConditioner giveGiftCondition = (self, other, indPpl) =>
		{	
			if( self.moods[MoodTypes.energTired] > -0.4f && self != other && roomMan.IsPersonInSameRoomAsMe(self, other)){
				if(beings.Find(x=>x.name == self.name).possessions.Exists(y=>y.Name=="game" || y.Name=="company") && self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.4f && self.GetOpinionValue(TraitTypes.CharitableGreedy,other) > 0.2f){
					if(beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="game" || y.Name=="company").value > 0f){ //LvlOfInfl0.4f
						return true;
					}
				}
			}
			return false; 
        };

		RuleConditioner poisonCondition = (self, other, indPpl) =>
		{	if(self.moods[MoodTypes.angryFear] < -0.3f && self != other  && roomMan.IsPersonInSameRoomAsMe(self, other)){
				if(self.GetOpinionValue(TraitTypes.NiceNasty,other) < -0.4f)
				return true;
			}
			return false; 
        };

		RuleConditioner gossipCondition = (self, other, indPpl) =>
		{	if(self != other  && self.moods[MoodTypes.energTired] > -0.4f && roomMan.IsPersonInSameRoomAsMe(self, other) && (self.moods[MoodTypes.angryFear] < 0.0f || self.moods[MoodTypes.hapSad] < 0.0f))
				{return true;}
			return false; 
        };

		RuleConditioner argueCondition = (self, other, indPpl) =>
		{	if(self != other && roomMan.IsPersonInSameRoomAsMe(self, other)){
				if(self.GetOpinionValue(TraitTypes.NiceNasty,other) < 0.0f || self.moods[MoodTypes.angryFear] < -0.1f)
					return true;
			}
			return false; 
        };

		RuleConditioner makeDistractionCondition = (self, other, indPpl) =>
		{	if(self.CalculateTraitType(TraitTypes.NiceNasty) < 0.3f  && self.moods[MoodTypes.energTired] > -0.4f && roomMan.IsPersonInSameRoomAsMe(self, other) && self != other)
					{ return true; }
			return false; 
        };

		RuleConditioner reminisceCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["chat"] && x.GetSubject() == other && HowLongAgo(x.GetTime()) < 10f) && self != other  &&
			     self.moods[MoodTypes.energTired] > -0.4f && roomMan.IsPersonInSameRoomAsMe(self, other))
					if(self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.2f)
						{ return true; }
			return false; 
        };

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
			return false; 
        };

		RuleConditioner praiseCondition = (self, other, indPpl) =>
		{	if((self.interPersonal.Exists(x=>x.RoleRefPersExists(other.name))) && self.moods[MoodTypes.hapSad] > 0.0f && self.moods[MoodTypes.energTired] > -0.5f && roomMan.IsPersonInSameRoomAsMe(self, other)) 
				{ return true; }
			return false; 
        };

		RuleConditioner cryCondition = (self, other, indPpl) =>
		{	if( self != other){

				if(relationSystem.historyBook.Exists(x=>(x.GetAction()==relationSystem.posActions["kill"] || x.GetAction()==relationSystem.posActions["flee"] || 
				                                         x.GetAction()==relationSystem.posActions["convict"] ) && HowLongAgo(HowLongAgo(x.GetTime())) < 30f))
					{ debug.Write("CRY CONDITION TRUE"); return true; }
				if(relationSystem.historyBook.Exists(x=>(x.GetAction()==relationSystem.posActions["poison"] || x.GetAction()==relationSystem.posActions["sabotage"] || 
				                                         x.GetAction()==relationSystem.posActions["fight"] || x.GetAction()==relationSystem.posActions["convict"]) && x.GetDirect() == other && HowLongAgo(x.GetTime()) < 30f))
					{ return true; }
			}
			if(relationSystem.historyBook.Exists(x=>x.GetDirect() == self && (x.GetAction()==relationSystem.posActions["poison"] ||
			   x.GetAction()==relationSystem.posActions["sabotage"] || x.GetAction()==relationSystem.posActions["fight"]) && HowLongAgo(x.GetTime()) < 30f))
				{ return true; }

			if(self.moods[MoodTypes.hapSad] < -0.2f){
				return true;
			}
			return false; 
        };

		RuleConditioner consoleCondition = (self, other, indPpl) =>
		{	if( self != other){
				if(relationSystem.historyBook.Exists(x=>(x.GetAction()==relationSystem.posActions["cry"] && x.GetSubject() == other && HowLongAgo(x.GetTime()) < 10f))){
					return true;
				}
			}
			return false; 
        };


// --------------------------------- CULTURAL CONDITIONS
		
		RuleConditioner convictCondition = (self, other, indPpl) =>
		{	if((relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["steal"] && x.GetSubject()==other) ||
			  relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["fight"] && x.GetSubject()==other) ||
			  relationSystem.historyBook.Exists(x=>x.GetRule().GetRuleStrength() < -0.5f && HowLongAgo(x.GetTime()) < 10f)) && roomMan.IsPersonInSameRoomAsMe(self, other) && self != other)
				{ return true; }
			return false; 
        };

		RuleConditioner fightCondition = (self, other, indPpl) =>
		{	if(self != other && roomMan.IsPersonInSameRoomAsMe(self, other)){
				if((self.CalculateTraitType(TraitTypes.NiceNasty) < 0.0 || self.moods[MoodTypes.angryFear] > 0.4f)  && self.moods[MoodTypes.energTired] > -0.8f){
					return true; 
				}
			}
			return false; 
        };
		
		RuleConditioner bribeCondition = (self, other, indPpl) =>
		{	//MONEY
			if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["convict"] && x.GetDirect()==self && x.GetSubject()==other)  && self != other && roomMan.IsPersonInSameRoomAsMe(self, other)){
				return true; 
			}
			return false; 
        };

		RuleConditioner argueInnocenceCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["convict"] && x.GetDirect()==other) && self.CalculateTraitType(TraitTypes.NiceNasty) > 0.0f
			     && self != other && roomMan.IsPersonInSameRoomAsMe(self, other))
				{ return true; }
			if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["fight"] && x.GetSubject()==other) && self.CalculateTraitType(TraitTypes.NiceNasty) > 0.0f
			   && self != other && roomMan.IsPersonInSameRoomAsMe(self, other))
				{ return true; }

			return false; 
        };

		RuleConditioner argueGuiltinessCondition = (self, other, indPpl) =>
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["convict"] && x.GetSubject()==other) && self.CalculateTraitType(TraitTypes.NiceNasty) < 0.0f
			     && self != other && roomMan.IsPersonInSameRoomAsMe(self, other))
				{ return true; }
			return false; 
        };

		RuleConditioner stealCondition = (self, other, indPpl) =>
		{	if(self != other  && self.moods[MoodTypes.energTired] > -0.4f && beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="money").value <= 50f
			   && beings.Find(x=>x.name == other.name).possessions.Find(y=>y.Name=="money").value >= 50f && self.CalculateTraitType(TraitTypes.CharitableGreedy) < 0.2f)
				  { return true; }
			return false; 
        };

		RuleConditioner makefunofCondition = (self, other, indPpl) =>
		{	if(self != other  && self.moods[MoodTypes.energTired] > -0.6f && self.moods[MoodTypes.hapSad] > -0.8f && self.moods[MoodTypes.hapSad] < 0.4f && roomMan.IsPersonInSameRoomAsMe(self, other)) 
				{ return true; }
			return false; 
        };

		RuleConditioner telljokeCondition = (self, other, indPpl) =>
		{	if(self != other  && self.moods[MoodTypes.energTired] > -0.5f && self.moods[MoodTypes.hapSad] > -0.4f && roomMan.IsPersonInSameRoomAsMe(self, other)) 
			{ return true; }
			return false; 
        };

		RuleConditioner prankCondition = (self, other, indPpl) =>
		{	if(self != other  && self.moods[MoodTypes.energTired] > -0.5f && self.moods[MoodTypes.hapSad] > -0.4f && roomMan.IsPersonInSameRoomAsMe(self, other)) 
			{ if(self.GetOpinionValue(TraitTypes.NiceNasty,other) < 0.4f) {return true;} }
			return false; 
        };

		RuleConditioner harassCondition = (self, other, indPpl) =>
		{	if(self != other  && self.moods[MoodTypes.energTired] > -0.5f && self.moods[MoodTypes.hapSad] > -0.4f && roomMan.IsPersonInSameRoomAsMe(self, other)) 
			{ if(self.GetOpinionValue(TraitTypes.NiceNasty,other) < 0.0f) {return true;} }
			return false; 
        };

		RuleConditioner playgameCondition = (self, other, indPpl) =>
		{	if(beings.Find(x=>x.name == self.name).possessions.Exists(y=>y.Name=="game") && roomMan.IsPersonInSameRoomAsMe(self, other)){
				if(beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="game").value > 0){
					if(self != other  && self.moods[MoodTypes.energTired] > -0.2f) 
						{return true;} 
				}
			}
			return false; 
        };

		RuleConditioner orderCondition = (self, other, indPpl) =>
		{	if(self != other){
				if(self != other && (self.CheckRoleName("bunce") || self.CheckRoleName("buncess")) && roomMan.IsPersonInSameRoomAsMe(self, other)){ 
					if(self.GetOpinionValue(TraitTypes.NiceNasty,other) < 0.4f) 
					{ return true;} 
				}
			}

			if(self != other && roomMan.IsPersonInSameRoomAsMe(self, other)){
				if(self.CalculateTraitType(TraitTypes.CharitableGreedy) < 0.1f && self.GetOpinionValue(TraitTypes.NiceNasty,other) < 0.2f){
					return true;
				}
			}
			return false; 
        };

		RuleConditioner killCondition = (self, other, indPpl) =>
		{	if((relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["steal"] && (x.GetSubject()==other)) ||
			       relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["fight"] && (x.GetSubject()==other)) ||
			       relationSystem.historyBook.Exists(x=>x.GetRule().GetRuleStrength() < -0.4f && HowLongAgo(x.GetTime()) < 10f)) && roomMan.IsPersonInSameRoomAsMe(self, other) && self != other)
				   		{ return true; }

			if(self != other){
				if(self.GetOpinionValue(TraitTypes.NiceNasty,other) < -0.6 && self.CalculateTraitType(TraitTypes.NiceNasty) < -0.1){
					return true;
				}
			}

			return false; 
        };

		RuleConditioner buyCompanyCondition = (self, other, indPpl) =>
		{	if(beings.Find(x=>x.name == other.name).possessions.Exists(y=>y.Name=="company")){
				if(beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="money").value >= 100f && beings.Find(x=>x.name == other.name).possessions.Find(y=>y.Name=="company").value >= 1f
				   && self != other && roomMan.IsPersonInSameRoomAsMe(self, other)) { return true; }
			}
			return false; 
        };

		RuleConditioner sellCompanyCondition = (self, other, indPpl) =>
		{	if(beings.Find(x=>x.name == other.name).possessions.Exists(y=>y.Name=="company")){
				if(beings.Find(x=>x.name == other.name).possessions.Find(y=>y.Name=="money").value >= 100f && beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="company").value >= 1f
				   && self != other && roomMan.IsPersonInSameRoomAsMe(self, other)) { return true; }
			}
			return false; 
        };

		RuleConditioner sabotageCondition = (self, other, indPpl) =>
		{	if(beings.Find(x=>x.name == other.name).possessions.Exists(y=>y.Name=="company")){
				if(beings.Find(x=>x.name == other.name).possessions.Find(y=>y.Name=="company").value > 0f){
					if(self.CalculateTraitType(TraitTypes.NiceNasty) < -0.1f && self != other  &&
					   self.moods[MoodTypes.energTired] > -0.4f){
						{ return true; }
					}
				}
			}
			return false; 
        };

		RuleConditioner advertiseCondition = (self, other, indPpl) =>
		{	if(self != other  && self.moods[MoodTypes.energTired] > -0.4f && roomMan.IsPersonInSameRoomAsMe(self, other)) { return true; }
			return false; 
        };

		RuleConditioner DemandtoLeaveGuildCondition = (self, other, indPpl) =>
		{	if(self.CalculateTraitType(TraitTypes.NiceNasty) < -0.3f && self != other &&
			    beings.Find(x=>x.name == other.name).possessions.Find(y=>y.Name=="money").value <= 0f && roomMan.IsPersonInSameRoomAsMe(self, other)){
						{ return true; }
			} 
			return false; 
        };

		RuleConditioner buyGoodsCondition = (self, other, indPpl) =>
		{	if(beings.Find(x=>x.name == self.name).possessions.Exists(y=>y.Name=="goods")){
				if(beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="goods").value < 2f && beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="money").value > 30f
				   && self != other && self.moods[MoodTypes.energTired] > -0.4f && roomMan.IsPersonInSameRoomAsMe(self, other)) 
					{ return true; }
			}
			return false; 
        };

		RuleConditioner sellGoodsCondition = (self, other, indPpl) =>
		{	if(beings.Find(x=>x.name == self.name).possessions.Exists(y=>y.Name=="goods")){
				if(beings.Find(x=>x.name == self.name).possessions.Find(y=>y.Name=="goods").value > 1f && self != other && self.moods[MoodTypes.energTired] > -0.4f && roomMan.IsPersonInSameRoomAsMe(self, other)) 
					{ return true; }
			}
			return false; 
        };

		RuleConditioner movetolivingroomCondition = (self, other, indPpl) =>
		{	
			//debug.Write("CHECKING MOVE "+roomMan.GetRoomIAmIn(self)+" "+self.name);
			if(HowLongAgo(relationSystem.historyBook.Find(x=> (x.GetAction()==relationSystem.posActions["movetolivingroom"] || x.GetAction()==relationSystem.posActions["movetokitchen"] || x.GetAction()==relationSystem.posActions["movetoentryhall"]) && x.GetSubject()==self).GetTime()) < 10f){
			//	debug.Write("FALSE");
				return false;
			}
			if(self != null && !(roomMan.GetRoomIAmIn(self) == "Stue")){
				//if((self.moods[MoodTypes.energTired] < -0.2f)){
					return true;
				//}
			}

			if(roomMan.GetRoomIAmIn(other) == "Stue" && self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.0f){
				return true;
			}
			return false; 
        };

		RuleConditioner movetoentryhallCondition = (self, other, indPpl) =>
        {

			if(HowLongAgo(relationSystem.historyBook.Find(x=> (x.GetAction()==relationSystem.posActions["movetolivingroom"] || x.GetAction()==relationSystem.posActions["movetokitchen"] || x.GetAction()==relationSystem.posActions["movetoentryhall"]) && x.GetSubject()==self).GetTime()) < 10f){
				return false;
			}
			if (self != null && (roomMan.GetRoomIAmIn(self) == "Stue")) { 
				//if((self.moods[MoodTypes.energTired] < -0.2f)){
					return true;
				//}
				if(roomMan.GetRoomIAmIn(other) == "Indgang" && self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.0f){
					return true;
				}
			}
			return false; 
        };

		RuleConditioner movetokitchenCondition = (self, other, indPpl) =>
        {
			if(HowLongAgo(relationSystem.historyBook.Find(x=> (x.GetAction()==relationSystem.posActions["movetolivingroom"] || x.GetAction()==relationSystem.posActions["movetokitchen"] || x.GetAction()==relationSystem.posActions["movetoentryhall"]) && x.GetSubject()==self).GetTime()) < 10f){
				return false;
			}
            if (self != null && (roomMan.GetRoomIAmIn(self) == "Stue")) { 
				//if((self.moods[MoodTypes.energTired] < -0.2f)){
					return true;
				//}
				if(roomMan.GetRoomIAmIn(other) == "Køkken" && self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.0f){
					return true;
				}
			}
			return false; 
        };

		#endregion adding Conditions



        #region Make RulePreferences
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
			    if(self != other){
				    if(self.CheckRoleName("partner",other)){
					    return Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),self.moods[MoodTypes.arousDisgus]);
				    }
			    }
			    float ret = 0 + Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),(0.5f*self.GetOpinionValue(TraitTypes.CharitableGreedy,other)));
			    ret += Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.HonestFalse,other),ret);
			    ret += Calculator.UnboundAdd(self.moods[MoodTypes.arousDisgus],ret);
			    return ret;
		    };

		    RulePreference flirtPreference = (self, other) => { 
			    float r = Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),self.moods[MoodTypes.arousDisgus]);
			    //r += Calculator.UnboundAdd(Calculator.NegPosTransform(self.GetLvlOfInflToPerson(other)),r);
			    return r;
		    };

		    RulePreference chatPreference = (self, other) => { 
			    return Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),self.GetOpinionValue(TraitTypes.HonestFalse,other)); //LVLOFINFL
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
			    r += Calculator.UnboundAdd(self.moods[MoodTypes.angryFear],r);
			    return r;
		    };

		    RulePreference makeDistractionPreference = (self, other) => {
			    float r = Calculator.UnboundAdd(-self.GetOpinionValue(TraitTypes.NiceNasty,other),-self.CalculateTraitType(TraitTypes.NiceNasty));
			    r += Calculator.UnboundAdd(-self.moods[MoodTypes.angryFear],r);
			    return r;
		    };

		    RulePreference reminiscePreference = (self, other) => { 
			    float r = Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),0); //Calculator.NegPosTransform(self.GetLvlOfInflToPerson(other))
			    r += Calculator.UnboundAdd(-self.moods[MoodTypes.energTired],r);
			    return r;
		    };

		    RulePreference denyPreference = (self, other) => { 
			    float r = Calculator.UnboundAdd(-self.GetOpinionValue(TraitTypes.NiceNasty,other),-self.GetOpinionValue(TraitTypes.HonestFalse,other));
			    r += Calculator.UnboundAdd((0.5f*(-self.CalculateTraitType(TraitTypes.NiceNasty))),r);
			    //r += Calculator.UnboundAdd(-(Calculator.NegPosTransform(self.GetLvlOfInflToPerson(other))),r);
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
				    if(relationSystem.historyBook.Exists(x=>(x.GetAction()==relationSystem.posActions["kill"] || x.GetAction()==relationSystem.posActions["flee"] ) && HowLongAgo(x.GetTime()) < 10f)){
					    //debug.Write("THIS ONE "+self.moods[MoodTypes.hapSad]+" "+self.CalculateTraitType(TraitTypes.NiceNasty));
					    r = Calculator.UnboundAdd(self.CalculateTraitType(TraitTypes.NiceNasty),self.moods[MoodTypes.hapSad]);
				    }
				    else if(relationSystem.historyBook.Exists(x=>(x.GetAction()==relationSystem.posActions["sabotage"] || x.GetAction()==relationSystem.posActions["poison"]
				                                                  || x.GetAction()==relationSystem.posActions["fight"]) && HowLongAgo(x.GetTime()) < 10f)){
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
			    if(self != other){
				    if(self.CheckRoleName("partner",other)){
					    return Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),0);//Calculator.NegPosTransform(self.GetLvlOfInflToPerson(other))
				    }
			    }

			    float r = Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),0); //Calculator.NegPosTransform(self.GetLvlOfInflToPerson(other))
			    r += Calculator.UnboundAdd(self.moods[MoodTypes.arousDisgus],r);
			    return r;
		    };

		    RulePreference chooseAnotherAsPartnerPreference = (self, other) => {
			    float r = Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),self.GetOpinionValue(TraitTypes.HonestFalse,other));
			    //r += Calculator.UnboundAdd(Calculator.NegPosTransform(self.GetLvlOfInflToPerson(other)),r);
			    r += Calculator.UnboundAdd(self.moods[MoodTypes.arousDisgus],r);
			    return r;
		    };

		    RulePreference StayAsPartnerPreference = (self, other) => { 
			    float r = Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),self.GetOpinionValue(TraitTypes.HonestFalse,other));
			    //r += Calculator.UnboundAdd(Calculator.NegPosTransform(self.GetLvlOfInflToPerson(other)),r);
			    return r;
		    };

		    RulePreference LeavePartnerPreference = (self, other) => { 
			    float r = Calculator.UnboundAdd(-self.GetOpinionValue(TraitTypes.NiceNasty,other),-self.GetOpinionValue(TraitTypes.HonestFalse,other));
			    //r += Calculator.UnboundAdd(-Calculator.NegPosTransform(self.GetLvlOfInflToPerson(other)),r);
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
			    float r = Calculator.UnboundAdd(-self.GetOpinionValue(TraitTypes.NiceNasty,other),-self.CalculateTraitType(TraitTypes.NiceNasty));
			    r += Calculator.UnboundAdd(self.moods[MoodTypes.angryFear],r);
			    r += Calculator.UnboundAdd(-self.moods[MoodTypes.hapSad],r);
			    return r;
		    };

		    RulePreference prankPreference = (self, other) => {
			    float r = Calculator.UnboundAdd(-self.GetOpinionValue(TraitTypes.NiceNasty,other),-self.CalculateTraitType(TraitTypes.NiceNasty));
			    r += Calculator.UnboundAdd(self.moods[MoodTypes.angryFear],r);
			    r += Calculator.UnboundAdd(-self.moods[MoodTypes.hapSad],r);
			    return r;
		    };

		    RulePreference playgamePreference = (self, other) => { 
			    float r = Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),-self.moods[MoodTypes.energTired]);
			    r += Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.HonestFalse,other),r);
			    return r;
		    };


		    RulePreference orderPreference = (self, other) => { 
			    float r = Calculator.UnboundAdd(-self.GetOpinionValue(TraitTypes.NiceNasty,other),-self.CalculateTraitType(TraitTypes.NiceNasty));
			    r += Calculator.UnboundAdd(-(self.moods[MoodTypes.energTired]),r);
			    return r;
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

			    if(roomMan.GetRoomIAmIn(other) == "Stue"){
				    r += Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),r);
			    }

			    return r;
		    };

		    RulePreference moveToKitchenPreference = (self, other) => { 
			    float r = Calculator.UnboundAdd(0.1f,-self.moods[MoodTypes.energTired]);
			    r += Calculator.UnboundAdd(-self.moods[MoodTypes.hapSad],r);
			    r += Calculator.UnboundAdd(-self.moods[MoodTypes.angryFear],r);

			    if(roomMan.GetRoomIAmIn(other) == "Køkken"){
				    r += Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),r);
			    }
			    return r;
		    };

		    RulePreference moveToEntryHallPreference = (self, other) => { 
			    float r = Calculator.UnboundAdd(0.1f,-self.moods[MoodTypes.energTired]);
			    r += Calculator.UnboundAdd(-self.moods[MoodTypes.hapSad],r);
			    r += Calculator.UnboundAdd(-self.moods[MoodTypes.angryFear],r);

			    if(roomMan.GetRoomIAmIn(other) == "Indgang"){
				    r += Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),r);
			    }
			    return r;
		    };
        #endregion

        // ---------------------------------------------------------------------------------------------------------------------- CREATING RULES


		#region Rules
		    // INTERPERSONAL RULES
		    CreateNewRule("kiss", "kiss", kissCondition,kissPreference);
		    CreateNewRule("chooseanotheraspartner", "chooseanotheraspartner", chooseAnotherAsPartnerCondition,chooseAnotherAsPartnerPreference);
		    CreateNewRule("stayaspartner", "stayaspartner", stayAsPartnerCondition,StayAsPartnerPreference);
		    CreateNewRule("leavepartner", "leavepartner", LeavePartnerCondition,LeavePartnerPreference);
		    CreateNewRule("askAboutPartnerStatus", "askAboutPartnerStatus", askAboutPartnerStatusCondition,askAboutPartnerStatusPreference);

		    CreateNewRule("flirt", "flirt", flirtCondition,flirtPreference);
		    CreateNewRule("chat", "chat", chatCondition,chatPreference);
		    CreateNewRule("givegift", "givegift", giveGiftCondition,giveGiftPreference);
		    CreateNewRule("gossip", "gossip", gossipCondition,gossipPreference);
		    CreateNewRule("argue", "argue", argueCondition,arguePreference);
		    CreateNewRule("deny", "deny", denyCondition,denyPreference);
		    CreateNewRule("makedistraction", "makedistraction", makeDistractionCondition,makeDistractionPreference);
		    CreateNewRule("reminisce", "reminisce", reminisceCondition,reminiscePreference);
		    CreateNewRule("praise", "praise", praiseCondition,praisePreference);
		    CreateNewRule("makefunof", "makefunof", makefunofCondition,makeFunOfPreference);
		    CreateNewRule("telljoke", "telljoke", telljokeCondition,telljokePreference);
		    CreateNewRule("prank", "prank", prankCondition,prankPreference);
		    CreateNewRule("harass", "harass", harassCondition,harassPreference);
		    CreateNewRule("cry", "cry", cryCondition,cryPreference);
		    CreateNewRule("console", "console", consoleCondition,consolePreference);

		    // CULTURAL RULES
		    CreateNewRule("greet", "greet",  GreetCondition,greetPreference);
		    CreateNewRule("greetfbunce", "greet",  GreetCondition,greetPreference);
		    CreateNewRule("greetfcess", "greet",  GreetCondition,greetPreference);
		    CreateNewRule("greetfbunsant", "greet",  GreetCondition,greetPreference);
		    CreateNewRule("convict", "convict",  convictCondition,convictPreference);
		    CreateNewRule("convictfcess", "convict",  convictCondition,convictPreference);
		    CreateNewRule("convictfbunce", "convict",  convictCondition,convictPreference);
		    CreateNewRule("fight", "fight", fightCondition,fightPreference);
		    CreateNewRule("bribe", "bribe", bribeCondition,bribePreference);
		    CreateNewRule("bribefbunce", "bribe", bribeCondition,bribePreference);
		    CreateNewRule("bribefcess", "bribe", bribeCondition,bribePreference);
		    CreateNewRule("bribefbunsant", "bribe", bribeCondition,bribePreference);
		    CreateNewRule("argueinnocence", "argueinnocence", argueInnocenceCondition,argueInnocencePreference);
		    CreateNewRule("argueinnocencefbunce", "argueinnocence", argueInnocenceCondition,argueInnocencePreference);
		    CreateNewRule("argueinnocencefcess", "argueinnocence", argueInnocenceCondition,argueInnocencePreference);
		    CreateNewRule("argueguiltiness", "argueguiltiness", argueGuiltinessCondition,argueGuiltinessPreference);
		    CreateNewRule("argueguiltinessfbunce", "argueguiltiness", argueGuiltinessCondition,argueGuiltinessPreference);
		    CreateNewRule("argueguiltinessfcess", "argueguiltiness", argueGuiltinessCondition,argueGuiltinessPreference);
		    CreateNewRule("steal", "steal", stealCondition,stealPreference);
		    CreateNewRule("kill", "kill",killCondition,killPreference);
		    CreateNewRule("killfbunsant", "kill",killCondition,killPreference);
		    CreateNewRule("killfbunce", "kill",killCondition,killPreference);
		    CreateNewRule("killfcess", "kill",killCondition,killPreference);
		    CreateNewRule("poison", "poison", poisonCondition,poisonPreference);
		    CreateNewRule("poisonfbunce", "poison", poisonCondition,poisonPreference);
		    CreateNewRule("poisonfcess", "poison", poisonCondition,poisonPreference);
		    CreateNewRule("poisonfbunsant", "poison", poisonCondition,poisonPreference);
		    CreateNewRule("playgame", "playgame", playgameCondition,playgamePreference);
		    CreateNewRule("playgamefbunce", "playgame", playgameCondition,playgamePreference);
		    CreateNewRule("playgamefcess", "playgame", playgameCondition,playgamePreference);
		    CreateNewRule("playgamefbunsant", "playgame", playgameCondition,playgamePreference);
		    CreateNewRule("order", "order", orderCondition,orderPreference);
		    CreateNewRule("orderfbunce", "order", orderCondition,orderPreference);
		    CreateNewRule("orderfcess", "order", orderCondition,orderPreference);
		    CreateNewRule("orderfbunsant", "order", orderCondition,orderPreference);

		    CreateNewRule("movetolivingroomfbunce", "movetolivingroom", movetolivingroomCondition,moveToLivingRoomPreference);
		    CreateNewRule("movetolivingroomfcess", "movetolivingroom", movetolivingroomCondition,moveToLivingRoomPreference);
		    CreateNewRule("movetolivingroomfbunsant", "movetolivingroom", movetolivingroomCondition,moveToLivingRoomPreference);
		    CreateNewRule("movetokitchenfbunce", "movetokitchen", movetokitchenCondition,moveToKitchenPreference);
		    CreateNewRule("movetokitchenfcess", "movetokitchen", movetokitchenCondition,moveToKitchenPreference);
		    CreateNewRule("movetokitchenfbunsant", "movetokitchen", movetokitchenCondition,moveToKitchenPreference);
		    CreateNewRule("movetoentryhallfbunce", "movetoentryhall", movetoentryhallCondition,moveToEntryHallPreference);
		    CreateNewRule("movetoentryhallfcess", "movetoentryhall", movetoentryhallCondition,moveToEntryHallPreference);
		    CreateNewRule("movetoentryhallfbunsant", "movetoentryhall", movetoentryhallCondition,moveToEntryHallPreference);
		    CreateNewRule("buycompany", "buycompany", buyCompanyCondition,buyCompanyPreference);
		    CreateNewRule("sabotage", "sabotage", sabotageCondition,sabotagePreference);
		    CreateNewRule("advertise", "advertise", advertiseCondition,advertisePreference);
		    CreateNewRule("demandtoleaveguild", "demandtoleaveguild", DemandtoLeaveGuildCondition,demandoLeaveGuildPreference);
		    CreateNewRule("sellcompany", "sellcompany", sellCompanyCondition,sellCompanyPreference);
		    CreateNewRule("buygoods", "buygoods", buyGoodsCondition,buyCompanyPreference);
		    CreateNewRule("sellgoods", "sellgoods", sellGoodsCondition,sellGoodsPreference);

		    //SELF RULES
		    CreateNewRule("donothing", "donothing", emptyCondition, doNothingPreference);
		    CreateNewRule("flee", "flee", fleeCondition,fleePreference);


    // ------------------------------------------------------------------------------------------------------------------------------------------ RULES THAT MIGHT HAPPEN (REACTION RULES)

		    // ------------- INTERPERSONAL
		    List<Rule> kissRulesToTrigger = new List<Rule>(); kissRulesToTrigger.Add(GetRule("givegift"));
		    AddPossibleRulesToRule("kiss",kissRulesToTrigger); AddPossibleRulesToRule("deny",kissRulesToTrigger); 
		    AddPossibleRulesToRule("deny",kissRulesToTrigger);

		    List<Rule> askAboutPartnerStatusRulesToTrigger = new List<Rule>(); askAboutPartnerStatusRulesToTrigger.Add(GetRule("chooseanotheraspartner"));
		    askAboutPartnerStatusRulesToTrigger.Add(GetRule("stayaspartner")); askAboutPartnerStatusRulesToTrigger.Add(GetRule("leavepartner"));
		    AddPossibleRulesToRule("askAboutPartnerStatus",askAboutPartnerStatusRulesToTrigger);

		    List<Rule> chooseanotheraspartnerRulesToTrigger = new List<Rule>(); chooseanotheraspartnerRulesToTrigger.Add(GetRule("kiss")); chooseanotheraspartnerRulesToTrigger.Add(GetRule("flirt")); 
		    chooseanotheraspartnerRulesToTrigger.Add(GetRule("deny")); chooseanotheraspartnerRulesToTrigger.Add(GetRule("fight"));
		    AddPossibleRulesToRule("chooseanotheraspartner",chooseanotheraspartnerRulesToTrigger);

		    List<Rule> stayaspartnerRulesToTrigger = new List<Rule>(); stayaspartnerRulesToTrigger.Add(GetRule("kiss")); stayaspartnerRulesToTrigger.Add(GetRule("flirt")); 
		    AddPossibleRulesToRule("stayaspartner",stayaspartnerRulesToTrigger);

		    List<Rule> leavepartnerRulesToTrigger = new List<Rule>(); leavepartnerRulesToTrigger.Add(GetRule("deny")); leavepartnerRulesToTrigger.Add(GetRule("poison"));
		    leavepartnerRulesToTrigger.Add(GetRule("fight"));
		    AddPossibleRulesToRule("leavepartner",leavepartnerRulesToTrigger);

		    List<Rule> flirtRulesToTrigger = new List<Rule>(); flirtRulesToTrigger.Add(GetRule("deny")); flirtRulesToTrigger.Add(GetRule("flirt"));
		    flirtRulesToTrigger.Add(GetRule("kiss")); flirtRulesToTrigger.Add(GetRule("fight"));
		    AddPossibleRulesToRule("flirt",flirtRulesToTrigger);

		    List<Rule> chatRulesToTrigger = new List<Rule>(); chatRulesToTrigger.Add(GetRule("chat")); chatRulesToTrigger.Add(GetRule("deny"));
		    chatRulesToTrigger.Add(GetRule("gossip")); chatRulesToTrigger.Add(GetRule("flirt")); chatRulesToTrigger.Add(GetRule("askAboutPartnerStatus"));
		    chatRulesToTrigger.Add(GetRule("reminisce")); chatRulesToTrigger.Add(GetRule("argue"));
		    AddPossibleRulesToRule("chat",chatRulesToTrigger);

		    List<Rule> giveGiftRulesToTrigger = new List<Rule>(); giveGiftRulesToTrigger.Add(GetRule("reminisce")); giveGiftRulesToTrigger.Add(GetRule("flirt"));
		    giveGiftRulesToTrigger.Add(GetRule("deny")); giveGiftRulesToTrigger.Add(GetRule("askAboutPartnerStatus"));
		    AddPossibleRulesToRule("givegift",giveGiftRulesToTrigger);

		    List<Rule> gossipRulesToTrigger = new List<Rule>(); gossipRulesToTrigger.Add(GetRule("reminisce")); gossipRulesToTrigger.Add(GetRule("flirt"));
		    gossipRulesToTrigger.Add(GetRule("deny")); gossipRulesToTrigger.Add(GetRule("argue"));
		    AddPossibleRulesToRule("gossip",gossipRulesToTrigger);

		    List<Rule> argueRulesToTrigger = new List<Rule>(); argueRulesToTrigger.Add(GetRule("argue")); argueRulesToTrigger.Add(GetRule("fight"));
		    argueRulesToTrigger.Add(GetRule("sabotage")); argueRulesToTrigger.Add(GetRule("order")); 
		    argueRulesToTrigger.Add(GetRule("deny")); argueRulesToTrigger.Add(GetRule("harass"));
		    AddPossibleRulesToRule("argue",argueRulesToTrigger);

		    List<Rule> denyRulesToTrigger = new List<Rule>(); denyRulesToTrigger.Add(GetRule("argue")); denyRulesToTrigger.Add(GetRule("poison")); 
		    denyRulesToTrigger.Add(GetRule("fight"));
		    AddPossibleRulesToRule("deny",denyRulesToTrigger);

		    List<Rule> makedistractionRulesToTrigger = new List<Rule>(); makedistractionRulesToTrigger.Add(GetRule("argue")); makedistractionRulesToTrigger.Add(GetRule("poison")); 
		    makedistractionRulesToTrigger.Add(GetRule("steal")); makedistractionRulesToTrigger.Add(GetRule("deny")); makedistractionRulesToTrigger.Add(GetRule("sabotage"));
		    AddPossibleRulesToRule("makedistraction",makedistractionRulesToTrigger);

		    List<Rule> reminisceRulesToTrigger = new List<Rule>(); reminisceRulesToTrigger.Add(GetRule("chat")); reminisceRulesToTrigger.Add(GetRule("gossip"));
		    reminisceRulesToTrigger.Add(GetRule("flirt"));
		    AddPossibleRulesToRule("reminisce",reminisceRulesToTrigger);

		    List<Rule> praiseRulesToTrigger = new List<Rule>(); praiseRulesToTrigger.Add(GetRule("chat"));
		    praiseRulesToTrigger.Add(GetRule("flirt")); praiseRulesToTrigger.Add(GetRule("fight"));
		    AddPossibleRulesToRule("praise",praiseRulesToTrigger);

		    List<Rule> makefunofRulesToTrigger = new List<Rule>(); makefunofRulesToTrigger.Add(GetRule("makefunof")); makefunofRulesToTrigger.Add(GetRule("argue"));
		    makefunofRulesToTrigger.Add(GetRule("harass")); makefunofRulesToTrigger.Add(GetRule("deny"));
		    makefunofRulesToTrigger.Add(GetRule("telljoke"));
		    AddPossibleRulesToRule("makefunof",makefunofRulesToTrigger);

		    List<Rule> telljokeRulesToTrigger = new List<Rule>(); telljokeRulesToTrigger.Add(GetRule("makefunof")); telljokeRulesToTrigger.Add(GetRule("telljoke")); 
		    telljokeRulesToTrigger.Add(GetRule("chat")); telljokeRulesToTrigger.Add(GetRule("praise")); 
		    AddPossibleRulesToRule("telljoke",telljokeRulesToTrigger);

		    List<Rule> prankRulesToTrigger = new List<Rule>(); prankRulesToTrigger.Add(GetRule("makefunof")); prankRulesToTrigger.Add(GetRule("deny"));
		    prankRulesToTrigger.Add(GetRule("convict")); prankRulesToTrigger.Add(GetRule("argue")); prankRulesToTrigger.Add(GetRule("order"));
		    AddPossibleRulesToRule("prank",prankRulesToTrigger);

		    List<Rule> harassRulesToTrigger = new List<Rule>(); harassRulesToTrigger.Add(GetRule("telljoke")); harassRulesToTrigger.Add(GetRule("deny"));
		    harassRulesToTrigger.Add(GetRule("argue")); harassRulesToTrigger.Add(GetRule("fight")); harassRulesToTrigger.Add(GetRule("order"));
		    AddPossibleRulesToRule("harass",harassRulesToTrigger);

		    List<Rule> cryRulesToTrigger = new List<Rule>(); cryRulesToTrigger.Add(GetRule("cry")); cryRulesToTrigger.Add(GetRule("reminisce"));
		    cryRulesToTrigger.Add(GetRule("givegift")); cryRulesToTrigger.Add(GetRule("kiss")); cryRulesToTrigger.Add(GetRule("console")); 
		    AddPossibleRulesToRule("cry",cryRulesToTrigger);

		    List<Rule> consoleRulesToTrigger = new List<Rule>(); consoleRulesToTrigger.Add(GetRule("chat")); consoleRulesToTrigger.Add(GetRule("reminisce"));
		    consoleRulesToTrigger.Add(GetRule("kiss"));
		    AddPossibleRulesToRule("console",consoleRulesToTrigger);

		    // ------------- CULTURE
		    List<Rule> greetRulesToTrigger = new List<Rule>(); greetRulesToTrigger.Add(GetRule("chat")); greetRulesToTrigger.Add(GetRule("kiss"));
		    greetRulesToTrigger.Add(GetRule("greet"));
		    AddPossibleRulesToRule("greet",greetRulesToTrigger);
		    AddPossibleRulesToRule("greetfcess",greetRulesToTrigger);
		    AddPossibleRulesToRule("greetfbunce",greetRulesToTrigger);
		    AddPossibleRulesToRule("greetfbunsant",greetRulesToTrigger);

		    List<Rule> convictRulesToTrigger = new List<Rule>(); convictRulesToTrigger.Add(GetRule("bribe")); convictRulesToTrigger.Add(GetRule("poison")); 
		    convictRulesToTrigger.Add(GetRule("fight")); convictRulesToTrigger.Add(GetRule("flee")); 
		    AddPossibleRulesToRule("convict",convictRulesToTrigger);
		    AddPossibleRulesToRule("convictfcess",convictRulesToTrigger);
		    AddPossibleRulesToRule("convictfbunce",convictRulesToTrigger);

		    List<Rule> fightRulesToTrigger = new List<Rule>(); fightRulesToTrigger.Add(GetRule("fight")); fightRulesToTrigger.Add(GetRule("convict"));
		    fightRulesToTrigger.Add(GetRule("poison")); fightRulesToTrigger.Add(GetRule("sabotage")); fightRulesToTrigger.Add(GetRule("argue"));
		    AddPossibleRulesToRule("fight",fightRulesToTrigger);

		    List<Rule> bribeRulesToTrigger = new List<Rule>(); bribeRulesToTrigger.Add(GetRule("fight")); bribeRulesToTrigger.Add(GetRule("convict"));
		    bribeRulesToTrigger.Add(GetRule("gossip")); bribeRulesToTrigger.Add(GetRule("deny")); bribeRulesToTrigger.Add(GetRule("deny"));
		    AddPossibleRulesToRule("bribe",bribeRulesToTrigger);
		    AddPossibleRulesToRule("bribefbunce",bribeRulesToTrigger);
		    AddPossibleRulesToRule("bribefcess",bribeRulesToTrigger);
		    AddPossibleRulesToRule("bribefbunsant",bribeRulesToTrigger);

		    List<Rule> argueinnocenceRulesToTrigger = new List<Rule>(); argueinnocenceRulesToTrigger.Add(GetRule("chat")); argueinnocenceRulesToTrigger.Add(GetRule("argue"));
		    argueinnocenceRulesToTrigger.Add(GetRule("deny"));
		    AddPossibleRulesToRule("argueinnocence",argueinnocenceRulesToTrigger);
		    AddPossibleRulesToRule("argueinnocencefbunce",argueinnocenceRulesToTrigger);
		    AddPossibleRulesToRule("argueinnocencefcess",argueinnocenceRulesToTrigger);

		    List<Rule> argueguiltinessRulesToTrigger = new List<Rule>(); argueinnocenceRulesToTrigger.Add(GetRule("chat")); argueinnocenceRulesToTrigger.Add(GetRule("argue"));
		    argueinnocenceRulesToTrigger.Add(GetRule("deny"));
		    AddPossibleRulesToRule("argueguiltiness",argueguiltinessRulesToTrigger);
		    AddPossibleRulesToRule("argueguiltinessfbunce",argueguiltinessRulesToTrigger);
		    AddPossibleRulesToRule("argueguiltinessfcess",argueguiltinessRulesToTrigger);

		    List<Rule> stealRulesToTrigger = new List<Rule>();  stealRulesToTrigger.Add(GetRule("poison")); 
		    stealRulesToTrigger.Add(GetRule("fight")); stealRulesToTrigger.Add(GetRule("convict"));
		    AddPossibleRulesToRule("steal",stealRulesToTrigger); AddPossibleRulesToRule("buygoods",stealRulesToTrigger);

		    List<Rule> poisonRulesToTrigger = new List<Rule>(); poisonRulesToTrigger.Add(GetRule("fight")); poisonRulesToTrigger.Add(GetRule("argue"));
		    poisonRulesToTrigger.Add(GetRule("sabotage"));
		    AddPossibleRulesToRule("poison",poisonRulesToTrigger);
		    AddPossibleRulesToRule("poisonfbunce",poisonRulesToTrigger);
		    AddPossibleRulesToRule("poisonfcess",poisonRulesToTrigger);
		    AddPossibleRulesToRule("poisonfbunsant",poisonRulesToTrigger);

		    List<Rule> playgameRulesToTriger = new List<Rule>(); playgameRulesToTriger.Add(GetRule("telljoke")); playgameRulesToTriger.Add(GetRule("chat")); 
		    playgameRulesToTriger.Add(GetRule("reminisce")); 
		    AddPossibleRulesToRule("playgame",playgameRulesToTriger);
		    AddPossibleRulesToRule("playgamefbunce",playgameRulesToTriger);
		    AddPossibleRulesToRule("playgamefbunsant",playgameRulesToTriger);
		    AddPossibleRulesToRule("playgamefcess",playgameRulesToTriger);

		    List<Rule> orderRulesToTriger = new List<Rule>(); orderRulesToTriger.Add(GetRule("deny")); orderRulesToTriger.Add(GetRule("fight"));
		    orderRulesToTriger.Add(GetRule("flee"));
		    AddPossibleRulesToRule("order",orderRulesToTriger);
		    AddPossibleRulesToRule("orderfbunce",orderRulesToTriger);
		    AddPossibleRulesToRule("orderfcess",orderRulesToTriger);
		    AddPossibleRulesToRule("orderfbunsant",orderRulesToTriger);

		    List<Rule> killRulesToTrigger = new List<Rule>(); killRulesToTrigger.Add(GetRule("convict")); killRulesToTrigger.Add(GetRule("fight"));
		    killRulesToTrigger.Add(GetRule("kill"));
		    AddPossibleRulesToRule("kill",killRulesToTrigger);
		    AddPossibleRulesToRule("killfbunsant",killRulesToTrigger);
		    AddPossibleRulesToRule("killfcess",killRulesToTrigger);
		    AddPossibleRulesToRule("killfbunce",killRulesToTrigger);

		    // ------------- MERCHANT GUILD RULES

		    List<Rule> buycompanyRulesToTrigger = new List<Rule>();
		    buycompanyRulesToTrigger.Add(GetRule("argue"));
		    buycompanyRulesToTrigger.Add(GetRule("sabotage")); buycompanyRulesToTrigger.Add(GetRule("demandtoleaveguild"));
		    AddPossibleRulesToRule("buycompany",buycompanyRulesToTrigger);

		    List<Rule> sellcompanyRulesToTrigger = new List<Rule>();
		    sellcompanyRulesToTrigger.Add(GetRule("argue"));
		    sellcompanyRulesToTrigger.Add(GetRule("sabotage")); sellcompanyRulesToTrigger.Add(GetRule("demandtoleaveguild"));
		    AddPossibleRulesToRule("sellcompany",sellcompanyRulesToTrigger);

		    List<Rule> advertiseRulesToTrigger = new List<Rule>();
		    sellcompanyRulesToTrigger.Add(GetRule("argue")); sellcompanyRulesToTrigger.Add(GetRule("chat"));
		    sellcompanyRulesToTrigger.Add(GetRule("deny"));
		    AddPossibleRulesToRule("advertise",advertiseRulesToTrigger);

		    List<Rule> demandtoleaveguildRulesToTrigger = new List<Rule>();
		    demandtoleaveguildRulesToTrigger.Add(GetRule("argue")); demandtoleaveguildRulesToTrigger.Add(GetRule("poison")); 
		    demandtoleaveguildRulesToTrigger.Add(GetRule("fight")); demandtoleaveguildRulesToTrigger.Add(GetRule("deny")); 
		    demandtoleaveguildRulesToTrigger.Add(GetRule("order")); 
		    AddPossibleRulesToRule("demandtoleaveguild",demandtoleaveguildRulesToTrigger);

		    List<Rule> buygoodsRulesToTrigger = new List<Rule>(); 
		    buygoodsRulesToTrigger.Add(GetRule("chat"));
		    buygoodsRulesToTrigger.Add(GetRule("advertise"));
		    AddPossibleRulesToRule("buygoods",buygoodsRulesToTrigger);

		    List<Rule> sellgoodsRulesToTrigger = new List<Rule>(); 
		    buygoodsRulesToTrigger.Add(GetRule("chat")); buygoodsRulesToTrigger.Add(GetRule("argue")); buygoodsRulesToTrigger.Add(GetRule("sabotage"));
		    AddPossibleRulesToRule("sellgoods",sellgoodsRulesToTrigger);


    // ----------------------------------------------------------------------------------------------- ADDING RULES TO MASKS
	
	    // SElF
		    AddRuleToMask("John", "Self", "donothing", -1.0f);
		    AddRuleToMask("Therese", "Self", "donothing", -1.0f);
		    AddRuleToMask("Bill", "Self", "donothing", -1.0f);
		    AddRuleToMask("Heather", "Self", "donothing", -1.0f);
		    AddRuleToMask("Player", "Self", "donothing", -1.0f);

		    AddRuleToMask("John", "Self", "flee", 0.2f);
		    AddRuleToMask("Heather", "Self", "flee", -0.1f);
		    AddRuleToMask("Player", "Self", "flee", -0.1f);

		    AddRuleToMask("John", "Self", "chooseanotheraspartner", -0.2f);
		    AddRuleToMask("Therese", "Self", "chooseanotheraspartner", -0.2f);
		    AddRuleToMask("Bill", "Self", "chooseanotheraspartner", 0.4f);
		    AddRuleToMask("Heather", "Self", "chooseanotheraspartner", 0.5f);
		    AddRuleToMask("Player", "Self", "chooseanotheraspartner", -0.4f);
		
	    // INTERPERSONAL
		    AddRuleToMask("RomanticRelationship", "Partner", "kiss", 0.4f);
		
		    AddRuleToMask("RomanticRelationship", "Partner", "askAboutPartnerStatus", 0.5f);
		    AddRuleToMask("RomanticRelationship", "Partner", "stayaspartner", 0.2f);
		    AddRuleToMask("RomanticRelationship", "Partner", "leavepartner", 0.0f);

		    AddRuleToMask("RomanticRelationship", "Partner", "flirt", 0.4f);
		    AddRuleToMask("Friendship", "Friend", "flirt", -0.4f);

		    AddRuleToMask("RomanticRelationship", "Partner", "chat", 0.0f);
		    AddRuleToMask("Friendship", "Friend", "chat", 0.0f);
		    AddRuleToMask("Rivalry", "Enemy", "chat", -0.2f);

		    AddRuleToMask("RomanticRelationship", "Partner", "givegift", 0.4f);
		    AddRuleToMask("Rivalry", "Enemy", "givegift", 0.2f);
		    AddRuleToMask("Friendship", "Friend", "givegift", -0.3f);

		    AddRuleToMask("Rivalry", "Enemy", "gossip", -0.2f);
		    AddRuleToMask("Friendship", "Friend", "gossip", 0.1f);
		    AddRuleToMask("RomanticRelationship", "Partner", "gossip", -0.2f);

		    AddRuleToMask("Rivalry", "Enemy", "argue", 0.2f);
		    AddRuleToMask("Friendship", "Friend", "argue", -0.2f);
		    AddRuleToMask("RomanticRelationship", "Partner", "argue", -0.4f);

		    AddRuleToMask("Rivalry", "Enemy", "deny", 0.3f);
		    AddRuleToMask("Friendship", "Friend", "deny", -0.1f);
		    AddRuleToMask("RomanticRelationship", "Partner", "deny", -0.2f);

		    AddRuleToMask("Rivalry", "Enemy", "makedistraction", -0.1f);
		    AddRuleToMask("Friendship", "Friend", "makedistraction", -0.3f);
		    AddRuleToMask("RomanticRelationship", "Partner", "makedistraction", -0.5f);

		    AddRuleToMask("Rivalry", "Enemy", "makefunof", 0.3f);
		    AddRuleToMask("Friendship", "Friend", "makefunof", -0.1f);
		    AddRuleToMask("RomanticRelationship", "Partner", "makefunof", -0.4f);

		    AddRuleToMask("Rivalry", "Enemy", "telljoke", -0.2f);
		    AddRuleToMask("Friendship", "Friend", "telljoke", 0.4f);
		    AddRuleToMask("RomanticRelationship", "Partner", "telljoke", 0.2f);

		    AddRuleToMask("Rivalry", "Enemy", "prank", 0.2f);
		    AddRuleToMask("Friendship", "Friend", "prank", 0.0f);
		    AddRuleToMask("RomanticRelationship", "Partner", "prank", -0.3f);

		    AddRuleToMask("Rivalry", "Enemy", "harass", 0.3f);
		    AddRuleToMask("Friendship", "Friend", "harass", -0.4f);
		    AddRuleToMask("RomanticRelationship", "Partner", "harass", -0.6f);

		    AddRuleToMask("RomanticRelationship", "Partner", "reminisce", 0.3f);
		    AddRuleToMask("Friendship", "Friend", "reminisce", 0.1f);

		    AddRuleToMask("Friendship", "Friend", "praise", 0.3f);
		    AddRuleToMask("RomanticRelationship", "Partner", "praise", 0.2f);

		    AddRuleToMask("Friendship", "Friend", "cry", 0.8f);
		    AddRuleToMask("RomanticRelationship", "Partner", "cry", 1.0f);
		    AddRuleToMask("Rivalry", "Enemy", "cry", 0.6f);

		    AddRuleToMask("Friendship", "Friend", "console", 0.8f);
		    AddRuleToMask("RomanticRelationship", "Partner", "console", 1.0f);
		    AddRuleToMask("Rivalry", "Enemy", "console", -0.4f);

	    // CULTURE
		    AddRuleToMask("Bungary", "Bunsant", "fight", -0.5f);
		    AddRuleToMask("Bungary", "Bunsant", "bribefbunsant", -0.1f);
		    AddRuleToMask("Bungary", "Bunsant", "steal", -0.5f);
		    AddRuleToMask("Bungary", "Bunsant", "askforhelpinillicitactivity", -0.1f);
		    AddRuleToMask("Bungary", "Bunsant", "poisonfbunsant", -0.8f);
		    AddRuleToMask("Bungary", "Bunsant", "greetfbunsant", 0.5f);
		    AddRuleToMask("Bungary", "Bunsant", "playgamefbunsant", 0.2f);
		    AddRuleToMask("Bungary", "Bunsant", "orderfbunsant", -0.5f);
		    AddRuleToMask("Bungary", "Bunsant", "movetolivingroomfbunsant", 0.0f);
		    AddRuleToMask("Bungary", "Bunsant", "movetokitchenfbunsant", 0.0f);
		    AddRuleToMask("Bungary", "Bunsant", "movetoentryhallfbunsant", 0.0f);
		    AddRuleToMask("Bungary", "Bunsant", "killfbunsant", -0.9f);

		    AddRuleToMask("Bungary", "Bunce", "bribefbunce", 0.3f);
		    AddRuleToMask("Bungary", "Bunce", "convictfbunce", 1.0f);
		    AddRuleToMask("Bungary", "Bunce", "argueinnocencefbunce", 0.0f);
		    AddRuleToMask("Bungary", "Bunce", "argueguiltinessfbunce", 0.0f);
		    AddRuleToMask("Bungary", "Bunce", "poisonfbunce", -0.8f);
		    AddRuleToMask("Bungary", "Bunce", "greetfbunce", 1.0f);
		    AddRuleToMask("Bungary", "Bunce", "playgamefbunce", 0.2f);
		    AddRuleToMask("Bungary", "Bunce", "orderfbunce", 0.5f);
		    AddRuleToMask("Bungary", "Bunce", "movetolivingroomfbunce", 0.5f);
		    AddRuleToMask("Bungary", "Bunce", "movetokitchenfbunce", 0.1f);
		    AddRuleToMask("Bungary", "Bunce", "movetoentryhallfbunce", 0.3f);
		    AddRuleToMask("Bungary", "Bunce", "killfbunce", -0.8f);

		    AddRuleToMask("Bungary", "Buncess", "bribefcess", 0.3f);
		    AddRuleToMask("Bungary", "Buncess", "convictfcess", 0.8f);
		    AddRuleToMask("Bungary", "Buncess", "argueinnocencefcess", 0.2f);
		    AddRuleToMask("Bungary", "Buncess", "argueguiltinessfcess", -0.1f);
		    AddRuleToMask("Bungary", "Buncess", "poisonfcess", -0.8f);
		    AddRuleToMask("Bungary", "Buncess", "greetfcess", 1.0f);
		    AddRuleToMask("Bungary", "Buncess", "playgamefbuncess", 0.2f);
		    AddRuleToMask("Bungary", "Buncess", "orderfcess", 0.5f);
		    AddRuleToMask("Bungary", "Buncess", "movetolivingroomfcess", 0.6f);
		    AddRuleToMask("Bungary", "Buncess", "movetokitchenfcess", 0.2f);
		    AddRuleToMask("Bungary", "Buncess", "movetoentryhallfcess", 0.3f);
		    AddRuleToMask("Bungary", "Buncess", "killfcess", -0.9f);

		    AddRuleToMask("MerchantGuild", "Member", "buycompany", 0.2f);
		    AddRuleToMask("MerchantGuild", "Member", "sabotage", -0.5f);
		    AddRuleToMask("MerchantGuild", "Member", "advertise", 0.5f);
		    AddRuleToMask("MerchantGuild", "Member", "demandtoleaveguild", -0.3f);
		    AddRuleToMask("MerchantGuild", "Member", "sellcompany", -0.5f);
		    AddRuleToMask("MerchantGuild", "Member", "buygoods", 0.5f);
		    AddRuleToMask("MerchantGuild", "Member", "sellgoods", 0.7f);

		#endregion Rules


//  ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		//PEOPLE

		
		#region AddingPlayer
		    MaskAdds selfPersMask = new MaskAdds("Self", "Player", 0.0f);
	
		    List<MaskAdds>  culture = new List<MaskAdds>();
		    culture.Add(new MaskAdds("Bunsant", "Bungary", 0.6f));
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
	        relationSystem.AddLinkToPerson("Bill", TypeMask.interPers, "", "RomanticRelationship", 0);
		    relationSystem.AddLinkToPerson("Bill", TypeMask.interPers, "", "Rivalry", 0);
		    relationSystem.AddLinkToPerson("Bill", TypeMask.interPers, "", "Friendship",0);
		    relationSystem.AddRefToLinkInPerson("Bill",TypeMask.interPers,"partner","romanticrelationship","Therese",0.4f);
		    relationSystem.AddRefToLinkInPerson("Bill", TypeMask.interPers, "Enemy", "Rivalry","John",0.6f);
	        relationSystem.AddRefToLinkInPerson("Bill", TypeMask.interPers, "Friend", "Friendship", "Heather", 0.3f);
		    relationSystem.AddRefToLinkInPerson("Bill", TypeMask.interPers, "Enemy", "Rivalry","player",0.5f);
		    relationSystem.AddRefToLinkInPerson("Bill",TypeMask.culture,"bunce","Bungary","john",0.3f);
		    relationSystem.AddRefToLinkInPerson("Bill",TypeMask.culture,"bunce","Bungary","therese",0.5f);
		    relationSystem.AddRefToLinkInPerson("Bill",TypeMask.culture,"bunce","Bungary","heather",0.3f);
		    relationSystem.AddRefToLinkInPerson("Bill",TypeMask.culture,"bunce","Bungary","player",0.3f);
	
	        relationSystem.AddLinkToPerson("Therese", TypeMask.interPers, "", "RomanticRelationship", 0);
	        relationSystem.AddLinkToPerson("Therese", TypeMask.interPers, "", "Rivalry", 0);
	        relationSystem.AddLinkToPerson("Therese", TypeMask.interPers, "", "Friendship", 0);
		    relationSystem.AddRefToLinkInPerson("Therese", TypeMask.interPers, "Partner", "RomanticRelationship","Bill",0.5f);
		    relationSystem.AddRefToLinkInPerson("Therese", TypeMask.interPers, "Enemy", "Rivalry", "John", 0.2f);
		    relationSystem.AddRefToLinkInPerson("Therese", TypeMask.interPers, "Friend", "Friendship", "Heather", 0.6f);
		    relationSystem.AddRefToLinkInPerson("Therese", TypeMask.interPers, "Enemy", "Rivalry", "Player", 0.3f);
		    relationSystem.AddRefToLinkInPerson("Therese",TypeMask.culture,"buncess","Bungary","john",0.2f);
		    relationSystem.AddRefToLinkInPerson("Therese",TypeMask.culture,"buncess","Bungary","bill",0.6f);
		    relationSystem.AddRefToLinkInPerson("Therese",TypeMask.culture,"buncess","Bungary","heather",0.4f);
		    relationSystem.AddRefToLinkInPerson("Therese",TypeMask.culture,"buncess","Bungary","player",0.2f);

	        relationSystem.AddLinkToPerson("John", TypeMask.interPers, "", "Rivalry", 0);
		    relationSystem.AddLinkToPerson("John", TypeMask.interPers, "", "Romanticrelationship", 0);
		    relationSystem.AddLinkToPerson("John", TypeMask.interPers, "", "Friendship", 0);
		    relationSystem.AddRefToLinkInPerson("John", TypeMask.interPers, "Enemy", "Rivalry", "Bill", 0.7f);
		    relationSystem.AddRefToLinkInPerson("John", TypeMask.interPers, "Enemy", "Rivalry", "Therese", 0.4f);
		    relationSystem.AddRefToLinkInPerson("John", TypeMask.interPers, "Partner", "Romanticrelationship", "Heather", 0.8f);
		    relationSystem.AddRefToLinkInPerson("John", TypeMask.interPers, "Friend", "Friendship", "Player", 0.5f);	    
		    relationSystem.AddRefToLinkInPerson("John",TypeMask.culture,"bunsant","Bungary","bill",0.6f);
		    relationSystem.AddRefToLinkInPerson("John",TypeMask.culture,"bunsant","Bungary","therese",0.2f);
		    relationSystem.AddRefToLinkInPerson("John",TypeMask.culture,"bunsant","Bungary","heather",0.4f);
		    relationSystem.AddRefToLinkInPerson("John",TypeMask.culture,"bunsant","Bungary","player",0.1f);
	
	        relationSystem.AddLinkToPerson("Heather", TypeMask.interPers, "", "Friendship", 0);
		    relationSystem.AddLinkToPerson("Heather", TypeMask.interPers, "", "RomanticRelationship", 0);
		    relationSystem.AddRefToLinkInPerson("Heather", TypeMask.interPers, "Friend", "Friendship", "Bill", 0f);
		    relationSystem.AddRefToLinkInPerson("Heather", TypeMask.interPers, "Friend", "Friendship", "Therese", 0.7f);
		    relationSystem.AddRefToLinkInPerson("Heather", TypeMask.interPers, "Partner", "RomanticRelationship", "John", 0.5f);
		    relationSystem.AddRefToLinkInPerson("Heather", TypeMask.interPers, "Partner", "RomanticRelationship", "Player", 0.5f);
		    relationSystem.AddRefToLinkInPerson("Heather",TypeMask.culture,"bunsant","Bungary","bill",0.3f);
		    relationSystem.AddRefToLinkInPerson("Heather",TypeMask.culture,"bunsant","Bungary","therese",0.5f);
		    relationSystem.AddRefToLinkInPerson("Heather",TypeMask.culture,"bunsant","Bungary","john",0.6f);
		    relationSystem.AddRefToLinkInPerson("Heather",TypeMask.culture,"bunsant","Bungary","player",0.4f);
	
	        relationSystem.AddLinkToPerson("Player", TypeMask.interPers, "", "Rivalry", 0);
		    relationSystem.AddLinkToPerson("Player", TypeMask.interPers, "", "Friendship", 0);
		    relationSystem.AddLinkToPerson("Player", TypeMask.interPers, "", "RomanticRelationship", 0);
		    relationSystem.AddRefToLinkInPerson("Player", TypeMask.interPers, "Enemy", "Rivalry", "Bill", 0.5f);
		    relationSystem.AddRefToLinkInPerson("Player", TypeMask.interPers, "Enemy", "Rivalry", "Therese", 0.3f);
		    relationSystem.AddRefToLinkInPerson("Player", TypeMask.interPers, "Friend", "Friendship", "John", 0.5f);
		    relationSystem.AddRefToLinkInPerson("Player", TypeMask.interPers, "Partner", "RomanticRelationship", "Heather", 0.6f);
		    relationSystem.AddRefToLinkInPerson("Player",TypeMask.culture,"bunsant","Bungary","bill",0.5f);
		    relationSystem.AddRefToLinkInPerson("Player",TypeMask.culture,"bunsant","Bungary","therese",0.4f);
		    relationSystem.AddRefToLinkInPerson("Player",TypeMask.culture,"bunsant","Bungary","john",0.6f);
		    relationSystem.AddRefToLinkInPerson("Player",TypeMask.culture,"bunsant","Bungary","heather",0.5f);
		#endregion LINKS 

		#region Opinions
		        //BILL OPINIONS
		GetPerson("bill").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("therese"), 0.6f);
	    GetPerson("bill").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("therese"), 0.3f);
	    GetPerson("bill").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("therese"), -0.2f);
		GetPerson("bill").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("john"), -0.4f);
	    GetPerson("bill").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("john"), -0.6f);
	    GetPerson("bill").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("john"), 0.3f);
	    GetPerson("bill").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("heather"), 0.4f);
	    GetPerson("bill").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("heather"), 0.6f);
	    GetPerson("bill").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("heather"), -0.1f);
	    GetPerson("bill").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("player"), -0.2f);
	    GetPerson("bill").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("player"), -0.1f);
	    GetPerson("bill").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("player"), 0.1f);
	    GetPerson("therese").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("bill"), 0.7f);
	    GetPerson("therese").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("bill"), 0.5f);
	    GetPerson("therese").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("bill"), 0.3f);
	    GetPerson("therese").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("john"), -0.4f);
	    GetPerson("therese").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("john"), -0.7f);
	    GetPerson("therese").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("john"), -0.3f);
	    GetPerson("therese").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("heather"), 0.5f);
	    GetPerson("therese").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("heather"), -0.1f);
	    GetPerson("therese").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("heather"), 0.2f);
	    GetPerson("therese").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("player"), -0.3f);
	    GetPerson("therese").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("player"), 0.1f);
	    GetPerson("therese").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("player"), 0.1f);
	    GetPerson("john").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("bill"), -0.5f);
	    GetPerson("john").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("bill"), -0.5f);
	    GetPerson("john").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("bill"), -0.4f);
	    GetPerson("john").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("therese"), -0.3f);
	    GetPerson("john").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("therese"), 0.2f);
	    GetPerson("john").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("therese"), 0.3f);
	    GetPerson("john").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("heather"), 0.6f);
	    GetPerson("john").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("heather"), 0.4f);
	    GetPerson("john").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("heather"), 0.4f);
	    GetPerson("john").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("player"), 0.5f);
	    GetPerson("john").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("player"), 0.5f);
	    GetPerson("john").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("player"), -0.1f);
	    GetPerson("heather").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("bill"), 0.4f);
	    GetPerson("heather").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("bill"), -0.1f);
	    GetPerson("heather").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("bill"), -0.2f);
	    GetPerson("heather").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("therese"), 0.6f);
	    GetPerson("heather").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("therese"), 0.4f);
	    GetPerson("heather").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("therese"), 0.2f);
	    GetPerson("heather").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("john"), 0.4f);
	    GetPerson("heather").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("john"), -0.4f);
	    GetPerson("heather").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("john"), 0.2f);
	    GetPerson("heather").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("player"), 0.7f);
	    GetPerson("heather").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("player"), 0.3f);
	    GetPerson("heather").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("player"), 0.1f);
	    GetPerson("player").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("bill"), -0.4f);
	    GetPerson("player").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("bill"), -0.2f);
	    GetPerson("player").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("bill"), -0.7f);
	    GetPerson("player").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("therese"), 0.3f);
	    GetPerson("player").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("therese"), 0.1f);
	    GetPerson("player").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("therese"), 0.2f);
	    GetPerson("player").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("john"), 0.4f);
	    GetPerson("player").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("john"), -0.1f);
	    GetPerson("player").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("john"), -0.2f);
	    GetPerson("player").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("heather"), 0.6f);
	    GetPerson("player").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("heather"), 0.4f);
	    GetPerson("player").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("heather"), 0.5f);
		#endregion Opinions
	}
}
