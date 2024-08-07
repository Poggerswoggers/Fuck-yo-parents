using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] Sprite clickableSprite;
    [SerializeField] GameObject tilePrefab;
    [SerializeField] Transform GridOrigin;

    private GameObject[,] tiles;
    public List<Tile> CorrectTiles { get; set; } = new List<Tile>();

    readonly List<Tile> allTiles = new List<Tile>();

    Vector3 startPos, endPos;

    [Header("Path Rnner")]
    [SerializeField] Transform pathRunner;
    [SerializeField] float runnerSpeed;

    //reference
    PathFinder pf;

    Vector3 targetPosition = Vector3.zero;
    float dist = 0;

    private void Start()
    {
        LeanTween.reset();
    }

    public void GenerateGrid(int size, PathFinder pf)
    {
        this.pf = pf;
        Vector2Int parentPosition = Vector2Int.RoundToInt(GridOrigin.position);  //Get Grid parent position

        CorrectTiles.Clear();
        allTiles.Clear();
        foreach (Transform child in GridOrigin.GetChild(1))
        {
            Destroy(child.gameObject);
        }

        tiles = new GameObject[size, size];

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                GameObject tileObj = Instantiate(tilePrefab, GridOrigin.GetChild(1));
                tileObj.transform.position = new Vector3(x+ parentPosition.x+0.5f, y + parentPosition.y+0.5f, 0);
                tiles[x, y] = tileObj;

                // Assign click event
                Vector2Int pos = new Vector2Int(x, y);
                allTiles.Add(tileObj.GetComponent<Tile>());
                tileObj.GetComponent<Tile>().Init(pos, pf.OnTileClicked);
            }
        }
        //I love linq xoxo
        CorrectTiles = pf.MatchCorrectTile(allTiles);
    }

    public void SetMiscTile()
    {
        //This first loop set's the path to the first correct tile(entry)
        Vector3 posFirst = CorrectTiles[0].transform.position; 
        for(int i = 1; i < 5; i++)
        {
            Vector3 w_pos = new Vector3(posFirst.x, posFirst.y - i, 0);
            startPos = w_pos;
            pf.paintTile(w_pos);
      
        }
        //This second loop set's the path to the last correct tile(exit)
        Vector3 posLast = CorrectTiles[CorrectTiles.Count-1].transform.position;
        for (int i = 1; i < 5; i++)
        {
            Vector3 n_pos = new Vector3(posLast.x, posLast.y + i, 0);
            endPos = n_pos;
            pf.paintTile(n_pos);
        }
        pathRunner.position = startPos;
        pathRunner.GetComponentInChildren<SpriteRenderer>().sprite = pf.currentSequence.pathRunnerSprite;
    }

    public void ColorAllTiles(Color color)
    {
        foreach(Tile tile in allTiles)
        {
            tile.GetComponent<SpriteRenderer>().sprite = clickableSprite;
            tile.ChangeColor(color);
        }
    }

    public void RunPathCo(int index, bool state)
    {
        LTSeq sequence = LeanTween.sequence();
        Vector3 previousPosition = pathRunner.position;

        for (int i=0; i<index; i++)
        {
            targetPosition = CorrectTiles[i].transform.position;
            dist = (targetPosition - previousPosition).magnitude;
            float duration = dist / runnerSpeed;
            sequence.append(LeanTween.move(pathRunner.gameObject, CorrectTiles[i].transform.position,duration));

            previousPosition = targetPosition;
        }
        sequence.append(() =>
        {
            pf.SetOnGridComplete(GridEndBool(state, previousPosition));
        });
    }

    bool GridEndBool(bool state, Vector3 previousPosition)
    {
        if (!state)
        {
            targetPosition = endPos;
            dist = (targetPosition - previousPosition).magnitude;
            float duration = dist / runnerSpeed;
            LeanTween.move(pathRunner.gameObject, endPos, duration).setOnComplete(pf.CheckGridEnd);
        }
        else
        {
            LeanTween.delayedCall(1f, pf.CheckGridEnd);
        }
        return state;
    }
}
