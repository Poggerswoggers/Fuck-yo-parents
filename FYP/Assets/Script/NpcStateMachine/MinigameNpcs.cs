using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameNpcs : MonoBehaviour
{
    [SerializeField] string minigameSceneIndex;
    [Range(1,2)]
    [SerializeField] int levelScale;
    public void GetMinigameValue()
    {        
        ScoreManager.Instance.loadAddictiveScene(minigameSceneIndex, levelScale);
    }
}
