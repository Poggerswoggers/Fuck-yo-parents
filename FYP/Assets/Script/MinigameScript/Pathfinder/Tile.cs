using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private int x, y;
    private System.Action<int, int> onClick;

    public void Init(int x, int y, System.Action<int, int> onClick)
    {
        this.x = x;
        this.y = y;
        this.onClick = onClick;
    }

    void OnMouseDown()
    {
        onClick?.Invoke(x, y);
    }
}
