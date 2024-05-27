using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using UnityEngine.SceneManagement;

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
    public GameObject promptBox;
    [SerializeField] float promptBoxYPos;
    public GameObject infoBox;
    [SerializeField] float infoBoxXPos;
    [SerializeField] float phaseInSpeed;

    bool canContinueToNextLine = false;
    private Coroutine displayLineCoroutine;

    public string dialogueKnotName;
    //Reference
    SnapCamera sC;
    GameStateManager gSm;


    public override void EnterState(GameStateManager gameStateManager)
    {
        gSm = gameStateManager;
        LoadDialooguePanel(gameStateManager.snapState, dialogueKnotName);
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


    //Loads and tweens the dialogue boxes;
    public void LoadDialooguePanel(SnapCamera _sC, string knotName) 
    {
        sC = _sC;
        dialoguePanel.SetActive(true);
        //LeanTween.moveY(promptBox.GetComponent<RectTransform>(), promptBoxYPos, phaseInSpeed).setDelay(0.3f);
        //LeanTween.moveX(infoBox.GetComponent<RectTransform>(), infoBoxXPos, phaseInSpeed);

        //just needs leantween reset

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
            //Debug.Log("Display ran");
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            string infoText = inkStory.Continue();
            infoText = infoText?.Trim();
            Debug.Log(infoText);

            displayLineCoroutine = StartCoroutine(DisplayNextLineEffect(infoText));
        }
        else if(inkStory.currentChoices.Count ==0)
        {
            ExitDialogueMode();
            SceneManager.LoadScene("MCQ Scene", LoadSceneMode.Additive);
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

            button.onClick.AddListener(() => OnPromptClick(choice));
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

    void OnPromptClick(Choice choice)
    {
        if (!canContinueToNextLine) return;
        inkStory.ChooseChoiceIndex(choice.index);
        DisplayNewLine();
        RefreshChoiceView();

        
        //ScoreManager.Instance.UpdateScore();
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
        sC.BackToOutCam();
        dialoguePanel.SetActive(false);
        gSm.ChangeStat(sC);
    }

    public void ExitDialogueMode()
    {
        sC.BackToOutCam();
        dialoguePanel.SetActive(false);
    }
}
