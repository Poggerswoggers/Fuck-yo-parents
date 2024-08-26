using UnityEngine;

public class MinigameNpcs : MonoBehaviour, IQuestionable
{
    [SerializeField] string minigameSceneIndex;
    [Range(1,2)]
    [SerializeField] int levelScale;

    public void GetMinigameValue()
    {        
        LevelManager.Instance.LoadAddictiveScene(minigameSceneIndex, levelScale);
    }

    public QuestionTypes GetQuestionType() => thisQuestionType;
    [SerializeField] QuestionTypes thisQuestionType;
}
