using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] List<Button> levels;
    [SerializeField] LevelDataScriptable levelScriptable;
    static bool onFirstLaunch = false;

    [Header("Save System Ref")]
    [SerializeField] SaveSystem saveManager;

    void Awake()
    {
        if (!onFirstLaunch){
            onFirstLaunch = true;
            saveManager.RetrieveLevelData();
        }
        SetUnlockedLevels();
    }

    void SetUnlockedLevels()
    {
        for(int i=0; i < levels.Count; i++)
        {
            levels[i].interactable = levelScriptable.levelDataArray[i].unlocked;
        }
    }
}