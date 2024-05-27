using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcInteractState : NpcBaseState
{
    [SerializeField] bool isWandering;
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
        target = null;
 
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
        Collider2D hit = Physics2D.OverlapCircle(npcThis.position - Vector3.left*(Mathf.Sign(rb.velocity.x)*4.5f), interactRad, npcLayer);

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
        nSm.isWalking = true;
        Vector3 dir = new Vector3(target.position.x,target.position.y,0) - npcThis.position;
        Debug.Log(dir);

        if(dir.magnitude > 2)
        {
            rb.velocity = dir.normalized * nSm.roamState.speed * Time.deltaTime;
        }
        else
        {
            //NpcStateManager _nSm = target.GetComponent<NpcStateManager>();
            //_nSm.SwitchState(_nSm.interactingState);
            nSm.SwitchState(nSm.interactingState);


        }
    }

    public void BackToRoam()
    {
        Debug.Log("back to roam");
        isWandering = false;
        nSm.SwitchState(nSm.roamState);
    }
}
