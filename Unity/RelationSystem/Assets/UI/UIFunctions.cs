﻿using UnityEngine;
using System.Collections;

public class UIFunctions : MonoBehaviour {
	public static UIFunctions instance;

	string text;
	public UnityEngine.UI.InputField input;
	public UnityEngine.UI.Text GameBox;
	public UnityEngine.UI.Text PlayerBox;
	public UnityEngine.UI.Scrollbar GameScrollbar;
	public UnityEngine.UI.Scrollbar PlayerScrollbar;
	public Program program;


	public void Awake()
	{
		instance = this;
	}


	public void enteredCommand()
	{
		text = input.text;

		debug.Write ("Called text command with " + text);
	
		program.playerInput (text);

		input.text = "";
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

		instance.GameBox.text += input;
		instance.GameScrollbar.value = 0;
	}


	public static void WriteGameLine(string input)
	{
		WriteGame (input + "\n");
	}
}