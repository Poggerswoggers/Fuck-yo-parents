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
    [SerializeField] Vector2Int currentplayerTile;
    int gridSize;

    public List<Vector2Int> currentSequence { get; set;}
    public int index;
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
        currentplayerTile = currentSequence[0] - Vector2Int.up;  //Sets the first tile as the first tile in the created path
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
        gm.SetMiscTile();
        StartCoroutine(FlashCorrectSequenceCo());
    }

    public void OnTileClicked(Tile tile)
    {
        //This is to check that the clicked tile is adjacent to the previous tile
        if ((currentplayerTile - tile.getCoord()).sqrMagnitude > 1) return;          
        currentplayerTile = tile.getCoord();
        //Unsubscribe the tile from this onclick event
        tile.DisableOnClick();

        //Set Tile to be painted on the tilemap
        paintTile(tile.transform.position);

        //On tileClick event method
        Vector2Int pos = tile.getCoord();
        if (currentSequence.IndexOf(pos) == index && !failed)
        {
            index++;
        }
        else
        {
            failed = true;
        }
        //If the final tile is clicked
        if (currentSequence[currentSequence.Count-1] == pos)
        {
            isGameActive = false;
            //StartCoroutine(gm.RunPathCo());
            gm.RunPathCo();
        }
    }
    public void paintTile(Vector3 pos)
    {
        //Paint the til on the tilemap on vector3 pos
        Vector3Int posVec3 = tileMap.WorldToCell(pos);
        tileMap.SetTile(posVec3, defaultTile);
    }

    public void CheckGridEnd()
    {
        tileMap.ClearAllTiles();
        if (sequences.Count>0)
        {
            IncreaseGridSize();
        }
        else
        {
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
