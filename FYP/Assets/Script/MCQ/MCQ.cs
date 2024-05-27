using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MultipleChoiceQuestion")]
public class MCQ : ScriptableObject
{
    [TextArea(3, 5)]
    public string questionText;

    [TextArea(2, 5)]
    public string[] answerText = new string[4];

    public int CorrectOption;
    [TextArea(3, 5)]
    public string ExplanationText;
}
