using UnityEngine;
using System.Collections;

public class debug : MonoBehaviour {

	public static debug instance;
	public bool toggle = false;
	public static bool Toggle { get{ return instance.toggle; } }

	// Use this for initialization
	void Awake () {
		if(instance == null)
			instance = this;
	}
}