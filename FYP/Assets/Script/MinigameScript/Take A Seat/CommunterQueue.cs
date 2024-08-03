using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunterQueue 
{
    readonly Vector2 startPos;
    readonly Vector2 endPos;

    float distance;

    //Reference
    TakeASeatGm gm;
   public CommunterQueue(Vector2 startPos, Vector2 endPos, TakeASeatGm gm)
    {
        this.startPos = startPos;
        this.endPos = endPos;
        this.gm = gm;

        distance = (endPos - startPos).magnitude;
    }

    public void InitialiseCommuter(GameObject commuter)
    {
        gm.pooledCommunters.Remove(commuter);
        commuter.SetActive(true);
        commuter.transform.position = startPos;
        commuter.GetComponent<Commuter>().Move(endPos, distance);
    }

    public void MoveCommuter(Transform commuter)
    {
        float distance = (endPos - (Vector2)commuter.position).magnitude;
        commuter.GetComponent<Commuter>().Move(endPos, distance);
    }


    
}
