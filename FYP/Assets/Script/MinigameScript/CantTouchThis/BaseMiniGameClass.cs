using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseMiniGameClass : MonoBehaviour
{
    protected int score { get; set;}
    public bool isGameActive = false;  //Game Start bool

    protected bool isGameOver; //game Over bool

    protected MinigameManager gameManager;

    protected abstract IEnumerator InstructionCo();

    [Header("Instruction Panel")]
    public GameObject instructionPanel;

    protected virtual void Start()
    {
        gameManager = new MinigameManager(this);
        Instruction();
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

    //public abstract void Instructions();

    protected void Instruction()
    {
        StartCoroutine(InstructionCo());
    }
}
