using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Diagnostics;

public class UIFunctions : MonoBehaviour {
	public static UIFunctions instance;

    public string questionaireTest = "http://www.google.com";
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
	}

    
    public void ActivateGraphicActions()
    {
        if (program.shouldPlay)
        {
            program.shouldPlay = false;
            pauseThroughTextEnter = true;
            pauseToggle.isOn = true;
        }

        graphicActionPanel.SetActive(true);
        graphicActionPanelScript.UpdateButtons();
    }


    public void enterCommandStart()
    {
        if(program.shouldPlay)
        {
            program.shouldPlay = false;
            pauseThroughTextEnter = true;
            pauseToggle.isOn = true;
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
            pauseToggle.isOn = false;
        }
	}


	public static void WritePlayer(string input)
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
            instance.PlayerBox.text = instance.PlayerBox.text.Remove(0, 1000);
        }

		instance.PlayerBox.text += input;
		instance.PlayerScrollbar.value = 0;
	}


	public static void WritePlayerLine(string input)
	{
		WritePlayer (input + "\n");
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
            instance.GameBox.text = instance.GameBox.text.Remove(0, 1000);
        }

		instance.GameBox.text += input;
		instance.GameScrollbar.value = 0;
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
        Process.Start(questionaireTest);
		Application.Quit();
	}
}