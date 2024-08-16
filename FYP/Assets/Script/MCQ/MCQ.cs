using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "McqScriptable/MultipleChoiceQuestion")]
public class MCQ : ScriptableObject
{
    [TextArea(3, 5)]
    public string questionText;

    [TextArea(2, 5)]
    public string[] answerText = new string[4];

    public List<int> correctOptions;    //Multiple choice
    [TextArea(3, 5)]
    public string ExplanationText;

    public enum questionTypes
    {
        SingleChoice,
        MultiplyChoice
    }
    public questionTypes questionType;
}
