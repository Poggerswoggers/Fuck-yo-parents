using UnityEngine;
using System;


public class MinigameManager
{
    public BaseMiniGameClass currentMiniGame;
    public MinigameManager(BaseMiniGameClass currentMiniGame)
    {
        this.currentMiniGame = currentMiniGame;
        Debug.Log(currentMiniGame);
    }

    public Action EndSequence { get; set; }

    public void OnGameOver()
    {
        EndSequence?.Invoke();
    }
}
