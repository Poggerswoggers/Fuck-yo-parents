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

    public Transform GetCardPos()
    {
        return cardPos;
    }

    public List<NpcScriptable> ShuffleList(List<NpcScriptable> list) //Fisher-Yates shuffle
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            NpcScriptable temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
        return list;
    }

    // Start is called before the first frame update
    public override void StartGame()
    {
        npcScriptableList = ShuffleList(npcScriptableList); //Set the shuffled list
        
        for (int i = 0; i<10; i++)
        {
            waitingQueuePositionList.Add(queueStartPos + Vector2.left * positionSize * i);  //Create list of vector2 queue positions
        }
        npcQueue = new NpcQueue(waitingQueuePositionList, positionSize, this);  //Create a instance of npcqueue

        StartCoroutine(StartGameCo());

        //Add a npc every 5 seconds after 5 seconds
        InvokeRepeating("AddNpc", 5f, 5f); 
    }

    //Spawn some initial Npcs
    IEnumerator StartGameCo()
    {
        //Initialises some npcs to the int amount
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

        //Get randpm index from the scriptable List
        int index = (int)Random.Range(0, npcScriptableList.Count);
        npc.thisNpc = npcScriptableList[index];  //Assign the npc scriptable to the list's index
        npcQueue.AddNpc(npc);                    //Add the npc to the queue

        npcScriptableList.RemoveAt(index);       //Remove the scriptable object at index
        npcCount++;       
    }
    public override void UpdateGame()
    {
        if (Input.GetMouseButtonDown(0) && npcQueue.GetFirstInQueue() !=null && npcQueue.GetFirstInQueue().activeCard !=null)  //Checks
        {
            CrowdNpc frontNpc = npcQueue.GetFirstInQueue();

            if(frontNpc.activeCard.tappable)
            {
                npcQueue.RelocateAllNpc(frontNpc);
                if (frontNpc != null)
                {
                    frontNpc.SelfDestruct(); //Destroy the npc
                    frontNpc.MoveInQueue(entrancePos + Vector3.left * -10);  //Moves the npc off screen

                    for (int i = 0; i < cardPos.childCount; i++) { Destroy(cardPos.GetChild(i).gameObject); } //Destroy the child cards under the carpos 
                }
            }
        }


        if(Input.GetMouseButtonDown(1) && npcQueue.GetFirstInQueue() != null && npcQueue.GetFirstInQueue().activeCard != null) 
        {
            npcQueue.GetFirstInQueue().SwitchActiveCard(); //Swap card
        }
    }
}
