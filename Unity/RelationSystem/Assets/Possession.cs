using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Possession
{
	protected string name;
	public string Name{ get{ return name; } }
	public float value;
	public List<object> parameters;


}

public class Money : Possession
{
	//Parameters: moneyValue
	public Money()
	{
		name = "money";
		parameters.Add ((object)0.0);
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