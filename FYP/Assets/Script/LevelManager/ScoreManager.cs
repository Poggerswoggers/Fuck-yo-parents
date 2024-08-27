using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{
    //hi jason i added this
    //Scoring Feedback
    [SerializeField] GameObject scorePrefab1000;
    [SerializeField] GameObject scorePrefab500;
    [SerializeField] GameObject scorePrefab100;
    [SerializeField] Transform spawningPoint;
    [SerializeField] float spawnDelay = 0.1f;
    [SerializeField] float offsetAmount = 0.1f;

    [Header("Scores")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI minigameScoreText;
    [SerializeField] Image medalImage;

    [Header("Medal Manager")]
    [SerializeField] MedalManager medalManager;

    public void UpdateScore(int levelScore) => scoreText.text = levelScore.ToString();

    public void ScoreEffect(int score, int levelScore)
    {
        if (scorePrefab1000 != null && scorePrefab500 != null && scorePrefab100 != null)
        {
            int numberOfPrefabs1000 = score / 1000;
            int numberOfPrefabs500 = score % 1000 / 500;
            int numberOfPrefabs100 = score % 500 / 100;

            StartCoroutine(InstantiateScorePrefabsCo(numberOfPrefabs1000, numberOfPrefabs500, numberOfPrefabs100));
            StartCoroutine(UpdateScoreMinigameCo(score, levelScore));
        }
    }

    IEnumerator UpdateScoreMinigameCo(int scoreDiff, int levelScore)        //Lerp the score because idk how to show a tween value :(
    {
        //levelUIPanel.SetActive(true);
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

        LevelManager.Instance.EndLevel();
    }

    //hi jason i added this
    IEnumerator InstantiateScorePrefabsCo(int count, int count2, int count3)
    {
        Vector3 currentSpawnPoint = spawningPoint.position;
        currentSpawnPoint.z = 0;
        for (int i = 0; i < count; i++)
        {
            Instantiate(scorePrefab1000, currentSpawnPoint, Quaternion.identity);
            currentSpawnPoint.y -= offsetAmount;
            yield return new WaitForSeconds(spawnDelay);
        }
        for (int i = 0; i < count2; i++)
        {
            Instantiate(scorePrefab500, currentSpawnPoint, Quaternion.identity);
            currentSpawnPoint.y -= offsetAmount;
            yield return new WaitForSeconds(spawnDelay);
        }

        for (int i = 0; i < count3; i++)
        {
            Instantiate(scorePrefab100, currentSpawnPoint, Quaternion.identity);
            currentSpawnPoint.y -= offsetAmount;
            yield return new WaitForSeconds(spawnDelay);
        }


    }
}
