using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcInteractState : NpcBaseState
{
    bool isWandering;
    bool isInteracting;

    Transform npcThis;
    public float interactRad;
    [SerializeField] Transform target;

    [SerializeField] LayerMask npcLayer;

    [Header("Bounce Modifier")]
    [SerializeField] float yBounce;
    [SerializeField] float multiplier;

    //Reference
    NpcStateManager nSm;
    Rigidbody2D rb;
    public override void EnterState(NpcStateManager npcSm)
    {
        nSm = npcSm;
        npcThis = npcSm.transform;
        rb = npcThis.GetComponent<Rigidbody2D>();
        FindInteractTarget();
        
    }
    public override void UpdateState(NpcStateManager npcSm)
    {
        if (!isWandering)
        {
            npcSm.npcAnim.npcAnimation();
            npcSm.npcAnim.WalkAnim(multiplier, yBounce, nSm.hasBounce);
        }

        if (!target) return;
        MoveToTarget();
    }
    void FindInteractTarget()
    {
        Collider2D hit = Physics2D.OverlapCircle(npcThis.position - Vector3.left*(Mathf.Sign(npcThis.localScale.x)*4), interactRad, npcLayer);

        if(hit !=null && hit.transform != npcThis)
        {
            target = hit.transform;
        }
        else
        {
            isWandering = true;
            npcThis.GetComponent<NpcAnimation>().WanderAnim(this);           
        }

    }

    void MoveToTarget()
    {
        Vector3 dir = new Vector3(target.position.x-2,target.position.y,0) - npcThis.position;

        if(dir.magnitude > 0.1)
        {
            rb.velocity = dir.normalized * nSm.roamState.speed * Time.deltaTime;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    public void BackToRoam()
    {
        isWandering = false;
        nSm.SwitchState(nSm.roamState);
    }
}
