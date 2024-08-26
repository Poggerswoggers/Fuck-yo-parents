using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour, IQuestionable
{
    public static ScoreManager Instance { get; private set; }
    public static int selectedMinigameDifficulty = 1;
    public Action<int> OnScoreChange;

    [SerializeField] GameObject levelUIPanel;

    [Header("Scores")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI minigameScoreText;

    [SerializeField] int maxScore;
    public int levelScore { get; private set; }

    [Header("Vulnerable Commuter Count")]
    [SerializeField] TextMeshProUGUI vulnerableComCountText;        //Vulnerable count in the level text
    int vulnerableComCount;


    string addictiveScene;      //String name of addictive scene

    //target Ref
    [SerializeField] List<Transform> levelTargets;
    [SerializeField] GameStateManager gSm;

    //hi jason i added this
    //Scoring Feedback
    [SerializeField] GameObject scorePrefab;
    [SerializeField] Transform spawningPoint;
    [SerializeField] float spawnDelay = 0.1f;
    [SerializeField] float offsetAmount = 0.1f;

    [Header("Runtime MCQ Popup")]
        float mcqPopupTimer;
    [SerializeField] float mcqPopUpTime;

    int minigameCount;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
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
        vulnerableComCountText.text = $"{vulnerableComCount}/{levelTargets.Count}";
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
        scoreText.text = levelScore.ToString();
    }

    public void Updatetargets(Transform target)
    {
        if (levelTargets.Contains(target))
        {
            vulnerableComCount++;
            vulnerableComCountText.text = $"{vulnerableComCount}/{levelTargets.Count}";
            mcqPopupTimer = mcqPopUpTime;
        }
    }

    public void UnloadAddictiveScene(int score)
    {
        SceneManager.UnloadSceneAsync(addictiveScene);

        minigameCount--;

        //hi jason i added this
        if (scorePrefab != null) {
            int numberOfPrefabs = score / 1000;
            StartCoroutine(InstantiateScorePrefabsCo(numberOfPrefabs));
            StartCoroutine(UpdateScoreMinigameCo(score));
        }


        //
        if (minigameCount > 0)
        {
            gSm.ChangeState(gSm.snapState);
        }
        gSm.ClearNpc();
    }

    public void LoadAddictiveScene(string sceneName, int minigameDifficulty)
    {
        gSm.ChangeState(gSm.minigameState);
        levelUIPanel.SetActive(false);
        addictiveScene = sceneName;
        selectedMinigameDifficulty = minigameDifficulty;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    IEnumerator UpdateScoreMinigameCo(int scoreDiff)        //Lerp the score because idk how to show a tween value :(
    {
        levelScore += scoreDiff;

        levelUIPanel.SetActive(true);
        minigameScoreText.gameObject.SetActive(true);
        float elapsedLerp = 0;
        float _pointsGained = 0;

        while (elapsedLerp < 1.5f)
        {
            _pointsGained = Mathf.Lerp(_pointsGained, scoreDiff, elapsedLerp / 1.5f);
            minigameScoreText.text = _pointsGained.ToString("00");
            elapsedLerp += Time.deltaTime;

            yield return null;
        }
        minigameScoreText.gameObject.SetActive(false);
        scoreText.text = levelScore.ToString();

        if (minigameCount == 0) { EndLevel(); }  //End level
    }
    public void EndLevel()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.lvlDone);
        }
        Debug.Log("End Game");
        gSm.ChangeState(gSm.endState);
    }

    //hi jason i added this
    IEnumerator InstantiateScorePrefabsCo(int count)
    {
        Vector3 currentSpawnPoint = spawningPoint.position;
        currentSpawnPoint.z = 0;
        for (int i = 0; i < count; i++)
        {
            Instantiate(scorePrefab, currentSpawnPoint, Quaternion.identity);
            currentSpawnPoint.y -= offsetAmount;  
            yield return new WaitForSeconds(spawnDelay);
        }
    }



    private void Update()
    {
        PopupTimer();
    }

    void PopupTimer()
    {
        if (gSm.GetCurrentGameState() == gSm.snapState)
        {
            mcqPopupTimer-= Time.deltaTime;
            if(mcqPopupTimer < 0)
            {
                mcqPopupTimer = mcqPopUpTime;
                PopupQuestion();
            }
        }
    }

    void PopupQuestion()
    {
        DisableLevelUI();
        gSm.mcqState.SetQuestionVariables(GetQuestionType(), guideSprite);
        gSm.ChangeState(gSm.mcqState);
    }

    [Header("Pop up question fields")]
    [SerializeField] Sprite guideSprite;
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
