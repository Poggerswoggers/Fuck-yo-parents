using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="LevelData")]
public class LevelDataScriptable : ScriptableObject
{
    public LevelData[] levelDataArray;
}

[System.Serializable]
public class LevelData
{
    public bool unlocked;
    public float Score;
}
