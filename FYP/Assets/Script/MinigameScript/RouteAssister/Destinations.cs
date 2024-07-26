using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Route Assistor/Bus routes to destination")]
public class Destinations : ScriptableObject
{
    [Header("End destination name")]
    public string destinationName;
    [Header("Travel map")]
    public Sprite map;

    public enum MRTRoutes
    {
        RedLine,
        GreenLine,
        PurpleLine,
        BrownLine,
        BlueLine,
        YellowLine
    }
    [Header("Sequence of travel")]
    public List<MRTRoutes> route;
}
