using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TakeASeatGm : BaseMiniGameClass
{
    public static Action<int> vulnerableMatch;

    [SerializeField] Transform grabbedCommuter;
    Vector3 offset;

    [Header("Seats")]
    [SerializeField] List<Seat> seats;
    [SerializeField] float sittingDur;


    [Header("Object Pool")]
    [SerializeField] Transform commuterParent;
    public List<GameObject> pooledCommunters { get; set; }
    [SerializeField] GameObject objectToPool;
    [SerializeField] List<RoundsConfig> levels;
    RoundsConfig thisLevel;

    [Header("Start/End Pos")]
    [SerializeField] Transform startPos;
    [SerializeField] Transform endPos;

    [Header("Config")]
    [SerializeField] float timeBetweenCommuters;
    float _timeBetweenCommuters;
    int vulnerable = 3;
    int count = 0;

    [Header("Minigame cam")]
    [SerializeField] Camera minigameCam;

    [Header("Audio")]
    [SerializeField] AudioClip levelMusic;

    //Reference
    CommunterQueue communterQueue;

    public override void EndSequenceMethod()
    {
        score = 2000 - 500*(2 * vulnerable - count);
        UnloadedAndUpdateScore(score);
    }

    public override void StartGame()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayMusic(levelMusic);
        }
        SetDifficulty();
        PoolObject();
        LeanTween.reset();
        _timeBetweenCommuters = timeBetweenCommuters;

        communterQueue = new CommunterQueue(startPos.position, endPos.position, this);
        communterQueue.Speed = thisLevel.commuterSpeed;
    }

    public override void UpdateGame()
    {
        SpawnCommuter();
        PlayerInput();
    }
    void PlayerInput()
    {
        Vector3 mousePos = minigameCam.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (!hit.transform) return;
            if (hit.transform.TryGetComponent(out Commuter com))
            {
                com.isMoving(false);
                grabbedCommuter = hit.transform;
                LeanTween.cancel(grabbedCommuter.gameObject);
                offset = grabbedCommuter.position - mousePos;
            }
        }

        if (Input.GetMouseButtonUp(0) && grabbedCommuter)
        {
            if (!OccupySeat())
            {
                communterQueue.MoveCommuter(grabbedCommuter);
            }
            grabbedCommuter = null;
        }

        if (grabbedCommuter)
        {
            mousePos += offset;
            grabbedCommuter.position = mousePos;
        }
    }

    bool OccupySeat()
    {
        foreach(Seat seat in seats)
        {
            if (seat.occupied) continue;
            if(Vector2.Distance(grabbedCommuter.position, seat.transform.position) < 1.5f)
            {
                seat.SitCommuter(grabbedCommuter);
                if (grabbedCommuter.GetComponent<Commuter>().isVulnerable)
                {
                    vulnerableMatch?.Invoke(count+=1); 
                }
                return true;
            }
        }
        return false;
    }

    void SpawnCommuter()
    {
        _timeBetweenCommuters -= Time.deltaTime;
        if (_timeBetweenCommuters < 0)
        {
            _timeBetweenCommuters = timeBetweenCommuters;
            if (StillHaveCommuters()){
                communterQueue.InitialiseCommuter(pooledCommunters[0]);
            }
            else
            {
                RoundCheck();
            }
        }
    }

    void RoundCheck()
    {
      
        if (commuterParent.childCount != 0) return;
        isGameActive = false;        
        for(int i =0; i<seats.Count; i++)
        {
            Transform commuter = seats[i].GetSeatedCommuter();
            if (commuter != null)
            {
                communterQueue.MoveCommuter(commuter);
            }
        }
        LeanTween.delayedCall(3f, (thisLevel.rounds > 0) ? PoolObject : gameManager.OnGameOver);
    }

    bool StillHaveCommuters()
    {
        return (pooledCommunters.Count > 0);
    }

    void PoolObject()
    {
        var list = thisLevel.commuterAsset.commuter();
        int amountToPool = list.Count;

        thisLevel.rounds--;
        pooledCommunters = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool, commuterParent);
            tmp.GetComponentInChildren<SpriteRenderer>().sprite = list[i];
            tmp.GetComponent<Commuter>().isVulnerable = CommuterInitialise(i);           
            tmp.SetActive(false);
            pooledCommunters.Add(tmp);
        }
        pooledCommunters = Helper.Shuffle(pooledCommunters);
        isGameActive = true;
    }

    bool CommuterInitialise(int i)
    {
        bool isVulnerable = (i < vulnerable) ? true : false;
        return isVulnerable;
    }

    protected override void SetDifficulty()
    {
        switch (GetDifficulty())
        {
            case difficulty.One:
                thisLevel = levels[0];
                break;
            case difficulty.Two:
                thisLevel = levels[1];
                break;

        }
    }
}
[System.Serializable]
internal struct RoundsConfig
{
    public int rounds;
    public int commuterSpeed;
    public NpcAsset commuterAsset;
}
