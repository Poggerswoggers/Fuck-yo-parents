using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndState : GameBaseState
{
    [SerializeField] int levelIndex;
    [SerializeField] GameObject levelEndPanel;
    [SerializeField] LevelDataScriptable levelData;
    [SerializeField] SaveSystem saveSystem;

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
        levelData.levelDataArray[levelIndex].Score = ScoreManager.Instance.levelScore;
        levelData.levelDataArray[levelIndex + 1].unlocked = true;
        saveSystem.SaveLevelData();
    }
}
