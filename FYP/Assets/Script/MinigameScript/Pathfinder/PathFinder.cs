using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : BaseMiniGameClass
{
    public override void EndSequenceMethod()
    {
        //throw new System.NotImplementedException();
    }

    public override void StartGame()
    {
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
}
