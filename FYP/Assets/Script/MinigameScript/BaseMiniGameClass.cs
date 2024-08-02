using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public abstract class BaseMiniGameClass : MonoBehaviour
{
    public enum difficulty
    {
        One = 1,
        Two = 2,
    }
    public difficulty GetDifficulty()
    {
        if (Enum.IsDefined(typeof(difficulty), ScoreManager.selectedMinigameDifficulty))    
        {
            return (difficulty)ScoreManager.selectedMinigameDifficulty;
        }
        else
        {
            return difficulty.One;
        }
    }

    protected int score;
    public bool isGameActive = false;  //Game Start bool
    
    protected MinigameManager gameManager;

    protected abstract IEnumerator InstructionCo();

    [Header("Instruction Panel")]
    [SerializeField] GameObject gameOverPanel;
    public GameObject instructionPanel;

    protected virtual void Start()
    {
        gameManager = new MinigameManager(this);
        gameManager.EndSequence += EndSequenceMethod;
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
        ScoreManager.Instance?.UnloadAddictiveScene(i);
    }

    private void OnDisable(){
        gameManager.EndSequence -= EndSequenceMethod;
    }

    protected virtual void SetDifficulty()
    {

    }
}

