using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JigSawPiece : MonoBehaviour
{
    public bool occupied { get; set; }
    public Vector2Int GetPos()
    {
        return (Vector2Int)Vector3Int.FloorToInt(transform.position);
    }
}
