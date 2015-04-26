using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActionText : MonoBehaviour {

	public Text[] texts; 
	int c;
	public Vector3 startPos;
	Vector3[] positions = new Vector3[5];
	int pc;

	float timeSinceLastUpdate = 0;

	// Use this for initialization
	void Start () {
		c = 0;
		startPos = transform.position;
		float it = 0;
		for(int i=0;i<positions.Length;i++){
			Vector3 temp = startPos;
			temp.y += it;
			positions[i] = temp;
			it += 25f;
			//Debug.Log (positions[i]);
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		timeSinceLastUpdate += Time.deltaTime;
	}


	public void NewActionText(string input){
		if (timeSinceLastUpdate > 1f) {
			pc = 0;
		}
		if (timeSinceLastUpdate < 0.1f) {
			pc++;
			if(pc == positions.Length) pc = 0;
		}
		texts [c].rectTransform.position = positions[pc];
		//Debug.Log (pc);

		//Debug.Log (""+texts [c].rectTransform.position);
		foreach (Text t in texts) {
			if(Vector3.Distance(texts[c].rectTransform.position,t.rectTransform.position) < 0.5f){
				//Debug.Log("TRUE");
				texts[c].rectTransform.position = new Vector3(startPos.x,texts[c].rectTransform.position.y+50f,startPos.z);
			}
		}
		//Debug.Log(""+texts[c].rectTransform.position);
		texts[c].color = new Color (0, 0, 0, 1);
		texts[c].text = input;
		StartCoroutine (FadeText (texts[c]));
		c++;
		if (c == texts.Length) {
			c=0;
		}
		timeSinceLastUpdate = 0;
		//instance.ActionText.CrossFadeAlpha (0.1f,0.4f,false);
	}

	IEnumerator FadeText(Text t){
		float a = 1f;
		float y = 0;
		while (t.color.a > 0.0f) {
			t.color = new Color(t.color.r,t.color.g,t.color.b,a);
			t.transform.position = new Vector3(startPos.x,t.transform.position.y+y,startPos.z);
			y += Time.deltaTime*0.1f;
			a -= Time.deltaTime*0.2f;
			yield return 0;
		}
	}



}
