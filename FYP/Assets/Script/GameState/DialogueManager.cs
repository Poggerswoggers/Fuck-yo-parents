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

    public GameObject pictureFrame;
    public GameObject picture;
    public Sprite[] pictures;

    [Header("UI Panel")]
    [SerializeField] GameObject dialoguePanel;

    bool canContinueToNextLine = false;
    private Coroutine displayLineCoroutine;

    //Npc data

    int correctOption;

    public override void EnterState(GameStateManager gameStateManager)
    {
        Cursor.visible = true;
        gSm = gameStateManager;
        LoadDialooguePanel(gSm.NSm.name);
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
        pictureFrame.SetActive(false);
        dialoguePanel.SetActive(false);
        gSm.snapState.BackToOutCam();
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
        //pictureFrame.SetActive(false);
        if (inkStory.canContinue)
        {
            if (displayLineCoroutine != null)
            StopCoroutine(displayLineCoroutine);
            
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
                gSm.mcqState.SetQuestionVariables(gSm.NSm.GetComponent<IQuestionable>().GetQuestionType(), gSm.NSm.GetComponentInChildren<SpriteRenderer>().sprite);  
                gSm.ChangeState(gSm.mcqState);
            }
            else
            {
                (SnapCamera.SnapAction ?? BackButton).Invoke();
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
        LevelManager.Instance.OnScoreChange?.Invoke((correctPrompt) ? -200 : 200);
        //hi jason i added this
        if (correctPrompt)
        {
            Debug.Log("I edited");
            ChoosingNPCPicture(gSm.NSm.name);
        }
        //
    }

    //hi jason i added this
    void ChoosingNPCPicture(string knotName)
    {
        Debug.Log(knotName);
        foreach(var img in pictures)
        {
            if (img.name == knotName)
            {
                Debug.Log("true");
                pictureFrame.SetActive(true);
                picture.GetComponent<Image>().sprite = img;
                break;
            }
        }
    }
    //
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

    public void BackButton()
    {
        if(AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.buttonClick);
        }
        RefreshChoiceView();
        gSm.ChangeState(gSm.snapState);
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
