using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Linq;

public class PathFinder : BaseMiniGameClass
{
    [Header("Camera")]
    [SerializeField] Camera minigameCam;

    [Header("Color")]
    [SerializeField] Color tileDefaultColor;
    [SerializeField] Color tileFlashColor;

    [SerializeField] GridManager gm;
    [Header("Sequence")]
    [SerializeField] List<SequenceScriptable> sequenceScriptables;  //All sequences 
    public SequenceScriptable currentSequence { get; set; }
    [SerializeField] Vector2Int currentplayerTile;
    int gridSize;

    public List<Vector2Int> currentSequenceVec2;
    private int index;
    private bool state = false;

    [Header("Delay Time")]
    [SerializeField] float delayTime;
    [Header("TileMap")] //Tilemap
    [SerializeField] Tilemap tileMap;
    [SerializeField] TileBase defaultTile;

    [Header("Reset Button")]
    [SerializeField] Button resetButton;

    [Header("Pass/Fail UI Panel")]
    [SerializeField] Image panel;

    [Header("Audio")]
    [SerializeField] AudioClip levelMusic;

    private void Awake()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayMusic(levelMusic);
        }
    }

    public override void EndSequenceMethod()
    {
        Debug.Log(score);
        UnloadedAndUpdateScore(score);
    }

    public override void StartGame()
    {
        score = 2000;
        SetDifficulty();
        IncreaseGridSize();
    }

    public override void UpdateGame()
    {
        //Mouse screen raycast to click on the tile
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(minigameCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                hit.transform.GetComponent<Tile>().OnClick();
            }
        }
    }

    void IncreaseGridSize()
    {
        resetButton.gameObject.SetActive(true);
        panel.gameObject.SetActive(false);
        //After completing a sequence, this is called to start a new sequence
        currentSequenceVec2 = currentSequence.correctSequences[0].sequence;
        gridSize = currentSequence.correctSequences[0].gridSize;
        currentSequence.correctSequences.RemoveAt(0);
        Retry();
    }
    public void Retry()
    {
        currentplayerTile = currentSequenceVec2[0] - Vector2Int.up;  //Sets the first tile as the first tile in the created path
        StopAllCoroutines();
        tileMap.ClearAllTiles();
        index = 0;
        state = false;
        gm.GenerateGrid(gridSize, this);
        RearrangedSequence();
        isGameActive = false;
    }


    IEnumerator FlashCorrectSequenceCo()
    {
        //Flashes the correct sequence and start the game
        //isGameActive controls whether the update loop runs
        yield return new WaitForSeconds(delayTime);
        foreach (Tile p in gm.CorrectTiles)
        {
            if (AudioManager.instance != null){
                AudioManager.instance.PlaySFX(AudioManager.instance.correctGrid);
            }
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
        Tile[] correctTilesCopy = gm.CorrectTiles.ToArray();
        foreach (Tile p in correctTilesCopy)
        {
            int i = currentSequenceVec2.IndexOf(p.getCoord());
            Tile tmp = correctTilesCopy[i];
            gm.CorrectTiles[index] = correctTilesCopy[i];
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
        if (currentSequenceVec2.IndexOf(pos) == index && !state){
            index++;
        }
        else
        {
            state = true;
        }
        //If the final tile is clicked
        if (currentSequenceVec2[currentSequenceVec2.Count-1] == pos)
        {
            isGameActive = false;
            resetButton.gameObject.SetActive(false);
            gm.RunPathCo(index, state);

            score += (!state) ? 0 : 1000;
        }

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.correctGrid);
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
        if (currentSequence.correctSequences.Count>0)
        {
            IncreaseGridSize();
        }
        else
        {
            EndSequenceMethod();
        }
    }

    public void SetOnGridComplete(bool pass)
    {
        panel.gameObject.SetActive(pass);
        panel.sprite = currentSequence.failedSnippet;
    }

    protected override void SetDifficulty()
    {
        switch (GetDifficulty())
        {
            case difficulty.One:
                currentSequence = Instantiate(sequenceScriptables[0]);
                break;
            case difficulty.Two:
                currentSequence = Instantiate(sequenceScriptables[1]);
                break;
        }
    }

    public List<Tile> MatchCorrectTile(List<Tile> allTiles)
    {
        return allTiles.Where(c => currentSequenceVec2.Contains(c.getCoord())).ToList();
    }
}
