using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class overlapArea
{
    float _overlapArea;
    public struct points
    {
        public Vector2 minPoint;
        public Vector2 maxPoint;
    }


    float CalculateOverLap(points r1, points r2)
    {
        float area1 = Mathf.Abs(r1.minPoint.x - r1.maxPoint.x) * Mathf.Abs(r1.minPoint.y - r1.maxPoint.y);

        float area2 = Mathf.Abs(r2.minPoint.x - r2.maxPoint.x) * Mathf.Abs(r2.minPoint.y - r2.maxPoint.y);

        float x_dist = Mathf.Min(r1.maxPoint.x, r2.maxPoint.x) - Mathf.Max(r1.minPoint.x, r2.minPoint.x);
        float y_dist = Mathf.Min(r1.maxPoint.y, r2.maxPoint.y) - Mathf.Max(r1.minPoint.y, r2.minPoint.y);

        float area = 0;
        if (x_dist > 0 && y_dist > 0)
        {
            area = x_dist * y_dist;
            _overlapArea = area / area2;
        }
        return _overlapArea;
        //return (area1 + area2 - area);
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

    public float GetOverlapArea(Transform target, Transform pmd)
    {
        return CalculateOverLap(GetRectBound(target), GetRectBound(pmd));
    }
}
