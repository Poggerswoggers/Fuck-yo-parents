using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "McqScriptable/McqCategories")]
public class McqCategories : ScriptableObject
{
    [SerializeField] List<QuestionCategory> questionCategory;
    public List<QuestionCategory> QuestionCategoryRef => questionCategory;
}

[System.Serializable]
public class QuestionCategory
{
    public QuestionTypes thisQuestionType;
    public List<MCQ> questionList;
}
