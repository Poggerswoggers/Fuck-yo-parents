using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

public class DialogueManager : GameBaseState
{
    //Typing Speed
    [SerializeField] float typingSpeed;

    [SerializeField] TextAsset inkStoryJson;
    private Story inkStory;

    //
    [SerializeField] GridLayoutGroup optionContainer;
    [SerializeField] Button buttonPrefab;

    [SerializeField] TextMeshProUGUI infoTextField;

    [Header("UI Panel")]
    [SerializeField] GameObject dialoguePanel;
    //[SerializeField] RectTransform promptBox;
    //[SerializeField] float promptBoxYPos;
    //[SerializeField] RectTransform infoBox;
    //[SerializeField] float infoBoxXPos;
    [SerializeField] float phaseInSpeed;

    bool canContinueToNextLine = false;
    private Coroutine displayLineCoroutine;

    //Npc data
    string dialogueKnotName;
    MCQ questionScriptable;
    int correctOption;

    public override void EnterState(GameStateManager gameStateManager)
    {
        //infoTextField.text = "";
        gSm = gameStateManager;
        Initialise();
        StartCoroutine(LoadDialooguePanel(dialogueKnotName));
    }

    public override void UpdateState(GameStateManager gameStateManager)
    {
        if (dialoguePanel.activeInHierarchy && inkStory != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (canContinueToNextLine)
                {
                    DisplayNewLine();
                }
                else
                {
                    StopAllCoroutines();
                    infoTextField.text = inkStory.currentText.Trim(); //Auto fills when mouse button 0 is pressed
                }
            }
            if (infoTextField.text == inkStory.currentText.Trim()) //If the dialoguetext == story line text, display choices
            {
                DisplayChoices();
                canContinueToNextLine = true;
            }
        }
    }

    public override void ExitState(GameStateManager gameStateManager)
    {
        
    }
    public void Initialise()
    {
        questionScriptable = gSm.nSm.question;
        dialogueKnotName = gSm.nSm.DialogueKnotName;
    }

    //Loads and tweens the dialogue boxes;
    IEnumerator LoadDialooguePanel(string knotName) 
    {
        yield return new WaitForSeconds(phaseInSpeed);
        dialoguePanel.SetActive(true);
        
        //LeanTween.moveY(promptBox, promptBoxYPos, phaseInSpeed).setDelay(0.3f);
        //LeanTween.moveX(infoBox, infoBoxXPos, phaseInSpeed);

        StartStory(knotName);
    }

    void StartStory(string knotName)
    {
        inkStory = new Story(inkStoryJson.text);
        inkStory.ChoosePathString(knotName);
        //BindExternalFunctions();
        DisplayNewLine();
    }

    void DisplayNewLine()
    {
        if (inkStory.canContinue)
        {
            //Debug.Log("Display ran");
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            string infoText = inkStory.Continue();
            infoText = infoText?.Trim();
            correctOption = (int)inkStory.variablesState["correctAnswer"];

            displayLineCoroutine = StartCoroutine(DisplayNextLineEffect(infoText));
        }
        else if(inkStory.currentChoices.Count ==0)
        {
            if(questionScriptable !=null)
            {
                gSm.ChangeStat(gSm.mcqState);
                ExitDialogueMode();
            }
            else
            {
                backButton();
            }
        }
    }

    IEnumerator DisplayNextLineEffect(string infoText)
    {
        infoTextField.text = "";
        canContinueToNextLine = false;

        yield return new WaitForSeconds(0.3f);
        foreach (char letter in infoText.ToCharArray())
        {
            infoTextField.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void DisplayChoices()
    {
        if(inkStory.currentChoices.Count < 0) return;
        if (optionContainer.GetComponentsInChildren<Button>().Length > 0) return;

        for(int i = 0; i<inkStory.currentChoices.Count; i++)
        {
            var choice = inkStory.currentChoices[i];
            var button = CreateOptionButton(choice.text);

            if (correctOption == 0)
            {
                button.onClick.AddListener(() => OnPromptClick(choice, true));               
            }
            else if(i == correctOption-1)
            {
                button.onClick.AddListener(() => OnPromptClick(choice, true));
            }
            else
            {
                button.onClick.AddListener(() => OnPromptClick(choice, false));
            }
        }
        

    }

    Button CreateOptionButton(string text) //Create button with text
    {
        //Create the choice button
        var choiceButton = Instantiate(buttonPrefab);
        choiceButton.transform.SetParent(optionContainer.transform, false);


        var buttonText = choiceButton.GetComponentInChildren<TextMeshProUGUI>();

        buttonText.text = text;

        return choiceButton;
    }

    void OnPromptClick(Choice choice, bool correctPrompt)
    {
        if (!canContinueToNextLine) return;
        inkStory.ChooseChoiceIndex(choice.index);
        DisplayNewLine();
        RefreshChoiceView();

        if (!correctPrompt)
        {
            ScoreManager.Instance.OnScoreChange?.Invoke(500);
        }
    }

    void RefreshChoiceView()
    {
        if(optionContainer != null)
        {
            foreach(var button in optionContainer.GetComponentsInChildren<Button>())
            {
                Destroy(button.gameObject);
            }
        }
    }


    // Update is called once per frame

    public void backButton()
    {
        RefreshChoiceView();
        ExitDialogueMode();
        gSm.ChangeStat(gSm.snapState);
    }

    void ExitDialogueMode()
    {
        gSm.snapState.BackToOutCam();
        dialoguePanel.SetActive(false);
    }
}
