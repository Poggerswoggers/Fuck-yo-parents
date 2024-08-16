using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogueManager : GameBaseState
{
    //Typing Speed
    [SerializeField] float typingSpeed;

    [SerializeField] TextAsset inkStoryJson;
    private Story inkStory;

    //
    [SerializeField] VerticalLayoutGroup optionContainer;
    [SerializeField] Button buttonPrefab;

    [SerializeField] TextMeshProUGUI infoTextField;

    [Header("UI Panel")]
    [SerializeField] GameObject dialoguePanel;

    bool canContinueToNextLine = false;
    private Coroutine displayLineCoroutine;

    //Npc data

    int correctOption;

    public override void EnterState(GameStateManager gameStateManager)
    {
        Debug.Log("started");

        ScoreManager.Instance.DisableUI(false);
        Cursor.visible = true;
        gSm = gameStateManager;
        Initialise();
        LoadDialooguePanel(gSm.NSm.DialogueKnotName);
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
        dialoguePanel.SetActive(false);
        gSm.snapState.BackToOutCam();
    }
    public void Initialise()
    {
        //questionScriptable = gSm.NSm.question;
        //dialogueKnotName = gSm.NSm.DialogueKnotName;
    }

    //Loads and tweens the dialogue boxes;
    void LoadDialooguePanel(string knotName) 
    {
        dialoguePanel.SetActive(true);
        StartStory(knotName);
    }

    void StartStory(string knotName)
    {
        inkStory = new Story(inkStoryJson.text);
        inkStory.ChoosePathString(knotName);
        DisplayNewLine();
    }

    void DisplayNewLine()
    {
        if (inkStory.canContinue)
        {
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            string infoText = inkStory.Continue();
            while (Helper.IsNullOrWhiteSpace(infoText))
            {
                infoText = inkStory.Continue();
            }

            infoText = infoText?.Trim();
            correctOption = (int)inkStory.variablesState["correctAnswer"];

            displayLineCoroutine = StartCoroutine(DisplayNextLineEffect(infoText));
        }
        else if(inkStory.currentChoices.Count ==0)
        {
            if(gSm.NSm.MinigameNpc)
            {
                gSm.ChangeStat(gSm.mcqState);
            }
            else
            {
                (SnapCamera.SnapAction ?? backButton).Invoke();
            }
        }
    }

    IEnumerator DisplayNextLineEffect(string infoText)
    {
        infoTextField.text = "";
        canContinueToNextLine = false;

        char[] textArray = infoText.ToCharArray();

        yield return new WaitForSeconds(0.3f);
        for(int i =0; i < infoText.Length; i++)
        {
            if (textArray[i].Equals('<'))
            {
                infoTextField.text += Helper.GetCompleteRichTextTag(ref i, textArray);
            }
            else
            {
                infoTextField.text += textArray[i];
            }
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

            //Set event triggers
            EventTrigger buttonEvent = button.GetComponent<EventTrigger>();
            buttonEvent.AddListener(EventTriggerType.PointerEnter, EnterHover);
            buttonEvent.AddListener(EventTriggerType.PointerExit, ExitHover);
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
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.buttonClick);
        }

        if (!canContinueToNextLine) return;
        inkStory.ChooseChoiceIndex(choice.index);
        DisplayNewLine();
        RefreshChoiceView();

        if (correctOption == 0) return;
        ScoreManager.Instance.OnScoreChange?.Invoke((correctPrompt)? -200: 200);      
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
        if(AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.buttonClick);
        }
        RefreshChoiceView();
        gSm.ChangeStat(gSm.snapState);
    }

    void EnterHover(PointerEventData eventData)
    {
        float facing = eventData.pointerEnter.transform.lossyScale.x;
        LeanTween.scale(eventData.pointerEnter, new Vector3(Mathf.Sign(facing),1,1)*1.05f, 0.2f);
    }
    void ExitHover(PointerEventData eventData)
    {
        float facing = eventData.pointerEnter.transform.lossyScale.x;
        LeanTween.scale(eventData.pointerEnter, new Vector3(Mathf.Sign(facing), 1,1), 0.2f);
    }
}
