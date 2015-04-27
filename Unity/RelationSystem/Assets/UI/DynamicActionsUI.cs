using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    public Text playText;
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

        List<string> actions = new List<string>();

        foreach (MAction action in program.relationSystem.posActions.Values)
        {
            string actionName = AddSpacesToSentence(action.name, false);

            actions.Add(actionName.ToLower());
        }

        List<string> people = program.relationSystem.CreateActiveListsListNames();

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


            buttonButton.onClick.AddListener(() => { ActionButtonPressed(button.name);});
            actionsButtons.Add(buttonButton);
            i++;
        }

        i = 0;

        foreach (string person in people)
        {
            GameObject button = Instantiate(_button);
            RectTransform buttonTrans = button.GetComponent<RectTransform>();
            Button buttonButton = button.GetComponent<Button>();

            if (person != RelationSystem.playerName)
                button.name = person;
            else
                button.name = "";

            button.GetComponentInChildren<Text>().text = button.name;


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


            buttonButton.onClick.AddListener(() => { PersonButtonPressed(button.name); });
            targets.Add(buttonButton);

            i++;
        }
    }


    string AddSpacesToSentence(string text, bool preserveAcronyms)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;
        StringBuilder newText = new StringBuilder(text.Length * 2);
        newText.Append(text[0]);
        for (int i = 1; i < text.Length; i++)
        {
            if (char.IsUpper(text[i]))
                if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                    (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                     i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                    newText.Append(' ');
            newText.Append(text[i]);
        }
        return newText.ToString();
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
        chosenAction = chosenAction.Replace(" ", "");

        if (chosenPerson != "")
            program.playerInput(chosenAction + " " + chosenPerson);
        else
            program.playerInput(chosenAction);

        if (uiFunctions.pauseThroughTextEnter)
        {
            program.shouldPlay = true;
            uiFunctions.pauseThroughTextEnter = false;
            playText.text = "Playing";
            uiFunctions.pauseToggle.isOn = false;
        }

        chosenAction = "";
        chosenPerson = "";
        outputText.text = "";

        this.gameObject.SetActive(false);
    }


    public void CancelPressed()
    {
        this.gameObject.SetActive(false);

        if (uiFunctions.pauseThroughTextEnter)
        {
            program.shouldPlay = true;
            uiFunctions.pauseThroughTextEnter = false;
            playText.text = "Playing";
            uiFunctions.pauseToggle.isOn = false;
        }

        chosenAction = "";
        chosenPerson = "";
        outputText.text = "";

        this.gameObject.SetActive(false);
    }
}
