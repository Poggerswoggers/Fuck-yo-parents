using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="LevelData")]
public class LevelDataScriptable : ScriptableObject
{
    public  int levelIndex;

    public bool unlocked;
    public float scoreToUnlock;
}
