using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameBaseState : MonoBehaviour
{
    protected GameStateManager gSm;

    public abstract void EnterState(GameStateManager gameStateManager);
    public abstract void ExitState(GameStateManager gameStateManager);
    public abstract void UpdateState(GameStateManager gameStateManager);
}
