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

    public enum QuestionTypes
    {
        Deaf,
        Blind,
        Physical
    }
    public QuestionTypes thisQuestionType;

}
