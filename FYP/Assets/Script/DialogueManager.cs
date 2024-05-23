using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
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

    void Start()
    {
        LoadDialooguePanel();
    }

    //Loads and tweens the dialogue boxes;
    public void LoadDialooguePanel() 
    {
        LeanTween.moveY(promptBox.GetComponent<RectTransform>(), promptBoxYPos, phaseInSpeed).setDelay(0.3f);
        LeanTween.moveX(infoBox.GetComponent<RectTransform>(), infoBoxXPos, phaseInSpeed);

        StartStory();
    }

    void StartStory()
    {
        inkStory = new Story(inkStoryJson.text);
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
            infoText = infoText?.Trim();

            displayLineCoroutine = StartCoroutine(DisplayNextLineEffect(infoText));
        }
        else if(inkStory.currentChoices.Count ==0)
        {
            ExitDialogueMode();
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


        //UIEvent.Score?.Invoke();
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
    void Update()
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


    public void backButton()
    {
        //Camera.main.orthographicSize = 5f;
        //camMode = false;
        //closestGameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
        //blackout.SetActive(false);
    }

    void ExitDialogueMode()
    {
        dialoguePanel.SetActive(false);
    }
}
