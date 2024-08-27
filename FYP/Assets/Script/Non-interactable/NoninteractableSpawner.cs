using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoninteractableSpawner : MonoBehaviour
{
    [SerializeField] int spawnCount;
    [SerializeField] GameObject spawnerPrefab;

    [SerializeField] Transform parent;
    // Start is called before the first frame update
    void Awake()
    {
        float xExtent = MapBound.UpperBound.x;
        float yExtent = MapBound.UpperBound.y;
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 randomPosition = new Vector2(Random.Range(-xExtent, xExtent), Random.Range(-yExtent, yExtent));
            GameObject spawned = Instantiate(spawnerPrefab, randomPosition, Quaternion.identity);
            spawned.transform.SetParent(parent);
        }
    }
}
