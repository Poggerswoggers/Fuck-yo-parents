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
    [SerializeField] TextMeshProUGUI nameField;

    [Header("UI Panel")]
    public GameObject dialoguePanel;
    public RectTransform promptBox;
    [SerializeField] float promptBoxYPos;
    public RectTransform infoBox;
    [SerializeField] float infoBoxXPos;
    [SerializeField] float phaseInSpeed;

    bool canContinueToNextLine = false;
    private Coroutine displayLineCoroutine;

    //Npc data
    string dialogueKnotName;
    MCQ questionScriptable;
    public int correctOption;

    //Reference
    SnapCamera sC;
    GameStateManager gSm;


    public override void EnterState(GameStateManager gameStateManager)
    {
        //infoTextField.text = "";
        gSm = gameStateManager;
        StartCoroutine(LoadDialooguePanel(gameStateManager.snapState, dialogueKnotName));
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
    public void Initialise(MCQ _questionScriptable, string _knotName)
    {
        questionScriptable = _questionScriptable;
        dialogueKnotName = _knotName;
    }

    //Loads and tweens the dialogue boxes;
    IEnumerator LoadDialooguePanel(SnapCamera _sC, string knotName) 
    {
        sC = _sC;

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
                LoadQuestions();
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

        Debug.Log(correctPrompt);
        if (!correctPrompt)
        {
            ScoreManager.OnScoreChange?.Invoke(500);
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
        gSm.ChangeStat(sC);
    }

    void ExitDialogueMode()
    {
        sC.BackToOutCam();
        dialoguePanel.SetActive(false);
    }

    void BindExternalFunctions()
    {
        //inkStory.BindExternalFunction("LoadQuestion", (string questionName) => LoadQuestions(questionName));
    }
    void LoadQuestions()
    {
        gSm.mcqState.questionScriptable = questionScriptable;
        gSm.ChangeStat(gSm.mcqState);
    }
}
