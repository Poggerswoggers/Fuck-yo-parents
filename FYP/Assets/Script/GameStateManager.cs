using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    GameBaseState currentState;

    public SnapCamera snapState;
    public DialogueManager dialogueStat;


    private void Start()
    {
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

    public void ChangeStat(GameBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
}
