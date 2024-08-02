using UnityEngine;

public class MinigameState : GameBaseState
{
    [SerializeField] GameObject normalNpcParent;
    public override void EnterState(GameStateManager gameStateManager)
    {
        normalNpcParent.SetActive(false);
    }

    public override void ExitState(GameStateManager gameStateManager)
    {
        normalNpcParent.SetActive(true);
    }

    public override void UpdateState(GameStateManager gameStateManager)
    {
        
    }
}
