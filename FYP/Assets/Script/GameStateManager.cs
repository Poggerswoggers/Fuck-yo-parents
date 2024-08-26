using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{    
    public NpcStateManager NSm { get; set; }
    GameBaseState currentState;

    [Header("Game State")]
    public SnapCamera snapState;
    public DialogueManager dialogueStat;
    public McqManager mcqState;
    public MinigameState minigameState;
    public PauseState pauseState;
    public LevelEndState endState;
    private void Start()
    {
        LeanTween.reset();
        currentState = snapState;
        currentState.EnterState(this);
    }

    private void Update()
    {
        if(currentState !=null)
        {
            currentState.UpdateState(this);
        }
    }

    public void ChangeState(GameBaseState state)
    {
        currentState.ExitState(this);
        currentState.gameObject.SetActive(false);
        currentState = state;
        currentState.gameObject.SetActive(true);
        state.EnterState(this);
    }

    public void ClearNpc()
    {
        Destroy(NSm.gameObject);
    }

    public GameBaseState GetCurrentGameState()
    {
        return currentState;
    }
}
