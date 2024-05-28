using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcInteractingState : NpcBaseState
{
    Vector3 target;
    
    //Reference
    NpcStateManager nSm;
    Transform npcThis;
    Rigidbody2D rb;

    [SerializeField] float interactDur;
    float _interactDur;

    [Header("Bounce Modifier")]
    [SerializeField] float yBounce;
    [SerializeField] float multiplier;
    public override void EnterState(NpcStateManager npcSm)
    {
        nSm = npcSm;
        npcThis = npcSm.transform;
        rb = npcThis.GetComponent<Rigidbody2D>();

        rb.velocity = Vector2.zero;
        nSm.isWalking = false;

        nSm.isBusy = true;
    }

    public override void UpdateState(NpcStateManager npcSm)
    {
        npcSm.npcAnim.npcAnimation();
        npcSm.npcAnim.BounceAnim(multiplier, yBounce, nSm.hasBounce);

    }
    
}
