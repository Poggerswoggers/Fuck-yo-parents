using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NpcBaseState
{
    
    public abstract void EnterState(NpcStateManager npcSm);

    public abstract void UpdateState(NpcStateManager npcSm);
}
