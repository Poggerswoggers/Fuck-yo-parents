using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStateManager: MonoBehaviour
{
    NpcBaseState currentState;
    [SerializeReference] NpcRoamState roamState = new NpcRoamState();


    private void Start()
    {
        currentState = roamState;

        currentState.EnterState(this);
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

}
