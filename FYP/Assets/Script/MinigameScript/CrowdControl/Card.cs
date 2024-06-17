using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card
{
    public bool tappable;
    public Sprite cardSprite;

    public enum CardType
    {
        EzlinkCard,
        PopularCard,
        AnimalKaizerCard
    }
    public CardType cardType;
}
