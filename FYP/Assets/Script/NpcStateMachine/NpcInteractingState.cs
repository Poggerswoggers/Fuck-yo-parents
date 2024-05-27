using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcInteractingState : NpcBaseState
{

    //Reference
    NpcStateManager nSm;
    Transform npcThis;
    Rigidbody2D rb;
    public override void EnterState(NpcStateManager npcSm)
    {
        nSm = npcSm;
        npcThis = npcSm.transform;
        rb = npcThis.GetComponent<Rigidbody2D>();

        rb.velocity = Vector2.zero;
        nSm.isWalking = false;
    }

    public override void UpdateState(NpcStateManager npcSm)
    {
        
    }
    
}
