using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="LevelData")]
public class LevelDataScriptable : ScriptableObject
{
    [SerializeField] bool unlocked;
    [SerializeField] float scoreToUnlock;
}
