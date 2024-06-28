using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public abstract class BaseMiniGameClass : MonoBehaviour
{
    protected int score { get; set;}
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

    public virtual void EndSequenceMethod()
    {
        ScoreManager.Instance.UnloadAddictiveScene();
    }


    private void OnEnable(){
        EndSequence += EndSequenceMethod;
    }
    private void OnDisable(){
        EndSequence -= EndSequenceMethod;
    }


}

