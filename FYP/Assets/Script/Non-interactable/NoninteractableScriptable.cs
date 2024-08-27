using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Non-interactable npc fields")]
public class NoninteractableScriptable : ScriptableObject
{
    public float speed;
    public float borderMargin = 0.5f;

    public List<Sprite> possibleSprites;
    public Sprite GetRandomSprite()
    {
        int i = Random.Range(0, possibleSprites.Count);
        return possibleSprites[i];
    }
}
