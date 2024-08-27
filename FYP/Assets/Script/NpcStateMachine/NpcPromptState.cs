using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcPromptState : NpcBaseState
{
    public override void EnterState(NpcStateManager npcSm)
    {
        npcThis = npcSm.transform;
        npcThis.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        npcThis.GetComponent<Collider2D>().enabled = false;
        nSm = npcSm;
        nSm.npcAnim.StopWalkAnim();
    }
    public override void UpdateState(NpcStateManager npcSm)
    {
        
    }
    public override void ExitState(NpcStateManager npcSm)
    {
        npcThis.GetComponent<Collider2D>().enabled = true;
        npcSm.hasBeenTalkTo = true;
    }

    public override void OnCollisionEnter2D(NpcStateManager npcSm, Collision2D collision)
    {
        
    }
}
