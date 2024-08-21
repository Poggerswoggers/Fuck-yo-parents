using System.Collections;
using UnityEngine;
using System;

public abstract class BaseMiniGameClass : MonoBehaviour
{
    public enum difficulty
    {
        One = 1,
        Two = 2,
    }
    public difficulty GetDifficulty()
    {
        return Enum.IsDefined(typeof(difficulty), ScoreManager.selectedMinigameDifficulty)? 
            (difficulty)ScoreManager.selectedMinigameDifficulty : difficulty.One;
    }

    protected int score;
    public bool isGameActive = false;  //Game Start bool
    
    protected MinigameManager gameManager;

    protected virtual IEnumerator InstructionCo()
    {
        instructionPanel.SetActive(true);
        yield return new WaitForSeconds(10f);
        instructionPanel.SetActive(false);
        StartGame();
    }

    [Header("Instruction Panel")]
    [SerializeField] GameObject gameOverPanel;    
    [SerializeField] protected GameObject instructionPanel;


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

    protected virtual void SetDifficulty() { }

    public void SkipInstructions()
    {
        StopCoroutine(InstructionCo());
        instructionPanel.SetActive(false);
        StartGame();
    }
}

