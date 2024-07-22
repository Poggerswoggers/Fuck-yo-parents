using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : BaseMiniGameClass
{
    [Header("Game config")]
    [SerializeField] int tries;

    [SerializeField] GridManager gm;
    [Header("Sequence")]
    [SerializeField] List<CorrectSequences> sequences;
    [SerializeField] List<Vector2Int> playerSequence = new List<Vector2Int>();
    public List<Vector2Int> currentSequence { get; set;}
    int index;

    [SerializeField] int gridSize;
    [Header("Delay Time")]
    [SerializeField] float delayTime;
    [Header("FlashTiles")]
    [SerializeField] GameObject correctTiles;
    public override void EndSequenceMethod()
    {
        //throw new System.NotImplementedException();
    }

    public override void StartGame()
    {
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

    public void IncreaseGridSize()
    {
        //After completing a sequence, this is called to start a new sequence
        isGameActive = false;
        gridSize++;
        sequences.RemoveAt(0);
        currentSequence = sequences[0].correctSequence;
        playerSequence.Clear();
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

    public void OnTileClicked(int x, int y)
    {
        //On tileClick event method
        Vector2Int tileVector2 = new Vector2Int(x, y);
        playerSequence.Add(tileVector2);

        if (tileVector2 == currentSequence[index])
        {   
            Debug.Log("yes");
        }
        if (currentSequence[currentSequence.Count-1] == tileVector2)
        {
            CheckGridEnd();
        }
        index++;
    }

    public void CheckGridEnd()
    {
        if(playerSequence.Count == currentSequence.Count)
        {
           
        }
        if(tries > 0)
        {
            tries--;
            IncreaseGridSize();
        }
        else
        {
            Debug.Log("Game Over");
        }
    }

    [System.Serializable]
    internal class CorrectSequences
    {
        public List<Vector2Int> correctSequence = new List<Vector2Int>();
    }
}
