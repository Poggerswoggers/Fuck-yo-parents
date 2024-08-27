using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="LevelData")]
public class LevelDataScriptable : ScriptableObject
{
    public LevelData[] levelDataArray;

    [Header("MinigameMode")]
    public bool unlocked;
}

[System.Serializable]
public class LevelData
{
    public bool unlocked;
    public int Score;
    public MedalManager medalManager;


}

[System.Serializable]
public struct MedalManager
{
    public int bronzeMedalScore;
    public int silverMedalScore;
    public int goldMedalScore;
}
