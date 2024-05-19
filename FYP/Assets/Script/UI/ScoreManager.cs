using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] float levelScore;

    private void OnEnable()
    { 
        UIEvent.Score += UpdateScore;
    }

    private void OnDisable()
    {
        UIEvent.Score -= UpdateScore;
    }


    void UpdateScore()
    {
        levelScore += 1;
    }
}
