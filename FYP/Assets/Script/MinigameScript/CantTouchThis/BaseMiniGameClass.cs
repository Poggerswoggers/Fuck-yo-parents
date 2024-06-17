using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMiniGameClass : MonoBehaviour
{
    protected int score { get; set;}
    protected bool isGameActive;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        StartGame();
    }
    public abstract void StartGame();

    // Update is called once per frame
    protected virtual void Update()
    {
        if(isGameActive)
        {
            UpdateGame();
        }
    }
    public abstract void UpdateGame();
}
