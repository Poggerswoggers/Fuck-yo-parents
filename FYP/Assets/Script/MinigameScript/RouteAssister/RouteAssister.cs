using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RouteAssister : BaseMiniGameClass
{
    public List<int> busNumbers;
    Camera cam;

    public override void EndSequenceMethod()
    {
        
    }

    public override void StartGame()
    {
        cam = Camera.main;
        isGameActive = true;
    }


    public override void UpdateGame()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        }
    }

    protected override IEnumerator InstructionCo()
    {
        //throw new System.NotImplementedException();
        StartGame();
        yield return null;
    }

    
}
