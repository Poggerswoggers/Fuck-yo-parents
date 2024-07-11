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
        CheckCorrectSequence(currentSequence);
        gm.GenerateGrid(gridSize, this);
        isGameActive = true;
    }

    public override void UpdateGame()
    {
        
    }

    protected override IEnumerator InstructionCo()
    {
        yield return null;
        StartGame();
    }

    public void IncreaseGridSize()
    {
        gridSize++;
        sequences.RemoveAt(0);
        currentSequence = sequences[0].correctSequence;
        playerSequence.Clear();
        index = 0;
        gm.GenerateGrid(gridSize, this);
    }

    void CheckCorrectSequence(List<Vector2Int> correctSequence)
    {
        for(int i =0; i< correctSequence.Count; i++)
        {
            correctSequence[i] = Vector2Check(correctSequence[i], i);
        }
        StartCoroutine(FlashCorrectSequenceCo());
    }

    IEnumerator FlashCorrectSequenceCo()
    {
        yield return new WaitForSeconds(1f);
        foreach(Tile p in gm.GetCorrectTile())
        {
            p.ChangeColor(Color.green);
            yield return new WaitForSeconds(delayTime);
        }

        yield return new WaitForSeconds(delayTime * 2);      
        foreach(Tile p in gm.GetCorrectTile())
        {
            p.ChangeColor(Color.white);
        }
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
