using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public Transform gridParent;


    private GameObject[,] tiles;
    public void GenerateGrid(int size, PathFinder pf)
    {
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
                tileObj.transform.position = new Vector3(x, y, 0);
                tiles[x, y] = tileObj;

                // Assign click event
                int posX = x;
                int posY = y;
                tileObj.GetComponent<Tile>().Init(posX, posY, pf.OnTileClicked);
            }
        }
    }
}
