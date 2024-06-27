using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameNpcs : MonoBehaviour
{
    [SerializeField] int  minigameSceneIndex;

    public int GetGameIndex()
    {
        return minigameSceneIndex;
    }
}
