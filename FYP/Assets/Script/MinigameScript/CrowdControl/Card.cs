using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card
{
    public bool tappable;

    public enum CardType
    {
        EzlinkCard,
        PopularCard,
        PokemonCard
    }
    public CardType cardType;
}
