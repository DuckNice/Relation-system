using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIFunctions : MonoBehaviour {
	public static UIFunctions instance;

	string text;
	public InputField input;
	public Text GameBox;
	public Text PlayerBox;
	public Scrollbar GameScrollbar;
	public Scrollbar PlayerScrollbar;
	public Text StatText;
    public Toggle pauseToggle;
    public GameObject graphicActionPanel;
    public DynamicActionsUI graphicActionPanelScript;
	public Program program;
    private bool pauseThroughTextEnter = false;


	public void Awake()
	{
		instance = this;
	}
	public void Start()
	{
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
}