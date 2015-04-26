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

		AddUpdateList ("Entrance");
		AddUpdateList ("Living Room");
		AddUpdateList ("Hallway");
		AddUpdateList ("Kitchen");
		AddUpdateList ("Jail");
	}


	public void CreateFirstBeings()
	{
		Being Bill = new Being ("Bill", relationSystem);
		Being Therese = new Being ("Therese", relationSystem);
		Being John = new Being ("John", relationSystem);
		Being Heather = new Being ("Heather", relationSystem);
		Being Player = new Being (playerName, relationSystem);

		relationSystem.AddListToActives("Entrance");
		relationSystem.AddListToActives("Living Room");
		relationSystem.AddListToActives("Kitchen");

		beings.Add (Bill);
		beings.Add (Therese);
		beings.Add (John);
		beings.Add (Heather);
		beings.Add (Player);

		foreach (Being b in beings) {
			int r = UnityEngine.Random.Range (0,3);
			debug.Write(""+r);
			switch(r){
			case 0:
				roomMan.EnterRoom("Entrance", GetPerson(b.name));
				break;
			case 1:
				roomMan.EnterRoom("Living Room", GetPerson(b.name));
				break;
			case 2:
				roomMan.EnterRoom("Kitchen", GetPerson(b.name));
				break;
			}

		}

		/*
		roomMan.EnterRoom("Entrance", GetPerson("Bill"));
        roomMan.EnterRoom("Entrance", GetPerson("Therese"));
        roomMan.EnterRoom("Entrance", GetPerson("John"));
		roomMan.EnterRoom("Entrance", GetPerson("Heather"));
        roomMan.EnterRoom("Entrance", GetPerson(playerName));
		*/


		Bill.possessions.Add (new Money (100f));
		Bill.possessions.Add (new Goods (5f));
		Bill.possessions.Add (new Company("Bill's Wares"));
		Therese.possessions.Add (new Money (70f));
		John.possessions.Add (new Money (5f));
		Heather.possessions.Add (new Money (20f));
		Player.possessions.Add (new Money (30f));
		Player.possessions.Add (new Goods (2f));
		Player.possessions.Add (new Company("A Poor Excuse for A Company"));
		Heather.possessions.Add (new Game ("Chess"));
		John.possessions.Add (new Game ("Cards"));

		foreach (Being b in beings) {
			b.name = b.name.ToLower();
		}
	}


	public void CreateFirstMasks()
	{
		CreateNewMask(playerName, new float[]{}, TypeMask.selfPerc, new string[]{});

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
		#region addingConditions

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
		{	if(relationSystem.historyBook.Exists(x=>(x.GetAction()==relationSystem.posActions["fight"] || x.GetAction()==relationSystem.posActions["poison"]) && x.GetDirect() == self && HowLongAgo(x.GetTime()) < 10f)){
					return true; 
			}

			if(self.moods[MoodTypes.angryFear] < -0.8f){
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
				if(self.moods[MoodTypes.arousDisgus] > 0.5f  && self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.5f && roomMan.IsPersonInSameRoomAsMe(self, other) )
					{ return true; }}
			return false;
		};

		RuleConditioner askIfShouldBePartnerCondition = (self, other, indPpl) =>
		{	if( self != other){
				if((relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["kiss"] && x.GetSubject() == other && x.GetDirect() != self)
				    && (self.CheckRoleName("partner",other)))
				   && roomMan.IsPersonInSameRoomAsMe(self, other))
				{  return true; }
			}

			if( self != other){
				if((relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["stayaspartner"] && x.GetSubject() == other && x.GetDirect() != self)
				    && (self.CheckRoleName("partner",other)))
				   && roomMan.IsPersonInSameRoomAsMe(self, other))
				{  return true; }
			}

			if(self != other){
				if(!(self.CheckRoleName("partner",other)) && roomMan.IsPersonInSameRoomAsMe(self, other)){
					if(self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.4f){
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
			debug.Write(""+relationSystem.posActions["askifshouldbepartner"].name);
			if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["askifshouldbepartner"] && x.GetSubject() == other && x.GetDirect() == self && HowLongAgo(x.GetTime()) < 10f)){
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
		{	if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["stayaspartner"] && x.GetSubject() == self && HowLongAgo(x.GetTime()) > 10f)){
				return false;
			}

			if(relationSystem.historyBook.Exists(x=>x.GetAction()==relationSystem.posActions["askifshouldbepartner"] && x.GetSubject() == other && x.GetDirect() == self && HowLongAgo(x.GetTime()) < 10f)){
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
		{	if(self != other && roomMan.IsPersonInSameRoomAsMe(self, other)){ //LvlOfInfl0.2
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
				                                         x.GetAction()==relationSystem.posActions["convict"] || x.GetAction()==relationSystem.posActions["poison"] || 
				                                         x.GetAction()==relationSystem.posActions["sabotage"] || x.GetAction()==relationSystem.posActions["fight"] || 
				                                         x.GetAction()==relationSystem.posActions["convict"])
				                                     && (x.GetDirect() == other || x.GetSubject() == other) && HowLongAgo(x.GetTime()) < 10f))
					{ return true; }
			}
			if(relationSystem.historyBook.Exists(x=>x.GetDirect() == self && (x.GetAction()==relationSystem.posActions["poison"] ||
			   x.GetAction()==relationSystem.posActions["sabotage"] || x.GetAction()==relationSystem.posActions["fight"] || x.GetAction()==relationSystem.posActions["steal"]) && HowLongAgo(x.GetTime()) < 10f))
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
		{	if((relationSystem.historyBook.Exists(x=>(x.GetAction()==relationSystem.posActions["steal"] ||
                                                      x.GetAction()==relationSystem.posActions["fight"] || x.GetAction()==relationSystem.posActions["kill"] ||
                                                      x.GetAction()==relationSystem.posActions["poison"]  || x.GetAction()==relationSystem.posActions["sabotage"] ||
                                                      x.GetRule().GetRuleStrength() < -0.5f) 
			&& x.GetSubject()==other && x.GetAction()!=relationSystem.posActions["donothing"])) && roomMan.IsPersonInSameRoomAsMe(self, other) && self != other)
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
					   self.moods[MoodTypes.energTired] > -0.4f && !roomMan.IsPersonInSameRoomAsMe(self, other)){
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
			if(HowLongAgo(relationSystem.historyBook.Find(x=> (x.GetAction()==relationSystem.posActions["movetolivingroom"] || x.GetAction()==relationSystem.posActions["movetolivingroom"] || x.GetAction()==relationSystem.posActions["movetolivingroom"]) && x.GetSubject()==self).GetTime()) < 10f){
			//	debug.Write("FALSE");
				return false;
			}
			if(self != null && !(roomMan.GetRoomIAmIn(self) == "Living Room")){
				//if((self.moods[MoodTypes.energTired] < -0.2f)){
					return true;
				//}
			}

			if(roomMan.GetRoomIAmIn(other) == "Living Room" && self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.0f){
				return true;
			}
			return false; 
        };

		RuleConditioner movetoentryhallCondition = (self, other, indPpl) =>
        {

			if(HowLongAgo(relationSystem.historyBook.Find(x=> (x.GetAction()==relationSystem.posActions["movetolivingroom"] || x.GetAction()==relationSystem.posActions["movetolivingroom"] || x.GetAction()==relationSystem.posActions["movetolivingroom"]) && x.GetSubject()==self).GetTime()) < 10f){
				return false;
			}
			if (self != null && (roomMan.GetRoomIAmIn(self) == "Living Room")) { 
				//if((self.moods[MoodTypes.energTired] < -0.2f)){
					return true;
				//}
			}
			if (self != null && (roomMan.GetRoomIAmIn(self) == "Living Room")) { 

				if(roomMan.GetRoomIAmIn(other) == "Entrance" && self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.0f){
					return true;
				}
			}
			return false; 
        };

		RuleConditioner movetokitchenCondition = (self, other, indPpl) =>
        {
			if(HowLongAgo(relationSystem.historyBook.Find(x=> (x.GetAction()==relationSystem.posActions["movetolivingroom"] || x.GetAction()==relationSystem.posActions["movetolivingroom"] || x.GetAction()==relationSystem.posActions["movetolivingroom"]) && x.GetSubject()==self).GetTime()) < 10f){
				return false;
			}
            if (self != null && (roomMan.GetRoomIAmIn(self) == "Living Room")) { 
				//if((self.moods[MoodTypes.energTired] < -0.2f)){
					return true;
				//}
			}
			if(roomMan.GetRoomIAmIn(other) == "Kitchen" && self.GetOpinionValue(TraitTypes.NiceNasty,other) > 0.0f){
				return true;
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
				float r = Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),self.CalculateTraitType(TraitTypes.NiceNasty));
			    return r;
		    };

		    RulePreference kissPreference = (self, other) => {
			    if(self != other){
				    if(self.CheckRoleName("partner",other)){
					    return Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),self.moods[MoodTypes.arousDisgus]);
				    }
			    }
			    float ret = 0 + Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),self.CalculateTraitType(TraitTypes.CharitableGreedy));
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
				r += Calculator.UnboundAdd(self.moods[MoodTypes.energTired],r);
			    r += Calculator.UnboundAdd(self.moods[MoodTypes.hapSad],r);
			    r += Calculator.UnboundAdd(self.CalculateTraitType(TraitTypes.NiceNasty),r);

				r += Calculator.UnboundAdd(-self.CalculateTraitType(TraitTypes.NiceNasty),r);
				r += Calculator.UnboundAdd(-self.CalculateTraitType(TraitTypes.CharitableGreedy),r);
			    return r;
		    };

		    RulePreference cryPreference = (self, other) => { 
			    float r = 0;
			    if(self != other){
				    if(relationSystem.historyBook.Exists(x=>(x.GetAction()==relationSystem.posActions["kill"] || x.GetAction()==relationSystem.posActions["flee"] || 
				                                             x.GetAction()==relationSystem.posActions["sabotage"] || x.GetAction()==relationSystem.posActions["poison"] ||
				                                        	 x.GetAction()==relationSystem.posActions["fight"] && x.GetAction()==relationSystem.posActions["steal"]) && x.GetDirect() == other && HowLongAgo(x.GetTime()) < 10f)){
					    //debug.Write("OR THIS ONE "+self.moods[MoodTypes.hapSad]+" "+self.CalculateTraitType(TraitTypes.NiceNasty));
					    r = Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),self.GetOpinionValue(TraitTypes.HonestFalse,other));
					    r += Calculator.UnboundAdd(self.CalculateTraitType(TraitTypes.NiceNasty),r);
				    }
				else if(relationSystem.historyBook.Exists(x=>(x.GetAction()==relationSystem.posActions["fight"]) && x.GetSubject() == other && HowLongAgo(x.GetTime()) < 10f)){
					r = Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),self.GetOpinionValue(TraitTypes.HonestFalse,other));
					r += Calculator.UnboundAdd(self.CalculateTraitType(TraitTypes.NiceNasty),r);
				}
				    else{
					    //debug.Write("OR END "+self.moods[MoodTypes.hapSad]+" "+self.CalculateTraitType(TraitTypes.NiceNasty));
						r = Calculator.UnboundAdd(self.CalculateTraitType(TraitTypes.CharitableGreedy),self.moods[MoodTypes.hapSad]);
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

		    RulePreference askIfShouldBePartnerPreference = (self, other) => {
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
				float r = -(relationSystem.historyBook.Find(x=>(x.GetAction()==relationSystem.posActions["steal"] ||
			       		    x.GetAction()==relationSystem.posActions["fight"] || x.GetAction()==relationSystem.posActions["kill"] ||
			       		    x.GetAction()==relationSystem.posActions["poison"]  || x.GetAction()==relationSystem.posActions["sabotage"] ||
			                x.GetRule().GetRuleStrength() < -0.5f) && x.GetSubject()==other).GetRule().GetRuleStrength());
				
				r += Calculator.UnboundAdd(-(self.GetOpinionValue(TraitTypes.NiceNasty,other)),r);
				r += Calculator.UnboundAdd(-(self.GetOpinionValue(TraitTypes.HonestFalse,other)),r);    
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
				r += Calculator.UnboundAdd(self.CalculateTraitType(TraitTypes.CharitableGreedy),r);
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
			    r += Calculator.UnboundAdd(Mathf.Abs(self.moods[MoodTypes.angryFear]),r);

			    if(roomMan.GetRoomIAmIn(other) == "Living Room"){
				    r += Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),r);
			    }

			    return r;
		    };

		    RulePreference moveToKitchenPreference = (self, other) => { 
			    float r = Calculator.UnboundAdd(0.1f,-self.moods[MoodTypes.energTired]);
			    r += Calculator.UnboundAdd(-self.moods[MoodTypes.hapSad],r);
			    r += Calculator.UnboundAdd(-self.moods[MoodTypes.angryFear],r);

			    if(roomMan.GetRoomIAmIn(other) == "Kitchen"){
				    r += Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),r);
			    }
			    return r;
		    };

		    RulePreference moveToEntryHallPreference = (self, other) => { 
			    float r = Calculator.UnboundAdd(0.1f,-self.moods[MoodTypes.energTired]);
			    r += Calculator.UnboundAdd(-self.moods[MoodTypes.hapSad],r);
			    r += Calculator.UnboundAdd(-self.moods[MoodTypes.angryFear],r);

			    if(roomMan.GetRoomIAmIn(other) == "Entrance"){
				    r += Calculator.UnboundAdd(self.GetOpinionValue(TraitTypes.NiceNasty,other),r);
			    }
			    return r;
		    };
        #endregion

        // ---------------------------------------------------------------------------------------------------------------------- CREATING RULES


		#region Rules
		    // INTERPERSONAL RULES
		    CreateNewRule("kiss", "kiss", kissCondition,kissPreference);
		    CreateNewRule("chooseAnotherAsPartner", "chooseAnotherAsPartner", chooseAnotherAsPartnerCondition,chooseAnotherAsPartnerPreference);
		    CreateNewRule("stayAsPartner", "stayAsPartner", stayAsPartnerCondition,StayAsPartnerPreference);
		    CreateNewRule("leavePartner", "leavePartner", LeavePartnerCondition,LeavePartnerPreference);
		    CreateNewRule("askIfShouldBePartner", "askIfShouldBePartner", askIfShouldBePartnerCondition,askIfShouldBePartnerPreference);

		    CreateNewRule("flirt", "flirt", flirtCondition,flirtPreference);
		    CreateNewRule("chat", "chat", chatCondition,chatPreference);
		    CreateNewRule("giveGift", "giveGift", giveGiftCondition,giveGiftPreference);
		    CreateNewRule("gossip", "gossip", gossipCondition,gossipPreference);
		    CreateNewRule("argue", "argue", argueCondition,arguePreference);
		    CreateNewRule("deny", "deny", denyCondition,denyPreference);
		    CreateNewRule("makeDistraction", "makeDistraction", makeDistractionCondition,makeDistractionPreference);
		    CreateNewRule("reminisce", "reminisce", reminisceCondition,reminiscePreference);
		    CreateNewRule("praise", "praise", praiseCondition,praisePreference);
		    CreateNewRule("makeFunOf", "makeFunOf", makefunofCondition,makeFunOfPreference);
		    CreateNewRule("tellJoke", "tellJoke", telljokeCondition,telljokePreference);
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
			CreateNewRule("fightfbunce", "fight", fightCondition,fightPreference);
			CreateNewRule("fightfcess", "fight", fightCondition,fightPreference);
			CreateNewRule("fightfbunsant", "fight", fightCondition,fightPreference);
		    CreateNewRule("bribe", "bribe", bribeCondition,bribePreference);
		    CreateNewRule("bribefbunce", "bribe", bribeCondition,bribePreference);
		    CreateNewRule("bribefcess", "bribe", bribeCondition,bribePreference);
		    CreateNewRule("bribefbunsant", "bribe", bribeCondition,bribePreference);
		    CreateNewRule("argueInnocence", "argueInnocence", argueInnocenceCondition,argueInnocencePreference);
		    CreateNewRule("argueinnocencefbunce", "argueInnocence", argueInnocenceCondition,argueInnocencePreference);
		    CreateNewRule("argueinnocencefcess", "argueInnocence", argueInnocenceCondition,argueInnocencePreference);
		    CreateNewRule("argueGuiltiness", "argueGuiltiness", argueGuiltinessCondition,argueGuiltinessPreference);
		    CreateNewRule("argueguiltinessfbunce", "argueGuiltiness", argueGuiltinessCondition,argueGuiltinessPreference);
		    CreateNewRule("argueguiltinessfcess", "argueGuiltiness", argueGuiltinessCondition,argueGuiltinessPreference);
		    CreateNewRule("steal", "steal", stealCondition,stealPreference);
		    CreateNewRule("kill", "kill",killCondition,killPreference);
		    CreateNewRule("killfbunsant", "kill",killCondition,killPreference);
		    CreateNewRule("killfbunce", "kill",killCondition,killPreference);
		    CreateNewRule("killfcess", "kill",killCondition,killPreference);
		    CreateNewRule("poison", "poison", poisonCondition,poisonPreference);
		    CreateNewRule("poisonfbunce", "poison", poisonCondition,poisonPreference);
		    CreateNewRule("poisonfcess", "poison", poisonCondition,poisonPreference);
		    CreateNewRule("poisonfbunsant", "poison", poisonCondition,poisonPreference);
		    CreateNewRule("playGame", "playGame", playgameCondition,playgamePreference);
		    CreateNewRule("playgamefbunce", "playGame", playgameCondition,playgamePreference);
		    CreateNewRule("playgamefcess", "playGame", playgameCondition,playgamePreference);
		    CreateNewRule("playgamefbunsant", "playGame", playgameCondition,playgamePreference);
		    CreateNewRule("order", "order", orderCondition,orderPreference);
		    CreateNewRule("orderfbunce", "order", orderCondition,orderPreference);
		    CreateNewRule("orderfcess", "order", orderCondition,orderPreference);
		    CreateNewRule("orderfbunsant", "order", orderCondition,orderPreference);

			CreateNewRule("moveToLivingRoom", "moveToLivingRoom", movetolivingroomCondition,moveToLivingRoomPreference);
		    CreateNewRule("movetolivingroomfbunce", "moveToLivingRoom", movetolivingroomCondition,moveToLivingRoomPreference);
		    CreateNewRule("movetolivingroomfcess", "moveToLivingRoom", movetolivingroomCondition,moveToLivingRoomPreference);
		    CreateNewRule("movetolivingroomfbunsant", "moveToLivingRoom", movetolivingroomCondition,moveToLivingRoomPreference);
			CreateNewRule("moveToKitchen", "moveToKitchen", movetokitchenCondition,moveToKitchenPreference);
		    CreateNewRule("movetokitchenfbunce", "moveToKitchen", movetokitchenCondition,moveToKitchenPreference);
		    CreateNewRule("movetokitchenfcess", "moveToKitchen", movetokitchenCondition,moveToKitchenPreference);
		    CreateNewRule("movetokitchenfbunsant", "moveToKitchen", movetokitchenCondition,moveToKitchenPreference);
			CreateNewRule("moveToEntryHall", "moveToEntryHall", movetoentryhallCondition,moveToEntryHallPreference);
		    CreateNewRule("movetoentryhallfbunce", "moveToEntryHall", movetoentryhallCondition,moveToEntryHallPreference);
		    CreateNewRule("movetoentryhallfcess", "moveToEntryHall", movetoentryhallCondition,moveToEntryHallPreference);
		    CreateNewRule("movetoentryhallfbunsant", "moveToEntryHall", movetoentryhallCondition,moveToEntryHallPreference);
		    CreateNewRule("buyCompany", "buyCompany", buyCompanyCondition,buyCompanyPreference);
		    CreateNewRule("sabotage", "sabotage", sabotageCondition,sabotagePreference);
		    CreateNewRule("advertise", "advertise", advertiseCondition,advertisePreference);
		    CreateNewRule("demandToLeaveGuild", "demandToLeaveGuild", DemandtoLeaveGuildCondition,demandoLeaveGuildPreference);
		    CreateNewRule("sellCompany", "sellCompany", sellCompanyCondition,sellCompanyPreference);
		    CreateNewRule("buyGoods", "buyGoods", buyGoodsCondition,buyGoodsPreference);
		    CreateNewRule("sellGoods", "sellGoods", sellGoodsCondition,sellGoodsPreference);

		    //SELF RULES
		    CreateNewRule("doNothing", "doNothing", emptyCondition, doNothingPreference);
		    CreateNewRule("flee", "flee", fleeCondition,fleePreference);

		#endregion Rules

    // ------------------------------------------------------------------------------------------------------------------------------------------ RULES THAT MIGHT HAPPEN (REACTION RULES)

		#region RulesToTrigger
		    // ------------- INTERPERSONAL
		    List<Rule> kissRulesToTrigger = new List<Rule>(); kissRulesToTrigger.Add(GetRule("giveGift"));
			AddPossibleRulesToRule("kiss",kissRulesToTrigger); AddPossibleRulesToRule("deny",kissRulesToTrigger); AddPossibleRulesToRule("chat",kissRulesToTrigger); AddPossibleRulesToRule("flirt",kissRulesToTrigger); 
		AddPossibleRulesToRule("askIfShouldBePartner",kissRulesToTrigger); 
			AddPossibleRulesToRule("gossip",kissRulesToTrigger); AddPossibleRulesToRule("deny",kissRulesToTrigger);

		    List<Rule> askIfShouldBePartnerRulesToTrigger = new List<Rule>(); askIfShouldBePartnerRulesToTrigger.Add(GetRule("chooseAnotherAsPartner"));
		    askIfShouldBePartnerRulesToTrigger.Add(GetRule("stayAsPartner")); askIfShouldBePartnerRulesToTrigger.Add(GetRule("leavePartner"));
		    AddPossibleRulesToRule("askIfShouldBePartner",askIfShouldBePartnerRulesToTrigger);

		    List<Rule> chooseanotheraspartnerRulesToTrigger = new List<Rule>(); chooseanotheraspartnerRulesToTrigger.Add(GetRule("kiss")); chooseanotheraspartnerRulesToTrigger.Add(GetRule("flirt")); 
			chooseanotheraspartnerRulesToTrigger.Add(GetRule("deny")); chooseanotheraspartnerRulesToTrigger.Add(GetRule("fight")); chooseanotheraspartnerRulesToTrigger.Add(GetRule("argue"));
		    AddPossibleRulesToRule("chooseAnotherAsPartner",chooseanotheraspartnerRulesToTrigger);

		    List<Rule> stayaspartnerRulesToTrigger = new List<Rule>(); stayaspartnerRulesToTrigger.Add(GetRule("kiss")); stayaspartnerRulesToTrigger.Add(GetRule("flirt")); 
			stayaspartnerRulesToTrigger.Add(GetRule("givegift")); 
		    AddPossibleRulesToRule("stayAsPartner",stayaspartnerRulesToTrigger);

		    List<Rule> leavepartnerRulesToTrigger = new List<Rule>(); leavepartnerRulesToTrigger.Add(GetRule("deny")); leavepartnerRulesToTrigger.Add(GetRule("poison"));
		leavepartnerRulesToTrigger.Add(GetRule("fight")); leavepartnerRulesToTrigger.Add(GetRule("argue")); 
		    AddPossibleRulesToRule("leavePartner",leavepartnerRulesToTrigger);

		    List<Rule> flirtRulesToTrigger = new List<Rule>(); flirtRulesToTrigger.Add(GetRule("deny")); flirtRulesToTrigger.Add(GetRule("flirt"));
		flirtRulesToTrigger.Add(GetRule("kiss")); flirtRulesToTrigger.Add(GetRule("fight")); flirtRulesToTrigger.Add(GetRule("tellJoke")); flirtRulesToTrigger.Add(GetRule("gossip"));
		    AddPossibleRulesToRule("flirt",flirtRulesToTrigger);

		    List<Rule> chatRulesToTrigger = new List<Rule>(); chatRulesToTrigger.Add(GetRule("chat")); chatRulesToTrigger.Add(GetRule("deny"));
		chatRulesToTrigger.Add(GetRule("gossip")); chatRulesToTrigger.Add(GetRule("flirt")); chatRulesToTrigger.Add(GetRule("tellJoke"));
		    chatRulesToTrigger.Add(GetRule("reminisce")); chatRulesToTrigger.Add(GetRule("argue"));
		    AddPossibleRulesToRule("chat",chatRulesToTrigger);

		    List<Rule> giveGiftRulesToTrigger = new List<Rule>(); giveGiftRulesToTrigger.Add(GetRule("reminisce")); giveGiftRulesToTrigger.Add(GetRule("flirt"));
		giveGiftRulesToTrigger.Add(GetRule("deny")); giveGiftRulesToTrigger.Add(GetRule("askIfShouldBePartner")); giveGiftRulesToTrigger.Add(GetRule("praise"));
		    AddPossibleRulesToRule("giveGift",giveGiftRulesToTrigger);

		    List<Rule> gossipRulesToTrigger = new List<Rule>(); gossipRulesToTrigger.Add(GetRule("reminisce")); gossipRulesToTrigger.Add(GetRule("flirt"));
		gossipRulesToTrigger.Add(GetRule("deny")); gossipRulesToTrigger.Add(GetRule("argue")); gossipRulesToTrigger.Add(GetRule("fight")); gossipRulesToTrigger.Add(GetRule("praise"));
		    AddPossibleRulesToRule("gossip",gossipRulesToTrigger);

		    List<Rule> argueRulesToTrigger = new List<Rule>(); argueRulesToTrigger.Add(GetRule("argue")); argueRulesToTrigger.Add(GetRule("fight"));
		    argueRulesToTrigger.Add(GetRule("sabotage")); argueRulesToTrigger.Add(GetRule("order")); 
			argueRulesToTrigger.Add(GetRule("deny")); argueRulesToTrigger.Add(GetRule("harass")); argueRulesToTrigger.Add(GetRule("kill")); argueRulesToTrigger.Add(GetRule("flee"));
		argueRulesToTrigger.Add(GetRule("flee")); argueRulesToTrigger.Add(GetRule("moveToLivingRoom")); argueRulesToTrigger.Add(GetRule("moveToKitchen")); argueRulesToTrigger.Add(GetRule("moveToKitchen"));
		argueRulesToTrigger.Add(GetRule("cry"));
		    AddPossibleRulesToRule("argue",argueRulesToTrigger);

		    List<Rule> denyRulesToTrigger = new List<Rule>(); denyRulesToTrigger.Add(GetRule("argue")); denyRulesToTrigger.Add(GetRule("poison")); 
		denyRulesToTrigger.Add(GetRule("fight")); denyRulesToTrigger.Add(GetRule("prank")); denyRulesToTrigger.Add(GetRule("harass")); denyRulesToTrigger.Add(GetRule("sabotage"));
		    AddPossibleRulesToRule("deny",denyRulesToTrigger);

		    List<Rule> makedistractionRulesToTrigger = new List<Rule>(); makedistractionRulesToTrigger.Add(GetRule("argue")); makedistractionRulesToTrigger.Add(GetRule("poison")); 
		    makedistractionRulesToTrigger.Add(GetRule("steal")); makedistractionRulesToTrigger.Add(GetRule("deny")); makedistractionRulesToTrigger.Add(GetRule("sabotage"));
		makedistractionRulesToTrigger.Add(GetRule("order"));
		    AddPossibleRulesToRule("makeDistraction",makedistractionRulesToTrigger);

		    List<Rule> reminisceRulesToTrigger = new List<Rule>(); reminisceRulesToTrigger.Add(GetRule("chat")); reminisceRulesToTrigger.Add(GetRule("gossip"));
		reminisceRulesToTrigger.Add(GetRule("flirt")); reminisceRulesToTrigger.Add(GetRule("deny")); reminisceRulesToTrigger.Add(GetRule("tellJoke")); reminisceRulesToTrigger.Add(GetRule("makeFunOf"));
		    AddPossibleRulesToRule("reminisce",reminisceRulesToTrigger);

		    List<Rule> praiseRulesToTrigger = new List<Rule>(); praiseRulesToTrigger.Add(GetRule("chat"));
		praiseRulesToTrigger.Add(GetRule("flirt")); praiseRulesToTrigger.Add(GetRule("fight")); praiseRulesToTrigger.Add(GetRule("kiss")); praiseRulesToTrigger.Add(GetRule("gossip"));
		    AddPossibleRulesToRule("praise",praiseRulesToTrigger);

		    List<Rule> makefunofRulesToTrigger = new List<Rule>(); makefunofRulesToTrigger.Add(GetRule("makeFunOf")); makefunofRulesToTrigger.Add(GetRule("argue"));
		    makefunofRulesToTrigger.Add(GetRule("harass")); makefunofRulesToTrigger.Add(GetRule("deny"));
		makefunofRulesToTrigger.Add(GetRule("tellJoke")); makefunofRulesToTrigger.Add(GetRule("fight")); makefunofRulesToTrigger.Add(GetRule("reminisce"));
		    AddPossibleRulesToRule("makeFunOf",makefunofRulesToTrigger);

		    List<Rule> telljokeRulesToTrigger = new List<Rule>(); telljokeRulesToTrigger.Add(GetRule("makeFunOf")); telljokeRulesToTrigger.Add(GetRule("tellJoke")); 
		telljokeRulesToTrigger.Add(GetRule("chat")); telljokeRulesToTrigger.Add(GetRule("praise")); telljokeRulesToTrigger.Add(GetRule("gossip"));  telljokeRulesToTrigger.Add(GetRule("flirt")); 
		    AddPossibleRulesToRule("tellJoke",telljokeRulesToTrigger);

		    List<Rule> prankRulesToTrigger = new List<Rule>(); prankRulesToTrigger.Add(GetRule("makeFunOf")); prankRulesToTrigger.Add(GetRule("deny"));
		prankRulesToTrigger.Add(GetRule("convict")); prankRulesToTrigger.Add(GetRule("argue")); prankRulesToTrigger.Add(GetRule("order"));  prankRulesToTrigger.Add(GetRule("harass"));  
		prankRulesToTrigger.Add(GetRule("fight")); 
		    AddPossibleRulesToRule("prank",prankRulesToTrigger);

		    List<Rule> harassRulesToTrigger = new List<Rule>(); harassRulesToTrigger.Add(GetRule("tellJoke")); harassRulesToTrigger.Add(GetRule("deny"));
		harassRulesToTrigger.Add(GetRule("argue")); harassRulesToTrigger.Add(GetRule("fight")); harassRulesToTrigger.Add(GetRule("order")); harassRulesToTrigger.Add(GetRule("poison")); 
		harassRulesToTrigger.Add(GetRule("askIfShouldBePartner"));
		    AddPossibleRulesToRule("harass",harassRulesToTrigger);

		    List<Rule> cryRulesToTrigger = new List<Rule>(); cryRulesToTrigger.Add(GetRule("cry")); cryRulesToTrigger.Add(GetRule("reminisce"));
		    cryRulesToTrigger.Add(GetRule("giveGift")); cryRulesToTrigger.Add(GetRule("kiss")); cryRulesToTrigger.Add(GetRule("console")); 
		cryRulesToTrigger.Add(GetRule("chat"));  cryRulesToTrigger.Add(GetRule("argue")); 
		    AddPossibleRulesToRule("cry",cryRulesToTrigger);

		    List<Rule> consoleRulesToTrigger = new List<Rule>(); consoleRulesToTrigger.Add(GetRule("chat")); consoleRulesToTrigger.Add(GetRule("reminisce"));
		consoleRulesToTrigger.Add(GetRule("kiss")); consoleRulesToTrigger.Add(GetRule("flirt"));
		    AddPossibleRulesToRule("console",consoleRulesToTrigger);

		    // ------------- CULTURE
		    List<Rule> greetRulesToTrigger = new List<Rule>(); greetRulesToTrigger.Add(GetRule("chat")); greetRulesToTrigger.Add(GetRule("kiss"));
		greetRulesToTrigger.Add(GetRule("greet")); greetRulesToTrigger.Add(GetRule("tellJoke"));
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
		fightRulesToTrigger.Add(GetRule("poison")); fightRulesToTrigger.Add(GetRule("sabotage")); fightRulesToTrigger.Add(GetRule("argue")); fightRulesToTrigger.Add(GetRule("bribe"));  
		fightRulesToTrigger.Add(GetRule("kill"));  fightRulesToTrigger.Add(GetRule("order"));
		    AddPossibleRulesToRule("fight",fightRulesToTrigger);
			AddPossibleRulesToRule("fightfbunce",fightRulesToTrigger);
			AddPossibleRulesToRule("fightfbunsant",fightRulesToTrigger);
			AddPossibleRulesToRule("fightfcess",fightRulesToTrigger);


		    List<Rule> bribeRulesToTrigger = new List<Rule>(); bribeRulesToTrigger.Add(GetRule("fight")); bribeRulesToTrigger.Add(GetRule("convict"));
		bribeRulesToTrigger.Add(GetRule("gossip")); bribeRulesToTrigger.Add(GetRule("deny")); bribeRulesToTrigger.Add(GetRule("steal")); 
		    AddPossibleRulesToRule("bribe",bribeRulesToTrigger);
		    AddPossibleRulesToRule("bribefbunce",bribeRulesToTrigger);
		    AddPossibleRulesToRule("bribefcess",bribeRulesToTrigger);
		    AddPossibleRulesToRule("bribefbunsant",bribeRulesToTrigger);

		    List<Rule> argueinnocenceRulesToTrigger = new List<Rule>(); argueinnocenceRulesToTrigger.Add(GetRule("chat")); argueinnocenceRulesToTrigger.Add(GetRule("argue"));
		argueinnocenceRulesToTrigger.Add(GetRule("deny")); argueinnocenceRulesToTrigger.Add(GetRule("praise"));
		    AddPossibleRulesToRule("argueInnocence",argueinnocenceRulesToTrigger);
		    AddPossibleRulesToRule("argueinnocencefbunce",argueinnocenceRulesToTrigger);
		    AddPossibleRulesToRule("argueinnocencefcess",argueinnocenceRulesToTrigger);

		    List<Rule> argueguiltinessRulesToTrigger = new List<Rule>(); argueinnocenceRulesToTrigger.Add(GetRule("chat")); argueinnocenceRulesToTrigger.Add(GetRule("argue"));
		argueinnocenceRulesToTrigger.Add(GetRule("deny")); argueinnocenceRulesToTrigger.Add(GetRule("fight"));
		    AddPossibleRulesToRule("argueGuiltiness",argueguiltinessRulesToTrigger);
		    AddPossibleRulesToRule("argueguiltinessfbunce",argueguiltinessRulesToTrigger);
		    AddPossibleRulesToRule("argueguiltinessfcess",argueguiltinessRulesToTrigger);

		    List<Rule> stealRulesToTrigger = new List<Rule>();  stealRulesToTrigger.Add(GetRule("poison")); 
		stealRulesToTrigger.Add(GetRule("fight")); stealRulesToTrigger.Add(GetRule("convict")); stealRulesToTrigger.Add(GetRule("kill")); stealRulesToTrigger.Add(GetRule("sabotage"));  stealRulesToTrigger.Add(GetRule("cry"));
		    AddPossibleRulesToRule("steal",stealRulesToTrigger); AddPossibleRulesToRule("buyGoods",stealRulesToTrigger);

		    List<Rule> poisonRulesToTrigger = new List<Rule>(); poisonRulesToTrigger.Add(GetRule("fight")); poisonRulesToTrigger.Add(GetRule("argue"));
		poisonRulesToTrigger.Add(GetRule("sabotage"));  poisonRulesToTrigger.Add(GetRule("kill"));  poisonRulesToTrigger.Add(GetRule("cry"));
		    AddPossibleRulesToRule("poison",poisonRulesToTrigger);
		    AddPossibleRulesToRule("poisonfbunce",poisonRulesToTrigger);
		    AddPossibleRulesToRule("poisonfcess",poisonRulesToTrigger);
		    AddPossibleRulesToRule("poisonfbunsant",poisonRulesToTrigger);

		    List<Rule> playgameRulesToTriger = new List<Rule>(); playgameRulesToTriger.Add(GetRule("tellJoke")); playgameRulesToTriger.Add(GetRule("chat")); 
		playgameRulesToTriger.Add(GetRule("reminisce")); playgameRulesToTriger.Add(GetRule("prank"));  playgameRulesToTriger.Add(GetRule("reminisce")); 
		    AddPossibleRulesToRule("playGame",playgameRulesToTriger);
		    AddPossibleRulesToRule("playgamefbunce",playgameRulesToTriger);
		    AddPossibleRulesToRule("playgamefbunsant",playgameRulesToTriger);
		    AddPossibleRulesToRule("playgamefcess",playgameRulesToTriger);

		    List<Rule> orderRulesToTriger = new List<Rule>(); orderRulesToTriger.Add(GetRule("deny")); orderRulesToTriger.Add(GetRule("fight"));
		orderRulesToTriger.Add(GetRule("flee")); orderRulesToTriger.Add(GetRule("bribe"));
		orderRulesToTriger.Add(GetRule("moveToLivingRoom")); orderRulesToTriger.Add(GetRule("moveToKitchen")); orderRulesToTriger.Add(GetRule("moveToEntryHall"));
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
		buycompanyRulesToTrigger.Add(GetRule("sabotage")); buycompanyRulesToTrigger.Add(GetRule("demandToLeaveGuild")); buycompanyRulesToTrigger.Add(GetRule("buyCompany")); buycompanyRulesToTrigger.Add(GetRule("sellCompany"));
		buycompanyRulesToTrigger.Add(GetRule("steal")); buycompanyRulesToTrigger.Add(GetRule("fight"));
		    AddPossibleRulesToRule("buyCompany",buycompanyRulesToTrigger);

		    List<Rule> sellcompanyRulesToTrigger = new List<Rule>();
		    sellcompanyRulesToTrigger.Add(GetRule("argue"));
		sellcompanyRulesToTrigger.Add(GetRule("sabotage")); sellcompanyRulesToTrigger.Add(GetRule("demandToLeaveGuild")); buycompanyRulesToTrigger.Add(GetRule("buyCompany")); buycompanyRulesToTrigger.Add(GetRule("sellCompany"));
		sellcompanyRulesToTrigger.Add(GetRule("fight"));
		    AddPossibleRulesToRule("sellCompany",sellcompanyRulesToTrigger);

		    List<Rule> advertiseRulesToTrigger = new List<Rule>();
		    sellcompanyRulesToTrigger.Add(GetRule("argue")); sellcompanyRulesToTrigger.Add(GetRule("chat"));
		sellcompanyRulesToTrigger.Add(GetRule("deny")); sellcompanyRulesToTrigger.Add(GetRule("steal"));
		    AddPossibleRulesToRule("advertise",advertiseRulesToTrigger);

		    List<Rule> demandtoleaveguildRulesToTrigger = new List<Rule>();
		    demandtoleaveguildRulesToTrigger.Add(GetRule("argue")); demandtoleaveguildRulesToTrigger.Add(GetRule("poison")); 
		    demandtoleaveguildRulesToTrigger.Add(GetRule("fight")); demandtoleaveguildRulesToTrigger.Add(GetRule("deny")); 
		    demandtoleaveguildRulesToTrigger.Add(GetRule("order")); 
		    AddPossibleRulesToRule("demandToLeaveGuild",demandtoleaveguildRulesToTrigger);

		    List<Rule> buygoodsRulesToTrigger = new List<Rule>(); 
		    buygoodsRulesToTrigger.Add(GetRule("chat"));
		    buygoodsRulesToTrigger.Add(GetRule("advertise"));
		    AddPossibleRulesToRule("buyGoods",buygoodsRulesToTrigger);

		    List<Rule> sellgoodsRulesToTrigger = new List<Rule>(); 
		    buygoodsRulesToTrigger.Add(GetRule("chat")); buygoodsRulesToTrigger.Add(GetRule("argue")); buygoodsRulesToTrigger.Add(GetRule("sabotage"));
		    AddPossibleRulesToRule("sellGoods",sellgoodsRulesToTrigger);

		#endregion RulesToTrigger

    // ----------------------------------------------------------------------------------------------- ADDING RULES TO MASKS
	
		#region addingRulesToMask
	    // SElF
		    AddRuleToMask("John", "Self", "doNothing", -1.0f);
		    AddRuleToMask("Therese", "Self", "doNothing", -1.0f);
		    AddRuleToMask("Bill", "Self", "doNothing", -1.0f);
		    AddRuleToMask("Heather", "Self", "doNothing", -1.0f);
		    AddRuleToMask(playerName, "Self", "doNothing", -1.0f);

		    AddRuleToMask("John", "Self", "flee", 0.2f);
		    AddRuleToMask("Heather", "Self", "flee", -0.1f);
		    AddRuleToMask(playerName, "Self", "flee", -0.1f);

		    AddRuleToMask("John", "Self", "chooseAnotherAsPartner", -0.2f);
		    AddRuleToMask("Therese", "Self", "chooseAnotherAsPartner", -0.2f);
		    AddRuleToMask("Bill", "Self", "chooseAnotherAsPartner", 0.4f);
		    AddRuleToMask("Heather", "Self", "chooseAnotherAsPartner", 0.5f);
		    AddRuleToMask(playerName, "Self", "chooseAnotherAsPartner", -0.4f);
		
	    // INTERPERSONAL
		    AddRuleToMask("RomanticRelationship", "Partner", "kiss", 0.1f);
			AddRuleToMask("Friendship", "Friend", "kiss", -0.4f);
			AddRuleToMask("Rivalry", "Enemy", "kiss", -0.4f);

		    AddRuleToMask("RomanticRelationship", "Partner", "askIfShouldBePartner", 0.6f);
		    AddRuleToMask("RomanticRelationship", "Partner", "stayAsPartner", 0.3f);
		    AddRuleToMask("RomanticRelationship", "Partner", "leavePartner", -0.1f);

		    AddRuleToMask("RomanticRelationship", "Partner", "flirt", 0.1f);
		    AddRuleToMask("Friendship", "Friend", "flirt", -0.2f);

		    AddRuleToMask("RomanticRelationship", "Partner", "chat", 0.4f);
		    AddRuleToMask("Friendship", "Friend", "chat", 0.5f);
		    AddRuleToMask("Rivalry", "Enemy", "chat", 0.2f);

		    AddRuleToMask("RomanticRelationship", "Partner", "giveGift", 0.4f);
		    AddRuleToMask("Rivalry", "Enemy", "giveGift", 0.2f);
		    AddRuleToMask("Friendship", "Friend", "giveGift", -0.3f);

		    AddRuleToMask("Rivalry", "Enemy", "gossip", -0.2f);
		    AddRuleToMask("Friendship", "Friend", "gossip", 0.2f);
		    AddRuleToMask("RomanticRelationship", "Partner", "gossip", 0.1f);

		    AddRuleToMask("Rivalry", "Enemy", "argue", 0.3f);
		    AddRuleToMask("Friendship", "Friend", "argue", -0.3f);
		    AddRuleToMask("RomanticRelationship", "Partner", "argue", -0.4f);

		    AddRuleToMask("Rivalry", "Enemy", "deny", 0.3f);
		    AddRuleToMask("Friendship", "Friend", "deny", -0.2f);
		    AddRuleToMask("RomanticRelationship", "Partner", "deny", -0.3f);

		    AddRuleToMask("Rivalry", "Enemy", "makeDistraction", -0.1f);
		    AddRuleToMask("Friendship", "Friend", "makeDistraction", -0.3f);
		    AddRuleToMask("RomanticRelationship", "Partner", "makeDistraction", -0.5f);

		    AddRuleToMask("Rivalry", "Enemy", "makeFunOf", 0.4f);
		    AddRuleToMask("Friendship", "Friend", "makeFunOf", -0.1f);
		    AddRuleToMask("RomanticRelationship", "Partner", "makeFunOf", -0.4f);

		    AddRuleToMask("Rivalry", "Enemy", "tellJoke", -0.3f);
		    AddRuleToMask("Friendship", "Friend", "tellJoke", 0.3f);
		    AddRuleToMask("RomanticRelationship", "Partner", "tellJoke", 0.2f);

		    AddRuleToMask("Rivalry", "Enemy", "prank", 0.3f);
		    AddRuleToMask("Friendship", "Friend", "prank", -0.1f);
		    AddRuleToMask("RomanticRelationship", "Partner", "prank", -0.3f);

		    AddRuleToMask("Rivalry", "Enemy", "harass", 0.3f);
		    AddRuleToMask("Friendship", "Friend", "harass", -0.4f);
		    AddRuleToMask("RomanticRelationship", "Partner", "harass", -0.5f);

		    AddRuleToMask("RomanticRelationship", "Partner", "reminisce", 0.3f);
		    AddRuleToMask("Friendship", "Friend", "reminisce", 0.1f);

		    AddRuleToMask("Friendship", "Friend", "praise", 0.2f);
		    AddRuleToMask("RomanticRelationship", "Partner", "praise", 0.3f);

		    AddRuleToMask("Friendship", "Friend", "cry", 0.9f);
		    AddRuleToMask("RomanticRelationship", "Partner", "cry", 1.0f);
		    AddRuleToMask("Rivalry", "Enemy", "cry", 0.2f);

		    AddRuleToMask("Friendship", "Friend", "console", 0.8f);
		    AddRuleToMask("RomanticRelationship", "Partner", "console", 1.0f);
		    AddRuleToMask("Rivalry", "Enemy", "console", -0.4f);

			AddRuleToMask("Friendship", "Friend", "greet", 0.7f);
			AddRuleToMask("RomanticRelationship", "Partner", "greet", 0.6f);
			AddRuleToMask("Rivalry", "Enemy", "greet", -0.4f);

	    // CULTURE
		    AddRuleToMask("Bungary", "Bunsant", "fightfbunsant", -0.5f);
		    AddRuleToMask("Bungary", "Bunsant", "bribefbunsant", -0.1f);
		    AddRuleToMask("Bungary", "Bunsant", "steal", -0.6f);
		    AddRuleToMask("Bungary", "Bunsant", "askforhelpinillicitactivity", -0.1f);
		    AddRuleToMask("Bungary", "Bunsant", "poisonfbunsant", -0.8f);
		    //AddRuleToMask("Bungary", "Bunsant", "greetfbunsant", 0.5f);
		    AddRuleToMask("Bungary", "Bunsant", "playgamefbunsant", 0.1f);
		    AddRuleToMask("Bungary", "Bunsant", "orderfbunsant", -0.5f);
		    AddRuleToMask("Bungary", "Bunsant", "movetolivingroomfbunsant", 0.0f);
		    AddRuleToMask("Bungary", "Bunsant", "movetokitchenfbunsant", 0.0f);
		    AddRuleToMask("Bungary", "Bunsant", "movetoentryhallfbunsant", 0.0f);
		    AddRuleToMask("Bungary", "Bunsant", "killfbunsant", -0.9f);

			AddRuleToMask("Bungary", "Bunsant", "fightfbunce", -0.3f);
		    AddRuleToMask("Bungary", "Bunce", "bribefbunce", 0.3f);
		    AddRuleToMask("Bungary", "Bunce", "convictfbunce", 1.0f);
		    AddRuleToMask("Bungary", "Bunce", "argueinnocencefbunce", 0.0f);
		    AddRuleToMask("Bungary", "Bunce", "argueguiltinessfbunce", 0.0f);
		    AddRuleToMask("Bungary", "Bunce", "poisonfbunce", -0.8f);
		   // AddRuleToMask("Bungary", "Bunce", "greetfbunce", 1.0f);
		    AddRuleToMask("Bungary", "Bunce", "playgamefbunce", 0.2f);
		    AddRuleToMask("Bungary", "Bunce", "orderfbunce", 0.3f);
		    AddRuleToMask("Bungary", "Bunce", "movetolivingroomfbunce", 0.5f);
		    AddRuleToMask("Bungary", "Bunce", "movetokitchenfbunce", 0.1f);
		    AddRuleToMask("Bungary", "Bunce", "movetoentryhallfbunce", 0.3f);
		    AddRuleToMask("Bungary", "Bunce", "killfbunce", -0.7f);

			AddRuleToMask("Bungary", "Bunsant", "fightfcess", -0.7f);
		    AddRuleToMask("Bungary", "Buncess", "bribefcess", 0.3f);
		    AddRuleToMask("Bungary", "Buncess", "convictfcess", 1.0f);
		    AddRuleToMask("Bungary", "Buncess", "argueinnocencefcess", 0.2f);
		    AddRuleToMask("Bungary", "Buncess", "argueguiltinessfcess", -0.1f);
		    AddRuleToMask("Bungary", "Buncess", "poisonfcess", -0.8f);
		    //AddRuleToMask("Bungary", "Buncess", "greetfcess", 1.0f);
		    AddRuleToMask("Bungary", "Buncess", "playgamefbuncess", 0.2f);
		    AddRuleToMask("Bungary", "Buncess", "orderfcess", 0.3f);
		    AddRuleToMask("Bungary", "Buncess", "movetolivingroomfcess", 0.6f);
		    AddRuleToMask("Bungary", "Buncess", "movetokitchenfcess", 0.1f);
		    AddRuleToMask("Bungary", "Buncess", "movetoentryhallfcess", 0.3f);
		    AddRuleToMask("Bungary", "Buncess", "killfcess", -0.9f);

		    AddRuleToMask("MerchantGuild", "Member", "buyCompany", 0.2f);
		    AddRuleToMask("MerchantGuild", "Member", "sabotage", -0.5f);
		    AddRuleToMask("MerchantGuild", "Member", "advertise", 0.5f);
		    AddRuleToMask("MerchantGuild", "Member", "demandToLeaveGuild", -0.3f);
		    AddRuleToMask("MerchantGuild", "Member", "sellCompany", -0.5f);
		    AddRuleToMask("MerchantGuild", "Member", "buyGoods", 0.5f);
		    AddRuleToMask("MerchantGuild", "Member", "sellGoods", 0.7f);

		#endregion addingRulesToMask

//  ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		//PEOPLE

		
		#region AddingPlayer
		    MaskAdds selfPersMask = new MaskAdds("Self", playerName, 0.0f);
	
		    List<MaskAdds>  culture = new List<MaskAdds>();
		    culture.Add(new MaskAdds("Bunsant", "Bungary", 0.6f));
		    culture.Add(new MaskAdds("Member", "MerchantGuild", 0.4f));

		    relationSystem.CreateNewPerson(selfPersMask, culture, new List<MaskAdds>(), 0.4f, 0.6f, 0.5f, new float[] { 0.4f, 0.2f, 0.5f },new float[]{0.0f,0.0f,0.0f});
		#endregion AddingPlayer
		
		#region AddingBill
		    selfPersMask = new MaskAdds("Self", "Bill", 0.0f);
		
		    culture = new List<MaskAdds>();
		    culture.Add(new MaskAdds("Bunce", "Bungary", 0.7f));
		    //culture.Add(new MaskAdds("Follower", "Cult", 0.4f,new List<Person>()));
		    culture.Add(new MaskAdds("Member", "MerchantGuild", 0.4f));
		
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

		#region LINKS
	        relationSystem.AddLinkToPerson("Bill", TypeMask.interPers, "", "RomanticRelationship", 0);
		    relationSystem.AddLinkToPerson("Bill", TypeMask.interPers, "", "Rivalry", 0);
		    relationSystem.AddLinkToPerson("Bill", TypeMask.interPers, "", "Friendship",0);
		    relationSystem.AddRefToLinkInPerson("Bill",TypeMask.interPers,"partner","romanticrelationship","Therese",0.4f);
		    relationSystem.AddRefToLinkInPerson("Bill", TypeMask.interPers, "Enemy", "Rivalry","John",0.6f);
	        relationSystem.AddRefToLinkInPerson("Bill", TypeMask.interPers, "Friend", "Friendship", "Heather", 0.3f);
		    relationSystem.AddRefToLinkInPerson("Bill", TypeMask.interPers, "Enemy", "Rivalry",playerName,0.5f);
		    relationSystem.AddRefToLinkInPerson("Bill",TypeMask.culture,"bunce","Bungary","john",0.3f);
		    relationSystem.AddRefToLinkInPerson("Bill",TypeMask.culture,"bunce","Bungary","therese",0.5f);
		    relationSystem.AddRefToLinkInPerson("Bill",TypeMask.culture,"bunce","Bungary","heather",0.3f);
		    relationSystem.AddRefToLinkInPerson("Bill",TypeMask.culture,"bunce","Bungary",playerName,0.3f);
	
	        relationSystem.AddLinkToPerson("Therese", TypeMask.interPers, "", "RomanticRelationship", 0);
	        relationSystem.AddLinkToPerson("Therese", TypeMask.interPers, "", "Rivalry", 0);
	        relationSystem.AddLinkToPerson("Therese", TypeMask.interPers, "", "Friendship", 0);
		    relationSystem.AddRefToLinkInPerson("Therese", TypeMask.interPers, "Partner", "RomanticRelationship","Bill",0.5f);
		    relationSystem.AddRefToLinkInPerson("Therese", TypeMask.interPers, "Enemy", "Rivalry", "John", 0.2f);
		    relationSystem.AddRefToLinkInPerson("Therese", TypeMask.interPers, "Friend", "Friendship", "Heather", 0.6f);
		    relationSystem.AddRefToLinkInPerson("Therese", TypeMask.interPers, "Enemy", "Rivalry", playerName, 0.3f);
		    relationSystem.AddRefToLinkInPerson("Therese",TypeMask.culture,"buncess","Bungary","john",0.2f);
		    relationSystem.AddRefToLinkInPerson("Therese",TypeMask.culture,"buncess","Bungary","bill",0.6f);
		    relationSystem.AddRefToLinkInPerson("Therese",TypeMask.culture,"buncess","Bungary","heather",0.4f);
		    relationSystem.AddRefToLinkInPerson("Therese",TypeMask.culture,"buncess","Bungary",playerName,0.2f);

	        relationSystem.AddLinkToPerson("John", TypeMask.interPers, "", "Rivalry", 0);
		    relationSystem.AddLinkToPerson("John", TypeMask.interPers, "", "Romanticrelationship", 0);
		    relationSystem.AddLinkToPerson("John", TypeMask.interPers, "", "Friendship", 0);
		    relationSystem.AddRefToLinkInPerson("John", TypeMask.interPers, "Enemy", "Rivalry", "Bill", 0.7f);
		    relationSystem.AddRefToLinkInPerson("John", TypeMask.interPers, "Enemy", "Rivalry", "Therese", 0.4f);
		    relationSystem.AddRefToLinkInPerson("John", TypeMask.interPers, "Partner", "Romanticrelationship", "Heather", 0.8f);
		    relationSystem.AddRefToLinkInPerson("John", TypeMask.interPers, "Friend", "Friendship", playerName, 0.5f);	    
		    relationSystem.AddRefToLinkInPerson("John",TypeMask.culture,"bunsant","Bungary","bill",0.6f);
		    relationSystem.AddRefToLinkInPerson("John",TypeMask.culture,"bunsant","Bungary","therese",0.2f);
		    relationSystem.AddRefToLinkInPerson("John",TypeMask.culture,"bunsant","Bungary","heather",0.4f);
		    relationSystem.AddRefToLinkInPerson("John",TypeMask.culture,"bunsant","Bungary",playerName,0.1f);
	
	        relationSystem.AddLinkToPerson("Heather", TypeMask.interPers, "", "Friendship", 0);
		    relationSystem.AddLinkToPerson("Heather", TypeMask.interPers, "", "RomanticRelationship", 0);
		    relationSystem.AddRefToLinkInPerson("Heather", TypeMask.interPers, "Friend", "Friendship", "Bill", 0f);
		    relationSystem.AddRefToLinkInPerson("Heather", TypeMask.interPers, "Friend", "Friendship", "Therese", 0.7f);
		    relationSystem.AddRefToLinkInPerson("Heather", TypeMask.interPers, "Partner", "RomanticRelationship", "John", 0.5f);
		    relationSystem.AddRefToLinkInPerson("Heather", TypeMask.interPers, "Partner", "RomanticRelationship", playerName, 0.5f);
		    relationSystem.AddRefToLinkInPerson("Heather",TypeMask.culture,"bunsant","Bungary","bill",0.3f);
		    relationSystem.AddRefToLinkInPerson("Heather",TypeMask.culture,"bunsant","Bungary","therese",0.5f);
		    relationSystem.AddRefToLinkInPerson("Heather",TypeMask.culture,"bunsant","Bungary","john",0.6f);
		    relationSystem.AddRefToLinkInPerson("Heather",TypeMask.culture,"bunsant","Bungary",playerName,0.4f);
	
	        relationSystem.AddLinkToPerson(playerName, TypeMask.interPers, "", "Rivalry", 0);
		    relationSystem.AddLinkToPerson(playerName, TypeMask.interPers, "", "Friendship", 0);
		    relationSystem.AddLinkToPerson(playerName, TypeMask.interPers, "", "RomanticRelationship", 0);
		    relationSystem.AddRefToLinkInPerson(playerName, TypeMask.interPers, "Enemy", "Rivalry", "Bill", 0.5f);
		    relationSystem.AddRefToLinkInPerson(playerName, TypeMask.interPers, "Enemy", "Rivalry", "Therese", 0.3f);
		    relationSystem.AddRefToLinkInPerson(playerName, TypeMask.interPers, "Friend", "Friendship", "John", 0.5f);
		    relationSystem.AddRefToLinkInPerson(playerName, TypeMask.interPers, "Partner", "RomanticRelationship", "Heather", 0.6f);
		    relationSystem.AddRefToLinkInPerson(playerName,TypeMask.culture,"bunsant","Bungary","bill",0.5f);
		    relationSystem.AddRefToLinkInPerson(playerName,TypeMask.culture,"bunsant","Bungary","therese",0.4f);
		    relationSystem.AddRefToLinkInPerson(playerName,TypeMask.culture,"bunsant","Bungary","john",0.6f);
		    relationSystem.AddRefToLinkInPerson(playerName,TypeMask.culture,"bunsant","Bungary","heather",0.5f);
		#endregion LINKS 

		#region Opinions
		        //BILL OPINIONS
		GetPerson("bill").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("therese"), 0.6f);
	    GetPerson("bill").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("therese"), 0.3f);
	    GetPerson("bill").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("therese"), -0.2f);
		GetPerson("bill").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("john"), -0.4f);
	    GetPerson("bill").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("john"), -0.6f);
	    GetPerson("bill").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("john"), 0.3f);
	    GetPerson("bill").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("heather"), 0.3f);
	    GetPerson("bill").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("heather"), 0.4f);
	    GetPerson("bill").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("heather"), -0.1f);
	    GetPerson("bill").SetOpinionValue(TraitTypes.NiceNasty, GetPerson(playerName), -0.2f);
	    GetPerson("bill").SetOpinionValue(TraitTypes.HonestFalse, GetPerson(playerName), -0.1f);
	    GetPerson("bill").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson(playerName), 0.1f);
	    GetPerson("therese").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("bill"), 0.7f);
	    GetPerson("therese").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("bill"), 0.5f);
	    GetPerson("therese").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("bill"), 0.3f);
	    GetPerson("therese").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("john"), -0.4f);
	    GetPerson("therese").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("john"), -0.7f);
	    GetPerson("therese").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("john"), -0.3f);
	    GetPerson("therese").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("heather"), 0.5f);
	    GetPerson("therese").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("heather"), -0.1f);
	    GetPerson("therese").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("heather"), 0.2f);
	    GetPerson("therese").SetOpinionValue(TraitTypes.NiceNasty, GetPerson(playerName), -0.3f);
	    GetPerson("therese").SetOpinionValue(TraitTypes.HonestFalse, GetPerson(playerName), 0.1f);
	    GetPerson("therese").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson(playerName), 0.1f);
	    GetPerson("john").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("bill"), -0.5f);
	    GetPerson("john").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("bill"), -0.5f);
	    GetPerson("john").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("bill"), -0.4f);
	    GetPerson("john").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("therese"), -0.3f);
	    GetPerson("john").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("therese"), 0.2f);
	    GetPerson("john").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("therese"), 0.3f);
	    GetPerson("john").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("heather"), 0.6f);
	    GetPerson("john").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("heather"), 0.4f);
	    GetPerson("john").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("heather"), 0.4f);
	    GetPerson("john").SetOpinionValue(TraitTypes.NiceNasty, GetPerson(playerName), 0.5f);
	    GetPerson("john").SetOpinionValue(TraitTypes.HonestFalse, GetPerson(playerName), 0.5f);
	    GetPerson("john").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson(playerName), -0.1f);
	    GetPerson("heather").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("bill"), 0.4f);
	    GetPerson("heather").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("bill"), -0.1f);
	    GetPerson("heather").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("bill"), -0.2f);
	    GetPerson("heather").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("therese"), 0.6f);
	    GetPerson("heather").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("therese"), 0.4f);
	    GetPerson("heather").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("therese"), 0.2f);
	    GetPerson("heather").SetOpinionValue(TraitTypes.NiceNasty, GetPerson("john"), 0.8f);
	    GetPerson("heather").SetOpinionValue(TraitTypes.HonestFalse, GetPerson("john"), -0.4f);
	    GetPerson("heather").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("john"), 0.3f);
	    GetPerson("heather").SetOpinionValue(TraitTypes.NiceNasty, GetPerson(playerName), 0.5f);
	    GetPerson("heather").SetOpinionValue(TraitTypes.HonestFalse, GetPerson(playerName), 0.3f);
	    GetPerson("heather").SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson(playerName), 0.1f);
	    GetPerson(playerName).SetOpinionValue(TraitTypes.NiceNasty, GetPerson("bill"), -0.4f);
	    GetPerson(playerName).SetOpinionValue(TraitTypes.HonestFalse, GetPerson("bill"), -0.2f);
	    GetPerson(playerName).SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("bill"), -0.7f);
	    GetPerson(playerName).SetOpinionValue(TraitTypes.NiceNasty, GetPerson("therese"), 0.3f);
	    GetPerson(playerName).SetOpinionValue(TraitTypes.HonestFalse, GetPerson("therese"), 0.1f);
	    GetPerson(playerName).SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("therese"), 0.2f);
	    GetPerson(playerName).SetOpinionValue(TraitTypes.NiceNasty, GetPerson("john"), 0.4f);
	    GetPerson(playerName).SetOpinionValue(TraitTypes.HonestFalse, GetPerson("john"), -0.1f);
	    GetPerson(playerName).SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("john"), -0.2f);
	    GetPerson(playerName).SetOpinionValue(TraitTypes.NiceNasty, GetPerson("heather"), 0.6f);
	    GetPerson(playerName).SetOpinionValue(TraitTypes.HonestFalse, GetPerson("heather"), 0.4f);
	    GetPerson(playerName).SetOpinionValue(TraitTypes.CharitableGreedy, GetPerson("heather"), 0.5f);
		#endregion Opinions
	}
}
