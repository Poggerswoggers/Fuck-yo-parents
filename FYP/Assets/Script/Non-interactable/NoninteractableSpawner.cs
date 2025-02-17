using System.Collections;
using System.Collections.Generic;
using TMPro;
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
            Vector3 spawnPosition = FindValidSpawnPosition(xExtent, yExtent);
            Transform spawned = Instantiate(spawnerPrefab, spawnPosition, Quaternion.identity).transform;
            spawned.SetParent(transform);
            spawned.GetComponent<NonInteractableNpc>().Noninteractablefields = Noninteractablefields;
            spawned.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Noninteractablefields.possibleSprites[i];
        }
    }

    Vector3 FindValidSpawnPosition(float xExtent, float yExtent)
    {
        Vector3 spawnPosition;
        int attempts = 0;
        do
        {
            spawnPosition = new Vector3(
                Random.Range(-xExtent, xExtent),
                Random.Range(-yExtent, yExtent),
                0
            );

            attempts++;
            if (attempts > 20) break; 
        }
        while (Physics2D.OverlapCircle(spawnPosition, 0.5f, LayerMask.GetMask("Obstacle")));

        return spawnPosition;
    }

}
