using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerCollision : MonoBehaviour
{
    public HeadTilt HeadTilt;

    private void Start()
    {
        HeadTilt = HeadTilt.GetComponent<HeadTilt>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ScoreManager.OnScoreChange?.Invoke(250);        
        HeadTilt.HitPassenger();
    }
}
