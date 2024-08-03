using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commuter : MonoBehaviour
{
    [SerializeField] float speed;
    public bool isVulnerable { get; set; }

    public void Move(Vector2 endPos, float dist)
    {
        isMoving(true);
        float dur = dist / speed;
        LeanTween.move(gameObject, endPos, dur).setOnComplete(()=> Destroy(gameObject));
    }

    public void isMoving(bool state)
    {
        GetComponentInChildren<MinigameCommuterBounce>().enabled = state;
    }
}
