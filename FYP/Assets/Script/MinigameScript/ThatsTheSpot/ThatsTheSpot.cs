using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ThatsTheSpot : MonoBehaviour
{
    [SerializeField] Transform TargetArea;

    private void Start()
    {
        GetRect(TargetArea);
    }

    void CalculateOverlap(Rect rect)
    {
        
      
    }

    void GetRect(Transform areaTransform)
    {
        var bound = areaTransform.GetComponent<SpriteRenderer>().bounds.size;
        Debug.Log(bound);
    }
}
