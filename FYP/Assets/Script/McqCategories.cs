using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "McqScriptable/McqCategories")]
public class McqCategories : ScriptableObject
{
    public QuestionTypes thisQuestionType;
    public List<MCQ> questionList;
}
