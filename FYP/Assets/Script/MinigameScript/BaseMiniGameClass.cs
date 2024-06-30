using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public abstract class BaseMiniGameClass : MonoBehaviour
{
    protected int score;
    public bool isGameActive = false;  //Game Start bool

    public bool isGameOver { get; set; }

    protected MinigameManager gameManager;

    public Action EndSequence {get; set;}

    protected abstract IEnumerator InstructionCo();

    [Header("Instruction Panel")]
    [SerializeField] GameObject gameOverPanel;
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

    public abstract void EndSequenceMethod();

    protected void UnloadedAndUpdateScore(int i)
    {
        ScoreManager.Instance.UnloadAddictiveScene(i);
    }


    private void OnEnable(){
        EndSequence += EndSequenceMethod;
    }
    private void OnDisable(){
        EndSequence -= EndSequenceMethod;
    }


}

