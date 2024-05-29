using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class McqManager : GameBaseState
{
    //UI elements
    public GameObject mcqPanel;
    [SerializeField] TextMeshProUGUI questionText;

    [SerializeField] Button choiceButtonPrefab;
    [SerializeField] GameObject buttonContainer;

    public MCQ questionScriptable;
    int mcqCount;
    public override void EnterState(GameStateManager gameStateManager)
    {
        mcqCount = questionScriptable.answerText.Length;
        questionText.text = questionScriptable.questionText;

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

            if(i == questionScriptable.CorrectOption-1)
            {
                button.onClick.AddListener(() => onPromptClick(true));
            }
            else
            {
                button.onClick.AddListener(() => onPromptClick(false));
            }
        }

    }

    Button CreateButtonOption(string text)
    {
        var choiceButton = Instantiate(choiceButtonPrefab);
        choiceButton.transform.SetParent(buttonContainer.transform, false);

        var buttonText = choiceButton.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = text;

        return choiceButton;
    }

    void onPromptClick(bool correctAns)
    {
        Debug.Log(gameObject + " " + correctAns);
        if(correctAns)
        {
            ScoreManager.OnScoreChange?.Invoke(1);
        }
        RefreshChoiceView();
    }

    void RefreshChoiceView()
    {
        //Destroy Panel
    }

    public override void UpdateState(GameStateManager gameStateManager)
    {
        
    }

    public override void ExitState(GameStateManager gameStateManager)
    {
        
    }
}
