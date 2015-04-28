using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class fadeouttext : MonoBehaviour {

	Text thisText;


	// Use this for initialization
	void Start () {
		thisText = GetComponent<Text> ();
	}
	

	public void ChangeAlphaText(float f){
		if (thisText != null) {
			thisText.color = new Color (thisText.color.r, thisText.color.g, thisText.color.b, f);
		}
	}
}
