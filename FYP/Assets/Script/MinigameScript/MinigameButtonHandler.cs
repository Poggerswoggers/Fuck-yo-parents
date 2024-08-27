using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameButtonHandler : MonoBehaviour
{
    [SerializeField] string minigameSceneName;

    public void SetMinigameEnum(string value)
    {
        minigameSceneName = value;
        MinigameLevelManager.Instance.LoadAddictiveScene(value, 2); 
    }
}
