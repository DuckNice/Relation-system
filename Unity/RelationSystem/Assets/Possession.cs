using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Possession
{
	protected string name;
	public string Name{ get{ return name; } }
	public float value;
	public List<object> parameters = new List<object>();
}


public class Money : Possession
{
	//Parameters: moneyValue
	public Money()
	{
		name = "money";
		float value = 0.0f;
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