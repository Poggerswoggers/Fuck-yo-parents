using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class McqManager : GameBaseState
{
    //UI elements
    [SerializeField] GameObject mcqPanel;
    [SerializeField] GameObject NextButton;

    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] Image spriteHolder;

    [Header("Choice Stuff")]
    [SerializeField] Button choiceButtonPrefab;
    [SerializeField] GameObject buttonContainer;
    [SerializeField] Color selectedColor;

    List<int> playerChoice;

    MCQ questionScriptable;
    int mcqCount;
    

    public override void EnterState(GameStateManager gameStateManager)
    {
        gSm = gameStateManager;
        questionScriptable = gSm.nSm.question;  //Get the mcq scriptable object

        mcqCount = questionScriptable.answerText.Length;
        questionText.text = questionScriptable.questionText;
        spriteHolder.GetComponent<Image>().sprite = questionScriptable.characterSprite;


        DisplayChoices();
        mcqPanel.SetActive(true);
    }

    void DisplayChoices()
    {
        if (mcqCount < 0) return;
        if (buttonContainer.GetComponentsInChildren<Button>().Length > 0) return;

        for (int i =0; i<mcqCount; i++)
        {
            var choice = questionScriptable.answerText[i];
            var button = CreateButtonOption(choice);

            bool isCorrect = false;
            AnswerChoice _buttonOption;
            switch (questionScriptable.questionType)
            {
                case MCQ.questionTypes.SingleChoice:
                    isCorrect = (i == questionScriptable.CorrectOption - 1);
                    _buttonOption = new AnswerChoice(button, isCorrect, i);
                    button.onClick.AddListener(() => onPromptClick(_buttonOption));
                    break;

                case MCQ.questionTypes.MultiplyChoice:
                    isCorrect = (questionScriptable.correctOptions.Contains(i+1));
                    _buttonOption = new AnswerChoice(button, isCorrect, i);
                    Debug.Log(isCorrect);
                    button.onClick.AddListener(() => onPromptClick(_buttonOption));
                    break;
            }
        }
    }
    Button CreateButtonOption(string text)
    {
        var choiceButton = Instantiate(choiceButtonPrefab);
        choiceButton.transform.SetParent(buttonContainer.transform, false);
        var buttonText = choiceButton.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = text;
        //((char)('A' + index)) + ": "
        return choiceButton;
    }

    void onPromptClick(AnswerChoice answerChoice)
    {
        switch (questionScriptable.questionType)
        {
            case MCQ.questionTypes.SingleChoice:
                break;
            case MCQ.questionTypes.MultiplyChoice:
                answerChoice.selected = !answerChoice.selected;
                if (answerChoice.selected)
                {
                    answerChoice.thisButton.image.color = selectedColor;
                    playerChoice.Add(answerChoice.index);
                }
                else
                {
                    answerChoice.thisButton.image.color = Color.white;
                    playerChoice.Remove(answerChoice.index);
                }
                break;
        }
    }

    public void LockChoices()
    {
        if (questionScriptable.correctOptions.OrderBy(x => x).SequenceEqual(playerChoice.OrderBy(x => x)))
        {
            Debug.Log("correct");
        }
    }
    //Maybe set a new button

    public void GoToMiniGame()
    {
        mcqPanel.SetActive(false);
        int index = gSm.nSm.GetComponent<MinigameNpcs>().GetGameIndex();
        ScoreManager.Instance.loadAddictiveScene(index);
    }
    void DestroyQuestion()
    {
        if (buttonContainer != null)
        {
            foreach (var button in buttonContainer.GetComponentsInChildren<Button>())
            {
                Destroy(button.gameObject);
            }
        }
    }
    public override void UpdateState(GameStateManager gameStateManager)
    {
        
    }

    public override void ExitState(GameStateManager gameStateManager)
    {
        
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
