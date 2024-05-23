using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcPromptState : NpcBaseState
{
    NpcStateManager nSm;
    Transform npcthis;
    public override void EnterState(NpcStateManager npcSm)
    {
        nSm = npcSm;
        nSm.npcAnim.StopWalkAnim();
    }
    public override void UpdateState(NpcStateManager npcSm)
    {

    }

}
