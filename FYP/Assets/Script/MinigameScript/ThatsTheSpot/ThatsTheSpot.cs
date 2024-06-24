using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThatsTheSpot : BaseMiniGameClass
{
    [SerializeField] Transform targetArea;
    [SerializeField] Transform pmdTransform;

    //Reference
    [SerializeField] PrecisionSlider slider;

    public float overlapArea;
    public struct points
    {
        public Vector2 minPoint;
        public Vector2 maxPoint;
    }



    protected override IEnumerator InstructionCo()
    {
        yield return null;
        StartGame();
    }

    public override void StartGame()
    {
        //Debug.Log(CalculateOverLap(meme(targetArea), meme(targetArea2)));   

        isGameActive = true;
    }

    float CalculateOverLap(points r1, points r2)
    {
        float area1 = Mathf.Abs(r1.minPoint.x - r1.maxPoint.x) * Mathf.Abs(r1.minPoint.y - r1.maxPoint.y);

        float area2 = Mathf.Abs(r2.minPoint.x - r2.maxPoint.x) * Mathf.Abs(r2.minPoint.y - r2.maxPoint.y);

        float x_dist = Mathf.Min(r1.maxPoint.x, r2.maxPoint.x) - Mathf.Max(r1.minPoint.x, r2.minPoint.x);
        float y_dist = Mathf.Min(r1.maxPoint.y, r2.maxPoint.y) - Mathf.Max(r1.minPoint.y, r2.minPoint.y);

        Debug.Log(x_dist + " " + y_dist);

        float area = 0;
        if (x_dist > 0 && y_dist > 0)
        {
            area = x_dist * y_dist;
            overlapArea = area / area2;
        }

        return (area1 + area2 - area);
    }

    points GetRectBound(Transform targetTransform)
    {
        var boundMax = targetTransform.GetComponent<SpriteRenderer>().bounds.max;
        var boundMin = targetTransform.GetComponent<SpriteRenderer>().bounds.min;
        points target;

        target.minPoint = boundMin;
        target.maxPoint = boundMax;
        return target;
    }

    public override void UpdateGame()
    {
        //throw new System.NotImplementedException();
    }

    public override void EndSequenceMethod()
    {
        //throw new System.NotImplementedException();
    }
}
