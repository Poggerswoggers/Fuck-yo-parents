using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelData/Medals store")]
public class MedalsUnlock : ScriptableObject
{
    [SerializeField] Sprite bronzeMedal;
    [SerializeField] Sprite silverMedal;
    [SerializeField] Sprite goldMedal;
}
