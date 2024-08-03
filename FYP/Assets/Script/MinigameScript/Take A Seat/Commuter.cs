using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commuter : MonoBehaviour
{
    [SerializeField] float speed;
    public bool isVulnerable { get; set; }
    public void Move(Vector2 endPos, float dist)
    {
        float dur = dist / speed;
        LeanTween.moveLocal(gameObject, endPos, dur).setOnComplete(()=> Destroy(gameObject));
    }
}
