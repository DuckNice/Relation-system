using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class playerWatcherText : MonoBehaviour {

	Text text;
	string st;

	// Use this for initialization
	void Start () {

		text = this.GetComponent<Text>();

		if(debug.PlayerActive){
			text.text = "TUTORIAL\n\nUse \"Playing\" toggle in the bottom to start and pause.\n\n" +
				"When playing, the characters will be doing actions in the window to the right.\n\n" +
					"This is the non-interactive version, so your interaction is limited to controlling when the story is developing, and how fast (with the speed slider).\n" +
					"Sit back and read the procedural story.";
		}
		else{
			text.text = "TUTORIAL\n\nUse \"Playing\" toggle in the bottom to start and pause.\n\n" +
				"When playing, the characters will be doing actions in the window to the right. (You can control the speed with the speed slider)\n\n" +
					"This is the interactive version. At any point, you can enter an action in the command line, or select an action to do with the button below. \n" +
					"For most actions you need to select a person to do the action against, as well.";
		}


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
