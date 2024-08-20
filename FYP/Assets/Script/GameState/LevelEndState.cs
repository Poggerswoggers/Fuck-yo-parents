using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndState : GameBaseState
{
    [SerializeField] GameObject levelEndPanel;
    [SerializeField] LevelDataScriptable levelData;

    public override void EnterState(GameStateManager gameStateManager)
    {
        levelEndPanel.SetActive(true);
    }

    public override void ExitState(GameStateManager gameStateManager)
    {
        
    }

    public override void UpdateState(GameStateManager gameStateManager)
    {
        
    }
}
