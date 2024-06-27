using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CrowdControl : BaseMiniGameClass
{    
    //Initial Queue
    [SerializeField] int startNpc;
    public int npcCount { get; set; }        //Current npc count

    //
    int npcCleared;         //Hope to move to a score manager
    [SerializeField] TextMeshProUGUI clearText;
    //

    //AddNpcTImer
    [SerializeField] float addDelay;

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

    //Card
    [SerializeField] Transform cardPos;

    [Header("Tap Delay")]
    [SerializeField] float tapDelay;
    [SerializeField] float _tapDelay;

    [Header("References")]
    [SerializeField] ReaderScreen rS;
    NpcQueue npcQueue;

    [Header("Scriptable List")]
    [SerializeField] List<NpcScriptable> npcScriptableList;
    //[SerializeField] NpcScriptable ahMaScriptable;

    [Header("Prefabs")]
    [SerializeField] GameObject npcPrefab;
    //[SerializeField] GameObject ahmaPrefab;

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

    protected override IEnumerator InstructionCo()
    {
        StartGame();
        yield return null;
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
        isGameActive = true;
        //Add a npc every 5 seconds after 5 seconds
        InvokeRepeating("AddNpc", 5f, addDelay); 
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
        npcScriptableList.RemoveAt(index);
        
        npcCount++;
        addDelay *= 0.9f;
    }

    //I think this is pretty messy
    public override void UpdateGame()
    {
        if (Input.GetMouseButtonDown(1) && npcQueue.GetFirstInQueue() != null && npcQueue.GetFirstInQueue().activeCard != null)
        {
            npcQueue.GetFirstInQueue().SwitchActiveCard(); //Swap card
        }

        _tapDelay -= Time.deltaTime;

        if (_tapDelay > 0) return; //For when u tap a invalid card u have a time penalty 
        if (Input.GetMouseButtonDown(0) && npcQueue.GetFirstInQueue() !=null && npcQueue.GetFirstInQueue().activeCard !=null)  //Checks
        {
            CrowdNpc frontNpc = npcQueue.GetFirstInQueue();
            frontNpc.TapCard();

            StartCoroutine(RelocateAndReQueue(frontNpc));
        }      
    }

    IEnumerator RelocateAndReQueue(CrowdNpc frontNpc)
    {
        yield return new WaitForSeconds(0.2f); //Delay
    
        if (frontNpc.activeCard.tappable)
        {
            rS.ScreenReader(true);  //Changes screen reader color
            npcQueue.RelocateAllNpc(frontNpc); 
            if (frontNpc != null)
            {
                frontNpc.SelfDestruct(2); //Destroy the npc
                npcCount--;
                npcCleared++;
                clearText.text = npcCleared + "/15";
                frontNpc.MoveInQueue(entrancePos + Vector3.left * -10);  //Moves the npc off screen

                yield return new WaitForSeconds(0.2f);
                for (int i = 0; i < cardPos.childCount; i++) { Destroy(cardPos.GetChild(i).gameObject); } //Destroy the child cards under the carpos 
            }
        }
        else
        {
            rS.ScreenReader(false); 
            _tapDelay = tapDelay;
        }
    }

    public override void EndSequenceMethod()
    {
        //throw new System.NotImplementedException();
    }
}
