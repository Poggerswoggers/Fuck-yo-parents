using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcInteractingState : NpcBaseState
{
    Vector3 target;

    public float interactDur { get; set; }
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

        Initialise();

        nSm.npcAnim.Chatting(interactDur);
        npcThis.GetComponent<Collider2D>().enabled = false;
        
    }
    void Initialise()
    {
        npcThis.gameObject.layer = LayerMask.NameToLayer("BusyNpc"); //Change npc layer so it cant be tagged by other NPCs

        _interactDur = interactDur;
    }
    public override void UpdateState(NpcStateManager npcSm)
    {
        npcSm.npcAnim.npcAnimation();
        npcSm.npcAnim.BounceAnim(multiplier, yBounce, nSm.hasBounce, 0.2f);


        //When Interact duration ends return to roam
        _interactDur -= Time.deltaTime;
        if(_interactDur < 0)
        {
            nSm.SwitchState(nSm.roamState);
        }
    }

    public override void ExitState(NpcStateManager npcSm)
    {
        npcThis.GetComponent<Collider2D>().enabled = true;
    }

    public override void OnCollisionEnter2D(NpcStateManager npcSm, Collision2D collision)
    {
        
    }

}
