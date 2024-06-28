using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

    [Header("Answer Stuff")]
    [SerializeField] TextMeshProUGUI answerTextPrefab;
    [SerializeField] GameObject answerContainer;

    MCQ questionScriptable;
    int mcqCount;
    

    public override void EnterState(GameStateManager gameStateManager)
    {
        gSm = gameStateManager;
        NextButton.SetActive(false);
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
            var button = CreateButtonOption(choice, i);

            if(i == questionScriptable.CorrectOption-1)
            {
                button.onClick.AddListener(() => onPromptClick(true, button));
            }
            else
            {
                button.onClick.AddListener(() => onPromptClick(false, button));
            }
        }

    }

    Button CreateButtonOption(string text, int index)
    {
        var choiceButton = Instantiate(choiceButtonPrefab);
        choiceButton.transform.SetParent(buttonContainer.transform, false);
        var buttonText = choiceButton.GetComponentInChildren<TextMeshProUGUI>();

        var answerText = Instantiate(answerTextPrefab);
        answerText.transform.SetParent(answerContainer.transform, false);
        var _answerText = answerText.GetComponentInChildren<TextMeshProUGUI>();

        switch (index)
        {
            case 0: 
                buttonText.text = "A";
                _answerText.text = "A: " + text;
                break;
            case 1:
                buttonText.text = "B";
                _answerText.text = "B: " + text;
                break;
            case 2:
                buttonText.text = "C";
                _answerText.text = "C: " + text;
                break;
            case 3:
                buttonText.text = "D";
                _answerText.text = "D: " + text;
                break;

        }
        return choiceButton;
    }

    void onPromptClick(bool correctAns, Button button)
    {
        if(correctAns)
        {
            questionText.text = questionScriptable.ExplanationText;
            NextButton.SetActive(true);
            DestroyQuestion();
        }
        else
        {
            ScoreManager.Instance.OnScoreChange?.Invoke(500);
            Destroy(button.gameObject);
        }
    }

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
            foreach (var text in answerContainer.GetComponentsInChildren<TextMeshProUGUI>())
            {
                Destroy(text.gameObject);
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
