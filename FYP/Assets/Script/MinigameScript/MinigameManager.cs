using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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

    }
}
