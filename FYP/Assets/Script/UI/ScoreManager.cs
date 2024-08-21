using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
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
    [SerializeField] TextMeshProUGUI vulnerableComCountText;
    int vulnerableComCount;


    string addictiveScene;

    //target Ref
    [SerializeField] List<Transform> levelTargets;
    [SerializeField] GameStateManager gSm;

    //hi jason i added this
    //Scoring Feedback
    [SerializeField] GameObject scorePrefab;
    [SerializeField] Transform spawningPoint;
    [SerializeField] float spawnDelay = 0.1f;
    [SerializeField] float offsetAmount = 0.1f;
    //

    public List<Transform> _levelTarget
    {
        get
        {
            return levelTargets;
        }
    }

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
        vulnerableComCount = levelTargets.Count;
        vulnerableComCountText.text = vulnerableComCount.ToString();
        minigameCount = levelTargets.Count;
        UpdateScore(-maxScore);
    }

    public void DisableUI(bool state)
    {
        levelUIPanel.SetActive(state);
    }

    public void UpdateScore(int points)
    {
        levelScore -= points;
        scoreText.text = levelScore.ToString();
    }

    public void Updatetargets(Transform target)
    {
        if (levelTargets.Contains(target))
        {
            levelTargets.Remove(target);
            vulnerableComCount--;
            vulnerableComCountText.text = vulnerableComCount.ToString();
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
        for (int i = 0; i < count; i++)
        {
            Instantiate(scorePrefab, currentSpawnPoint, Quaternion.identity);
            currentSpawnPoint.y -= offsetAmount;  
            yield return new WaitForSeconds(spawnDelay);
        }
    }
    //

}
