using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdAhma : CrowdNpc
{
    [SerializeField] float patienceTime;
    bool completed;

    // Update is called once per frame
    public override void Update()
    {
        patienceTime -= Time.deltaTime;
        if (patienceTime < 0 && !completed)
        {
            completed = true;
            base.MoveInQueue(new Vector2(-11, 3));
            base.SelfDestruct(6);

            FindObjectOfType<CrowdControl>().npcCount--;
        }
    }
}
