using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBound : MonoBehaviour
{
    [SerializeField] Transform boundaryObj;
    public static Vector2 Bound { get; private set; }

    private void Start()
    {
        var bound = boundaryObj.GetComponent<SpriteRenderer>().bounds;
        Bound = bound.extents;
    }
}
