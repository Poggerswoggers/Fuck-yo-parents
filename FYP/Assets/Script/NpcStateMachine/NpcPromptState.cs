using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcPromptState : NpcBaseState
{
    public override void EnterState(NpcStateManager npcSm)
    {
        npcThis = npcSm.transform;
        npcThis.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        nSm = npcSm;
        nSm.npcAnim.StopWalkAnim();

        isBusy = true;
    }
    public override void UpdateState(NpcStateManager npcSm)
    {
        
    }
    public override void ExitState(NpcStateManager npcSm)
    {
        isBusy = false;
    }
}
