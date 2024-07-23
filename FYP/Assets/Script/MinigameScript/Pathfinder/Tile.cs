using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool correctPath { get; set; }
    Vector2Int xy;
    private System.Action<Tile> onClick;

    public void Init(Vector2Int xy, System.Action<Tile> onClick)
    {
        this.xy = xy;
        this.onClick = onClick;
    }
    public void ChangeColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }

    public void OnClick()
    {
        onClick?.Invoke(this);
    }

    public Vector2Int getCoord()
    {
        return xy;
    }
}
