using UnityEngine;

public class MinigameNpcs : MonoBehaviour
{
    [SerializeField] string minigameSceneIndex;
    [Range(1,2)]
    [SerializeField] int levelScale;

    public void GetMinigameValue()
    {        
        ScoreManager.Instance.loadAddictiveScene(minigameSceneIndex, levelScale);
    }

    [SerializeField] QuestionTypes thisQuestionType;

    public QuestionTypes GetQuestionType()
    {
        return thisQuestionType;
    }
}
