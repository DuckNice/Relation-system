using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Possession
{
	protected string name;
	public string Name{ get{ return name; } }
	public string objectName;
	public float value;
	public List<object> parameters = new List<object>();
}


public class Money : Possession
{
	//Parameters: moneyValue
	public Money(float amount = 0.0f)
	{
		name = "money";
		value = amount;
		parameters.Add (value);
	}
}


public class Axe : Possession
{
	///<summary>Parameters: 
	///  Weight
	///  Material
	///  Sharpness
	///  Durability</summary>
	public Axe(float weight, string material, float sharpness, float durability)
	{
		name = "Axe";
		parameters.Add ((object)weight);
		parameters.Add ((object)material);
		parameters.Add ((object)sharpness);
		parameters.Add ((object)durability);

	}
}


public class Goods : Possession
{
	public Goods(float amount = 0.0f){
		name = "goods";
		objectName = "a good";
		value = amount;
		parameters.Add (value);
	}
}


public class Company : Possession
{
	public Company(string _name){
		name = "company";
		objectName = _name;
		value = 1;
		parameters.Add (value);
	}
}

public class Game : Possession
{
	public Game(string _name){
		name = "game";
		objectName = _name;
		value = 1;
		parameters.Add (value);
	}

}

