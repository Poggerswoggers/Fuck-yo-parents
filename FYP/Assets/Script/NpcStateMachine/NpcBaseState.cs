using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NpcBaseState
{
    protected Rigidbody2D rb;
    protected NpcStateManager nSm;
    protected Transform npcThis;

    public abstract void EnterState(NpcStateManager npcSm);

    public abstract void UpdateState(NpcStateManager npcSm);

    //public abstract IEnumerator CoroutineState (NpcStateManager npcSm);
}
