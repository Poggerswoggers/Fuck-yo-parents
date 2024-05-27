using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] float levelScore;

    public static ScoreManager Instance { get; private set; }


    private void OnEnable()
    {
        UIEvent.Score += UpdateScore;
    }

    private void OnDisable()
    {
        UIEvent.Score -= UpdateScore;
    }

    private void Awake()
    {
        if(Instance!= null && Instance == this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void UpdateScore()
    {
        levelScore += 1;
    }
}
