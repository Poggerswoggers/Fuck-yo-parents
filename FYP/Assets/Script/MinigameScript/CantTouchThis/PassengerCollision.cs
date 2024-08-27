using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerCollision : MonoBehaviour
{
    [SerializeField] HeadTilt HeadTilt;
    private void Start()
    {
        HeadTilt = HeadTilt.GetComponent<HeadTilt>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        LeanTween.moveLocalY(collision.transform.GetChild(0).gameObject, 0.2f, 0.5f).setEaseShake();
        HeadTilt.HitPassenger();
    }
}
