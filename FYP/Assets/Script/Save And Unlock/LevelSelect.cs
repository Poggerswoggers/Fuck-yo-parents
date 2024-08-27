using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] List<Button> levels;
    [SerializeField] LevelDataScriptable levelScriptable;

    [SerializeField] MedalsUnlock medalsUnlockScriptable;

    [Header("Save System Ref")]
    [SerializeField] SaveSystem saveManager;
    [SerializeField] List<Image> medalsList;

    void Awake()
    {
        saveManager.RetrieveLevelData();
        SetUnlockedLevels();

        
    }

    void SetUnlockedLevels()
    {
        for(int i=0; i < levels.Count; i++)
        {
            levels[i].interactable = levelScriptable.levelDataArray[i].unlocked;
            medalsList[i].sprite = medalsUnlockScriptable.ConvertScoreToMedal(
                levelScriptable.levelDataArray[i].medalManager,
                levelScriptable.levelDataArray[i].Score);
            medalsList[i].gameObject.SetActive((medalsList[i].sprite != null));
        }
    }
}
