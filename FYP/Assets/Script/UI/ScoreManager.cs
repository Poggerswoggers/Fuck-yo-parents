using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static Action<int> OnScoreChange;
    public static Action<Transform> OnTargetChanged;

    [SerializeField] int maxScore;
    [SerializeField] int levelScore;
    [SerializeField] TextMeshProUGUI scoreText;

    //target Ref
    public List<Transform> levelTargets;

    private void OnEnable()
    {
        OnScoreChange += UpdateScore;
        OnTargetChanged += Updatetargets;
    }

    private void OnDisable()
    {
        OnScoreChange -= UpdateScore;
        OnTargetChanged += Updatetargets;
    }

    private void Start()
    {
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
}
