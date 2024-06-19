using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class MinigameManager
{
    public BaseMiniGameClass currentMiniGame;

    public Action Instruction;
    public MinigameManager(BaseMiniGameClass currentMiniGame)
    {
        this.currentMiniGame = currentMiniGame;

        Debug.Log(currentMiniGame);
    }

    public void OnGameOver()
    {
        currentMiniGame.isGameOver = true;
        currentMiniGame.EndSequence?.Invoke();
    }
}
