using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class LevelManager : MinigameLevelManager, IQuestionable
{
    public static new LevelManager Instance { get; private set; }
    public Action<int> OnScoreChange;

    [SerializeField] GameObject levelUIPanel;
    [SerializeField] int maxScore;

    [Header("Vulnerable Commuter Count")]
    [SerializeField] TextMeshProUGUI vulnerableComCountText;        //Vulnerable count in the level text
    int vulnerableComCount;
    int vulnerableComFound = 0;


    string addictiveScene;      //String name of addictive scene

    //target Ref
    [SerializeField] List<Transform> levelTargets;
    [SerializeField] GameStateManager gSm;
    [SerializeField] TextMeshProUGUI currentcommuterselect;

    int minigameCount;

    [Header("Audio")]
    [SerializeField] AudioClip levelMusic;

    //Reference Script
    [Header("References")]
    [SerializeField] ScoreManager scoreManager;

    public override void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        PlayLevelAudio();
        sceneLoader = new MinigameSceneLoader();
    }

    public void PlayLevelAudio()
    {
        if (AudioManager.instance != null)
        AudioManager.instance.PlayMusic(levelMusic, 0.8f);
    }

    private void OnEnable()
    {
        OnScoreChange += UpdateScore;
    }

    private void OnDisable()
    {
        OnScoreChange -= UpdateScore;
    }

    private void Start()
    {
        //Initialise Vulnerable commuter amount text
        vulnerableComCount = levelTargets.Count;
        vulnerableComCountText.text = $"{vulnerableComFound}/{levelTargets.Count}";
        minigameCount = levelTargets.Count;
        //Set popup timer
        mcqPopupTimer = mcqPopUpTime;
        //Initialise level score
        UpdateScore(-maxScore);
    }

    //Toggle UI to be in view or not
    public void EnableLevelUI() => LeanTween.move(levelUIPanel.GetComponent<RectTransform>(), Vector2.zero, 0.8f).setEaseInCubic(); 
    public void DisableLevelUI() => LeanTween.move(levelUIPanel.GetComponent<RectTransform>(), Vector2.up * 150, 1).setEaseInBack();

    public void UpdateScore(int points)
    {
        levelScore -= points;
        scoreManager.UpdateScore(levelScore);
    }

    public void Updatetargets(Transform target)
    {
        //Update target this stuff is ancient
        if (levelTargets.Contains(target))
        {
            levelTargets.Remove(target);
            vulnerableComFound++;
            vulnerableComCountText.text = $"{vulnerableComFound}/{vulnerableComCount}";
            mcqPopupTimer = mcqPopUpTime;
        }
    }

    public override void UnloadAddictiveScene(int score)
    {
        sceneLoader.UnloadAddictiveScene();
        //Unload scene here

        minigameCount--;

        levelScore += score;
        levelUIPanel.SetActive(true);
        scoreManager.ScoreEffect(score, levelScore);
        
        gSm.ClearNpc();
        if (minigameCount > 0)
        gSm.ChangeState(gSm.snapState);     //go back to snap state
    }

    public override void LoadAddictiveScene(string sceneName, int difficulty)
    {
        selectedMinigameDifficulty = difficulty;
        gSm.ChangeState(gSm.minigameState);
        levelUIPanel.SetActive(false);
        sceneLoader.LoadAddictiveScene(sceneName);
    }

    public void EndLevel()
    {
        if (minigameCount != 0) return;
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.lvlDone);
        }
        Debug.Log("End Game");

        gSm.ChangeState(gSm.endState);
    }

    private void Update()
    {
        PopupTimer();
        currentcommuterselect.text = gSm.GetCurrentGameState().ToString();
    }

    //Pop up timer to execute popup MCQ
    void PopupTimer()
    {
        if (gSm.GetCurrentGameState() == gSm.snapState)
        {
            mcqPopupTimer-= Time.deltaTime;
            if(mcqPopupTimer < 0)
            {
                mcqPopupTimer = mcqPopUpTime;
                russellPopup.SetActive(true);
                DisableLevelUI();
                LeanTween.delayedCall(3, PopupQuestion);
            }
        }
    }

    void PopupQuestion()
    {
        russellPopup.SetActive(false);
        gSm.mcqState.SetQuestionVariables(GetQuestionType(), guideSprite);
        gSm.ChangeState(gSm.mcqState);
    }
    
    public void ShowFrustratedNpc()
    {
        Debug.Log("hehe");
        foreach(var commuter in levelTargets)
        {
            StartCoroutine(commuter.GetComponent<NpcAnimation>().Frustrated(4.5f));
        }
    }

    [Header("Pop up question fields")]
    [SerializeField] Sprite guideSprite;
    [SerializeField] GameObject russellPopup;
    [SerializeField] float mcqPopUpTime;
    float mcqPopupTimer;

    QuestionTypes QuestionType
    {
        get
        {
            Array values = Enum.GetValues(typeof(QuestionTypes));
            System.Random random = new();
            return (QuestionTypes)values.GetValue(random.Next(values.Length));
        }
    }
    public QuestionTypes GetQuestionType()
    {
        return QuestionType;
    }
}
