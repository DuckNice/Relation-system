using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class playerWatcherText : MonoBehaviour {

	public Text text;
	string st;

	// Use this for initialization
	void Awake () {

	//	text = this.GetComponent<Text>();
	//	print(text);
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void SetTextForPlayer(){
		if(debug.PlayerActive){
			text.text = "TUTORIAL\n\nUse the \"Play\" toggle in the bottom to start and pause.\n\n" +
				"When playing, the characters will be doing actions in the middle window.\n\n" +
					"This is the non-interactive version, so your interaction is limited to controlling when the story is developing, and how fast (with the speed slider).\n" +
					"Sit back and read the procedural story.";
		}
		else{
			text.text = "TUTORIAL\n\nUse the \"Play\" toggle in the bottom to start and pause.\n\n" +
				"When playing, the characters will be doing actions in the middle window. (You can control the speed with the speed slider)\n\n" +
					"This is the interactive version. At any point, you can select an action to do with the \"Select actions\" button.\n" +
					"For most actions you need to select a person to do the action against, as well.";
		}
	}

}
