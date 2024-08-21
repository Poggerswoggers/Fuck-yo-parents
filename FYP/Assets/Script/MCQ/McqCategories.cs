using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "McqScriptable/McqCategories")]
public class McqCategories : ScriptableObject
{
    [SerializeField] List<QuestionCategory> questionCategory;

    readonly Dictionary<QuestionTypes, List<MCQ>> questionDictionary = new();

    public MCQ PullQuestion(QuestionTypes npcQuestionType)
    {
        Debug.Log("pulling");
        if (!questionDictionary.ContainsKey(npcQuestionType) || questionDictionary[npcQuestionType].Count == 0)
        {
            Debug.Log("no key found or empty list");
            QuestionCategory matchingCategory = questionCategory.Find(category => category.thisQuestionType == npcQuestionType);
            questionDictionary[npcQuestionType] = new List<MCQ>(matchingCategory.questionList);
        }
        Debug.Log("contains key");
        List<MCQ> questionSet = questionDictionary[npcQuestionType];

        int randomInt = Random.Range(0, questionSet.Count);
        MCQ question = questionSet[randomInt];
        questionDictionary[npcQuestionType].RemoveAt(randomInt);
        return question;
    }
}

[System.Serializable]
public class QuestionCategory
{
    public QuestionTypes thisQuestionType;
    public List<MCQ> questionList;
}
