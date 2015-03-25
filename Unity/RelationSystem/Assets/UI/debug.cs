using UnityEngine;
using System.Collections;

public class debug : MonoBehaviour {

	public static debug inst;
	public bool toggle = false;
	public static bool Toggle { get { return inst.toggle; } }

	// Use this for initialization
	void Awake () {
		if(inst == null)
			inst = this;
	}


	public static void Write(string input)
	{
		if(Toggle)
			Debug.Log (input);
	}
}