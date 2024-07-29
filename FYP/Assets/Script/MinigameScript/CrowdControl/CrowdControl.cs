using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CrowdControl : BaseMiniGameClass
{    
    //Initial Queue
    [SerializeField] int startNpc;

    int npcCleared;         //Hope to move to a score manager
    [SerializeField] TextMeshProUGUI clearText;
    //

    //AddNpcTImer
    [SerializeField] float addDelay;
    float _addDelay;

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
    [SerializeField] NpcScriptable ahmaScriptable;

    int ahmaIndex = 4;
    public int ahMaLeft { get; set; }

    [Header("Prefabs")]
    [SerializeField] GameObject npcPrefab;

    [Header("Slider Timer")]
    [SerializeField] SliderTimer timer;
    [SerializeField] float gameTime;
    public Transform GetCardPos()
    {
        return cardPos;
    }

    protected override IEnumerator InstructionCo()
    {
        StartGame();
        yield return null;
    }

    //Initialise game values
    private void Awake()
    {
        _addDelay = addDelay;
        npcScriptableList = Helper.Shuffle(npcScriptableList); //Set the shuffled list

        for (int i = 0; i < 20; i++)
        {
            waitingQueuePositionList.Add(queueStartPos + Vector2.left * positionSize * i);  //Create list of vector2 queue positions
        }
        npcQueue = new NpcQueue(waitingQueuePositionList, this);  //Create a instance of npcqueue
    }

    //Start to control when the instruction ends
    public override void StartGame()
    {      
        StartCoroutine(StartGameCo());
        timer.SetTImer(gameTime, () => gameManager.OnGameOver());

        isGameActive = true;
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
        if (npcScriptableList.Count == 0) return;
        CrowdNpc npc = Instantiate(npcPrefab, new Vector2(-15, waitingQueuePositionList[0].y), Quaternion.identity).GetComponent<CrowdNpc>();
        npc.transform.SetParent(this.transform);

        if(npcCleared >= ahmaIndex)
        {
            ahmaIndex += 5;
            npc.thisNpc = ahmaScriptable;
            npc.WaitTime();
        }
        else
        {
            npc.thisNpc = npcScriptableList[0];  //Assign the npc scriptable to the list's index
            npcScriptableList.RemoveAt(0);
        }
        npcQueue.AddNpc(npc);
    }

    //I think this is pretty messy
    public override void UpdateGame()
    {
        CardControl();

        _addDelay -= Time.deltaTime;
        if (_addDelay < 0)
        {
            AddNpc();
            _addDelay = addDelay;
        }
    }
    void CardControl()
    {
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollDelta) > 0.05f && npcQueue.GetFirstInQueue() != null && npcQueue.GetFirstInQueue().activeCard != null)
        {
            npcQueue.GetFirstInQueue().SwitchActiveCard(Mathf.Sign(scrollDelta)); //Swap card
        }

        _tapDelay -= Time.deltaTime;

        if (_tapDelay > 0) return; //For when u tap a invalid card u have a time penalty 
        if (Input.GetMouseButtonDown(0) && npcQueue.GetFirstInQueue() != null && npcQueue.GetFirstInQueue().activeCard != null)  //Checks
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
                npcCleared++;
                addDelay *= 0.9f;
                clearText.text = $"<color=#800000ff>{npcCleared}</color>/15";
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

        if(npcCleared == 15){ gameManager.OnGameOver(); }
    }

    public override void EndSequenceMethod()
    {
        score = 2000 - (15 - npcCleared) * 100 - ahMaLeft * 200;
        base.UnloadedAndUpdateScore(score);
    }

    public NpcQueue GetQueue(){
        return npcQueue;
    }
}
