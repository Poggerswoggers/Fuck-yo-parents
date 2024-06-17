using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdControl : BaseMiniGameClass
{
    [SerializeField] List<NpcScriptable> npcScriptableList;
    
    //Initial Queue
    [SerializeField] int startNpc;
    [SerializeField] int npcCount;

    //Queue Positions and gap
    [SerializeField] Vector2 queueStartPos;
    [SerializeField] float positionSize;
    List<Vector2> waitingQueuePositionList = new List<Vector2>();
    public Vector3 entrancePos 
    {
        get
        {
            return waitingQueuePositionList[0];
        }
    }

    [SerializeField] GameObject npcPrefab;

    //Card
    [SerializeField] Transform cardPos;

    //reference 
    NpcQueue npcQueue;

    public Vector2 GetCardPos()
    {
        return cardPos.position;
    }
    
    // Start is called before the first frame update
    public override void StartGame()
    {
        
        for (int i = 0; i<10; i++)
        {
            waitingQueuePositionList.Add(queueStartPos + Vector2.left * positionSize * i);
        }
        npcQueue = new NpcQueue(waitingQueuePositionList, positionSize, this);

        StartCoroutine(StartGameCo());

        //Add a npc every 5 seconds after 5 seconds
        InvokeRepeating("AddNpc", 5f, 5f);
    }

    //Spawn some initial Npcs
    IEnumerator StartGameCo()
    {
        for (int i = 0; i < startNpc; i++)
        {
            //This instaniates the Npc and adds it to the queue
            AddNpc();
            yield return new WaitForSeconds(0.5f);
        }
        isGameActive = true;
    }

    void AddNpc()
    {
        CrowdNpc npc = Instantiate(npcPrefab).GetComponent<CrowdNpc>();
        npc.thisNpc = npcScriptableList[0];
        npcQueue.AddNpc(npc);

        npcCount++;
    }
    public override void UpdateGame()
    {
        if (Input.GetMouseButtonDown(0) && npcQueue.GetFirstInQueue() !=null && npcQueue.GetFirstInQueue().activeCard !=null)
        {
            CrowdNpc frontNpc = npcQueue.GetFirstInQueue();

            if(frontNpc.activeCard.tappable)
            {
                npcQueue.RelocateAllNpc(frontNpc);
                if (frontNpc != null)
                {
                    frontNpc.SelfDestruct();
                    frontNpc.MoveInQueue(entrancePos + Vector3.left * -10);
                }
            }
        }


        if(Input.GetMouseButtonDown(1) && npcQueue.GetFirstInQueue() != null && npcQueue.GetFirstInQueue().activeCard != null)
        {
            npcQueue.GetFirstInQueue().SwitchActiveCard();
        }
    }
}
