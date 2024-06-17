using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcQueue
{
    private List<CrowdNpc> npcList;
    private List<Vector2> positionList;

    //Reference
    CrowdControl cc;
    public NpcQueue(List<Vector2> positionList, float gap, CrowdControl cc) //Constructor
    {
        this.cc = cc;
        this.positionList = positionList;


        npcList = new List<CrowdNpc>();
    }

    //Add npc to list and move it into nearest queue
    public void AddNpc(CrowdNpc npc)
    {
        npcList.Add(npc);
        npc.MoveInQueue(positionList[npcList.IndexOf(npc)]);  //Move to nearest queue position
        npc.Initialise(cc);  //Initialise the npc, pass value

        //npc.queueIndex = npcList.IndexOf(npc);
    }

    public CrowdNpc GetFirstInQueue()
    {
        if(npcList.Count == 0)
        {
            return null;
        }
        else
        {
            CrowdNpc npc = npcList[0]; 
            return npc;
        }
    }

    public void RelocateAllNpc(CrowdNpc npc)
    {
        //Remove the old npc
        npcList.Remove(npc);

        //Relocate
        for(int i =0; i< npcList.Count; i++)
        {
            npcList[i].MoveInQueue(positionList[i]);
            //npcList[i].queueIndex = i;
        }
    }
}
