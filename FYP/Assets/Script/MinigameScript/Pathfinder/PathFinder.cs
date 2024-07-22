using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : BaseMiniGameClass
{
    [SerializeField] GridManager gm;

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
        currentSequence = sequences[0].correctSequence;
        gm.GenerateGrid(gridSize, this);
        CheckCorrectSequence(currentSequence);
    }

    public override void UpdateGame()
    {
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
        yield return new WaitForSeconds(delayTime);
        foreach (Tile p in gm.correctTiles)
        {
            p.ChangeColor(Color.green);
            yield return new WaitForSeconds(0.5f);
        }
        foreach (Tile p in gm.correctTiles)
        {
            yield return new WaitForSeconds(0.5f);
            p.ChangeColor(Color.white);
        }
        yield return new WaitForSeconds(delayTime);      
        isGameActive = true;
    }

    public void RearrangedSequence()
    {
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
        Vector2Int tileVector2 = new Vector2Int(x, y);
        playerSequence.Add(tileVector2);

        if (tileVector2 == currentSequence[index])
        {   
            Debug.Log("yes");
        }
        index++;
        CheckGridEnd();
    }

    public void CheckGridEnd()
    {
        if(playerSequence.Count == currentSequence.Count)
        {
            IncreaseGridSize();
        }
    }

    [System.Serializable]
    internal class CorrectSequences
    {
        public List<Vector2Int> correctSequence = new List<Vector2Int>();
    }
}
