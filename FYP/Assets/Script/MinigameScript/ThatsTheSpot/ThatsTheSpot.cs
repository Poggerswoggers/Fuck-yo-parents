using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThatsTheSpot : BaseMiniGameClass
{
    [SerializeField]float tries = 2;
    bool tried;

    [SerializeField] Transform targetTransform;
    [SerializeField] Transform pmdTransform;


    overlapArea overlapAreaRef;


    public float overlapArea;

    [SerializeField] TextMeshProUGUI percentageText;

    public struct points
    {
        public Vector2 minPoint;
        public Vector2 maxPoint;
    }
    private void Awake()
    {
        overlapAreaRef = new overlapArea();
    }


    protected override IEnumerator InstructionCo()
    {
        yield return null;
        StartGame();
    }

    public override void StartGame()
    {
        //Debug.Log(overlapAreaRef.GetOverlapArea(targetTransform, pmdTransform));
        isGameActive = true;
    }

    

    public override void UpdateGame()
    {
        //throw new System.NotImplementedException();
    }

    public override void EndSequenceMethod()
    {
        //throw new System.NotImplementedException();
    }

    public void GetArea(Transform areaTransform, Transform pmdTransform)
    {
        overlapArea = overlapAreaRef.GetOverlapArea(areaTransform, pmdTransform);
        percentageText.gameObject.SetActive(true);
        percentageText.text = Mathf.RoundToInt(overlapArea * 100) + "%";

        if (!tried)
        {
            tried = true;
            if (tries > 0)
            {
                tries--;
                StartCoroutine(RetrySequenceCo(pmdTransform.GetComponent<PmdController>()));
            }
            else
            {
                Debug.Log("Game Over");
                gameManager.OnGameOver();
            }
        }
    }
    IEnumerator RetrySequenceCo(PmdController pmdC)
    {
        yield return new WaitForSeconds(3f);
        percentageText.gameObject.SetActive(false);
        pmdC.ResetAttempt();
        tried = false;
    }
}
