using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerCollision : MonoBehaviour
{
    public HeadTilt HeadTilt;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Lose");
    }
}
