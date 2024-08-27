using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBound : MonoBehaviour
{
    [SerializeField] Transform boundaryObj;
    [SerializeField] Transform upperBoundaryObj;
    public static Vector2 Bound { get; private set; }
    public static Vector2 UpperBound { get; private set; }

    private void Awake()
    {
        var bound = boundaryObj.GetComponent<SpriteRenderer>().bounds;
        Bound = bound.extents;

        var upperBound = upperBoundaryObj.GetComponent<SpriteRenderer>().bounds;
        UpperBound = upperBound.extents;
    }
}
