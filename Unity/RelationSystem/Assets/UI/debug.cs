using UnityEngine;
using System.Collections;

public class debug : MonoBehaviour {

	public static debug inst;
	public bool toggle = false;
	public static bool Toggle { get { return inst.toggle; } }
	[SerializeField]
	bool playerActive = false;
	public static bool PlayerActive { get { return inst.playerActive; } }
	public bool shouldShowTutorial = false;
	public static bool ShouldShowTutorial { get { return inst.shouldShowTutorial; } }

	[SerializeField]
	playerWatcherText pwt;

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


	public static void SetPlayerActiveness(bool b){
		inst.playerActive = b;
		
		if(b){
			UIFunctions.instance.actionsButton.SetActive(false);
		}
		inst.pwt.SetTextForPlayer(b);
	}



}