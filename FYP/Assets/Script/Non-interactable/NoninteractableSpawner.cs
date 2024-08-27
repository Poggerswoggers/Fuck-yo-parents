using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoninteractableSpawner : MonoBehaviour
{
    int spawnCount;

    [SerializeField] GameObject spawnerPrefab;
    [SerializeField] NoninteractableScriptable Noninteractablefields;

    // Start is called before the first frame update
    void Awake()
    {
        float xExtent = MapBound.UpperBound.x;
        float yExtent = MapBound.UpperBound.y;

        spawnCount = Noninteractablefields.possibleSprites.Count;

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 randomPosition = new Vector2(Random.Range(-xExtent+1.5f, xExtent - 1.5f), Random.Range(-yExtent + 1.5f, yExtent - 1.5f));
            Transform spawned = Instantiate(spawnerPrefab, randomPosition, Quaternion.identity).transform;
            spawned.SetParent(transform);
            spawned.GetComponent<NonInteractableNpc>().Noninteractablefields = Noninteractablefields;
            spawned.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Noninteractablefields.possibleSprites[i];
        }
    }
}
