using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seat : MonoBehaviour
{
    public bool occupied { get; set; }
    private Transform seatedCommuter;
    public void SitCommuter(Transform commuter)
    {
        seatedCommuter = commuter;
        commuter.parent =null;
        commuter.position = transform.position;
        commuter.GetComponent<Collider2D>().enabled = false;
        occupied = true;
    }

    public Transform GetSeatedCommuter()
    {
        Transform t = seatedCommuter;
        occupied = false;
        seatedCommuter = null;
        return t;
    }
}
