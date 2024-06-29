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
    public Action<int> OnScoreChange;

    [SerializeField] int maxScore;
    [SerializeField] int levelScore;
    [SerializeField] TextMeshProUGUI scoreText;
    GameObject scoreParent;

    int addictiveScene;

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
        scoreText.text = "SCORE:" + levelScore.ToString();
    }

    public void Updatetargets(Transform target )
    {
        if(levelTargets.Contains(target))
        {
            levelTargets.Remove(target);
        }
    }

    public void UnloadAddictiveScene()
    {
        scoreParent.SetActive(true);
        SceneManager.UnloadSceneAsync(addictiveScene);
        gSm.ChangeStat(gSm.snapState);
        gSm.ClearNpc();
    }
    public void loadAddictiveScene(int sceneId)
    {
        scoreParent.SetActive(false);
        addictiveScene = sceneId;
        SceneManager.LoadScene(sceneId, LoadSceneMode.Additive);
    }
}
