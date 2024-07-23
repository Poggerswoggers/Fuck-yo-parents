using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public Transform gridParent;


    private GameObject[,] tiles;
    public List<Tile> correctTiles { get; set; } = new List<Tile>();
    public void GenerateGrid(int size, PathFinder pf)
    {
        Vector2Int parentPosition = Vector2Int.RoundToInt(gridParent.position);  //Get Grid parent position

        correctTiles.Clear();
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        tiles = new GameObject[size, size];

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                GameObject tileObj = Instantiate(tilePrefab, gridParent);
                tileObj.transform.position = new Vector3(x+ parentPosition.x+0.5f, y + parentPosition.y+0.5f, 0);
                tiles[x, y] = tileObj;

                // Assign click event
                Vector2Int pos = new Vector2Int(x, y);

                if(pf.currentSequence.Contains(new Vector2Int(x, y)))
                {
                    tileObj.GetComponent<Tile>().correctPath = true;
                    correctTiles.Add(tileObj.GetComponent<Tile>());
                }
                tileObj.GetComponent<Tile>().Init(pos, pf.OnTileClicked);
            }
        }
    }
}
