using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commuter : MonoBehaviour
{
    public float speed;
    public void Move(Vector2 endPos, float dist)
    {
        float dur = dist / speed;
        LeanTween.moveLocal(gameObject, endPos, dur).setOnComplete(() => gameObject.SetActive(false));
    }
}
