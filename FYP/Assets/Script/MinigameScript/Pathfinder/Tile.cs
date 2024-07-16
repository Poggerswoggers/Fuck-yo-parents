using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool correctPath { get; set; }
    private int x, y;
    private System.Action<int, int> onClick;

    public void Init(int x, int y, System.Action<int, int> onClick)
    {
        this.x = x;
        this.y = y;
        this.onClick = onClick;
    }

    public void ChangeColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }

    public void OnClick()
    {
        onClick?.Invoke(x, y);
        ChangeColor(Color.red);
    }

    public Vector2Int getCoord()
    {
        return new Vector2Int(x, y);
    }
}
