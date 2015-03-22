using UnityEngine;
using System.Collections;

public class UIFunctions : MonoBehaviour {

	string text;
	public UnityEngine.UI.InputField input;


	public void enteredCommand()
	{
		text = input.text;

		if(debug.Toogle)
			print ("Called text command with " + text);



		input.text = "";
	}
}