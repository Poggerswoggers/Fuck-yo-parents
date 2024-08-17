using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionPuller
{
    readonly Dictionary<QuestionTypes, List<MCQ>> questionDictionary = new();

    public QuestionPuller(McqCategories categories)
    {
        int index = 0;
        foreach(QuestionCategory category in categories.questionCategory)
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
        MCQ question = questionSet[randomInt];
        questionDictionary[npcQuestionType].RemoveAt(randomInt);
        return question;
    }
}
[System.Serializable]
public enum QuestionTypes
{
    blind,
    deaf,
    physical,
    elderly,
    invis,
    general,
    intellecual
}
