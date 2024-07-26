using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinder : BaseMiniGameClass
{
    [Header("Color")]
    [SerializeField] Color tileDefaultColor;
    [SerializeField] Color tileFlashColor;

    [SerializeField] GridManager gm;
    [Header("Sequence")]
    [SerializeField] List<CorrectSequences> sequences;  //All sequences 
    List<Vector2Int> playerSequence = new List<Vector2Int>();  //Players click sequence
    int gridSize;

    public List<Vector2Int> currentSequence { get; set;}
    int index;
    bool failed;

    [Header("Delay Time")]
    [SerializeField] float delayTime;
    [Header("TileMap")] //Tilemap
    [SerializeField] Tilemap tileMap;
    [SerializeField] TileBase defaultTile;

    public override void EndSequenceMethod()
    {
        
    }

    public override void StartGame()
    {
        score = 2000;
        IncreaseGridSize();
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
        currentSequence = sequences[0].correctSequence;
        gridSize = sequences[0].gridSize;
        sequences.RemoveAt(0);
        Retry();
    }
    public void Retry()
    {
        StopAllCoroutines();
        tileMap.ClearAllTiles();
        index = 0;
        gm.GenerateGrid(gridSize, this);
        RearrangedSequence();
        isGameActive = false;
    }



    IEnumerator FlashCorrectSequenceCo()
    {
        //Flashes the correct sequence and start the game
        //isGameActive controls whether the update loop runs
        yield return new WaitForSeconds(delayTime);
        foreach (Tile p in gm.correctTiles)
        {
            p.ChangeColor(tileFlashColor);
            yield return new WaitForSeconds(0.5f);
        }        
        yield return new WaitForSeconds(delayTime);
        gm.ColorAllTiles(tileDefaultColor);
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
        //Start flash sequence and set enroute tiles
        gm.SetMiscTile(this);
        StartCoroutine(FlashCorrectSequenceCo());
    }

    public void OnTileClicked(Tile tile)
    {
        //On tileClick event method
        Vector2Int pos = tile.getCoord();
        failed = (currentSequence.IndexOf(pos) == index) ? false : true;
        playerSequence.Add(pos);    //Add tilePos to current sequence

        //Set Tilemap
        paintTile(tile.transform.position);
        //If the final tile is clicked
        if (currentSequence[currentSequence.Count-1] == pos)
        {
            StartCoroutine(CheckGridEnd());
        }
        index++;
    }
    public void paintTile(Vector3 pos)
    {
        Vector3Int posVec3 = tileMap.WorldToCell(pos);
        tileMap.SetTile(posVec3, defaultTile);
    }

    IEnumerator CheckGridEnd()
    {
        isGameActive = false;
        yield return new WaitForSeconds(1.5f);
        tileMap.ClearAllTiles();
        if (sequences.Count>0)
        {
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
        public int gridSize;
    }
}
