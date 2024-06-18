using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardObject : MonoBehaviour
{
    public Card card;
    SpriteRenderer sr;

    public void DecreaseSortOrder()
    {
        StartCoroutine(delay());
    }

    public void IncreaseSortOrder()
    {
        sr.sortingOrder = 5;
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(0.2f);
        sr.sortingOrder = 4;
    }
}
