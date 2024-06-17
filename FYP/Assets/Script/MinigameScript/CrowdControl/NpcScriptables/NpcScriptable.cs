using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CrowdControl/NpcScriptables")]
public class NpcScriptable : ScriptableObject
{
    public string npcName;
    public Sprite npcSprite;

    public List<CardObject> cardObjects;
}
