using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionPuller
{
    Dictionary<QuestionTypes, List<MCQ>> questionDictionary = new();

    public QuestionPuller(List<McqCategories> categories)
    {
        int index = 0;
        foreach(McqCategories category in categories)
        {
            questionDictionary[category.thisQuestionType] = category.questionList;
            index++;
        }
    }

    public MCQ PullQuestion(QuestionTypes npcQuestionType)
    {
        if (!questionDictionary.ContainsKey(npcQuestionType)) return null;

        List<MCQ> questionSet = questionDictionary[npcQuestionType];
        int randomInt = Random.Range(0, questionSet.Count);

        questionDictionary[npcQuestionType].RemoveAt(randomInt);
        return questionSet[randomInt];
    }
}
[System.Serializable]
public enum QuestionTypes
{
    blind,
    deaf,
}
