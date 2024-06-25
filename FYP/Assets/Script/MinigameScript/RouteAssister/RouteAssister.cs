using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RouteAssister : BaseMiniGameClass
{
    public List<int> busNumbers;

    public override void EndSequenceMethod()
    {
        
    }

    public override void StartGame()
    {
        
    }


    public override void UpdateGame()
    {
        
    }

    protected override IEnumerator InstructionCo()
    {
        throw new System.NotImplementedException();
    }

    public float ThisBusNumber(int i)
    {
        return busNumbers[i];
    }
}
