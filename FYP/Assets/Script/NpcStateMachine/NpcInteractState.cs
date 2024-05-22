using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcInteractState : NpcBaseState
{
    Transform npcThis;
    public float interactRad;
    [SerializeField] Transform target;
   
    public override void EnterState(NpcStateManager npcSm)
    {
        npcThis = npcSm.transform;
        FindInteractTarget();
        
    }
    public override void UpdateState(NpcStateManager npcSm)
    {
        
    }

    void FindInteractTarget()
    {
        RaycastHit2D hit = Physics2D.CircleCast(npcThis.position, interactRad, Vector2.zero);

        if (hit == false) return;
        Debug.Log(hit);
        target = hit.transform;
    }
}
