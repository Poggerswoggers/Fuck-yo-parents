using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Take a Seat/Commuter sprite list")]
public class NpcAsset : ScriptableObject
{
    public Sprite[] normalCommuter = new Sprite[10];
    public Sprite[] vulnerableCommuter = new Sprite[3];

    public List<Sprite> commuter()
    {
        var list = new List<Sprite>(vulnerableCommuter);
        list.AddRange(new List<Sprite>(normalCommuter));
        return list;
    }
}
