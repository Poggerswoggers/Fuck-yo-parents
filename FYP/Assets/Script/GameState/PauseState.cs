using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseState : GameBaseState
{
    [SerializeField] GameObject pauseMenu;
    public override void EnterState(GameStateManager gameStateManager)
    {
        Cursor.visible = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0;

        AudioManager.instance?.SetPauseState(true);
    }

    public override void ExitState(GameStateManager gameStateManager)
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        Cursor.visible = false;

        AudioManager.instance?.SetPauseState(false);
    }

    public override void UpdateState(GameStateManager gameStateManager)
    {

    }
}
