using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEndState : GameBaseState
{
    [SerializeField] int levelIndex;
    [SerializeField] GameObject levelEndPanel;
    [SerializeField] LevelDataScriptable levelData;
    [SerializeField] SaveSystem saveSystem;
    [SerializeField] Image medalImage;
    [SerializeField] bool saveLevel = true;

    [Header("Unlock Medal Scriptable")]
    [SerializeField] MedalsUnlock medalsUnlockScriptable;

    public override void EnterState(GameStateManager gameStateManager)
    {
        levelEndPanel.SetActive(true);
        UpdateLevels();
    }

    public override void ExitState(GameStateManager gameStateManager)
    {
        
    }

    public override void UpdateState(GameStateManager gameStateManager)
    {
        
    }

    void UpdateLevels()
    {
        if (saveLevel)
        {
            levelData.levelDataArray[levelIndex].Score = LevelManager.Instance.GetLevelScore();
            medalImage.sprite = medalsUnlockScriptable.ConvertScoreToMedal(
                                levelData.levelDataArray[levelIndex].medalManager,
                                levelData.levelDataArray[levelIndex].Score);
            medalImage.gameObject.SetActive((medalImage.sprite != null));
        }
        levelData.levelDataArray[levelIndex+1].unlocked = true;
        if(levelIndex == 2) { levelData.unlocked = true; }
        saveSystem.SaveLevelData();
    }
}
