using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelData/Medals store")]
public class MedalsUnlock : ScriptableObject
{
    [SerializeField] Sprite bronzeMedal;
    [SerializeField] Sprite silverMedal;
    [SerializeField] Sprite goldMedal;

    public Sprite ConvertScoreToMedal(MedalManager medalManager, int levelScore)
    {
        if (levelScore >= medalManager.goldMedalScore) { return goldMedal; }
        else if (levelScore >= medalManager.silverMedalScore) { return silverMedal; }
        else if (levelScore>=medalManager.bronzeMedalScore) { return bronzeMedal; }
        else
        {
            return null;
        }
    }
}
