using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThatsTheSpot : BaseMiniGameClass
{
    [SerializeField]float tries = 2;
    [SerializeField] List<GameObject> triesObject;
    bool tried;

    [SerializeField] Transform targetTransform;
    [SerializeField] Transform pmdTransform;


    overlapArea overlapAreaRef;


    public float overlapArea;

    [SerializeField] TextMeshProUGUI percentageText;

    int liveScore;
    int bestScore;

    public struct points
    {
        public Vector2 minPoint;
        public Vector2 maxPoint;
    }
    private void Awake()
    {
        overlapAreaRef = new overlapArea();
    }

    public override void StartGame()
    {
        isGameActive = true;
    }

    

    public override void UpdateGame()
    {
        //throw new System.NotImplementedException();
    }

    public override void EndSequenceMethod()
    {
        score = CalculateScore();
        Debug.Log(score);
        base.UnloadedAndUpdateScore(score);
    }

    int CalculateScore()
    {
        if (bestScore == 100) 
            return 2000;
        else if (bestScore >= 50) 
            return 1000;
        else 
            return 500;
    }

    public void GetArea(Transform areaTransform, Transform pmdTransform)
    {
        overlapArea = overlapAreaRef.GetOverlapArea(areaTransform, pmdTransform);
        percentageText.gameObject.SetActive(true);
        percentageText.text = Mathf.RoundToInt(overlapArea * 100) + "%";

        if (!tried)
        {
            tried = true;
            StartCoroutine(RetrySequenceCo(pmdTransform.GetComponent<PmdController>()));
            triesObject[(int)tries].SetActive(false);
        }
    }
    IEnumerator RetrySequenceCo(PmdController pmdC)
    {
        yield return new WaitForSeconds(3f);
        liveScore = Mathf.RoundToInt(overlapArea * 100);
        if(liveScore > bestScore) { bestScore = liveScore; }

        if (tries >0)
        {
            tries--;
            percentageText.gameObject.SetActive(false);
            pmdC.ResetAttempt();
            tried = false;

            overlapArea = 0;
        }
        else
        {
            Debug.Log("Game Over");
            gameManager.OnGameOver();
        }
    }
}
