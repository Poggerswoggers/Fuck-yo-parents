using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdNpc: MonoBehaviour
{
    [SerializeField] float speed;
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
    List<CardObject> cardList = new List<CardObject>();
    bool cardLoaded;

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

    IEnumerator MoveInQueueCo(Vector2 targetPos)
    {
        Vector3 pos = targetPos;
        float distance = (transform.position - pos).magnitude;

        float dur = distance / speed;
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
            cardLoaded = true;
        }
    }

    public void LoadCards(Vector2 cardPos)
    {
        foreach(CardObject card in _thisNpc.cardObjects)
        {           
            GameObject cardObj = Instantiate(card.gameObject, transform.GetChild(0));
            cardObj.transform.position = cardPos;

            cardList.Add(cardObj.GetComponent<CardObject>());
        }
        cardIndex = (int)Random.Range(0, cardList.Count);
        activeCard = cardList[cardIndex].card;

        SetActiveCardSprite();
    }

    public void SwitchActiveCard()
    {
        cardIndex = (cardIndex == cardList.Count - 1) ? 0 : cardIndex+1;

        activeCard = cardList[cardIndex].card;
        SetActiveCardSprite();
    }

    void SetActiveCardSprite()
    {
        for(int i =0; i< cardList.Count; i++)
        {
            cardList[i].gameObject.SetActive(cardList[i].card == activeCard);
        }
    }

    public void SelfDestruct()
    {
        Destroy(transform.GetChild(0).gameObject);
        Destroy(gameObject, 2);
    }
}
