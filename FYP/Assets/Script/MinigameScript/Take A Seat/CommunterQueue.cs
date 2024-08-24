using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunterQueue 
{
    readonly Vector2 startPos;
    readonly Vector2 endPos;
    readonly float distance;

    public float Speed { get; set; }

    //Reference
    readonly TakeASeatGm gm;

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

        float dur = distance / Speed;
        commuter.GetComponent<Commuter>().Move(endPos, dur);
    }

    public void MoveCommuter(Transform commuter)
    {
        float distance = (endPos - (Vector2)commuter.position).magnitude;

        float dur = distance / Speed;
        commuter.GetComponent<Commuter>().Move(endPos, dur);
    }

    public void InitialiseSeats(List<Seat> seatList, int vulnerbaleCount)
    {
        for (int i = 0; i < seatList.Count; i++)
        {
            seatList[i].gameObject.SetActive((i < vulnerbaleCount));
        }
    }
    
}
