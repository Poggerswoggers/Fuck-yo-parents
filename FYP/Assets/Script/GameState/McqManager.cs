using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class McqManager : GameBaseState
{
    //UI elements
    [SerializeField] GameObject mcqPanel;
    [SerializeField] Button NextButton;

    [Header("Ui elements")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] TextMeshProUGUI answerStatementText;
    [SerializeField] Image spriteHolder;

    [Header("Choice Stuff")]
    [SerializeField] Button choiceButtonPrefab;
    [SerializeField] GameObject buttonContainer;
    [SerializeField] Color selectedColor;
    [SerializeField] Color defaultColor;

    private List<int> playerChoice = new();
    readonly List<AnswerChoice> answerChoiceList = new List<AnswerChoice>();

    [Header("MCQ Categories")]
    [SerializeField] List<McqCategories> questionCategories;

    MCQ questionScriptable;
    QuestionPuller qp;

    private void Awake()
    {
        qp = new QuestionPuller(questionCategories);
    }

    public override void EnterState(GameStateManager gameStateManager)
    {
        gSm = gameStateManager;

        QuestionTypes storeType = gSm.NSm.MinigameNpc.GetQuestionType();
        questionScriptable = qp.PullQuestion(storeType);
        spriteHolder.sprite = gSm.NSm.GetComponentInChildren<SpriteRenderer>().sprite;

        //Sets the question, sprite and get question count   
        questionText.text = questionScriptable.questionText;
        answerStatementText.gameObject.SetActive(false);

        DisplayChoices();
        mcqPanel.SetActive(true);
    }

    void DisplayChoices()
    {
        int mcqCount = questionScriptable.answerText.Length;
        if (mcqCount < 0) return;
        if (buttonContainer.GetComponentsInChildren<Button>().Length > 0) return;

        for (int i =0; i<mcqCount; i++)
        {
            var choice = questionScriptable.answerText[i];
            var button = CreateButtonOption(choice);   //Creates the button

            bool isCorrect = false;
            isCorrect = (questionScriptable.correctOptions.Contains(i + 1));    //Determine if choice is correct based on scriptable object
            AnswerChoice _buttonOption = new(button, isCorrect, i + 1);    //Create new answerchoice which is map to each button    
            button.onClick.AddListener(() => onPromptClick(_buttonOption));         

            //Add answerChoice instance to the list
            answerChoiceList.Add(_buttonOption);
        }
        NextButton.onClick.AddListener(() => LockChoices());
    }
    Button CreateButtonOption(string text)
    {
        var choiceButton = Instantiate(choiceButtonPrefab);
        choiceButton.transform.SetParent(buttonContainer.transform, false);
        var buttonText = choiceButton.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = text?.Trim();
        //((char)('A' + index)) + ": "
        return choiceButton;
    }

    void onPromptClick(AnswerChoice answerChoice)
    {
        //Switch case for the different question types
        switch (questionScriptable.questionType)
        {
            case MCQ.questionTypes.SingleChoice:
                foreach(AnswerChoice aC in answerChoiceList) {
                    if(aC != answerChoice) { SetButton(false, aC); }
                }
                SetButton(true, answerChoice);
                break;
            case MCQ.questionTypes.MultiplyChoice:
                answerChoice.selected = !answerChoice.selected;
                SetButton(answerChoice.selected, answerChoice);
                break;
        }
    }

    void SetButton(bool selected, AnswerChoice answerChoice)
    {
        if (selected)
        {
            answerChoice.thisButton.image.color = selectedColor;
            playerChoice.Add(answerChoice.index);
        }
        else
        {
            answerChoice.thisButton.image.color = defaultColor;
            playerChoice.Remove(answerChoice.index);
        }
    }

    public void LockChoices()
    {
        playerChoice.Sort();             
        if (questionScriptable.correctOptions.SequenceEqual(playerChoice))
        {
            answerStatementText.text = "CORRECT";
            ScoreManager.Instance.OnScoreChange?.Invoke(-200);
        }
        else
        {
            answerStatementText.text = "INCORRECT";
            ScoreManager.Instance.OnScoreChange?.Invoke(300);
        }
        DestroyAnswers();
        NextButton.onClick.RemoveAllListeners();
        NextButton.onClick.AddListener(() => GoToMiniGame());
    }

    public void GoToMiniGame()
    {
        NextButton.onClick.RemoveAllListeners();
        mcqPanel.SetActive(false);
        gSm.NSm.GetComponent<MinigameNpcs>().GetMinigameValue();
        gSm.ChangeStat(gSm.minigameState);
    }
    void DestroyAnswers()
    {
        if (buttonContainer != null)
        {
            foreach (var button in buttonContainer.GetComponentsInChildren<Button>())
            {
                Destroy(button.gameObject);
            }
        }
        StartCoroutine(DisplayNextLineEffect(questionScriptable.ExplanationText));
        answerStatementText.gameObject.SetActive(true);
    }

    IEnumerator DisplayNextLineEffect(string infoText)
    {
        questionText.text = "";
        char[] textArray = infoText.ToCharArray();

        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < infoText.Length; i++)
        {
            if (textArray[i].Equals('<'))
            {
                questionText.text += Helper.GetCompleteRichTextTag(ref i, textArray);
            }
            else
            {
                questionText.text += textArray[i];
            }
            yield return new WaitForSeconds(0.025f);
        }
    }



    public override void UpdateState(GameStateManager gameStateManager)
    {
        
    }

    public override void ExitState(GameStateManager gameStateManager)
    {
        //Clear answerList and player'sChoice for new ones
        answerChoiceList.Clear();
        playerChoice.Clear();
    }
}
internal class AnswerChoice
{
    public readonly Button thisButton;
    public readonly bool isCorrect;
    public readonly int index;
    public bool selected;
    public AnswerChoice(Button refButton, bool isCorrect, int index)
    {
        thisButton = refButton;
        this.isCorrect = isCorrect;
        this.index = index;
    }
}
