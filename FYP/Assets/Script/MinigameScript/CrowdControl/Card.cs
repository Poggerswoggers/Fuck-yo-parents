using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card
{
    public bool tappable;

    public enum CardTypes
    {
        EzlinkCard,
        OtherCard
    }
    public CardTypes cardType;
}
