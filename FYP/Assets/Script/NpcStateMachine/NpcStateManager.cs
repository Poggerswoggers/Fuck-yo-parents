using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStateManager: MonoBehaviour
{
    public bool hasBounce;
    public bool isWalking;
    public string DialogueKnotName;

    public NpcAnimation npcAnim;
    public MCQ question;

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
        currentState = state;
        state.EnterState(this);
        Debug.Log(transform.name + "Switch to" + currentState);
    }

    public void blud(IEnumerator coroutineMethod)
    {
        StartCoroutine(coroutineMethod);
    }

    public NpcBaseState GetCurrentState()
    {
        return currentState;
    }
}
