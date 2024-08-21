using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndState : GameBaseState
{
    [SerializeField] int levelIndex;
    [SerializeField] GameObject levelEndPanel;
    [SerializeField] LevelDataScriptable levelData;
    [SerializeField] SaveSystem saveSystem;

    [SerializeField] bool saveLevel = true;

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
            levelData.levelDataArray[levelIndex].Score = ScoreManager.Instance.levelScore;
        }
        levelData.levelDataArray[levelIndex+1].unlocked = true;
        saveSystem.SaveLevelData();
    }
}
