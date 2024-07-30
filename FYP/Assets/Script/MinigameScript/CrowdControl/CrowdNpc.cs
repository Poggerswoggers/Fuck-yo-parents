using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdNpc: MonoBehaviour
{
    [SerializeField] float speed;
    float dur;
    CrowdControl cc;

    private NpcScriptable _thisNpc; // Backing field for the property
    public NpcScriptable thisNpc
    {
        get
        {
            return _thisNpc;
        }
        set
        {
            _thisNpc = value;
            sr = GetComponent<SpriteRenderer>();
            sr.sprite = thisNpc.npcSprite;
        }
    }
    public Card activeCard { get; private set;}
    GameObject activeCardGameObject;

    List<CardObject> cardList = new List<CardObject>();

    private int cardIndex;

    //Reference
    SpriteRenderer sr;

    public void Initialise(CrowdControl cc)
    {
        this.cc = cc;
    }

    public void MoveInQueue(Vector2 position)
    {
        StartCoroutine(MoveInQueueCo(position));
        //transform.position = Vector2.MoveTowards(transform.position, position, 2f);
    }

    IEnumerator MoveInQueueCo(Vector2 targetPos) //Move to vector position at constant speed
    {
        Vector3 pos = targetPos;
        float distance = (transform.position - pos).magnitude;

        dur = distance / speed;
        float elapseTime = 0;

        Vector3 initialPos = transform.position;

        while(elapseTime<dur)
        {
            transform.position = Vector2.Lerp(initialPos, targetPos, elapseTime / dur);
            elapseTime += Time.deltaTime;

            yield return null;
        }
        transform.position = targetPos;
        
        //Load the card when npc reaches the entrance
        if(transform.position == cc.entrancePos)
        {
            LoadCards(cc.GetCardPos()); 
        }
    }

    //Load the card and add them to the GO card's list and set a random active card from the list
    public void LoadCards(Transform cardPos)
    {
        foreach(CardObject card in _thisNpc.cardObjects)
        {           
            GameObject cardObj = Instantiate(card.gameObject, cardPos);

            cardList.Add(cardObj.GetComponent<CardObject>());
        }
        cardIndex = (int)Random.Range(0, cardList.Count);
        activeCard = cardList[cardIndex].card;

        SetActiveCardSprite();
    }

    public void SwitchActiveCard(float dir)
    {
        cardIndex += (int)dir;
        if (cardIndex < 0){
            cardIndex = cardList.Count - 1;
        }
        else if(cardIndex > cardList.Count - 1){
            cardIndex = 0;
        }
        activeCard = cardList[cardIndex].card;
        SetActiveCardSprite();
    }

    void SetActiveCardSprite()
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            if (cardList[i].card != activeCard)
            {
                //cardList[i].DecreaseSortOrder();
                cardList[i].gameObject.SetActive(false);
            }
            else
            {
                cardList[i].gameObject.SetActive(true);
                activeCardGameObject = cardList[i].gameObject;
            }
        }
    }
    public void SelfDestruct(float time)
    {
        
        Destroy(gameObject, time);
    }

    public void TapCard()
    {
        LeanTween.reset();
        LeanTween.rotateLocal(activeCardGameObject, new Vector3(0,0,-55), 0.2f);
        LeanTween.rotateLocal(activeCardGameObject, new Vector3(0, 0, 0), 0.2f).setDelay(0.8f);
    }

    public void WaitTime()
    {
        StartCoroutine(WaitTimeCo());
    }
    IEnumerator WaitTimeCo()
    {
        yield return new WaitForSeconds(10f);       //Delay die
        cc.GetQueue().RelocateAllNpc(this);         //Rearrange queue

        cc.ahMaLeft++;

        //Turn the other way
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        MoveInQueue(new Vector2(-10, transform.position.y));        //Move off screen
        SelfDestruct(dur);                          //Die
    }
}
