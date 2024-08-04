using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public static int selectedMinigameDifficulty =1;
    public Action<int> OnScoreChange;

    [SerializeField] int maxScore;
    [SerializeField] int levelScore;

    [Header("Scores")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI minigameScoreText;

    GameObject scoreParent;

    string addictiveScene;

    //target Ref
    [SerializeField] List<Transform> levelTargets;
    [SerializeField] GameStateManager gSm;
    public List<Transform> _levelTarget
    {
        get
        {
            return levelTargets;
        }
    }

    int minigameCount;

    [Header("Level End Panel")]
    [SerializeField] GameObject levelEndPanel;

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
        minigameCount = levelTargets.Count;
        scoreParent = scoreText.transform.parent.gameObject;
        UpdateScore(-maxScore);
    }


    public void UpdateScore(int points)
    {       
        levelScore -= points;
        scoreText.text = levelScore.ToString();
    }

    public void Updatetargets(Transform target )
    {
        if(levelTargets.Contains(target))
        {
            levelTargets.Remove(target);
        }
    }

    public void UnloadAddictiveScene(int score)
    {       
        SceneManager.UnloadSceneAsync(addictiveScene);

        minigameCount--;
        StartCoroutine(UpdateScoreMinigameCo(score));
        if (minigameCount > 0)
        {
            gSm.ChangeStat(gSm.snapState);
        }
        gSm.ClearNpc();        
    }

    public void loadAddictiveScene(string sceneName, int minigameDifficulty)
    {
        scoreParent.SetActive(false);
        addictiveScene = sceneName;
        selectedMinigameDifficulty = minigameDifficulty;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    IEnumerator UpdateScoreMinigameCo(int scoreDiff)        //Lerp the score because idk how to show a tween value :(
    {
        levelScore += scoreDiff;

        scoreParent.SetActive(true);
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

        if(minigameCount == 0) { EndLevel(); }  //End level
    }
    void EndLevel()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.lvlDone);
        }
        levelEndPanel.SetActive(true);
    }
}
