using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcInteractState : NpcBaseState
{
    [SerializeField] bool isWandering;
    [SerializeField] bool chaseTarget;
    [SerializeField] float interactRad;
    Transform target;
    Vector3 dir;
    [SerializeField] float chaseDur;
    float _chaseDur;

    [SerializeField] LayerMask npcLayer;

    [Header("Bounce Modifier")]
    [SerializeField] float yBounce;
    [SerializeField] float multiplier;

    public override void EnterState(NpcStateManager npcSm)
    {
        nSm = npcSm;
        npcThis = npcSm.transform;
        
        rb = npcThis.GetComponent<Rigidbody2D>();
        Initialise();
        FindInteractTarget();  
    }
    void Initialise()
    {
        target = null;
        _chaseDur = chaseDur;
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

    public override void ExitState(NpcStateManager npcSm)
    {
        
    }
    void FindInteractTarget()
    {
        Collider2D hit = Physics2D.OverlapCircle(npcThis.position - Vector3.left*(Mathf.Sign(rb.velocity.x)*4.5f), interactRad, npcLayer);

        if(hit !=null && hit.transform != npcThis)
        {
            NpcStateManager targetnSm = hit.GetComponent<NpcStateManager>();
            if(!targetnSm.GetCurrentState().isBusy && !targetnSm.interactState.chaseTarget)
            {
                target = hit.transform;
            }
            else
            {
                nSm.SwitchState(nSm.roamState);
            }
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
        chaseTarget = true;
        dir = target.position - npcThis.position;

        if(dir.magnitude > 2.5f)
        {
            rb.velocity = dir.normalized * nSm.roamState.speed * Time.deltaTime;
        }
        else
        {
            NpcStateManager _nSm = target.GetComponent<NpcStateManager>();

            if(_nSm.GetCurrentState() == _nSm.interactingState)
            {
                target = null;
                _nSm.SwitchState(_nSm.roamState);
            }
            else
            {
                chaseTarget = false;
                _nSm.npcAnim.faceTarget(npcThis);
                nSm.npcAnim.faceTarget(target);
                
                nSm.interactingState.interactDur = Random.Range(5, 10);
                _nSm.interactingState.interactDur = nSm.interactingState.interactDur;

                _nSm.SwitchState(_nSm.interactingState);
                nSm.SwitchState(nSm.interactingState);
            }
        }

        if(_chaseDur > 0)
        {
            _chaseDur -= Time.deltaTime;
        }
        else
        {
            nSm.SwitchState(nSm.roamState);
        }
    }

    public void BackToRoam(NpcBaseState state)
    {
        isWandering = false;
        nSm.SwitchState(state);
    }
}
