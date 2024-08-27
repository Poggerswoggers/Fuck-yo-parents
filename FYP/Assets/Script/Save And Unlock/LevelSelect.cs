using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] List<Button> levelsButton;
    [SerializeField] LevelDataScriptable levelScriptable;

    [SerializeField] MedalsUnlock medalsUnlockScriptable;

    [SerializeField] Button minigameButton;

    [Header("Save System Ref")]
    [SerializeField] SaveSystem saveManager;
    [SerializeField] List<Image> medalsList;

    [SerializeField] bool isLevelSelect;

    void Awake()
    {
        saveManager.RetrieveLevelData();

        if (isLevelSelect)
        {
            SetUnlockedLevels();
        }
        else
        {
            SetUnlockMinigame();
        }

        
    }

    void SetUnlockedLevels()
    {
        for(int i=0; i < levelsButton.Count; i++)
        {
            levelsButton[i].interactable = levelScriptable.levelDataArray[i].unlocked;
            medalsList[i].sprite = medalsUnlockScriptable.ConvertScoreToMedal(
                levelScriptable.levelDataArray[i].medalManager,
                levelScriptable.levelDataArray[i].Score);
            medalsList[i].gameObject.SetActive((medalsList[i].sprite != null));
        }
    }

    void SetUnlockMinigame()
    {
        minigameButton.interactable = levelScriptable.unlocked;
    }
}
