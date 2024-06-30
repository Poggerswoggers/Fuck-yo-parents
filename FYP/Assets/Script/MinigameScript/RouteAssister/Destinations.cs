using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Route Assistor/Bus routes to destination")]
public class Destinations : ScriptableObject
{
    public string destinationName;

    public List<int> busRoutes;

    public Sprite map;
}
