using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcInteractState : NpcBaseState
{
    Transform npcThis;
    public float interactRad;
    [SerializeField] Transform target;

    [SerializeField] LayerMask ignoreLayer;

    [Header("Bounce")]
    [SerializeField] float yBounce;
    [SerializeField] float multiplier;

    //Reference
    NpcStateManager nSm;
    public override void EnterState(NpcStateManager npcSm)
    {
        nSm = npcSm;
        npcThis = npcSm.transform;
        FindInteractTarget();
        
    }
    public override void UpdateState(NpcStateManager npcSm)
    {
        npcSm.npcAnim.npcAnimation();
        npcSm.npcAnim.BounceAnim(multiplier, yBounce);
    }

    void FindInteractTarget()
    {
        Collider2D hit = Physics2D.OverlapCircle(npcThis.position - Vector3.left*(Mathf.Sign(npcThis.localScale.x)*4), interactRad, ignoreLayer);
        
        if(hit !=null && hit.transform != npcThis)
        {
            target = hit.transform;
        }
        else
        {
            //star
        }

    } 
}
