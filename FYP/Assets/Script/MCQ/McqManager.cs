using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class McqManager : MonoBehaviour
{
    //UI elements
    public GameObject mcqPanel;
    [SerializeField] TextMeshProUGUI questionText;

    [SerializeField] Button choiceButtonPrefab;
    [SerializeField] GameObject buttonContainer;

    [SerializeField] MCQ questionScriptable;
    int mcqCount;
    void Start()
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
            UIEvent.Score?.Invoke();
        }
        RefreshChoiceView();
    }

    void RefreshChoiceView()
    {
        //Destroy Panel
    }
}
