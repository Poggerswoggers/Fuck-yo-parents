using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CrowdControl : BaseMiniGameClass
{    
    //Initial Queue
    [Header("Game Config")]
    [SerializeField] int startNpc;
    int commutersToClear;

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
    [SerializeField] List<RoundConfig> roundsConfig;
    List<NpcScriptable> npcScriptableList = new List<NpcScriptable>();
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

    //Initialise game values
    private void Awake()
    {
        SetDifficulty();
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
        clearText.text = $"<color=#800000ff>0</color>/{commutersToClear}";
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
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlaySFX(AudioManager.instance.cardTap);
            }

            rS.ScreenReader(true);  //Changes screen reader color
            npcQueue.RelocateAllNpc(frontNpc); 
            if (frontNpc != null)
            {
                frontNpc.SelfDestruct(2); //Destroy the npc
                npcCleared++;
                addDelay *= 0.9f;
                clearText.text = $"<color=#800000ff>{npcCleared}</color>/{commutersToClear}";
                frontNpc.MoveInQueue(entrancePos + Vector3.left * -10);  //Moves the npc off screen

                yield return new WaitForSeconds(0.2f);
                for (int i = 0; i < cardPos.childCount; i++) { Destroy(cardPos.GetChild(i).gameObject); } //Destroy the child cards under the carpos 
              
            }
        }
        else
        {
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlaySFX(AudioManager.instance.wrongCardTap);
            }
            rS.ScreenReader(false); 
            _tapDelay = tapDelay;
        }

        if(npcCleared == commutersToClear){ gameManager.OnGameOver(); }
    }

    public override void EndSequenceMethod()
    {
        score = 2000 - (commutersToClear - npcCleared) * 100 - ahMaLeft * 200;
        base.UnloadedAndUpdateScore(score);
    }

    public NpcQueue GetQueue(){
        return npcQueue;
    }

    public void TimePenalty()
    {
        timer.TimePenalty(5);
    }



    protected override void SetDifficulty()
    {
        switch (GetDifficulty())
        {
            case difficulty.One:
                npcScriptableList = roundsConfig[0].npcScriptableList;
                gameTime = roundsConfig[0].gameTime;
                commutersToClear = roundsConfig[0].commutersToClear;
                break;

            case difficulty.Two:
                npcScriptableList = roundsConfig[1].npcScriptableList;
                gameTime = roundsConfig[1].gameTime;
                commutersToClear = roundsConfig[1].commutersToClear;
                break;
        }
    }

    [System.Serializable]
    internal class RoundConfig
    {
        public float gameTime;
        public int commutersToClear;
        public List<NpcScriptable> npcScriptableList;
    }
}
