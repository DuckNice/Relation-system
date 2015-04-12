using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using NRelationSystem;

public class DynamicActionsUI : MonoBehaviour
{
    List<Button> actionsButtons = new List<Button>();
    List<Button> targets = new List<Button>();
    public Program program;
    public GameObject _button;
    public Text outputText;
    public UIFunctions uiFunctions;
    public string chosenAction = "";
    public string chosenPerson = "";
    int marginTop = 10;
    int sideMargin = 20;
    int middleMargin = 20;
    int maxActionsInArray = 10;
    Vector2 widthHeightOfInput = new Vector2(240, 30);
    Vector2 widthHeightOfSubmitCancel = new Vector2(100, 30);

    void Start()
    {
        #region textField, submit & cancel buttons
            outputText = Instantiate(outputText).GetComponent<Text>();
            outputText.transform.SetParent(this.transform);
            outputText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                        widthHeightOfInput.x,
                        widthHeightOfInput.y
                        );
            outputText.transform.position = new Vector3(
                        (widthHeightOfInput.x / 2) + sideMargin, 
                        Screen.height - (widthHeightOfInput.y / 2) - marginTop, 
                        0);


            GameObject submit = Instantiate(_button);
            RectTransform buttonTrans = submit.GetComponent<RectTransform>();
            Button buttonButton = submit.GetComponent<Button>();
            submit.name = "Submit";
            submit.GetComponentInChildren<Text>().text = "Submit";
            submit.transform.SetParent(this.transform);
            submit.transform.position = new Vector3(
                sideMargin + widthHeightOfInput.x + middleMargin + (widthHeightOfSubmitCancel.x/2),
                Screen.height - (widthHeightOfInput.y / 2) - marginTop,
                0
                );
            buttonTrans.sizeDelta = new Vector2(
                widthHeightOfSubmitCancel.x,
                widthHeightOfSubmitCancel.y
                );
            buttonButton.onClick.AddListener(() => { SubmitPressed();});

            GameObject cancel = Instantiate(_button);
            buttonTrans = cancel.GetComponent<RectTransform>();
            buttonButton = cancel.GetComponent<Button>();
            cancel.name = "Cancel";
            cancel.GetComponentInChildren<Text>().text = "Cancel";
            cancel.transform.SetParent(this.transform);
            cancel.transform.position = new Vector3(
                sideMargin + widthHeightOfInput.x + middleMargin + widthHeightOfSubmitCancel.x + middleMargin + (widthHeightOfSubmitCancel.x / 2),
                Screen.height - (widthHeightOfInput.y / 2) - marginTop,
                0
                );
            buttonTrans.sizeDelta = new Vector2(
                widthHeightOfSubmitCancel.x,
                widthHeightOfSubmitCancel.y
                );
            buttonButton.onClick.AddListener(() => { CancelPressed(); });
        #endregion
    }

	public void UpdateButtons()
    {
        for(int q = actionsButtons.Count - 1; q >= 0; q--)
        {
            Destroy(actionsButtons[q].gameObject);
            actionsButtons.RemoveAt(q);
        }

        for (int q = targets.Count - 1; q >= 0; q--)
        {
            Destroy(targets[q].gameObject);
            targets.RemoveAt(q);
        }

        List<string> actions = program.relationSystem.posActions.Keys.ToList();
        List<string> people = program.relationSystem.pplAndMasks.people.Keys.ToList();
        
        int actionArrays = (int) (actions.Count + maxActionsInArray - 1) / maxActionsInArray;
        int peopleArrays = (int) (people.Count + maxActionsInArray - 1) / maxActionsInArray;
        int buttonWidth = (Screen.width - middleMargin - (2 * sideMargin)) / (actionArrays + peopleArrays);
        int buttonHeight = (Screen.height - ((marginTop * 2) + (int)widthHeightOfInput.y)) / maxActionsInArray;

        int i = 0;

        foreach (string action in actions)
        {
            GameObject button = Instantiate(_button);
            RectTransform buttonTrans = button.GetComponent<RectTransform>();
            Button buttonButton = button.GetComponent<Button>();
            button.name = action;
            button.GetComponentInChildren<Text>().text = action;
            button.transform.SetParent(this.transform);
            button.transform.position = new Vector3(
                sideMargin + (buttonWidth / 2) + (buttonWidth * (int)(i / maxActionsInArray)),
                Screen.height - (marginTop * 2 + (int)widthHeightOfInput.y) - (buttonHeight / 2) - (buttonHeight * (i % maxActionsInArray)), 
                0
                );
            buttonTrans.sizeDelta = new Vector2(
                buttonWidth, 
                buttonHeight
                );
            buttonButton.onClick.AddListener(() => { ActionButtonPressed(action);});
            actionsButtons.Add(buttonButton);

            i++;
        }

        i = 0;

        foreach (string person in people)
        {
            GameObject button = Instantiate(_button);
            RectTransform buttonTrans = button.GetComponent<RectTransform>();
            Button buttonButton = button.GetComponent<Button>();
            button.name = person;
            button.GetComponentInChildren<Text>().text = person;

            button.transform.SetParent(this.transform);
            button.transform.position = new Vector3(
                sideMargin + (buttonWidth / 2) + (buttonWidth * actionArrays) + middleMargin + (buttonWidth * (int)(i / maxActionsInArray)),
                Screen.height - (marginTop * 2 + (int)widthHeightOfInput.y) - (buttonHeight / 2) - (buttonHeight * (i % maxActionsInArray)),
                0
                );
            buttonTrans.sizeDelta = new Vector2(
                buttonWidth,
                buttonHeight
                );

            buttonButton.onClick.AddListener(() => { PersonButtonPressed(person);});
            targets.Add(buttonButton);

            i++;
        }
    }


    public void ActionButtonPressed(string actionName)
    {
        chosenAction = actionName;
        WriteText();
    }


    public void PersonButtonPressed(string personName)
    {
        chosenPerson = personName;
        WriteText();
    }


    public void WriteText()
    {
        outputText.text =  chosenAction + " " + chosenPerson;
    }


    public void SubmitPressed()
    {
        program.playerInput(chosenAction + " " + chosenPerson);

        if (uiFunctions.pauseThroughTextEnter)
        {
            program.shouldPlay = true;
            uiFunctions.pauseThroughTextEnter = false;
            uiFunctions.pauseToggle.isOn = false;
        }

        this.gameObject.SetActive(false);

    }


    public void CancelPressed()
    {
        this.gameObject.SetActive(false);

        if (uiFunctions.pauseThroughTextEnter)
        {
            program.shouldPlay = true;
            uiFunctions.pauseThroughTextEnter = false;
            uiFunctions.pauseToggle.isOn = false;
        }

        this.gameObject.SetActive(false);
    }
}
