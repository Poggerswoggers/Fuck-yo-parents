using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Path Navigator correct paths")]
public class SequenceScriptable : ScriptableObject
{
    public List<CorrectSequence> correctSequences;
    public Sprite pathRunnerSprite;
}
[System.Serializable]
public struct CorrectSequence
{
    public List<Vector2Int> sequence;
    public int gridSize;
}
