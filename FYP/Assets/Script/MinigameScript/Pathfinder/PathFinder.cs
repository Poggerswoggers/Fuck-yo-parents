using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinder : BaseMiniGameClass
{
    [Header("Game config")]
    [SerializeField] int tries;

    [SerializeField] GridManager gm;
    [Header("Sequence")]
    [SerializeField] List<CorrectSequences> sequences;  //All sequences 
    //[SerializeField] List<Tile> playerSequence = new List<Tile>();  //Players click sequence
    public List<Vector2Int> currentSequence { get; set;}
    int index;
    bool failed;

    [SerializeField] int gridSize;
    [Header("Delay Time")]
    [SerializeField] float delayTime;
    [Header("FlashTiles")]
    [SerializeField] GameObject correctTiles;
    [Header("TileMap")] //Tilemap
    [SerializeField] Tilemap tileMap;
    [SerializeField] TileBase defaultTile;

    public override void EndSequenceMethod()
    {
        //throw new System.NotImplementedException();
    }

    public override void StartGame()
    {
        score = 2000;

        currentSequence = sequences[0].correctSequence;     //Current sequence
        gm.GenerateGrid(gridSize, this);                    //Generate the grid on start
        CheckCorrectSequence(currentSequence);              //Check sequence is valid
        tries--;
    }

    public override void UpdateGame()
    {
        //Mouse screen raycast to click on the tile
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                hit.transform.GetComponent<Tile>().OnClick();
            }
        }
    }

    protected override IEnumerator InstructionCo()
    {
        yield return null;
        StartGame();
    }

    void IncreaseGridSize()
    {
        //After completing a sequence, this is called to start a new sequence
        gridSize++;
        sequences.RemoveAt(0);
        currentSequence = sequences[0].correctSequence;
        Retry();
    }
    public void Retry()
    {
        tileMap.ClearAllTiles();
        isGameActive = false;
        index = 0;
        gm.GenerateGrid(gridSize, this);
        CheckCorrectSequence(currentSequence);
    }

    void CheckCorrectSequence(List<Vector2Int> correctSequence)
    {
        for(int i =0; i< correctSequence.Count; i++)
        {
            correctSequence[i] = Vector2Check(correctSequence[i], i);
        }
        RearrangedSequence();
    }

    IEnumerator FlashCorrectSequenceCo()
    {
        //Flashes the correct sequence and start the game
        //isGameActive controls whether the update loop runs
        yield return new WaitForSeconds(delayTime);
        foreach (Tile p in gm.correctTiles)
        {
            p.ChangeColor(Color.green);
            yield return new WaitForSeconds(0.5f);
        }
        foreach (Tile p in gm.correctTiles)
        {
            yield return new WaitForSeconds(0.1f);
            p.ChangeColor(Color.white);
        }
        yield return new WaitForSeconds(delayTime);      
        isGameActive = true;
    }

    public void RearrangedSequence()
    {
        //Rearranges the instaniated tile order to fit index order 
        int index = 0;
        Tile[] correctTilesCopy = gm.correctTiles.ToArray();
        foreach (Tile p in correctTilesCopy)
        {
            int i = currentSequence.IndexOf(p.getCoord());
            Tile tmp = correctTilesCopy[i];
            gm.correctTiles[index] = correctTilesCopy[i];
            correctTilesCopy[i] = tmp;
            index++;
           
        }
        //Start flash sequence
        StartCoroutine(FlashCorrectSequenceCo());
    }

    Vector2Int Vector2Check(Vector2Int points, int index)
    {
        if(points.x > gridSize)
        {
            points.x = 0;
        }
        if(points.y > gridSize)
        {
            points.y = index;
        }
        return points;
    }

    public void OnTileClicked(Tile tile)
    {
        //On tileClick event method
        Vector2Int pos = tile.getCoord();
        failed = (currentSequence.IndexOf(pos) == index) ? false : true;
        //Set Tilemap
        Vector3Int posVec3 = tileMap.WorldToCell(tile.transform.position);
        tileMap.SetTile(posVec3, defaultTile);

        //If the final tile is clicked
        if (currentSequence[currentSequence.Count-1] == pos)
        {
            CheckGridEnd();
        }
        index++;
    }

    public void CheckGridEnd()
    {
        tileMap.ClearAllTiles();
        if (tries > 0)
        {
            tries--;
            IncreaseGridSize();
        }
        else
        {
            isGameActive = false;
            score -= (failed) ? 500 : 0;
        }
    }

    [System.Serializable]
    internal class CorrectSequences
    {
        public List<Vector2Int> correctSequence = new List<Vector2Int>();
    }
}
