using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStateManager: MonoBehaviour
{
    [SerializeField] bool _hasBounce;
    public bool HasBounce => _hasBounce;
    public bool isWalking { get; set; }
    public bool hasBeenTalkTo {  get; set; } = false;

    public NpcAnimation npcAnim;

    NpcBaseState currentState;
    [SerializeReference] public NpcRoamState roamState = new NpcRoamState();
    [SerializeReference] public NpcInteractState interactState = new NpcInteractState();
    [SerializeReference] public NpcPromptState promptState = new NpcPromptState();
    [SerializeReference] public NpcInteractingState interactingState = new NpcInteractingState();

    private void Start()
    {
        currentState = roamState;
        npcAnim = npcAnim.GetComponent<NpcAnimation>();

        currentState.EnterState(this);
    }

    private void FixedUpdate()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(NpcBaseState state)
    {
        currentState.ExitState(this);    

        currentState = state;
        state.EnterState(this);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentState.OnCollisionEnter2D(this, collision);
    }

    public NpcBaseState GetCurrentState()
    {
        return currentState;
    }

    public MinigameNpcs MinigameNpc => GetComponent<MinigameNpcs>();

}
