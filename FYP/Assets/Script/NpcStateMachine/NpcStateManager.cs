using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStateManager: MonoBehaviour
{
    public NpcAnimation npcAnim;
    NpcBaseState currentState;
    [SerializeReference] public NpcRoamState roamState = new NpcRoamState();
    [SerializeReference] public NpcInteractState interactState = new NpcInteractState();


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
        currentState = state;
        state.EnterState(this);
        Debug.Log(currentState);
    }
}
