using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Diagnostics;

public class UIFunctions : MonoBehaviour {
	public static UIFunctions instance;

    public string questionnaireWatcher = "http://www.google.com";
	public string questionnairePlayer = "http://www.google.com";
	string text;
	public InputField input;
	public Text GameBox;
	public Text PlayerBox;
	public Scrollbar GameScrollbar;
	public Scrollbar PlayerScrollbar;
	public Text StatText;
	public Text RoomText;
	public Scrollbar RoomScrollbar;
    public Toggle pauseToggle;
    public GameObject graphicActionPanel;
    public DynamicActionsUI graphicActionPanelScript;
	public Program program;
    public bool pauseThroughTextEnter = false;
	public bool exitButtonActive;
	public GameObject exitButtonObject;
    public Text playText;
    public GameObject tutorialPanel;
    public GameObject dynamicUITut;
	public Text RecentActionsText;
    public Slider gameSpeedSlider;
    public bool firstTimeOpenActionsMenu = true;
	public GameObject ActionTutPanel;
	bool ShouldShowTutorial;
	public Image playingBack;
	public GameObject actionsButton;

	public ActionText acText;
	public GameObject RetryButton;


	public void Awake()
	{
		instance = this;

	}


	public void Start()
	{
		exitButtonActive = false;
		if (debug.Toggle) {
			StatText.transform.parent.gameObject.SetActive (true);
		} else {
			StatText.transform.parent.gameObject.SetActive (false);
		}

		if(debug.ShouldShowTutorial)
			tutorialPanel.SetActive(true);

		if(debug.PlayerActive){
			actionsButton.SetActive(false);
		}

	}

    
    public void ActivateGraphicActions()
    {
        if (program.shouldPlay)
        {
            program.shouldPlay = false;
            pauseThroughTextEnter = true;
            playText.text = "Paused";
            pauseToggle.isOn = true;
        }

		if(firstTimeOpenActionsMenu && debug.ShouldShowTutorial)
        {
           // tutorialPanel.SetActive(true);
            dynamicUITut.SetActive(true);
			ActionTutPanel.SetActive(true);
            firstTimeOpenActionsMenu = false;
        }

        graphicActionPanel.SetActive(true);
        graphicActionPanelScript.UpdateButtons();


		RecentActionsText.text += instance.GameBox.text;
        if (RecentActionsText.text.Length > 150)
        {
            int lastNewline = RecentActionsText.text.IndexOf("\n");
            RecentActionsText.text = RecentActionsText.text.Remove(0, lastNewline + 1);
        }
    }


    public void enterCommandStart()
    {
        if(program.shouldPlay)
        {
            program.shouldPlay = false;
            pauseThroughTextEnter = true;
            playText.text = "Paused";
            pauseToggle.isOn = true;
        }
    }


    public void ChangeGameSpeed()
    {
        program.timePace = gameSpeedSlider.maxValue - gameSpeedSlider.value;
    }


    public void PressedPlay()
    {
        if (!pauseThroughTextEnter)
        {
            if (program.shouldPlay)
            {
                program.shouldPlay = false;
                playText.text = "Paused";
				//playText.color = Color.red;
				playingBack.color = Color.red;
                pauseToggle.isOn = true;
            }
            else
            {
                program.shouldPlay = true;
                playText.text = "Playing";
				//playText.color = Color.green;
				playingBack.color = Color.green;
                pauseToggle.isOn = false;
            }
        }
    }


	public void enteredCommand()
	{
		text = input.text;

		if (text != "") {
			debug.Write ("Called text command with " + text);
			
			program.playerInput (text);
			
			input.text = "";
		}

        if (pauseThroughTextEnter)
        {
            program.shouldPlay = true;
            pauseThroughTextEnter = false;
            playText.text = "Playing";
            pauseToggle.isOn = false;
        }
	}


	public static void WritePlayer(string input, bool shouldGoRed=true)
	{
		if(debug.Toggle)
		{
			if(instance == null)
			{
				debug.Write ("Error: No instance referenced in UIFunctions");
			}
			else if(instance.PlayerBox == null)
			{
				debug.Write ("Error: No PlayerBox referenced in UIFunctions");
			}
			else if(instance.PlayerScrollbar == null)
			{
				debug.Write ("Error: No PlayerScrollbar referenced in UIFunctions");
			}
		}

        if (instance.PlayerBox.text.Length > 10000)
        {
            int lastNewline = instance.PlayerBox.text.IndexOf("\n", 1000);
            instance.PlayerBox.text = instance.PlayerBox.text.Remove(0, lastNewline + 1);
        }

		instance.PlayerBox.text = input;
		instance.PlayerScrollbar.value = 0;
		if(shouldGoRed){
			instance.PlayerBox.color = Color.red;
			instance.StartCoroutine (ChangeTextCol (instance.PlayerBox));
		}
	}


	public static void WritePlayerLine(string input,bool shouldGoRed=true)
	{
		WritePlayer (input + "\n",shouldGoRed);
	}


	public static void WriteGame(string input)
	{
		if(debug.Toggle)
		{
			if(instance == null)
			{
				debug.Write ("Error: No instance referenced in UIFunctions");
			}
			else if(instance.GameBox == null)
			{
				debug.Write ("Error: No GameBox referenced in UIFunctions");
			}
			else if(instance.GameScrollbar == null)
			{
				debug.Write ("Error: No GameScrollbar referenced in UIFunctions");
			}
		}

        if(instance.GameBox.text.Length > 10000)
        {
            int lastNewline = instance.GameBox.text.IndexOf("\n", 1000);
            instance.GameBox.text = instance.GameBox.text.Remove(0, lastNewline + 1);
            //instance.GameBox.text = instance.GameBox.text.Remove(0, 1000);
        }


	//	if (RecentActionsText.text.Length > 275) {
	//		RecentActionsText.text = RecentActionsText.text.
	//	}
		if(instance.GameBox.text.Length > 0){
			string first = input;
			if(first[0] == 'B'){
				instance.GameBox.text += "<color=#008000ff>"+input+"</color>";

			}
			else if(first[0] == 'T'){
				instance.GameBox.text += "<color=#800080ff>"+input+"</color>";
			}
			else if(first[0] == 'J'){
				instance.GameBox.text += "<color=#0000ffff>"+input+"</color>";
			}
			else if(first[0] == 'H'){
				instance.GameBox.text += "<color=#ffa500ff>"+input+"</color>";
			}
			else if(first[0] == 'Y'){
				instance.GameBox.text += "<color=#000000ff>"+input+"</color>";
			}
			else{
				instance.GameBox.text += "<color=#000000ff>"+input+"</color>";
			}
		}
		else{
			instance.GameBox.text += input;
		}
		instance.GameScrollbar.value = 0;

		if(input != "Welcome. Press play toggle to start\n\n\n")
			instance.acText.NewActionText(input);

	}


	public static void WriteGameLine(string input)
	{
		WriteGame (input + "\n");
	}


	public static void WriteGameStatsInWindow(string input){
		instance.StatText.text = input;
	}


	public static void WriteRoomsInWindow(string input){
		instance.RoomText.text = input;
	}


	public static void ActivateExitButton(){
		instance.exitButtonActive = true;
		instance.exitButtonObject.SetActive(true);
	}


	public void ExitGameAndEnterQuestionnaire(){
		    //ENTER QUESTIONNAIRE
		
		//Process blarg = new Process();
		
		if(debug.PlayerActive){
			Process.Start(instance.questionnaireWatcher);
		}
		else{
			Process.Start(instance.questionnairePlayer);
		}
		Application.Quit();
	}


	static IEnumerator ChangeTextCol(Text s){
		float t = 4f;
		while(t>0){
			t -= Time.deltaTime;
			yield return 0;
		}
		s.color = Color.black;
	}


	public static void ActivateRetryButton(){
		instance.RetryButton.SetActive(true);
	}


	public static void ResetGame(){
		Application.LoadLevel(Application.loadedLevel);
	}
}